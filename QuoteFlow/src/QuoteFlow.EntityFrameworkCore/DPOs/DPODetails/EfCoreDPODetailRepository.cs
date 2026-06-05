using Dapper;
using QuoteFlow.EntityFrameworkCore;
using QuoteFlow.SaleOrders;
using QuoteFlow.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.DPOs.DPODetails;

public class EfCoreDPODetailRepository : EfCoreRepository<QuoteFlowDbContext, DPODetail, Guid>, IDPODetailRepository
{
    public EfCoreDPODetailRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<DPODetail>> GetListAsync(
        string? filterText = null,
        Guid? dPOId = null,
        string? status = null,
        string? golfaCode = null,
        string? model = null,
        string? spec1 = null,
        string? spec2 = null,
        int? qtyMin = null,
        int? qtyMax = null,
        decimal? unitPriceMin = null,
        decimal? unitPriceMax = null,
        decimal? amountMin = null,
        decimal? amountMax = null,
        DateTime? requestedETAMin = null,
        DateTime? requestedETAMax = null,
        Guid? sPOId = null,
        string? sPOCode = null,
        string? customerTaxCode = null,
        string? customerName = null,
        int? lockStockMin = null,
        int? lockStockMax = null,
        int? lockStockSOMin = null,
        int? lockStockSOMax = null,
        int? lockShipmentMin = null,
        int? lockShipmentMax = null,
        int? deliveredMin = null,
        int? deliveredMax = null,
        int? needDeliveryMin = null,
        int? needDeliveryMax = null,
        string? note = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter(await GetQueryableAsync(), filterText, dPOId, status, golfaCode, model, spec1, spec2, qtyMin, qtyMax, unitPriceMin, unitPriceMax, amountMin, amountMax, requestedETAMin, requestedETAMax, sPOId, sPOCode, customerTaxCode, customerName, lockStockMin, lockStockMax, lockStockSOMin, lockStockSOMax, lockShipmentMin, lockShipmentMax, deliveredMin, deliveredMax, needDeliveryMin, needDeliveryMax, note);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? DPODetailConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListNoLockAsync(dbContext, cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        string? filterText = null,
        Guid? dPOId = null,
        string? status = null,
        string? golfaCode = null,
        string? model = null,
        string? spec1 = null,
        string? spec2 = null,
        int? qtyMin = null,
        int? qtyMax = null,
        decimal? unitPriceMin = null,
        decimal? unitPriceMax = null,
        decimal? amountMin = null,
        decimal? amountMax = null,
        DateTime? requestedETAMin = null,
        DateTime? requestedETAMax = null,
        Guid? sPOId = null,
        string? sPOCode = null,
        string? customerTaxCode = null,
        string? customerName = null,
        int? lockStockMin = null,
        int? lockStockMax = null,
        int? lockStockSOMin = null,
        int? lockStockSOMax = null,
        int? lockShipmentMin = null,
        int? lockShipmentMax = null,
        int? deliveredMin = null,
        int? deliveredMax = null,
        int? needDeliveryMin = null,
        int? needDeliveryMax = null,
        string? note = null,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter(await GetDbSetAsync(), filterText, dPOId, status, golfaCode, model, spec1, spec2, qtyMin, qtyMax, unitPriceMin, unitPriceMax, amountMin, amountMax, requestedETAMin, requestedETAMax, sPOId, sPOCode, customerTaxCode, customerName, lockStockMin, lockStockMax, lockStockSOMin, lockStockSOMax, lockShipmentMin, lockShipmentMax, deliveredMin, deliveredMax, needDeliveryMin, needDeliveryMax, note);
        return await query.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<DPODetail> ApplyFilter(
        IQueryable<DPODetail> query,
        string? filterText = null,
        Guid? dPOId = null,
        string? status = null,
        string? golfaCode = null,
        string? model = null,
        string? spec1 = null,
        string? spec2 = null,
        int? qtyMin = null,
        int? qtyMax = null,
        decimal? unitPriceMin = null,
        decimal? unitPriceMax = null,
        decimal? amountMin = null,
        decimal? amountMax = null,
        DateTime? requestedETAMin = null,
        DateTime? requestedETAMax = null,
        Guid? sPOId = null,
        string? sPOCode = null,
        string? customerTaxCode = null,
        string? customerName = null,
        int? lockStockMin = null,
        int? lockStockMax = null,
        int? lockStockSOMin = null,
        int? lockStockSOMax = null,
        int? lockShipmentMin = null,
        int? lockShipmentMax = null,
        int? deliveredMin = null,
        int? deliveredMax = null,
        int? needDeliveryMin = null,
        int? needDeliveryMax = null,
        string? note = null)
    {
        return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Status!.Contains(filterText!) || e.GolfaCode!.Contains(filterText!) || e.Model!.Contains(filterText!) || e.Spec1!.Contains(filterText!) || e.Spec2!.Contains(filterText!) || e.SPOCode!.Contains(filterText!) || e.CustomerTaxCode!.Contains(filterText!) || e.CustomerName!.Contains(filterText!) || e.Note!.Contains(filterText!))
                .WhereIf(dPOId.HasValue, e => e.DPOId == dPOId)
                .WhereIf(!string.IsNullOrWhiteSpace(status), e => e.Status.Contains(status))
                .WhereIf(!string.IsNullOrWhiteSpace(golfaCode), e => e.GolfaCode.Contains(golfaCode))
                .WhereIf(!string.IsNullOrWhiteSpace(model), e => e.Model.Contains(model))
                .WhereIf(!string.IsNullOrWhiteSpace(spec1), e => e.Spec1.Contains(spec1))
                .WhereIf(!string.IsNullOrWhiteSpace(spec2), e => e.Spec2.Contains(spec2))
                .WhereIf(qtyMin.HasValue, e => e.Qty >= qtyMin!.Value)
                .WhereIf(qtyMax.HasValue, e => e.Qty <= qtyMax!.Value)
                .WhereIf(unitPriceMin.HasValue, e => e.UnitPrice >= unitPriceMin!.Value)
                .WhereIf(unitPriceMax.HasValue, e => e.UnitPrice <= unitPriceMax!.Value)
                .WhereIf(amountMin.HasValue, e => e.Amount >= amountMin!.Value)
                .WhereIf(amountMax.HasValue, e => e.Amount <= amountMax!.Value)
                .WhereIf(requestedETAMin.HasValue, e => e.RequestedETA >= requestedETAMin!.Value)
                .WhereIf(requestedETAMax.HasValue, e => e.RequestedETA <= requestedETAMax!.Value)
                .WhereIf(sPOId.HasValue, e => e.SPOId == sPOId)
                .WhereIf(!string.IsNullOrWhiteSpace(sPOCode), e => e.SPOCode.Contains(sPOCode))
                .WhereIf(!string.IsNullOrWhiteSpace(customerTaxCode), e => e.CustomerTaxCode.Contains(customerTaxCode))
                .WhereIf(!string.IsNullOrWhiteSpace(customerName), e => e.CustomerName.Contains(customerName))
                .WhereIf(lockStockMin.HasValue, e => e.LockStock >= lockStockMin!.Value)
                .WhereIf(lockStockMax.HasValue, e => e.LockStock <= lockStockMax!.Value)
                .WhereIf(lockStockSOMin.HasValue, e => e.LockStockSO >= lockStockSOMin!.Value)
                .WhereIf(lockStockSOMax.HasValue, e => e.LockStockSO <= lockStockSOMax!.Value)
                .WhereIf(lockShipmentMin.HasValue, e => e.LockShipment >= lockShipmentMin!.Value)
                .WhereIf(lockShipmentMax.HasValue, e => e.LockShipment <= lockShipmentMax!.Value)
                .WhereIf(deliveredMin.HasValue, e => e.Delivered >= deliveredMin!.Value)
                .WhereIf(deliveredMax.HasValue, e => e.Delivered <= deliveredMax!.Value)
                .WhereIf(needDeliveryMin.HasValue, e => e.NeedDelivery >= needDeliveryMin!.Value)
                .WhereIf(needDeliveryMax.HasValue, e => e.NeedDelivery <= needDeliveryMax!.Value)
                .WhereIf(!string.IsNullOrWhiteSpace(note), e => e.Note.Contains(note));
    }

    public virtual async Task LockStockAsync(
        Guid dpoDetailId,
        string golfaCode,
        Guid stockCategoryId,
        int lockQty,

        string userName,
        string userFullName,
        string? note = null,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new DynamicParameters();
        parameters.Add("@dpoDetailId", dpoDetailId, DbType.Guid);
        parameters.Add("@golfaCode", golfaCode, DbType.String, size: 50);
        parameters.Add("@stockCategoryId", stockCategoryId, DbType.Guid);
        parameters.Add("@lockQty", lockQty, DbType.Int32);
        parameters.Add("@note", note, DbType.String, size: 500);
        parameters.Add("@userName", userName, DbType.String, size: 50);
        parameters.Add("@userFullName", userFullName, DbType.String, size: 500);
        parameters.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

        await connection.ExecuteAsync(
            "usp_DPO_LockStock",
            parameters,
            commandType: CommandType.StoredProcedure,
            transaction: await GetDbTransactionAsync()
        );

        var errorMessage = parameters.Get<string>("@errMsg");
        if (!string.IsNullOrEmpty(errorMessage))
        {
            throw new UserFriendlyException(errorMessage);
        }
    }

    public virtual async Task<List<SaleOrderListModalDPO>> GetSaleOrderListModalDPOAsync(
    Guid dpoDetailId,
    CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var result = await (from so in dbContext.SaleOrders
                            join sod in dbContext.SaleOrderDetails on so.Id equals sod.SaleOrderId
                            join dpoDetail in dbContext.DPODetails on sod.DPODetailId equals dpoDetail.Id
                            join stock in dbContext.StockCategories on sod.StockCategoryId equals stock.Id into stockJoin
                            from stock in stockJoin.DefaultIfEmpty()

                            where dpoDetail.Id == dpoDetailId && (sod.IsDeleted == false || sod.IsDeleted == null)

                            select new SaleOrderListModalDPO
                            {
                                Status = sod.StatusCode,
                                SONo = so.SONo,
                                SOSAPNo = so.SOSAPNo,
                                DONo = so.SAPDONo,
                                StockName = stock.StockName,
                                Quantity = sod.Qty,
                                ModifiedBy = so.LastModifierName,
                                Modified = so.LastModificationTime
                            }).ToListNoLockAsync(dbContext, cancellationToken);

        return result;
    }

    public virtual async Task<List<SaleOrderListModalDelivery>> GetSaleOrderListModalDeliveryAsync(
    Guid dpoDetailId,
    CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var result = await (from so in dbContext.SaleOrders
                            join sod in dbContext.SaleOrderDetails on so.Id equals sod.SaleOrderId
                            join dpoDetail in dbContext.DPODetails on sod.DPODetailId equals dpoDetail.Id
                            join stock in dbContext.StockCategories on sod.StockCategoryId equals stock.Id into stockJoin

                            from stock in stockJoin.DefaultIfEmpty()

                            where dpoDetail.Id == dpoDetailId && sod.StatusCode == QuoteFlowStatuses.Closed && (sod.IsDeleted == false || sod.IsDeleted == null)

                            select new SaleOrderListModalDelivery
                            {

                                SONo = so.SONo,
                                SOSAPNo = so.SOSAPNo,
                                InvoiceNo = so.SAPInvoice,
                                InvoiceDate = so.SAPInvoiceDate,
                                StockName = stock.StockName,
                                Quantity = sod.Qty,
                                Note = sod.Note,
                                ModifiedBy = so.LastModifierName,
                                Modified = so.LastModificationTime
                            }).ToListNoLockAsync(dbContext, cancellationToken);

        return result;
    }

}