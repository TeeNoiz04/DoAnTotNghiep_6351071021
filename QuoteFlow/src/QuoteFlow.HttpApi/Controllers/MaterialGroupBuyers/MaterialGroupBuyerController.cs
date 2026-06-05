using Asp.Versioning;
using QuoteFlow.MaterialGroupBuyers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace QuoteFlow.Controllers.MaterialGroupBuyers;

[RemoteService]
[Area("app")]
[ControllerName("MaterialGroupBuyer")]
[Route("api/app/material-group-buyers")]

public class MaterialGroupBuyerController : AbpController, IMaterialGroupBuyersAppService
{
    protected IMaterialGroupBuyersAppService _materialGroupBuyersAppService;

    public MaterialGroupBuyerController(IMaterialGroupBuyersAppService materialGroupBuyersAppService)
    {
        _materialGroupBuyersAppService = materialGroupBuyersAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<MaterialGroupBuyerDto>> GetListAsync(GetMaterialGroupBuyersInput input)
    {
        return _materialGroupBuyersAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<MaterialGroupBuyerDto> GetAsync(Guid id)
    {
        return _materialGroupBuyersAppService.GetAsync(id);
    }

    [HttpPost]
    public virtual Task<List<MaterialGroupBuyerDto>> CreateAsync(MaterialGroupBuyerCreatesDto input)
    {
        return _materialGroupBuyersAppService.CreateAsync(input);
    }

    [HttpPut]
    public virtual Task<List<MaterialGroupBuyerDto>> UpdateAsync(MaterialGroupBuyerCreatesDto input)
    {
        return _materialGroupBuyersAppService.UpdateAsync(input);
    }

    [HttpDelete]
    [Route("buyer/{buyerId}")]
    public virtual Task DeleteAsync(Guid buyerId)
    {
        return _materialGroupBuyersAppService.DeleteAsync(buyerId);
    }
    [HttpGet]
    [Route("buyer/{buyerId}")]
    public Task<List<MaterialGroupBuyerDto>> GetListByBuyerAsync(Guid buyerId)
    {
        return _materialGroupBuyersAppService.GetListByBuyerAsync(buyerId);
    }
}