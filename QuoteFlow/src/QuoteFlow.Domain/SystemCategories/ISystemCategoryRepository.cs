using QuoteFlow.SystemCategories.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.SystemCategories;

public interface ISystemCategoryRepository : IRepository<SystemCategory, Guid>
{
    Task<List<SystemCategory>> GetListAsync(
        SystemCategoryFilterParams filterParams,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        SystemCategoryFilterParams filterParams,
        CancellationToken cancellationToken = default);
    Task<List<T>> GetListAsync<T>(
        SystemCategoryFilterParams filterParams,
        Expression<Func<SystemCategory, T>> selector,
        CancellationToken cancellationToken = default);
}