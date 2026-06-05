using QuoteFlow.EntityFrameworkCore;
using QuoteFlow.MaterialGroupBuyers.ParameterObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.MaterialGroupBuyers;

public class EfCoreMaterialGroupBuyerRepository : EfCoreRepository<QuoteFlowDbContext, MaterialGroupBuyer, Guid>, IMaterialGroupBuyerRepository
{
    public EfCoreMaterialGroupBuyerRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<MaterialGroupBuyer>> GetListAsync(
     MaterialGroupBuyerFilterParams filterParams,
     CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter(await GetQueryableAsync(), filterParams);
        query = query
            .Include(x => x.MaterialGroup)
            .Include(x => x.Buyer);

        query = query.OrderBy(string.IsNullOrWhiteSpace(filterParams.Sorting)
            ? MaterialGroupBuyerConsts.GetDefaultSorting(false)
            : filterParams.Sorting);

        var results = await query
            .PageBy(filterParams.SkipCount, filterParams.MaxResultCount)
            .ToListNoLockAsync(dbContext, cancellationToken);
        return results.DistinctBy(x => x.BuyerId).ToList();
    }


    public virtual async Task<long> GetCountAsync(
     MaterialGroupBuyerFilterParams filterParams,
     CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter(await GetDbSetAsync(), filterParams);

        // Count distinct BuyerId
        var count = await query
            .Select(x => x.BuyerId)
            .Distinct()
            .CountNoLockAsync(dbContext, cancellationToken);

        return count;
    }



    protected virtual IQueryable<MaterialGroupBuyer> ApplyFilter(
    IQueryable<MaterialGroupBuyer> query,
    MaterialGroupBuyerFilterParams filterParams)
    {
        return query
            .WhereIf(filterParams.MaterialGroupId.HasValue, e => e.MaterialGroupId == filterParams.MaterialGroupId)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.MaterialGroupCode), e => e.MaterialGroupCode.Contains(filterParams.MaterialGroupCode))
            .WhereIf(filterParams.BuyerId.HasValue, e => e.BuyerId == filterParams.BuyerId)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.BuyerShortName), e => e.BuyerShortName.Contains(filterParams.BuyerShortName))
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.Note), e => e.Note.Contains(filterParams.Note));
    }

}