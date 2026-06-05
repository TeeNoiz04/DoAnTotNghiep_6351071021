using QuoteFlow.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.SystemConfigurations;

[RemoteService(IsEnabled = false)]
[Authorize(QuoteFlowPermissions.FAAdmins.SystemConfiguration)]
public class SystemConfigurationsAppService : QuoteFlowAppService, ISystemConfigurationsAppService
{

    protected ISystemConfigurationRepository _systemConfigurationRepository;
    protected SystemConfigurationManager _systemConfigurationManager;

    public SystemConfigurationsAppService(ISystemConfigurationRepository systemConfigurationRepository, SystemConfigurationManager systemConfigurationManager)
    {

        _systemConfigurationRepository = systemConfigurationRepository;
        _systemConfigurationManager = systemConfigurationManager;

    }

    public virtual async Task<PagedResultDto<SystemConfigurationDto>> GetListAsync(GetSystemConfigurationsInput input)
    {
        var totalCount = await _systemConfigurationRepository.GetCountAsync(input.FilterText, input.CfgKey, input.CfgValue, input.Description, input.IsSystemCfg, input.CfgType);
        var items = await _systemConfigurationRepository.GetListAsync(input.FilterText, input.CfgKey, input.CfgValue, input.Description, input.IsSystemCfg, input.CfgType, input.Sorting, input.MaxResultCount, input.SkipCount);

        return new PagedResultDto<SystemConfigurationDto>
        {
            TotalCount = totalCount,
            Items = ObjectMapper.Map<List<SystemConfiguration>, List<SystemConfigurationDto>>(items)
        };
    }

    public virtual async Task<SystemConfigurationDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<SystemConfiguration, SystemConfigurationDto>(await _systemConfigurationRepository.GetAsync(id));
    }


    public virtual async Task DeleteAsync(Guid id)
    {
        await _systemConfigurationRepository.DeleteAsync(id);
    }


    public virtual async Task<SystemConfigurationDto> CreateAsync(SystemConfigurationCreateDto input)
    {

        var systemConfiguration = await _systemConfigurationManager.CreateAsync(
        input.CfgKey, input.CfgValue, input.IsSystemCfg, input.Description, input.CfgType
        );

        return ObjectMapper.Map<SystemConfiguration, SystemConfigurationDto>(systemConfiguration);
    }

    [Authorize(QuoteFlowPermissions.FAAdmins.EditSystemConfiguration)]
    public virtual async Task<SystemConfigurationDto> UpdateAsync(Guid id, SystemConfigurationUpdateDto input)
    {

        var systemConfiguration = await _systemConfigurationManager.UpdateAsync(
        id,
        input.CfgValue, input.IsSystemCfg, input.Description
        );

        return ObjectMapper.Map<SystemConfiguration, SystemConfigurationDto>(systemConfiguration);
    }
}