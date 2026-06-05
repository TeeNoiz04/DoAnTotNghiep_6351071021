using QuoteFlow.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.Materials.MaterialStocks.MaterialStockLockStocks;

public class EfCoreMaterialStockLockStockRepository : EfCoreRepository<QuoteFlowDbContext, MaterialStockLockStock, Guid>, IMaterialStockLockStockRepository
{
    public EfCoreMaterialStockLockStockRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<MaterialStockLockStock>> GetListAsync(
        string? filterText = null,
        string? golfaCode = null,
        Guid? dPODetailId = null,
        Guid? stockCategoryId = null,
        int? qtyMin = null,
        int? qtyMax = null,
        string? note = null,
        int? releasedLockMin = null,
        int? releasedLockMax = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter(await GetQueryableAsync(), filterText, golfaCode, dPODetailId, stockCategoryId, qtyMin, qtyMax, note, releasedLockMin, releasedLockMax);
        query = query
            .Include(e => e.StockCategory)
            .OrderBy(string.IsNullOrWhiteSpace(sorting) ? MaterialStockLockStockConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListNoLockAsync(dbContext, cancellationToken);
    }

    public override async Task<MaterialStockLockStock> GetAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = await GetQueryableAsync();
        query = query.Include(e => e.StockCategory).Where(x => x.Id == id);
        var result = await query.FirstOrDefaultNoLockAsync(dbContext, GetCancellationToken(cancellationToken))
            ?? throw new EntityNotFoundException(typeof(MaterialStockLockStock), id);

        return result;
    }

    public virtual async Task<long> GetCountAsync(
        string? filterText = null,
        string? golfaCode = null,
        Guid? dPODetailId = null,
        Guid? stockCategoryId = null,
        int? qtyMin = null,
        int? qtyMax = null,
        string? note = null,
        int? releasedLockMin = null,
        int? releasedLockMax = null,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter(await GetDbSetAsync(), filterText, golfaCode, dPODetailId, stockCategoryId, qtyMin, qtyMax, note, releasedLockMin, releasedLockMax);
        return await query.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }

    public virtual async Task<List<MaterialStockLockStock>> GetUnlockableByDetailIdAsync(
        Guid dpoDetailId,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = (await GetQueryableAsync())
            .Where(x => x.DPODetailId == dpoDetailId && x.ReleasedLock == 0)
            .Include(e => e.StockCategory);

        return await query.ToListNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<MaterialStockLockStock> ApplyFilter(
        IQueryable<MaterialStockLockStock> query,
        string? filterText = null,
        string? golfaCode = null,
        Guid? dPODetailId = null,
        Guid? stockCategoryId = null,
        int? qtyMin = null,
        int? qtyMax = null,
        string? note = null,
        int? releasedLockMin = null,
        int? releasedLockMax = null)
    {
        return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.GolfaCode!.Contains(filterText!) || e.Note!.Contains(filterText!))
                .WhereIf(!string.IsNullOrWhiteSpace(golfaCode), e => e.GolfaCode.Contains(golfaCode))
                .WhereIf(dPODetailId.HasValue, e => e.DPODetailId == dPODetailId)
                .WhereIf(stockCategoryId.HasValue, e => e.StockCategoryId == stockCategoryId)
                .WhereIf(qtyMin.HasValue, e => e.Qty >= qtyMin!.Value)
                .WhereIf(qtyMax.HasValue, e => e.Qty <= qtyMax!.Value)
                .WhereIf(!string.IsNullOrWhiteSpace(note), e => e.Note.Contains(note))
                .WhereIf(releasedLockMin.HasValue, e => e.ReleasedLock >= releasedLockMin!.Value)
                .WhereIf(releasedLockMax.HasValue, e => e.ReleasedLock <= releasedLockMax!.Value);
    }
}