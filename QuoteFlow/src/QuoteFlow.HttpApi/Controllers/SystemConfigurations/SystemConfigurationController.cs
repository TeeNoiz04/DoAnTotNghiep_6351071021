using Asp.Versioning;
using QuoteFlow.SystemConfigurations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace QuoteFlow.Controllers.SystemConfigurations;

[RemoteService]
[Area("app")]
[ControllerName("SystemConfiguration")]
[Route("api/app/system-configurations")]

public class SystemConfigurationController : AbpController, ISystemConfigurationsAppService
{
    protected ISystemConfigurationsAppService _systemConfigurationsAppService;

    public SystemConfigurationController(ISystemConfigurationsAppService systemConfigurationsAppService)
    {
        _systemConfigurationsAppService = systemConfigurationsAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<SystemConfigurationDto>> GetListAsync(GetSystemConfigurationsInput input)
    {
        return _systemConfigurationsAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<SystemConfigurationDto> GetAsync(Guid id)
    {
        return _systemConfigurationsAppService.GetAsync(id);
    }

    [HttpPost]
    public virtual Task<SystemConfigurationDto> CreateAsync(SystemConfigurationCreateDto input)
    {
        return _systemConfigurationsAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<SystemConfigurationDto> UpdateAsync(Guid id, SystemConfigurationUpdateDto input)
    {
        return _systemConfigurationsAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _systemConfigurationsAppService.DeleteAsync(id);
    }
}