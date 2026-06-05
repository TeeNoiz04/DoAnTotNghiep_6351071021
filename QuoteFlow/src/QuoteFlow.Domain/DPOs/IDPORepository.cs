using QuoteFlow.DPOs.Models;
using QuoteFlow.DPOs.ParameterObjects;
using QuoteFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.DPOs;

public interface IDPORepository : IRepository<DPO, Guid>
{
    Task<List<DPO>> GetListAsync(
        DPOFilterParams filterParams,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );

    Task<List<DPO>> GetListGICAsync(
       GICFilterParams filterParams,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );

    Task<List<DPO>> GetListGKRAsync(
        GKRFilterParams filterParams,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        DPOFilterParams filterParams,
        CancellationToken cancellationToken = default);

    Task LockStockAutoAsync(
        Guid dpoId,
        Guid stockCategoryId,
        string userName,
        string userFullName,
        CancellationToken cancellationToken = default);

    Task LockStockAutoV2Async(
        List<Guid> dpoDetailIds,
        Guid stockCategoryId,
        string userName,
        string userFullName,
        CancellationToken cancellationToken = default);

    Task LockShipmentAutoAsync(
        List<Guid> dpoDetailIds,
        string? note,
        string userName,
        string userFullName,
        CancellationToken cancellationToken = default);

    Task<List<DPOListPOsModel>> GetListAvailablePOsAsync(
        Guid dpoDetailId,
        string? materialCode,
        CancellationToken cancellationToken = default);

    Task<string?> LockShipmentAsync(
        Guid poDetailId,
        Guid dpoDetailId,
        string golfaCode,
        int qty,
        string? note,
        string userName,
        string userFullName,
        CancellationToken cancellationToken = default);

    Task<string?> UpdateLockShipmentAsync(
        Guid poDetailId,
        Guid dpoDetailId,
        string golfaCode,
        int qty,
        string? note,
        string userName,
        string userFullName,
        CancellationToken cancellationToken = default);

    Task<List<DPOLockStockEtaEtdModel>> GetListLockStockEtaEtdAsync(
        Guid dpoDetailId,
        Guid poDetailId,
        CancellationToken cancellationToken = default);

    Task<string?> DeleteDPOLockStockAsync(Guid dpoDetailId, Guid lockStockId, string userName, string userFullName);

    Task<string?> DeleteLockOnOrderStockAsync(Guid poDetailId, Guid dpoDetailId, string userName, string userFullName);

    Task<string?> UpdateLockStockAsync(
        Guid dpoDetailId,
        string golfaCode,
        Guid stockCategoryId,
        int lockQty,
        string? note,
        string userName,
        string userFullName,
        CancellationToken cancellationToken = default);

    // GIC-specific methods (no lock stock functionality)
    Task<long> GetGICCountAsync(
        GICFilterParams filterParams,
        CancellationToken cancellationToken = default);

    Task<long> GetGKRCountAsync(
        GKRFilterParams filterParams,
        CancellationToken cancellationToken = default);

    Task<List<DPO>> GetListPendingAsync(
        GKRFilterParams filterParams,
        string approverUsername,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default);

    Task<List<StatusCount>> GetGroupedCountAsync(
        DPOFilterParams filterParams,
        CancellationToken cancellationToken = default);

    Task<List<StatusCount>> GetGICGroupedCountAsync(
        GICFilterParams filterParams,
        CancellationToken cancellationToken = default);

    Task<List<StatusCount>> GetGKRGroupedCountAsync(
        GKRFilterParams filterParams,
        CancellationToken cancellationToken = default);

    Task<List<DPOReportDto>> GetListDPOReportAsync(Guid? buyerTypeId, Guid? buyerId, DateTime fromDate, DateTime toDate, bool hasFullBuyerAccess, string userName);
    Task<List<DPOProcessingReport>> GetListDPOProcessingReportAsync(Guid? buyerTypeId, Guid? buyerId, DateTime fromDate, DateTime toDate, bool hasFullBuyerAccess, string userName);
    Task<List<DPOMessage>> GetListMessagesAsync(Guid dpoId, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default);
    Task<long> GetCountMessagesAsync(Guid dpoId, CancellationToken cancellationToken = default);
    Task<DPO> GetWithDetailsAsync(Guid id, System.Linq.Expressions.Expression<System.Func<DPO, object>>? include = null, CancellationToken cancellationToken = default);
    Task<DPO> GetWithDetailsNoTrackingAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<DPOExportDataModel>> GetExportDataAsync(
        string? dpoNo,
        string? status,
        string? golfaCode,
        string? model,
        string? poNo,
        string? customerName,
        DateTime? fromDate,
        DateTime? toDate,
        string? buyerTypeId,
        string? buyerId,
        string? materialType,
        string? supplierCode,
        string? spoCode,
        string? taxCode,
        string? materialGroup,
        string? userName,
        bool fullBuyerPermission,
        CancellationToken cancellationToken = default);
    Task<string> GenerateGICNoAsync(
        string materialType,
        string gicTypeCode,
        string? gicProcessCode = null,
        string? buyerShortName = null,
        CancellationToken cancellationToken = default);

    Task<List<GICInternalUseModel>> GetListGICInternalUseAsync(
        GICFilterParams filterParams,
        CancellationToken cancellationToken = default
    );
    Task<List<GICFOCModel>> GetListGICFOCAsync(
        GICFilterParams filterParams,
        CancellationToken cancellationToken = default
    );
    Task<List<GICWarrantyModel>> GetListGICWarrantyAsync(
        GICFilterParams filterParams,
        CancellationToken cancellationToken = default
    );

    Task<string?> UpdateDPODetailAsync(
        Guid dpoDetailId,
        int qty,
        string? confirmNote,
        string? note,
        string userName,
        string userFullName,
        CancellationToken cancellationToken = default);
    Task<string?> UpdateDPODetailGKRAsync(
        Guid dpoDetailId,
        int qty,
        string? confirmNote,
        string? note,
        string userName,
        string userFullName,
        CancellationToken cancellationToken = default);

    Task<string?> AllocateGkrToDpoAsync(Guid dpoId, Guid gkrId, string? note, string userName, string userFullName);
    Task GenerateApprovalRouteAsync(Guid gkrId);
    Task<List<GKRApprovalRoute>> GetListApprovalRoutesAsync(Guid gkrId);
    Task<List<GKRExportModel>> GetExportGKRDataAsync(
       string? dpoNo,
       string? status,
       string? golfaCode,
       string? model,
       string? poNo,
       string? customerName,
       DateTime? fromDate,
       DateTime? toDate,
       string? buyerTypeId,
       string? buyerId,
       string? materialType,
       string? supplierCode,
       string? spoCode,
       string? taxCode,
       string? materialGroup,
       string? userName,
       bool fullBuyerPermission,
       CancellationToken cancellationToken = default);

    Task<List<GICExportModel>> GetExportGICDataAsync(
    string? gicNo,
    string? status,
    string? golfaCode,
    string? modelName,
    string? costCenter,
    string? gicType,
    string? gicProcess,
    DateTime? fromDate,
    DateTime? toDate,
    string? buyerTypeId,
    string? buyerId,
    string? materialType,
    string? userName,
    bool fullBuyerPermission,
    CancellationToken cancellationToken = default);
}