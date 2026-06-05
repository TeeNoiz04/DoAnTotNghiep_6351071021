using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace QuoteFlow.CfgDiscountRatios
{
    public interface ICfgDiscountRatiosAppService : IApplicationService
    {

        Task<PagedResultDto<CfgDiscountRatioDto>> GetListAsync(GetCfgDiscountRatiosInput input);

        //Task<CfgDiscountRatioDto> GetAsync(Guid id);

        //Task DeleteAsync(Guid id);

        //Task<CfgDiscountRatioDto> CreateAsync(CfgDiscountRatioCreateDto input);

        Task<CfgDiscountRatioDto> UpdateAsync(Guid id, CfgDiscountRatioUpdateDto input);
    }
}