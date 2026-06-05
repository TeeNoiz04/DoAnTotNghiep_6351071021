using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public static class WithNoLockQueryableExtensions
{
    private static async Task<TResult> ExecuteNoLockAsync<TResult>(
    DbContext dbContext,
    Func<Task<TResult>> action)
    {

        var hasActiveTransaction = dbContext.Database.CurrentTransaction != null;

        if (hasActiveTransaction)
        {

            return await action();
        }


        await using (var transaction = await dbContext.Database.BeginTransactionAsync(
            IsolationLevel.ReadUncommitted))
        {
            var result = await action();

            return result;
        }
    }


    public static Task<List<T>> ToListNoLockAsync<T>(
        this IQueryable<T> query,
        DbContext dbContext,
        CancellationToken cancellationToken = default)
    {
        return ExecuteNoLockAsync(
            dbContext,
            () => query.ToListAsync(cancellationToken)
        );
    }

    public static Task<T?> FirstOrDefaultNoLockAsync<T>(
        this IQueryable<T> query,
        DbContext dbContext,
        CancellationToken cancellationToken = default)
    {
        return ExecuteNoLockAsync(
            dbContext,
            () => query.FirstOrDefaultAsync(cancellationToken)
        );
    }

    public static Task<T?> SingleOrDefaultNoLockAsync<T>(
        this IQueryable<T> query,
        DbContext dbContext,
        CancellationToken cancellationToken = default)
    {
        return ExecuteNoLockAsync(
            dbContext,
            () => query.SingleOrDefaultAsync(cancellationToken)
        );
    }

    public static Task<long> CountNoLockAsync<T>(
        this IQueryable<T> query,
        DbContext dbContext,
        CancellationToken cancellationToken = default)
    {
        return ExecuteNoLockAsync(
            dbContext,
            () => query.LongCountAsync(cancellationToken)
        );
    }

    public static Task<bool> AnyNoLockAsync<T>(
        this IQueryable<T> query,
        DbContext dbContext,
        CancellationToken cancellationToken = default)
    {
        return ExecuteNoLockAsync(
            dbContext,
            () => query.AnyAsync(cancellationToken)
        );
    }
}
