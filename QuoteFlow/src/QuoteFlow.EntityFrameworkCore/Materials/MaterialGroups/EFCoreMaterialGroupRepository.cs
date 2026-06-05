using QuoteFlow.EntityFrameworkCore;
using QuoteFlow.Helper;
using QuoteFlow.Materials.MaterialGroups.ParameterObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.Materials.MaterialGroups;

public class EFCoreMaterialGroupRepository : EfCoreRepository<QuoteFlowDbContext, MaterialGroup, Guid>, IMaterialGroupRepository
{
    public EFCoreMaterialGroupRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public async Task<long> GetCountAsync(MaterialGroupFilterParams filterParams, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetDbSetAsync()), filterParams);
        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<T>> GetListAsync<T>(MaterialGroupFilterParams filterParams, Expression<Func<MaterialGroup, T>> selector, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetQueryableAsync()), filterParams);

        query = query.OrderBy(MaterialGroupConsts.GetDefaultSorting(false));
        var resultQuery = query
            .PageBy(filterParams.SkipCount, filterParams.MaxResultCount)
            .Select(selector).AsQueryable();

        var result = await resultQuery.ToListNoLockAsync(dbContext, cancellationToken);

        return result;
    }

    public virtual async Task<List<MaterialGroup>> GetListAsync(MaterialGroupFilterParams filterParams, string? sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter(await GetQueryableAsync(), filterParams);
        query = query.OrderBy(string.IsNullOrWhiteSpace(filterParams.Sorting) ? MaterialGroupConsts.GetDefaultSorting(false) : filterParams.Sorting);
        return await query.PageBy(filterParams.SkipCount, filterParams.MaxResultCount).ToListNoLockAsync(dbContext, cancellationToken);
    }

    protected virtual IQueryable<MaterialGroup> ApplyFilter(
        IQueryable<MaterialGroup> query,
        MaterialGroupFilterParams filterParams)
    {
        return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterParams.MaterialType), e => e.MaterialType == filterParams.MaterialType)
                .WhereIf(!string.IsNullOrWhiteSpace(filterParams.Code), QueryFilterHelper.BuildMultiFieldSearch<MaterialGroup>(filterParams.Code, e => e.Code ))
                .WhereIf(!string.IsNullOrWhiteSpace(filterParams.Name), QueryFilterHelper.BuildMultiFieldSearch<MaterialGroup>(filterParams.Name, e => e.Name))
                .WhereIf(!string.IsNullOrWhiteSpace(filterParams.Note), e => e.Note == filterParams.Note)
                .WhereIf(filterParams.IsDeActive.HasValue, e => e.IsDeActive == filterParams.IsDeActive);

    }
}
