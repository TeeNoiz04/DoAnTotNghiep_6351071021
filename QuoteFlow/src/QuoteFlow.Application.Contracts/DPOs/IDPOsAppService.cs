using QuoteFlow.DPOs.DPODetails;
using QuoteFlow.DPOs.DPOMessages;
using QuoteFlow.Materials.MaterialStocks.MaterialStockLockStocks;
using QuoteFlow.Messages;
using QuoteFlow.SaleOrders;
using QuoteFlow.Shared;
using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace QuoteFlow.DPOs;

public interface IDPOsAppService : IApplicationService
{

    Task<PagedResultDto<DPODto>> GetListAsync(GetDPOsInput input);

    Task<DPODto> GetAsync(Guid id);

    Task<ListResultDto<MaterialStockLockStockDto>> GetLockStocksAsync(Guid dpoId, Guid detailId);

    Task DeleteAsync(Guid id);

    Task<DPODto> CreateAsync(DPOCreateDto input);

    Task<DPODto> UpdateAsync(Guid id, DPOUpdateDto input);

    Task<IRemoteStreamContent> GetListAsExcelFileAsync(DPOExcelDownloadDto input);

    Task<QuoteFlow.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();

    Task<ExcelValidationResult<ImportDPODto>> ValidateAndParseDPOAsync(IRemoteStreamContent file, ImportDPOInput input);

    Task<DPODto> ImportDPOAsync(ExcelValidationResult<ImportDPODto> data, bool force = false);

    Task<List<DPODetailDto>> ConfirmNoteAsync(DPOConfirmNoteDto input);

    Task<List<DPODetailDto>> AddExtraFeeAsync(DPOAddExtraFeeDto input);

    Task<DPODto> CancelAsync(Guid id, DPOCancelDto input);

    Task LockStockAutoAsync(DPOLockStockAutoDto input);

    Task LockStockAutoV2Async(DPOLockStockAutoV2Dto input);

    Task LockShipmentAutoAsync(DPOLockShipmentAutoDto input);

    Task<DPODto> ConfirmLockStockAsync(Guid id, NoteMetadataDto input);

    Task<DPODto> ConfirmLockOnOrderAsync(Guid id, NoteMetadataDto input);

    Task<ListResultDto<DPOListPOsDto>> GetListAvailablePOsAsync(Guid dpoId, Guid dpoDetailId, string? materialCode);

    Task<ListResultDto<DPOLockOnOrderStockDto>> GetListLockOnOrderStockAsync(Guid dpoId, Guid dpoDetailId);

    Task LockShipmentAsync(Guid dpoDetailId, DPOLockShipmentDto input);

    Task UpdateLockShipmentAsync(Guid dpoDetailId, Guid poDetailId, DPOLockShipmentItemUpdateDto input);

    Task<ListResultDto<DPOLockStockEtaEtdDto>> GetListLockStockEtaEtdAsync(Guid dpoDetailId, Guid poDetailId);

    Task UpdateLockStockDetailAsync(Guid dpoDetailId, DPODetailUpdateLockStockDto input);
    Task DeleteLockStockAsync(Guid dpoDetailId, Guid lockStockId);
    Task DeleteLockOnOrderStockAsync(Guid dpoDetailId, Guid poDetailId);
    Task<DPODto> ApproveAsync(Guid id, NoteMetadataDto input);
    Task<DPODto> RejectAsync(Guid id, NoteMetadataDto input);
    Task<IRemoteStreamContent> GetListDPOReportAsync(GetDPOReportInputDto input);
    Task<List<DPODataReportDto>> GetDataDPOReportAsync(GetDPOReportInputDto input);
    Task<PagedResultDto<MessageDto>> GetListMessagesAsync(Guid dpoId, GetDPOMessagesInput input);
    Task<MessageDto> SendMessageAsync(Guid dpoId, MessageCreateDto input);
    Task<IRemoteStreamContent> ExportDataAsExcelAsync(DPOExportDataInputDto input);

    //DPO Details
    Task<PagedResultDto<DPODetailDto>> GetListDPODetailAsync(GetDPODetailsInput input);
    Task<DPODetailDto> GetDPODetailAsync(Guid id);
    Task DeleteDPODetailAsync(Guid id);
    Task<DPODetailDto> CreateDPODetailAsync(DPODetailCreateDto input);
    Task<DPODetailDto> UpdateAsync(Guid id, DPODetailUpdateDto input);
    Task<IRemoteStreamContent> GetListAsExcelFileAsync(DPODetailExcelDownloadDto input);
    Task<IRemoteStreamContent> GetListDPOProcessingReportAsync(GetDPOReportInputDto input);
    Task<Shared.DownloadTokenResultDto> GetDownloadTokenDPODtailAsync();
    Task LockStockAsync(Guid id, DPODetailLockStockDto input);
    Task<List<SaleOrderListModalDPODto>> GetListSaleOrderModalDPOAsync(Guid dpoDetailId);
    Task<List<SaleOrderListModalDeliveryDto>> GetListSaleOrderModalDeliveryAsync(Guid dpoDetailId);
    Task UpdateDPODetailAsync(Guid dpoDetailId, int qty, string? confirmNote, string? note);
    Task<List<DPOProcessingReportDto>> GetDataDPOProcessingReportAsync(GetDPOReportInputDto input);

    // Batch unlock stock with SSE progress streaming
    IAsyncEnumerable<BatchUnlockProgressEventDto> BatchAutoUnlockStockAsync(BatchAutoUnlockStockDto input);
    IAsyncEnumerable<BatchUnlockProgressEventDto> BatchAutoUnlockOnOrderStockAsync(BatchAutoUnlockStockDto input);
    // GKR Related operations
    Task<List<DpoGkrAllocationDto>> GetGkrAllocationsAsync(Guid dpoId);
    Task<List<DpoGkrAllocationDto>> GetAvailableGkrsForAllocationAsync(Guid dpoId);
    Task AllocateGkrToDpoAsync(DPOAllocateGKRDto input);

}