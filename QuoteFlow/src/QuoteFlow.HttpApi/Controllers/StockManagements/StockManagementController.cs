using Asp.Versioning;
using QuoteFlow.HistoryTrackings;
using QuoteFlow.Materials;
using QuoteFlow.Materials.MaterialStocks;
using QuoteFlow.MaterialStockUploadDetails;
using QuoteFlow.Shared.Excels;
using QuoteFlow.StockManagements;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace QuoteFlow.Controllers.StockManagements;

[RemoteService]
[Area("app")]
[ControllerName("StockManagement")]
[Route("api/app/stock-managements")]
public class StockManagementController : AbpController, IStockManagementAppService
{
    protected IStockManagementAppService _stockManagementAppService;
    public StockManagementController(IStockManagementAppService stockManagementAppService)
    {
        _stockManagementAppService = stockManagementAppService;
    }

    [HttpGet]

    public Task<PagedResultDto<MaterialStockDto>> GetListAsync(GetMaterialStocksInput input)
    {
        return _stockManagementAppService.GetListAsync(input);
    }
    [HttpGet]
    [Route("upload")]
    public Task<PagedResultDto<StockManagementUploadDto>> GetListUploadAsync(GetStockManagementApprovalsInput input)
    {
        return _stockManagementAppService.GetListUploadAsync(input);
    }

    [HttpGet]
    [Route("{id}/upload-details")]
    public Task<StockManagementUploadDto> GetUploadDetailAsync(Guid id)
    {
        return _stockManagementAppService.GetUploadDetailAsync(id);
    }

    [HttpGet]
    [Route("stock-histories")]
    public Task<ListResultDto<HistoryTrackingDto>> GetStockHistoryAsync(GetStockHistoriesInput input)
    {
        return _stockManagementAppService.GetStockHistoryAsync(input);
    }

    [HttpGet]
    [Route("stock-histories/export")]
    public Task<IRemoteStreamContent> GetStockHistoryAsExcelAsync(GetStockHistoriesInput input)
    {
        return _stockManagementAppService.GetStockHistoryAsExcelAsync(input);
    }

    [HttpPost]
    [Route("import-stock-inventory")]
    public Task ImportMaterialStockInventoryAsync(ExcelValidationResult<MaterialStockUploadDetailImportInventoryDto> data, string? note)
    {
        return _stockManagementAppService.ImportMaterialStockInventoryAsync(data, note);
    }

    [HttpPost]
    [Route("import-stock-transfer")]
    public Task ImportMaterialStockTransferAsync(ExcelValidationResult<MaterialStockUploadDetailImportTransferDto> data, string? note)
    {
        return _stockManagementAppService.ImportMaterialStockTransferAsync(data, note);
    }
    [HttpPost]
    [Route("validate-parse-stock-inventory")]
    public Task<ExcelValidationResult<MaterialStockUploadDetailImportInventoryDto>> ValidateAndParseStockInventoryAsync(IRemoteStreamContent file)
    {
        return _stockManagementAppService.ValidateAndParseStockInventoryAsync(file);
    }

    [HttpPost]
    [Route("validate-parse-stock-transfer")]
    public Task<ExcelValidationResult<MaterialStockUploadDetailImportTransferDto>> ValidateAndParseStockTransferAsync(IRemoteStreamContent file)
    {
        return _stockManagementAppService.ValidateAndParseStockTransferAsync(file);
    }

    [HttpGet]
    [Route("stock-management-list")]
    public Task<PagedResultDto<StockManagementListDto>> GetListStockManagementAsync(GetStockManagementsListInput input)
    {
        return _stockManagementAppService.GetListStockManagementAsync(input);
    }
    [HttpGet]
    [Route("{golfaCode}")]
    public Task<MaterialDto> GetAsync(string golfaCode)
    {
        return _stockManagementAppService.GetAsync(golfaCode);
    }


    [HttpGet]
    [Route("as-file-excel")]
    public Task<IRemoteStreamContent> GetListStockManagementExcelAsync(GetStockManagementsListInput input)
    {
        return _stockManagementAppService.GetListStockManagementExcelAsync(input);
    }

    [HttpGet("stock-qty")]
    public Task<List<StockQtyDto>> GetStockQtyAsync(string? materialCode, Guid? stockId)
    {
        return _stockManagementAppService.GetStockQtyAsync(materialCode, stockId);
    }

    [HttpGet("locked")]
    public Task<List<LockedDto>> GetLockedAsync(string? materialCode)
    {
        return _stockManagementAppService.GetLockedAsync(materialCode);
    }

    [HttpGet("stock-of-so")]
    public Task<List<StockOfSODto>> GetStockOfSOAsync(string? materialCode, Guid? stockId)
    {
        return _stockManagementAppService.GetStockOfSOAsync(materialCode, stockId);
    }

    [HttpGet("lock-shipment")]
    public Task<List<LockShipmentDto>> GetLockShipmentAsync(string? materialCode)
    {
        return _stockManagementAppService.GetLockShipmentAsync(materialCode);
    }

    [HttpGet("on-order-stock")]
    public Task<List<OnOrderStockDto>> GetOnOrderStockAsync(string? materialCode)
    {
        return _stockManagementAppService.GetOnOrderStockAsync(materialCode);
    }

    [HttpGet]
    [Route("report")]
    public Task<IRemoteStreamContent> GetListOverallStockReportAsync()
    {
        return _stockManagementAppService.GetListOverallStockReportAsync();
    }
    [HttpGet]
    [Route("data-report")]
    public Task<List<DataMaterialOverallStockReportDto>> GeDataOverallStockReportAsync()
    {
        return _stockManagementAppService.GeDataOverallStockReportAsync();
    }
    [HttpGet]
    [Route("inventory-report")]
    public Task<PagedResultDto<InventoryReportDto>> GetListInventoryReportAsync(GetInventoryReportsInput input)
    {
        return _stockManagementAppService.GetListInventoryReportAsync(input);
    }

    [HttpGet]
    [Route("inventory-report-excel")]
    public Task<IRemoteStreamContent> GetListExcelInventoryReportAsync(GetInventoryReportsInput input)
    {
        return _stockManagementAppService.GetListExcelInventoryReportAsync(input);
    }
}
