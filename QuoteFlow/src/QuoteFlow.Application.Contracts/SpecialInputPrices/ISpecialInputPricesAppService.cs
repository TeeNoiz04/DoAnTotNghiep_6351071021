using QuoteFlow.Shared.Excels;
using QuoteFlow.SpecialInputPrices.SpecialInputPriceDetails;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace QuoteFlow.SpecialInputPrices;

public interface ISpecialInputPricesAppService : IApplicationService
{

    Task<PagedResultDto<SpecialInputPriceDto>> GetListAsync(GetSpecialInputPricesInput input);

    Task<ListResultDto<SpecialInputPriceDetailDto>> GetDetailsAsync(Guid specialInputPriceId);

    Task<SpecialInputPriceDto> GetAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task<SpecialInputPriceDto> CreateAsync(SpecialInputPriceCreateDto input);

    Task<SpecialInputPriceDto> UpdateAsync(Guid id, SpecialInputPriceUpdateDto input);
    Task<ExcelValidationResult<SpecialInputPriceDetailImportDto>> ValidateAndParseSpecialInputPriceDetailsAsync(IRemoteStreamContent file);
    Task ImportSpecialInputPriceDetailsAsync(ExcelValidationResult<SpecialInputPriceDetailImportDto> data);
    Task<IRemoteStreamContent> GetInputPriceAsExcelAsync(GetSpecialInputPricesInput input);
}