using Asp.Versioning;
using QuoteFlow.Materials.MaterialGroups;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace QuoteFlow.Controllers.MaterialGroups;

[RemoteService]
[Area("app")]
[ControllerName("MaterialGroup")]
[Route("api/app/material-groups")]

public class MaterialGroupController : AbpController, IMaterialGroupsAppService
{
    protected IMaterialGroupsAppService _materialGroupsAppService;

    public MaterialGroupController(IMaterialGroupsAppService materialGroupsAppService)
    {
        _materialGroupsAppService = materialGroupsAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<MaterialGroupDto>> GetListAsync(GetMaterialGroupsInput input)
    {
        return _materialGroupsAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<MaterialGroupDto> GetAsync(Guid id)
    {
        return _materialGroupsAppService.GetAsync(id);
    }

    [HttpPost]
    public virtual Task<MaterialGroupDto> CreateAsync(MaterialGroupCreateDto input)
    {
        return _materialGroupsAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<MaterialGroupDto> UpdateAsync(Guid id, MaterialGroupUpdateDto input)
    {
        return _materialGroupsAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _materialGroupsAppService.DeleteAsync(id);
    }
}