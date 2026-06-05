using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace QuoteFlow.StockTracingDetails;

public interface IStockTracingDetailsAppService : IApplicationService
{

    Task<PagedResultDto<StockTracingDetailDto>> GetListAsync(GetStockTracingDetailsInput input);

    Task<StockTracingDetailDto> GetAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task<StockTracingDetailDto> CreateAsync(StockTracingDetailCreateDto input);

    Task<StockTracingDetailDto> UpdateAsync(Guid id, StockTracingDetailUpdateDto input);

    Task<IRemoteStreamContent> GetListAsExcelFileAsync(StockTracingDetailExcelDownloadDto input);

    Task<QuoteFlow.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();

}