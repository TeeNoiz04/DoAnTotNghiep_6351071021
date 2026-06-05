using QuoteFlow.Materials.ParameterObjects;
using QuoteFlow.StockManagements;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.Materials;

public interface IMaterialRepository : IRepository<Material, Guid>
{
    Task<List<Material>> GetListAsync(
        MaterialFilterParams filterParams,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        MaterialFilterParams filterParams,
        CancellationToken cancellationToken = default);

    Task<List<T>> GetListAsync<T>(
        MaterialFilterParams filterParams,
        Expression<Func<Material, T>> selector,
        CancellationToken cancellationToken = default);
    //GetListWithDeactiveAsync
    Task<List<T>> GetListWithDeactiveAsync<T>(
        MaterialFilterParams filterParams,
        Expression<Func<Material, T>> selector,
        CancellationToken cancellationToken = default);

    Task<List<StockManagementList>> GetStockManagementListAsync(
    StockManagementFilterParams filterParams,
     bool? exportExcel = null,
    CancellationToken cancellationToken = default);

    Task<Material> GetWithDetailAsync(string golfaCode, CancellationToken cancellationToken = default);

    Task<List<StockQty>> StockQtyAsync(string? materialCode, Guid? stockId);

    Task<List<Locked>> LockedAsync(string? materialCode);
    Task<List<StockOfSO>> StockOfSOAsync(string? materialCode, Guid? stockId);
    Task<List<LockShipment>> GetLockShipmentAsync(string? materialCode);
    Task<List<OnOrderStock>> OnOrderStockAsync(string? materialCode);

    Task<List<MaterialOverallStockReport>> GetListStockOverallAsync();
    Task<string?> ValidationInventoryAsync(
       Guid requestId,
       string userName,
       string userFullName,
       CancellationToken cancellationToken = default);

    Task<string?> ValidationTransferAsync(
       Guid requestId,
       string userName,
       string userFullName,
       CancellationToken cancellationToken = default);

    Task<List<InventoryReport>> GetInventoryReportAsync(
        ExcelInventoryReportFilterParams filterParams);

    Task<long> GetCountInventoryReportAsync(
        ExcelInventoryReportFilterParams filterParams);
    Task UpdateStatusAsync(Guid materialApprovalId);

    /// <summary>
    /// Bulk update material factory fields using SqlBulkCopy + MERGE for optimal performance.
    /// Uses ROWLOCK hint to prevent lock escalation.
    /// </summary>
    Task BulkUpdateFactoryAsync(List<ExcelMaterialFactoryUpdateParams> updates, CancellationToken cancellationToken = default);
    Task BulkUpdateMaterialWithOutPriceAsync(List<ExcelMaterialUpdateWithoutPrriceParams> updates, CancellationToken cancellationToken = default);
}