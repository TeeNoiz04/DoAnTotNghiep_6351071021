using QuoteFlow.EntityFrameworkCore;
using QuoteFlow.SaleOrderDetails;
using QuoteFlow.SaleOrders.SaleOrderDetails.ParameterObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.SaleOrders.SaleOrderDetails;

public class EfCoreSaleOrderDetailRepository : EfCoreRepository<QuoteFlowDbContext, SaleOrderDetail, Guid>, ISaleOrderDetailRepository
{
    public EfCoreSaleOrderDetailRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<SaleOrderDetail>> GetListAsync(
     SaleOrderDetailFilterParams filterParams,
     CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter(await GetQueryableAsync(), filterParams);
        query = query.OrderBy(string.IsNullOrWhiteSpace(filterParams.Sorting)
            ? SaleOrderDetailConsts.GetDefaultSorting(false)
            : filterParams.Sorting);

        return await query
            .PageBy(filterParams.SkipCount, filterParams.MaxResultCount)
            .ToListNoLockAsync(dbContext, cancellationToken);
    }
    public virtual async Task<List<T>> GetListAsync<T>(
       SaleOrderDetailFilterParams filterParams,
       Expression<Func<SaleOrderDetail, T>> selector,
       CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var query = ApplyFilter(await GetQueryableAsync(), filterParams);

        query = query.OrderBy(string.IsNullOrWhiteSpace(filterParams.Sorting)
            ? SaleOrderDetailConsts.GetDefaultSorting(false)
            : filterParams.Sorting);
        var resultQuery = query
            .PageBy(filterParams.SkipCount, filterParams.MaxResultCount)
            .Select(selector).AsQueryable();

        var result = await resultQuery.ToListAsync(cancellationToken);

        return result;
    }

    public virtual async Task<long> GetCountAsync(
     SaleOrderDetailFilterParams filterParams,
     CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter(await GetDbSetAsync(), filterParams);
        return await query.CountNoLockAsync(dbContext, cancellationToken);
    }


    protected virtual IQueryable<SaleOrderDetail> ApplyFilter(
    IQueryable<SaleOrderDetail> query,
    SaleOrderDetailFilterParams filter)
    {
        return query
            .WhereIf(!string.IsNullOrWhiteSpace(filter.StatusCode), e => e.StatusCode!.Contains(filter.StatusCode!))
            .WhereIf(!string.IsNullOrWhiteSpace(filter.GolfaCode), e => e.GolfaCode!.Contains(filter.GolfaCode!))
            .WhereIf(!string.IsNullOrWhiteSpace(filter.Note), e => e.Note!.Contains(filter.Note!))
            .WhereIf(filter.SaleOrderId.HasValue, e => e.SaleOrderId == filter.SaleOrderId)
            .WhereIf(filter.DPODetailId.HasValue, e => e.DPODetailId == filter.DPODetailId)
            .WhereIf(filter.QtyMin.HasValue, e => e.Qty >= filter.QtyMin.Value)
            .WhereIf(filter.QtyMax.HasValue, e => e.Qty <= filter.QtyMax.Value)
            .WhereIf(filter.PriceMin.HasValue, e => e.Price >= filter.PriceMin.Value)
            .WhereIf(filter.PriceMax.HasValue, e => e.Price <= filter.PriceMax.Value)
            .WhereIf(filter.AmountMin.HasValue, e => e.Amount >= filter.AmountMin.Value)
            .WhereIf(filter.AmountMax.HasValue, e => e.Amount <= filter.AmountMax.Value)
            .WhereIf(filter.VATMin.HasValue, e => e.VAT >= filter.VATMin.Value)
            .WhereIf(filter.VATMax.HasValue, e => e.VAT <= filter.VATMax.Value)
            .WhereIf(filter.StockCategoryId.HasValue, e => e.StockCategoryId == filter.StockCategoryId)
            .WhereIf(filter.LockStockId.HasValue, e => e.LockStockId == filter.LockStockId)
            .Where(x => x.IsDeleted == false || x.IsDeleted == null);
    }

    public override async Task<SaleOrderDetail> GetAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = await GetQueryableAsync();
        var result = await query
            .Include(x => x.DPODetail)
            .Where(x => x.Id == id)
            .FirstOrDefaultNoLockAsync(dbContext, GetCancellationToken(cancellationToken))
            ?? throw new EntityNotFoundException(typeof(SaleOrder), id);

        return result;
    }


}