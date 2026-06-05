using QuoteFlow.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.DPOs.DpoGkrUsages;

public class EfCoreDpoGkrUsageRepository : EfCoreRepository<QuoteFlowDbContext, DpoGkrUsage, Guid>, IDpoGkrUsageRepository
{
    public EfCoreDpoGkrUsageRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public virtual async Task<List<DpoGkrUsage>> GetListAsync(
        string? filterText = null,
        Guid? gkrId = null,
        Guid? dpoId = null,
        decimal? gkrQtyMin = null,
        decimal? gkrQtyMax = null,
        decimal? dpoQtyMin = null,
        decimal? dpoQtyMax = null,
        decimal? gkrLockStockQtyMin = null,
        decimal? gkrLockStockQtyMax = null,
        decimal? dpoLockStockQtyMin = null,
        decimal? dpoLockStockQtyMax = null,
        decimal? gkrLockShipmentQtyMin = null,
        decimal? gkrLockShipmentQtyMax = null,
        decimal? dpoLockShipmentQtyMin = null,
        decimal? dpoLockShipmentQtyMax = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter(await GetQueryableAsync(), filterText, gkrId, dpoId, gkrQtyMin, gkrQtyMax, dpoQtyMin, dpoQtyMax, gkrLockStockQtyMin, gkrLockStockQtyMax, dpoLockStockQtyMin, dpoLockStockQtyMax, gkrLockShipmentQtyMin, gkrLockShipmentQtyMax, dpoLockShipmentQtyMin, dpoLockShipmentQtyMax);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? DpoGkrUsageConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        string? filterText = null,
        Guid? gkrId = null,
        Guid? dpoId = null,
        decimal? gkrQtyMin = null,
        decimal? gkrQtyMax = null,
        decimal? dpoQtyMin = null,
        decimal? dpoQtyMax = null,
        decimal? gkrLockStockQtyMin = null,
        decimal? gkrLockStockQtyMax = null,
        decimal? dpoLockStockQtyMin = null,
        decimal? dpoLockStockQtyMax = null,
        decimal? gkrLockShipmentQtyMin = null,
        decimal? gkrLockShipmentQtyMax = null,
        decimal? dpoLockShipmentQtyMin = null,
        decimal? dpoLockShipmentQtyMax = null,
        CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter(await GetDbSetAsync(), filterText, gkrId, dpoId, gkrQtyMin, gkrQtyMax, dpoQtyMin, dpoQtyMax, gkrLockStockQtyMin, gkrLockStockQtyMax, dpoLockStockQtyMin, dpoLockStockQtyMax, gkrLockShipmentQtyMin, gkrLockShipmentQtyMax, dpoLockShipmentQtyMin, dpoLockShipmentQtyMax);
        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<DpoGkrUsage?> FindByGkrAndDpoAsync(
        Guid gkrId,
        Guid dpoId,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .FirstOrDefaultAsync(x => x.GkrId == gkrId && x.DpoId == dpoId, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<DpoGkrUsage>> GetListByGkrIdAsync(
        Guid gkrId,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.GkrId == gkrId)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<DpoGkrUsage>> GetListByDpoIdAsync(
        Guid dpoId,
        CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync())
            .Where(x => x.DpoId == dpoId)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<DpoGkrUsage> ApplyFilter(
        IQueryable<DpoGkrUsage> query,
        string? filterText = null,
        Guid? gkrId = null,
        Guid? dpoId = null,
        decimal? gkrQtyMin = null,
        decimal? gkrQtyMax = null,
        decimal? dpoQtyMin = null,
        decimal? dpoQtyMax = null,
        decimal? gkrLockStockQtyMin = null,
        decimal? gkrLockStockQtyMax = null,
        decimal? dpoLockStockQtyMin = null,
        decimal? dpoLockStockQtyMax = null,
        decimal? gkrLockShipmentQtyMin = null,
        decimal? gkrLockShipmentQtyMax = null,
        decimal? dpoLockShipmentQtyMin = null,
        decimal? dpoLockShipmentQtyMax = null)
    {
        return query
            .WhereIf(gkrId.HasValue, e => e.GkrId == gkrId)
            .WhereIf(dpoId.HasValue, e => e.DpoId == dpoId)
            .WhereIf(gkrQtyMin.HasValue, e => e.GkrQty >= gkrQtyMin!.Value)
            .WhereIf(gkrQtyMax.HasValue, e => e.GkrQty <= gkrQtyMax!.Value)
            .WhereIf(dpoQtyMin.HasValue, e => e.DpoQty >= dpoQtyMin!.Value)
            .WhereIf(dpoQtyMax.HasValue, e => e.DpoQty <= dpoQtyMax!.Value)
            .WhereIf(gkrLockStockQtyMin.HasValue, e => e.GkrLockStockQty >= gkrLockStockQtyMin!.Value)
            .WhereIf(gkrLockStockQtyMax.HasValue, e => e.GkrLockStockQty <= gkrLockStockQtyMax!.Value)
            .WhereIf(dpoLockStockQtyMin.HasValue, e => e.DpoLockStockQty >= dpoLockStockQtyMin!.Value)
            .WhereIf(dpoLockStockQtyMax.HasValue, e => e.DpoLockStockQty <= dpoLockStockQtyMax!.Value)
            .WhereIf(gkrLockShipmentQtyMin.HasValue, e => e.GkrLockShipmentQty >= gkrLockShipmentQtyMin!.Value)
            .WhereIf(gkrLockShipmentQtyMax.HasValue, e => e.GkrLockShipmentQty <= gkrLockShipmentQtyMax!.Value)
            .WhereIf(dpoLockShipmentQtyMin.HasValue, e => e.DpoLockShipmentQty >= dpoLockShipmentQtyMin!.Value)
            .WhereIf(dpoLockShipmentQtyMax.HasValue, e => e.DpoLockShipmentQty <= dpoLockShipmentQtyMax!.Value);
    }
}