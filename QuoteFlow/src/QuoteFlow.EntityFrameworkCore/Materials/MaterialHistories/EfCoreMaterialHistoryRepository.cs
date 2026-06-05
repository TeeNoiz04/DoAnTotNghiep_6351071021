using QuoteFlow.EntityFrameworkCore;
using QuoteFlow.Materials.MaterialHistories.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.Materials.MaterialHistories;

public class EfCoreMaterialHistoryRepository : EfCoreRepository<QuoteFlowDbContext, MaterialHistory, Guid>, IMaterialHistoryRepository
{
    public EfCoreMaterialHistoryRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<MaterialHistory>> GetListAsync(
        MaterialHistoryFilterParams filterParams,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter(await GetQueryableAsync(), filterParams);
        query = query.OrderBy(string.IsNullOrWhiteSpace(filterParams.Sorting) ? MaterialHistoryConsts.GetDefaultSorting(false) : filterParams.Sorting);
        return await query.PageBy(filterParams.SkipCount, filterParams.MaxResultCount).ToListNoLockAsync(dbContext, cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        MaterialHistoryFilterParams filterParams,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter(await GetDbSetAsync(), filterParams);
        return await query.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<MaterialHistory> ApplyFilter(
        IQueryable<MaterialHistory> query,
        MaterialHistoryFilterParams filterParams)
    {
        return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterParams.FilterText), e => e.Action!.Contains(filterParams.FilterText!) || e.Note!.Contains(filterParams.FilterText!))
                .WhereIf(filterParams.MaterialId.HasValue, e => e.MaterialId == filterParams.MaterialId)
                .WhereIf(!string.IsNullOrWhiteSpace(filterParams.Action), e => e.Action.Contains(filterParams.Action))
                .WhereIf(!string.IsNullOrWhiteSpace(filterParams.Note), e => e.Note.Contains(filterParams.Note));
    }
}