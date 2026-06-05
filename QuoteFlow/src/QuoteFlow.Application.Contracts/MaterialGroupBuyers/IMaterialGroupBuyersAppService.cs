using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace QuoteFlow.MaterialGroupBuyers;

public interface IMaterialGroupBuyersAppService : IApplicationService
{

    Task<PagedResultDto<MaterialGroupBuyerDto>> GetListAsync(GetMaterialGroupBuyersInput input);

    Task<MaterialGroupBuyerDto> GetAsync(Guid id);
    Task<List<MaterialGroupBuyerDto>> GetListByBuyerAsync(Guid BuyerId);

    Task DeleteAsync(Guid id);

    Task<List<MaterialGroupBuyerDto>> CreateAsync(MaterialGroupBuyerCreatesDto input);

    Task<List<MaterialGroupBuyerDto>> UpdateAsync(MaterialGroupBuyerCreatesDto input);
}