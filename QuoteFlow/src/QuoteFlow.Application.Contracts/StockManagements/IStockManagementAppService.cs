using QuoteFlow.HistoryTrackings;
using QuoteFlow.Materials;
using QuoteFlow.Materials.MaterialStocks;
using QuoteFlow.MaterialStockUploadDetails;
using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace QuoteFlow.StockManagements;
public interface IStockManagementAppService : IApplicationService
{
    Task<PagedResultDto<MaterialStockDto>> GetListAsync(GetMaterialStocksInput input);
    Task<PagedResultDto<StockManagementListDto>> GetListStockManagementAsync(GetStockManagementsListInput input);
    Task<IRemoteStreamContent> GetListStockManagementExcelAsync(GetStockManagementsListInput input);
    Task<PagedResultDto<StockManagementUploadDto>> GetListUploadAsync(GetStockManagementApprovalsInput input);

    Task<StockManagementUploadDto> GetUploadDetailAsync(Guid id);
    Task ImportMaterialStockTransferAsync(ExcelValidationResult<MaterialStockUploadDetailImportTransferDto> data, string? note);
    Task<ExcelValidationResult<MaterialStockUploadDetailImportTransferDto>> ValidateAndParseStockTransferAsync(IRemoteStreamContent file);

    Task<ExcelValidationResult<MaterialStockUploadDetailImportInventoryDto>> ValidateAndParseStockInventoryAsync(IRemoteStreamContent file);

    Task ImportMaterialStockInventoryAsync(ExcelValidationResult<MaterialStockUploadDetailImportInventoryDto> data, string? note);
    Task<MaterialDto> GetAsync(string golfaCode);

    Task<List<StockQtyDto>> GetStockQtyAsync(string? materialCode, Guid? stockId);

    Task<List<LockedDto>> GetLockedAsync(string? materialCode);
    Task<List<StockOfSODto>> GetStockOfSOAsync(string? materialCode, Guid? stockId);
    Task<List<LockShipmentDto>> GetLockShipmentAsync(string? materialCode);
    Task<List<OnOrderStockDto>> GetOnOrderStockAsync(string? materialCode);
    Task<IRemoteStreamContent> GetListOverallStockReportAsync();
    Task<List<DataMaterialOverallStockReportDto>> GeDataOverallStockReportAsync();

    Task<PagedResultDto<InventoryReportDto>> GetListInventoryReportAsync(GetInventoryReportsInput input);

    Task<IRemoteStreamContent> GetListExcelInventoryReportAsync(GetInventoryReportsInput input);

    Task<ListResultDto<HistoryTrackingDto>> GetStockHistoryAsync(GetStockHistoriesInput input);

    Task<IRemoteStreamContent> GetStockHistoryAsExcelAsync(GetStockHistoriesInput input);
}
