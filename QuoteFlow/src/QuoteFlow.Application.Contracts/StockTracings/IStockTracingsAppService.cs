using QuoteFlow.Shared.Excels;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace QuoteFlow.StockTracings;

public interface IStockTracingsAppService : IApplicationService
{

    Task<PagedResultDto<StockTracingDto>> GetListAsync(GetStockTracingsInput input);

    Task<StockTracingDto> GetAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task<StockTracingDto> CreateAsync(StockTracingCreateDto input);

    Task<StockTracingDto> UpdateAsync(Guid id, StockTracingUpdateDto input);

    Task<IRemoteStreamContent> GetListAsExcelFileAsync(StockTracingExcelDownloadDto input);

    Task<QuoteFlow.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();

    Task<ExcelValidationResult<StockTracingDeliveryImportDto>> ValidateAndStockTracingDeliveryAsync(IRemoteStreamContent file, DateTime fromDate, DateTime toDate, string? note);
    Task<ExcelValidationResult<StockTracingInventoryImportDto>> ValidateAndStockTracingInventoryAsync(IRemoteStreamContent file, DateTime dateEntered, string? note);
    Task<ExcelValidationResult<StockTracingReceiptImportDto>> ValidateAndStockTracingReceiptAsync(IRemoteStreamContent file, DateTime fromDate, DateTime toDate, string? note);
    Task<StockTracingDto> ImportStockTracingDeliveryAsync(ExcelValidationResult<StockTracingDeliveryImportDto> data, DateTime fromDate, DateTime toDate, string? note);

    Task<StockTracingDto> ImportStockTracingReceiptAsync(ExcelValidationResult<StockTracingReceiptImportDto> data, DateTime fromDate, DateTime toDate, string? note);
    Task<StockTracingDto> ImportStockTracingInvantoryAsync(ExcelValidationResult<StockTracingInventoryImportDto> data, DateTime? entered, string? note);
}