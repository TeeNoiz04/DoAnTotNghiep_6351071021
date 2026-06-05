using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace QuoteFlow.Helper;

public static class QueryFilterHelper
{
    /// <summary>
    /// Builds an OR-chain expression for multiple search terms on a single string field.
    /// Supports EF.Functions.Like if the term contains %.
    /// </summary>
    public static Expression<Func<T, bool>> BuildMultiFieldSearch<T>(
    string searchTerms,
    Expression<Func<T, string>> fieldSelector)
    {
        if (string.IsNullOrWhiteSpace(searchTerms))
            return e => true;

        var terms = searchTerms
            .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(t => t.Trim())
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .ToList();

        if (!terms.Any())
            return e => true;

        var param = Expression.Parameter(typeof(T), "e");
        var fieldExpr = new ReplaceVisitor(fieldSelector.Parameters[0], param)
            .Visit(fieldSelector.Body);

        var likeMethod = typeof(DbFunctionsExtensions).GetMethod(
            nameof(DbFunctionsExtensions.Like),
            new[] { typeof(DbFunctions), typeof(string), typeof(string), typeof(string) }
        )!;

        var efFunctions = Expression.Property(null, typeof(EF), nameof(EF.Functions));

        Expression? combined = null;

        foreach (var rawTerm in terms)
        {
            // 🔹 Step 1. Escape `_` and `[` (and `\` itself)
            var escaped = rawTerm
                .Replace("\\", "\\\\") // escape backslash itself
                .Replace("[", "[[]")   // escape '['
                .Replace("_", "[_]");  // escape underscore

            // 🔹 Step 2. Only allow `%` wildcard; leave it as-is.
            // No auto-wrapping with '%', because you said it must be exact unless user adds '%'

            var likeExpr = Expression.Call(
                null,
                likeMethod,
                efFunctions,
                fieldExpr!,
                Expression.Constant(escaped),
                Expression.Constant("\\", typeof(string)) // ESCAPE '\'
            );

            combined = combined == null ? likeExpr : Expression.OrElse(combined, likeExpr);
        }

        return Expression.Lambda<Func<T, bool>>(combined!, param);
    }

    /// <summary>
    /// Builds an expression for searching nested collection with multiple search terms.
    /// Usage: query.Where(BuildNestedCollectionSearch<Parent, Child>(searchTerms, p => p.Children, c => c.Name))
    /// </summary>
    public static Expression<Func<TParent, bool>> BuildNestedCollectionSearch<TParent, TChild>(
        string searchTerms,
        Expression<Func<TParent, ICollection<TChild>>> collectionSelector,
        Expression<Func<TChild, string>> fieldSelector)
    {
        if (string.IsNullOrWhiteSpace(searchTerms))
            return e => true;

        var terms = searchTerms
            .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(t => t.Trim().ToUpper())
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .ToList();

        if (!terms.Any())
            return e => true;

        // Parent parameter
        var parentParam = Expression.Parameter(typeof(TParent), "e");
        var collectionProperty = new ReplaceVisitor(collectionSelector.Parameters[0], parentParam)
            .Visit(collectionSelector.Body);

        // Child parameter for the Any() lambda
        var childParam = Expression.Parameter(typeof(TChild), "d");
        var childFieldExpr = new ReplaceVisitor(fieldSelector.Parameters[0], childParam)
            .Visit(fieldSelector.Body);

        // Handle possible null field value
        var coalesce = Expression.Coalesce(childFieldExpr, Expression.Constant(""));

        // Prepare EF.Functions.Like
        var likeMethod = typeof(DbFunctionsExtensions).GetMethod(
            nameof(DbFunctionsExtensions.Like),
            new[] { typeof(DbFunctions), typeof(string), typeof(string), typeof(string) }
        )!;

        var efFunctions = Expression.Property(null, typeof(EF), nameof(EF.Functions));

        Expression? combined = null;

        foreach (var rawTerm in terms)
        {
            // 🔹 Step 1: Escape `[`, `_`, and `\` for literal interpretation
            var escaped = rawTerm
                .Replace("\\", "\\\\")
                .Replace("[", "[[]")
                .Replace("_", "[_]");

            // 🔹 Step 2: Keep `%` as user-specified wildcard
            var patternConst = Expression.Constant(escaped);
            var escapeCharConst = Expression.Constant("\\", typeof(string));

            // LIKE with ESCAPE '\'
            var likeExpr = Expression.Call(
                null,
                likeMethod,
                efFunctions,
                coalesce,
                patternConst,
                escapeCharConst
            );

            combined = combined == null ? likeExpr : Expression.OrElse(combined, likeExpr);
        }

        // Build the inner lambda: d => LIKE conditions
        var innerLambda = Expression.Lambda<Func<TChild, bool>>(combined!, childParam);

        // e.Collection.Any(innerLambda)
        var anyMethod = typeof(Enumerable)
            .GetMethods()
            .First(m => m.Name == "Any" && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(TChild));

        var anyCall = Expression.Call(null, anyMethod, collectionProperty, innerLambda);

        // e.Collection != null && e.Collection.Any(...)
        var notNull = Expression.NotEqual(collectionProperty, Expression.Constant(null, typeof(ICollection<TChild>)));
        var finalExpr = Expression.AndAlso(notNull, anyCall);

        return Expression.Lambda<Func<TParent, bool>>(finalExpr, parentParam);
    }

    // Visitor to replace a parameter in an expression
    private class ReplaceVisitor : ExpressionVisitor
    {
        private readonly Expression _oldValue;
        private readonly Expression _newValue;

        public ReplaceVisitor(Expression oldValue, Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public override Expression Visit(Expression node)
        {
            return node == _oldValue ? _newValue : base.Visit(node);
        }
    }

    /// <summary>
    /// Builds an expression for searching double-nested collection with multiple search terms.
    /// Supports filtering on child collection items (e.g., IsDeleted).
    /// Usage: query.Where(BuildDoubleNestedCollectionSearch<Parent, Child, Nested>(
    ///     searchTerms, 
    ///     p => p.Children, 
    ///     c => c.NestedObject, 
    ///     n => n.Field,
    ///     c => !c.IsDeleted))
    /// </summary>
    public static Expression<Func<TParent, bool>> BuildDoubleNestedCollectionSearch<TParent, TChild, TNested>(
        string searchTerms,
        Expression<Func<TParent, ICollection<TChild>>> collectionSelector,
        Expression<Func<TChild, TNested>> nestedSelector,
        Expression<Func<TNested, string>> fieldSelector,
        Expression<Func<TChild, bool>>? childFilter = null)
        where TNested : class
    {
        if (string.IsNullOrWhiteSpace(searchTerms))
            return e => true;

        var terms = searchTerms
            .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(t => t.Trim().ToUpper())
            .Where(t => !string.IsNullOrWhiteSpace(t))
            .ToList();

        if (!terms.Any())
            return e => true;

        // Parent parameter: e
        var parentParam = Expression.Parameter(typeof(TParent), "e");
        var collectionProperty = new ReplaceVisitor(collectionSelector.Parameters[0], parentParam)
            .Visit(collectionSelector.Body);

        // Child parameter: d
        var childParam = Expression.Parameter(typeof(TChild), "d");

        // Get nested object: d.NestedObject
        var nestedObjectExpr = new ReplaceVisitor(nestedSelector.Parameters[0], childParam)
            .Visit(nestedSelector.Body);

        // Get field from nested object: d.NestedObject.Field
        var nestedParam = Expression.Parameter(typeof(TNested), "n");
        var fieldExpr = new ReplaceVisitor(fieldSelector.Parameters[0], nestedParam)
            .Visit(fieldSelector.Body);

        // Replace nested parameter with actual nested expression
        var finalFieldExpr = new ReplaceVisitor(nestedParam, nestedObjectExpr)
            .Visit(fieldExpr);

        // Handle possible null field value
        var coalesce = Expression.Coalesce(finalFieldExpr, Expression.Constant(""));

        // Prepare EF.Functions.Like
        var likeMethod = typeof(DbFunctionsExtensions).GetMethod(
            nameof(DbFunctionsExtensions.Like),
            new[] { typeof(DbFunctions), typeof(string), typeof(string), typeof(string) }
        )!;

        var efFunctions = Expression.Property(null, typeof(EF), nameof(EF.Functions));

        Expression? searchCondition = null;

        foreach (var rawTerm in terms)
        {
            // Step 1: Escape `[`, `_`, and `\` for literal interpretation
            var escaped = rawTerm
                .Replace("\\", "\\\\")
                .Replace("[", "[[]")
                .Replace("_", "[_]");

            // Step 2: Keep `%` as user-specified wildcard
            var patternConst = Expression.Constant(escaped);
            var escapeCharConst = Expression.Constant("\\", typeof(string));

            // LIKE with ESCAPE '\'
            var likeExpr = Expression.Call(
                null,
                likeMethod,
                efFunctions,
                coalesce,
                patternConst,
                escapeCharConst
            );

            searchCondition = searchCondition == null ? likeExpr : Expression.OrElse(searchCondition, likeExpr);
        }

        // Build combined condition for child:
        // 1. Child filter (e.g., !d.IsDeleted)
        // 2. Nested object != null
        // 3. Search condition on nested field

        Expression combinedChildCondition = searchCondition!;

        // Add nested object null check: d.NestedObject != null
        var nestedNotNull = Expression.NotEqual(
            nestedObjectExpr,
            Expression.Constant(null, typeof(TNested)));
        combinedChildCondition = Expression.AndAlso(nestedNotNull, combinedChildCondition);

        // Add child filter if provided: !d.IsDeleted && d.NestedObject != null && ...
        if (childFilter != null)
        {
            var childFilterExpr = new ReplaceVisitor(childFilter.Parameters[0], childParam)
                .Visit(childFilter.Body);
            combinedChildCondition = Expression.AndAlso(childFilterExpr, combinedChildCondition);
        }

        // Build the inner lambda: d => (!d.IsDeleted) && (d.NestedObject != null) && (LIKE conditions)
        var innerLambda = Expression.Lambda<Func<TChild, bool>>(combinedChildCondition, childParam);

        // e.Collection.Any(innerLambda)
        var anyMethod = typeof(Enumerable)
            .GetMethods()
            .First(m => m.Name == "Any" && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(TChild));

        var anyCall = Expression.Call(null, anyMethod, collectionProperty, innerLambda);

        // e.Collection != null && e.Collection.Any(...)
        var collectionNotNull = Expression.NotEqual(
            collectionProperty,
            Expression.Constant(null, typeof(ICollection<TChild>)));
        var finalExpr = Expression.AndAlso(collectionNotNull, anyCall);

        return Expression.Lambda<Func<TParent, bool>>(finalExpr, parentParam);
    }
}
