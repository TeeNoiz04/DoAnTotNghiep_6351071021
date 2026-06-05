using QuoteFlow.SaleOrders.ParameterObjects;
using QuoteFlow.SaleOrders.SaleOrderDetails;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.SaleOrders;

public interface ISaleOrderRepository : IRepository<SaleOrder, Guid>
{
    Task<List<SaleOrder>> GetListAsync(
        SaleOrderFilterParams filterParams,
        CancellationToken cancellationToken = default
    );

    Task<List<T>> GetListAsync<T>(
       SaleOrderFilterParams filterParams,
       Expression<Func<SaleOrder, T>> selector,
       CancellationToken cancellationToken = default);

    Task<long> GetCountAsync(
       SaleOrderFilterParams filterParams,
        CancellationToken cancellationToken = default);

    Task<string?> GetLatestCodeAsync(string prefix);

    Task<List<SaleOrderListDetailDPO>> GetListAddDetailDPOAsync(
        SaleOrderGetListDetailDPOParams filterParams,
        CancellationToken cancellationToken = default);
    Task<long> GetListAddDetailDPOCountAsync(
        SaleOrderGetListDetailDPOParams filterParams,
        CancellationToken cancellationToken = default);
    Task<List<SaleOrderListDetailGIC>> GetListAddDetailGICAsync(
        SaleOrderGetListDetailGICParams filterParams,
        CancellationToken cancellationToken = default);

    Task<string?> ReOpenSO(Guid saleOrderId, string userName, string fullName);
    Task<string?> SaveSODetailAsync(List<SaleOrderAddedDetailDPOParams> added);
    Task<string?> DeleteSOAsync(Guid saleOrderId, string userName, string fullName);
    Task<string?> DeleteSODetailAsync(Guid id, string userName, string fullName);
    Task<string?> ImportSAPDataAsync(Guid importGuid, string userName, string userFullName);
    Task<string?> ImportSAPDataGICAsync(Guid importGuid, string userName, string userFullName);

    Task<string?> ImportInternalUseChangeDataGICAsync(Guid importGuid, string userName, string userFullName);
    Task<string?> ConfirmDelivery(Guid saleOrderId, string userName, string userFullName);

    Task<string?> ConfirmDeliveryGIC(Guid saleOrderId, string userName, string userFullName);
    //SaleOrderExportSAPGICData
    Task<List<SaleOrderExportSAPGICData>> ExportSAPGICDataAsync(SaleOrderListExportSAPDataParams input, bool? isExport = false);
    Task<List<SaleOrderListExportSAPData>> ExportSAPDataAsync(SaleOrderListExportSAPDataParams input);
    Task<string?> UpdateSODetailExtrafeeAsync(SODetailExtrafeeUpdateParams input);
    Task<List<SaleOrderExportData>> ExportSODataAsync(SaleOrderListExportSAPDataParams input);
    Task<string?> ImportSAPDataGICInternalUseAsync(Guid importGuid, string userName, string userFullName);
}