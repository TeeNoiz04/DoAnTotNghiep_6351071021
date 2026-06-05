using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace QuoteFlow.Buyers;

[RemoteService]
[Area("app")]
[ControllerName("Buyer")]
[Route("api/app/buyers")]
public class BuyerController : AbpController, IBuyersAppService
{
    protected IBuyersAppService _buyerAppService;

    public BuyerController(IBuyersAppService buyerAppService)
    {
        _buyerAppService = buyerAppService;
    }
    [HttpPost]
    public Task<BuyerDto> CreateAsync(BuyerCreateDto input)
    {
        return _buyerAppService.CreateAsync(input);
    }
    [HttpDelete]
    [Route("{id}")]
    public Task DeleteAsync(Guid id)
    {
        return _buyerAppService.DeleteAsync(id);
    }
    [HttpGet]
    [Route("{id}")]
    public Task<BuyerDto> GetAsync(Guid id)
    {
        return _buyerAppService.GetAsync(id);
    }

    [HttpGet]
    [Route("as-excel-file")]
    public Task<IRemoteStreamContent> GetListAsExcelFileAsync(GetBuyersInput input)
    {
        return _buyerAppService.GetListAsExcelFileAsync(input);
    }

    [HttpGet]
    public Task<PagedResultDto<BuyerDto>> GetListAsync(GetBuyersInput input)
    {
        return _buyerAppService.GetListAsync(input);
    }
    [HttpPut]
    [Route("{id}")]
    public Task<BuyerDto> UpdateAsync(Guid id, BuyerUpdateDto input)
    {
        return _buyerAppService.UpdateAsync(id, input);
    }
}
