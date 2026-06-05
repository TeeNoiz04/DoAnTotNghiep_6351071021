using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace QuoteFlow.SpecialInputPrices.SpecialInputPriceDetails;

public interface ISpecialInputPriceDetailsAppService : IApplicationService
{

    Task<PagedResultDto<SpecialInputPriceDetailDto>> GetListAsync(GetSpecialInputPriceDetailsInput input);

    Task<SpecialInputPriceDetailDto> GetAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task<SpecialInputPriceDetailDto> CreateAsync(SpecialInputPriceDetailCreateDto input);

    Task<SpecialInputPriceDetailDto> UpdateAsync(Guid id, SpecialInputPriceDetailUpdateDto input);
}