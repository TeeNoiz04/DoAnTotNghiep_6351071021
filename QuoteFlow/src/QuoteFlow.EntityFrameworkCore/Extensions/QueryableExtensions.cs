using QuoteFlow.BuyerAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace QuoteFlow.Extensions;

public static class QueryableExtensions
{
    /// <summary>
    /// Applies buyer access filtering based on IBuyerRestrictable filter parameters
    /// BuyerAccessExtensions handles the logic, this just applies the filters
    /// </summary>
    public static IQueryable<T> ApplyBuyerFilter<T>(this IQueryable<T> query, IBuyerRestrictable filterParams, Expression<Func<T, Guid?>> buyerIdSelector)
    {
        // Apply specific buyer filter if provided
        if (filterParams.BuyerId.HasValue && filterParams.BuyerId.Value != Guid.Empty)
        {
            if (filterParams.RestrictedBuyerIds != null &&
                filterParams.RestrictedBuyerIds.Count > 0 &&
                !filterParams.RestrictedBuyerIds.Contains(filterParams.BuyerId.Value))
            {
                // Return an empty query result
                return query.Where(t => false);
            }

            var parameter = buyerIdSelector.Parameters[0];
            var buyerIdProperty = buyerIdSelector.Body;

            var equalExpression = Expression.Equal(
                buyerIdProperty,
                Expression.Constant(filterParams.BuyerId, typeof(Guid?)));
            var specificBuyerLambda = Expression.Lambda<Func<T, bool>>(equalExpression, parameter);

            return query.Where(specificBuyerLambda);
        }
        // Handle specific case for no buyer price offer - use Guid.Empty as workaround
        else if (filterParams.BuyerId.HasValue && filterParams.BuyerId.Value == Guid.Empty)
        {
            var parameter = buyerIdSelector.Parameters[0];
            var buyerIdProperty = buyerIdSelector.Body; // Type is Expression<Func<T, Guid?>> so the body is Guid?

            // 1. Create a constant expression for null (of type Guid?)
            var nullConstant = Expression.Constant(null, typeof(Guid?));

            // 2. Create the equality expression: buyerIdProperty == null
            var isNullExpression = Expression.Equal(buyerIdProperty, nullConstant);

            // 3. Create the Lambda expression
            var nullBuyerLambda = Expression.Lambda<Func<T, bool>>(isNullExpression, parameter);

            return query.Where(nullBuyerLambda);
        }

        // Apply restricted buyer IDs using the legacy method
        return query.ApplyBuyerFilter(filterParams.RestrictedBuyerIds, buyerIdSelector);
    }

    /// <summary>
    /// Applies buyer access filtering based on restricted buyer IDs list
    /// </summary>
    public static IQueryable<T> ApplyBuyerFilter<T>(this IQueryable<T> query, List<Guid> restrictedBuyerIds, Expression<Func<T, Guid?>> buyerIdSelector)
    {
        if (restrictedBuyerIds.Count > 0)
        {
            // Check for special "no access" marker (Guid.Empty)
            if (restrictedBuyerIds.Count == 1 && restrictedBuyerIds[0] == Guid.Empty)
            {
                return query.Where(_ => false);
            }

            var parameter = buyerIdSelector.Parameters[0];
            var buyerIdProperty = buyerIdSelector.Body;

            // x.BuyerId == null (records visible to everyone)
            var isNullCheck = Expression.Equal(
                buyerIdProperty,
                Expression.Constant(null, typeof(Guid?)));

            // x.BuyerId != null
            var notNullCheck = Expression.NotEqual(
                buyerIdProperty,
                Expression.Constant(null, typeof(Guid?)));

            // x.BuyerId.Value
            var buyerIdValue = Expression.Property(buyerIdProperty, "Value");

            // restrictedBuyerIds.Contains(x.BuyerId.Value)
            var containsMethod = typeof(List<Guid>).GetMethod("Contains", new[] { typeof(Guid) });
            var containsCall = Expression.Call(
                Expression.Constant(restrictedBuyerIds),
                containsMethod!,
                buyerIdValue);

            // x.BuyerId != null && restrictedBuyerIds.Contains(x.BuyerId.Value)
            var restrictedBuyerExpression = Expression.AndAlso(notNullCheck, containsCall);

            // x.BuyerId == null || (x.BuyerId != null && restrictedBuyerIds.Contains(x.BuyerId.Value))
            var finalExpression = Expression.OrElse(isNullCheck, restrictedBuyerExpression);

            var lambda = Expression.Lambda<Func<T, bool>>(finalExpression, parameter);
            query = query.Where(lambda);
        }

        return query;
    }

    public static IQueryable<T> ApplyLocationFilter<T>(
    this IQueryable<T> query,
    ILocationRestrictable filterParams,
    Expression<Func<T, Guid?>> locationIdSelector)
    {
        // Specific location
        if (filterParams.LocationId.HasValue && filterParams.LocationId.Value != Guid.Empty)
        {
            if (filterParams.RestrictedLocationIds != null &&
                filterParams.RestrictedLocationIds.Count > 0 &&
                !filterParams.RestrictedLocationIds.Contains(filterParams.LocationId.Value))
            {
                // Return an empty query result
                return query.Where(t => false);
            }

            var parameter = locationIdSelector.Parameters[0];
            var property = locationIdSelector.Body;

            var equalExpression = Expression.Equal(
                property,
                Expression.Constant(filterParams.LocationId, typeof(Guid?)));
            var lambda = Expression.Lambda<Func<T, bool>>(equalExpression, parameter);

            return query.Where(lambda);
        }

        // Restricted location IDs
        return query.ApplyLocationFilter(filterParams.RestrictedLocationIds, locationIdSelector);
    }

    public static IQueryable<T> ApplyLocationFilter<T>(
        this IQueryable<T> query,
        List<Guid> restrictedLocationIds,
        Expression<Func<T, Guid?>> locationIdSelector)
    {
        if (restrictedLocationIds.Count > 0)
        {
            if (restrictedLocationIds.Count == 1 && restrictedLocationIds[0] == Guid.Empty)
                return query.Where(_ => false);

            var parameter = locationIdSelector.Parameters[0];
            var property = locationIdSelector.Body;

            var notNullCheck = Expression.NotEqual(property, Expression.Constant(null, typeof(Guid?)));
            var valueProperty = Expression.Property(property, "Value");

            var containsMethod = typeof(List<Guid>).GetMethod("Contains", new[] { typeof(Guid) })!;
            var containsCall = Expression.Call(Expression.Constant(restrictedLocationIds), containsMethod, valueProperty);

            var finalExpression = Expression.AndAlso(notNullCheck, containsCall);
            var lambda = Expression.Lambda<Func<T, bool>>(finalExpression, parameter);

            query = query.Where(lambda);
        }

        return query;
    }

    public static IQueryable<T> ApplyMaterialTypeFilter<T>(
    this IQueryable<T> query,
    IMaterialTypeRestrictable filterParams,
    Expression<Func<T, string?>> materialTypeSelector)
    {
        // Specific material type
        if (!string.IsNullOrEmpty(filterParams.MaterialType))
        {
            if (filterParams.RestrictedMaterialTypes != null &&
                filterParams.RestrictedMaterialTypes.Count > 0 &&
                !filterParams.RestrictedMaterialTypes.Contains(filterParams.MaterialType))
            {
                // Return an empty query result
                return query.Where(t => false);
            }

            var parameter = materialTypeSelector.Parameters[0];
            var property = materialTypeSelector.Body;

            var equalExpression = Expression.Equal(
                property,
                Expression.Constant(filterParams.MaterialType, typeof(string)));
            var lambda = Expression.Lambda<Func<T, bool>>(equalExpression, parameter);

            return query.Where(lambda);
        }

        // Restricted material types
        return query.ApplyMaterialTypeFilter(filterParams.RestrictedMaterialTypes, materialTypeSelector);
    }

    public static IQueryable<T> ApplyMaterialTypeFilter<T>(
        this IQueryable<T> query,
        List<string> restrictedMaterialTypes,
        Expression<Func<T, string?>> materialTypeSelector)
    {
        if (restrictedMaterialTypes.Count > 0)
        {
            if (restrictedMaterialTypes.Count == 1 && restrictedMaterialTypes[0] == string.Empty)
                return query.Where(_ => false);

            var parameter = materialTypeSelector.Parameters[0];
            var property = materialTypeSelector.Body;

            var containsMethod = typeof(List<string>).GetMethod("Contains", new[] { typeof(string) })!;
            var containsCall = Expression.Call(Expression.Constant(restrictedMaterialTypes), containsMethod, property);

            var lambda = Expression.Lambda<Func<T, bool>>(containsCall, parameter);

            query = query.Where(lambda);
        }

        return query;
    }

}