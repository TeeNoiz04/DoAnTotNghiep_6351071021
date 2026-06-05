using QuoteFlow.SaleOrders;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.DPOs.DPODetails;

public interface IDPODetailRepository : IRepository<DPODetail, Guid>
{
    Task<List<DPODetail>> GetListAsync(
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
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
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
        CancellationToken cancellationToken = default);

    Task LockStockAsync(
        Guid dpoDetailId,
        string golfaCode,
        Guid stockCategoryId,
        int lockQty,

        string userName,
        string userFullName,
        string? note = null,
        CancellationToken cancellationToken = default);

    Task<List<SaleOrderListModalDPO>> GetSaleOrderListModalDPOAsync(
       Guid dpoDetailId,
       CancellationToken cancellationToken = default);

    Task<List<SaleOrderListModalDelivery>> GetSaleOrderListModalDeliveryAsync(
       Guid dpoDetailId,
       CancellationToken cancellationToken = default);
}