using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace QuoteFlow.Buyers;

public interface IBuyersAppService : IApplicationService
{
    Task<PagedResultDto<BuyerDto>> GetListAsync(GetBuyersInput input);

    Task<BuyerDto> GetAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task<BuyerDto> CreateAsync(BuyerCreateDto input);

    Task<BuyerDto> UpdateAsync(Guid id, BuyerUpdateDto input);

    Task<IRemoteStreamContent> GetListAsExcelFileAsync(GetBuyersInput input);
}
