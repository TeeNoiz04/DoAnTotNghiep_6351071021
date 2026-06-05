using QuoteFlow.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.Materials.MaterialStocks.MaterialStockLockShipments;

public class EfCoreMaterialStockLockShipmentRepository : EfCoreRepository<QuoteFlowDbContext, MaterialStockLockShipment, Guid>, IMaterialStockLockShipmentRepository
{
    public EfCoreMaterialStockLockShipmentRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<MaterialStockLockShipment>> GetListAsync(
        string? filterText = null,
        string? golfaCode = null,
        int? lockOnOrderMin = null,
        int? lockOnOrderMax = null,
        int? stockOnOrderMin = null,
        int? stockOnOrderMax = null,
        string? note = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter(await GetQueryableAsync(), filterText, golfaCode, lockOnOrderMin, lockOnOrderMax, stockOnOrderMin, stockOnOrderMax, note);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? MaterialStockLockShipmentConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListNoLockAsync(dbContext, cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        string? filterText = null,
        string? golfaCode = null,
        int? lockOnOrderMin = null,
        int? lockOnOrderMax = null,
        int? stockOnOrderMin = null,
        int? stockOnOrderMax = null,
        string? note = null,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter(await GetDbSetAsync(), filterText, golfaCode, lockOnOrderMin, lockOnOrderMax, stockOnOrderMin, stockOnOrderMax, note);
        return await query.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<MaterialStockLockShipment> ApplyFilter(
        IQueryable<MaterialStockLockShipment> query,
        string? filterText = null,
        string? golfaCode = null,
        int? lockOnOrderMin = null,
        int? lockOnOrderMax = null,
        int? stockOnOrderMin = null,
        int? stockOnOrderMax = null,
        string? note = null)
    {
        return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.GolfaCode!.Contains(filterText!) || e.Note!.Contains(filterText!))
                .WhereIf(!string.IsNullOrWhiteSpace(golfaCode), e => e.GolfaCode.Contains(golfaCode))
                .WhereIf(lockOnOrderMin.HasValue, e => e.LockOnOrder >= lockOnOrderMin!.Value)
                .WhereIf(lockOnOrderMax.HasValue, e => e.LockOnOrder <= lockOnOrderMax!.Value)
                .WhereIf(stockOnOrderMin.HasValue, e => e.StockOnOrder >= stockOnOrderMin!.Value)
                .WhereIf(stockOnOrderMax.HasValue, e => e.StockOnOrder <= stockOnOrderMax!.Value)
                .WhereIf(!string.IsNullOrWhiteSpace(note), e => e.Note.Contains(note));
    }
}