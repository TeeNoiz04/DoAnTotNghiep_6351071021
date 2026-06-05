using Asp.Versioning;
using QuoteFlow.DPOs;
using QuoteFlow.DPOs.DPODetails;
using QuoteFlow.DPOs.DPOMessages;
using QuoteFlow.Materials.MaterialStocks.MaterialStockLockStocks;
using QuoteFlow.Messages;
using QuoteFlow.SaleOrders;
using QuoteFlow.Shared;
using QuoteFlow.Shared.Excels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace QuoteFlow.Controllers.DPOs;

[RemoteService]
[Area("app")]
[ControllerName("DPO")]
[Route("api/app/dpos")]

public class DPOController : AbpController, IDPOsAppService
{
    protected IDPOsAppService _dPOsAppService;

    public DPOController(IDPOsAppService dPOsAppService)
    {
        _dPOsAppService = dPOsAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<DPODto>> GetListAsync(GetDPOsInput input)
    {
        return _dPOsAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<DPODto> GetAsync(Guid id)
    {
        return _dPOsAppService.GetAsync(id);
    }

    [HttpGet]
    [Route("{dpoId}/details/{detailId}/lock-stock")]
    public virtual Task<ListResultDto<MaterialStockLockStockDto>> GetLockStocksAsync(Guid dpoId, Guid detailId)
    {
        return _dPOsAppService.GetLockStocksAsync(dpoId, detailId);
    }

    [HttpPost]
    public virtual Task<DPODto> CreateAsync(DPOCreateDto input)
    {
        return _dPOsAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<DPODto> UpdateAsync(Guid id, DPOUpdateDto input)
    {
        return _dPOsAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _dPOsAppService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("as-excel-file")]
    public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(DPOExcelDownloadDto input)
    {
        return _dPOsAppService.GetListAsExcelFileAsync(input);
    }

    [HttpGet]
    [Route("download-token")]
    public virtual Task<QuoteFlow.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        return _dPOsAppService.GetDownloadTokenAsync();
    }

    [HttpPost]
    [Route("validate-and-parse/dpo")]
    public virtual Task<ExcelValidationResult<ImportDPODto>> ValidateAndParseDPOAsync(
        [FromForm] IRemoteStreamContent file,
        [FromForm] ImportDPOInput input)
    {
        return _dPOsAppService.ValidateAndParseDPOAsync(file, input);
    }

    [HttpPost]
    [Route("import/dpo")]
    public virtual Task<DPODto> ImportDPOAsync(ExcelValidationResult<ImportDPODto> data, [FromQuery] bool force = false)
    {
        return _dPOsAppService.ImportDPOAsync(data, force);
    }

    [HttpPost]
    [Route("confirm-note")]
    public virtual Task<List<DPODetailDto>> ConfirmNoteAsync(DPOConfirmNoteDto input)
    {
        return _dPOsAppService.ConfirmNoteAsync(input);
    }

    [HttpPost]
    [Route("add-extra-fee")]
    public virtual Task<List<DPODetailDto>> AddExtraFeeAsync(DPOAddExtraFeeDto input)
    {
        return _dPOsAppService.AddExtraFeeAsync(input);
    }

    [HttpPost]
    [Route("{id}/cancel")]
    public virtual Task<DPODto> CancelAsync(Guid id, DPOCancelDto input)
    {
        return _dPOsAppService.CancelAsync(id, input);
    }

    [HttpPost]
    [Route("lock-stock-auto")]
    [Route("lock-stock-auto-v1")] // Legacy endpoint for backward compatibility
    public virtual Task LockStockAutoAsync(DPOLockStockAutoDto input)
    {
        return _dPOsAppService.LockStockAutoAsync(input);
    }

    [HttpPost]
    [Route("lock-stock-auto-v2")]
    public virtual Task LockStockAutoV2Async(DPOLockStockAutoV2Dto input)
    {
        return _dPOsAppService.LockStockAutoV2Async(input);
    }

    [HttpPost]
    [Route("lock-shipment-auto")]
    public virtual Task LockShipmentAutoAsync(DPOLockShipmentAutoDto input)
    {
        return _dPOsAppService.LockShipmentAutoAsync(input);
    }

    [HttpPost]
    [Route("{id}/confirm-lock-stock")]
    public Task<DPODto> ConfirmLockStockAsync(Guid id, NoteMetadataDto input)
    {
        return _dPOsAppService.ConfirmLockStockAsync(id, input);
    }

    [HttpPost]
    [Route("{id}/confirm-lock-on-order")]
    public Task<DPODto> ConfirmLockOnOrderAsync(Guid id, NoteMetadataDto input)
    {
        return _dPOsAppService.ConfirmLockOnOrderAsync(id, input);
    }

    [HttpGet]
    [Route("{dpoId}/details/{dpoDetailId}/list-available-pos")]
    public Task<ListResultDto<DPOListPOsDto>> GetListAvailablePOsAsync(Guid dpoId, Guid dpoDetailId, [FromQuery] string? materialCode)
    {
        return _dPOsAppService.GetListAvailablePOsAsync(dpoId, dpoDetailId, materialCode);
    }

    [HttpGet]
    [Route("{dpoId}/details/{dpoDetailId}/list-lock-on-order-stock")]
    public Task<ListResultDto<DPOLockOnOrderStockDto>> GetListLockOnOrderStockAsync(Guid dpoId, Guid dpoDetailId)
    {
        return _dPOsAppService.GetListLockOnOrderStockAsync(dpoId, dpoDetailId);
    }

    [HttpPost]
    [Route("{dpoDetailId}/lock-shipment")]
    public Task LockShipmentAsync(Guid dpoDetailId, DPOLockShipmentDto input)
    {
        return _dPOsAppService.LockShipmentAsync(dpoDetailId, input);
    }

    [HttpGet]
    [Route("{dpoDetailId}/lock-stock-eta-etd/{poDetailId}")]
    public Task<ListResultDto<DPOLockStockEtaEtdDto>> GetListLockStockEtaEtdAsync(Guid dpoDetailId, Guid poDetailId)
    {
        return _dPOsAppService.GetListLockStockEtaEtdAsync(dpoDetailId, poDetailId);
    }

    [HttpPost]
    [Route("{dpoDetailId}/lock-stock-detail")]
    public Task UpdateLockStockDetailAsync(Guid dpoDetailId, DPODetailUpdateLockStockDto input)
    {
        return _dPOsAppService.UpdateLockStockDetailAsync(dpoDetailId, input);
    }

    [HttpPost]
    [Route("{dpoDetailId}/lock-shipment-item/{poDetailId}")]
    public Task UpdateLockShipmentAsync(Guid dpoDetailId, Guid poDetailId, DPOLockShipmentItemUpdateDto input)
    {
        return _dPOsAppService.UpdateLockShipmentAsync(dpoDetailId, poDetailId, input);
    }

    [HttpDelete]
    [Route("{dpoDetailId}/lock-stocks/{lockStockId}")]
    public virtual Task DeleteLockStockAsync(Guid dpoDetailId, Guid lockStockId)
    {
        return _dPOsAppService.DeleteLockStockAsync(dpoDetailId, lockStockId);
    }

    [HttpDelete]
    [Route("{dpoDetailId}/lock-on-order-stocks/{poDetailId}")]
    public virtual Task DeleteLockOnOrderStockAsync(Guid dpoDetailId, Guid poDetailId)
    {
        return _dPOsAppService.DeleteLockOnOrderStockAsync(dpoDetailId, poDetailId);
    }

    [HttpPost]
    [Route("{id}/approve")]
    public Task<DPODto> ApproveAsync(Guid id, NoteMetadataDto input)
    {
        return _dPOsAppService.ApproveAsync(id, input);
    }

    [HttpPost]
    [Route("{id}/reject")]
    public Task<DPODto> RejectAsync(Guid id, NoteMetadataDto input)
    {
        return _dPOsAppService.RejectAsync(id, input);
    }
    [HttpGet]
    [Route("report")]
    public Task<IRemoteStreamContent> GetListDPOReportAsync(GetDPOReportInputDto input)
    {
        return _dPOsAppService.GetListDPOReportAsync(input);
    }
    [HttpGet]
    [Route("report-r24")]
    public Task<IRemoteStreamContent> GetListDPOProcessingReportAsync(GetDPOReportInputDto input)
    {
        return _dPOsAppService.GetListDPOProcessingReportAsync(input);
    }
    [HttpGet]
    [Route("list-report-r24")]
    public Task<List<DPOProcessingReportDto>> GetDataDPOProcessingReportAsync(GetDPOReportInputDto input)
    {
        return _dPOsAppService.GetDataDPOProcessingReportAsync(input);
    }
    [HttpGet]
    [Route("data-report")]
    public Task<List<DPODataReportDto>> GetDataDPOReportAsync(GetDPOReportInputDto input)
    {
        return _dPOsAppService.GetDataDPOReportAsync(input);
    }

    [HttpGet]
    [Route("{dpoId}/messages")]
    public virtual Task<PagedResultDto<MessageDto>> GetListMessagesAsync(Guid dpoId, GetDPOMessagesInput input)
    {
        return _dPOsAppService.GetListMessagesAsync(dpoId, input);
    }

    [HttpPost]
    [Route("{dpoId}/messages")]
    public Task<MessageDto> SendMessageAsync(Guid dpoId, MessageCreateDto input)
    {
        return _dPOsAppService.SendMessageAsync(dpoId, input);
    }

    [HttpGet]
    [Route("export-data-excel")]
    public Task<IRemoteStreamContent> ExportDataAsExcelAsync(DPOExportDataInputDto input)
    {
        return _dPOsAppService.ExportDataAsExcelAsync(input);
    }

    //DPO Details
    [HttpGet]
    [Route("details")]
    public Task<PagedResultDto<DPODetailDto>> GetListDPODetailAsync(GetDPODetailsInput input)
    {
        return _dPOsAppService.GetListDPODetailAsync(input);
    }
    [HttpGet]
    [Route("details/{id}")]

    public Task<DPODetailDto> GetDPODetailAsync(Guid id)
    {
        return _dPOsAppService.GetDPODetailAsync(id);
    }
    [HttpDelete]
    [Route("details/{id}")]

    public Task DeleteDPODetailAsync(Guid id)
    {
        return _dPOsAppService.DeleteDPODetailAsync(id);
    }
    [HttpPost]
    [Route("details")]

    public Task<DPODetailDto> CreateDPODetailAsync(DPODetailCreateDto input)
    {
        return _dPOsAppService.CreateDPODetailAsync(input);
    }
    [HttpPut]
    [Route("details/{id}")]

    public Task<DPODetailDto> UpdateAsync(Guid id, DPODetailUpdateDto input)
    {
        return _dPOsAppService.UpdateAsync(id, input);
    }
    [HttpGet]
    [Route("details/as-excel-file")]

    public Task<IRemoteStreamContent> GetListAsExcelFileAsync(DPODetailExcelDownloadDto input)
    {
        return _dPOsAppService.GetListAsExcelFileAsync(input);
    }
    [HttpGet]
    [Route("details/download-token")]

    public Task<DownloadTokenResultDto> GetDownloadTokenDPODtailAsync()
    {
        return _dPOsAppService.GetDownloadTokenDPODtailAsync();
    }
    [HttpPost]
    [Route("details/{id}/lock-stock")]

    public Task LockStockAsync(Guid id, DPODetailLockStockDto input)
    {
        return _dPOsAppService.LockStockAsync(id, input);
    }
    [HttpGet]
    [Route("details/{dpoDetailId}/sale-order")]

    public Task<List<SaleOrderListModalDPODto>> GetListSaleOrderModalDPOAsync(Guid dpoDetailId)
    {
        return _dPOsAppService.GetListSaleOrderModalDPOAsync(dpoDetailId);
    }
    [HttpGet]
    [Route("details/{dpoDetailId}/delivery")]

    public Task<List<SaleOrderListModalDeliveryDto>> GetListSaleOrderModalDeliveryAsync(Guid dpoDetailId)
    {
        return _dPOsAppService.GetListSaleOrderModalDeliveryAsync(dpoDetailId);
    }

    [HttpPut]
    [Route("update-detail")]
    public Task UpdateDPODetailAsync(Guid dpoDetailId, int qty, string? confirmNote, string? note)
    {
        return _dPOsAppService.UpdateDPODetailAsync(dpoDetailId, qty, confirmNote, note);
    }

    [HttpGet]
    [Route("{dpoId}/gkr-allocations")]
    public virtual Task<List<DpoGkrAllocationDto>> GetGkrAllocationsAsync(Guid dpoId)
    {
        return _dPOsAppService.GetGkrAllocationsAsync(dpoId);
    }

    [HttpGet]
    [Route("{dpoId}/available-gkrs")]
    public virtual Task<List<DpoGkrAllocationDto>> GetAvailableGkrsForAllocationAsync(Guid dpoId)
    {
        return _dPOsAppService.GetAvailableGkrsForAllocationAsync(dpoId);
    }

    [HttpPost]
    [Route("allocate-gkr")]
    public virtual Task AllocateGkrToDpoAsync(DPOAllocateGKRDto input)
    {
        return _dPOsAppService.AllocateGkrToDpoAsync(input);
    }

    /// <summary>
    /// Batch auto-unlock stock with Server-Sent Events (SSE) progress streaming
    /// </summary>
    [HttpPost]
    [Route("details/batch-auto-unlock")]
    public virtual async Task BatchAutoUnlockStockAsync([FromBody] BatchAutoUnlockStockDto input)
    {
        // Set SSE headers
        Response.ContentType = "text/event-stream";
        Response.Headers.Add("Cache-Control", "no-cache");
        Response.Headers.Add("Connection", "keep-alive");

        // Stream progress events
        await foreach (var progressEvent in _dPOsAppService.BatchAutoUnlockStockAsync(input))
        {
            // Serialize event to JSON
            var json = JsonSerializer.Serialize(progressEvent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            // Write SSE format
            var eventType = progressEvent.EventType switch
            {
                "started" => "started",
                "complete" => "complete",
                _ => "progress"
            };

            var sseMessage = $"event: {eventType}\ndata: {json}\n\n";
            var bytes = Encoding.UTF8.GetBytes(sseMessage);

            await Response.Body.WriteAsync(bytes);
            await Response.Body.FlushAsync();
        }
    }

    [HttpPost]
    [Route("details/batch-auto-unlock-on-order-stock")]
    public virtual async Task BatchAutoUnlockOnOrderStockAsync([FromBody] BatchAutoUnlockStockDto input)
    {
        // Set SSE headers
        Response.ContentType = "text/event-stream";
        Response.Headers.Add("Cache-Control", "no-cache");
        Response.Headers.Add("Connection", "keep-alive");

        // Stream progress events
        await foreach (var progressEvent in _dPOsAppService.BatchAutoUnlockOnOrderStockAsync(input))
        {
            // Serialize event to JSON
            var json = JsonSerializer.Serialize(progressEvent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            // Write SSE format
            var eventType = progressEvent.EventType switch
            {
                "started" => "started",
                "complete" => "complete",
                _ => "progress"
            };

            var sseMessage = $"event: {eventType}\ndata: {json}\n\n";
            var bytes = Encoding.UTF8.GetBytes(sseMessage);

            await Response.Body.WriteAsync(bytes);
            await Response.Body.FlushAsync();
        }
    }

    // Interface implementation (stub - actual implementation handled above)
    IAsyncEnumerable<BatchUnlockProgressEventDto> IDPOsAppService.BatchAutoUnlockStockAsync(BatchAutoUnlockStockDto input)
    {
        return _dPOsAppService.BatchAutoUnlockStockAsync(input);
    }

    // Interface implementation (stub - actual implementation handled above)
    IAsyncEnumerable<BatchUnlockProgressEventDto> IDPOsAppService.BatchAutoUnlockOnOrderStockAsync(BatchAutoUnlockStockDto input)
    {
        return _dPOsAppService.BatchAutoUnlockOnOrderStockAsync(input);
    }
}