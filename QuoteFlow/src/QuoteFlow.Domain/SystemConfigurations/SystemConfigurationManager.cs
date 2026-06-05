using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.SystemConfigurations;

public class SystemConfigurationManager : DomainService
{
    protected ISystemConfigurationRepository _systemConfigurationRepository;

    public SystemConfigurationManager(ISystemConfigurationRepository systemConfigurationRepository)
    {
        _systemConfigurationRepository = systemConfigurationRepository;
    }

    public virtual async Task<SystemConfiguration> CreateAsync(
    string cfgKey, string cfgValue, bool isSystemCfg, string? description = null, string? cfgType = null)
    {
        Check.NotNullOrWhiteSpace(cfgKey, nameof(cfgKey));
        Check.Length(cfgKey, nameof(cfgKey), SystemConfigurationConsts.CfgKeyMaxLength);
        Check.NotNullOrWhiteSpace(cfgValue, nameof(cfgValue));
        Check.Length(cfgValue, nameof(cfgValue), SystemConfigurationConsts.CfgValueMaxLength);
        Check.Length(description, nameof(description), SystemConfigurationConsts.DescriptionMaxLength);

        var systemConfiguration = new SystemConfiguration(
         GuidGenerator.Create(),
         cfgKey, cfgValue, isSystemCfg, description, cfgType
         );

        return await _systemConfigurationRepository.InsertAsync(systemConfiguration);
    }

    public virtual async Task<SystemConfiguration> UpdateAsync(
        Guid id,
        string cfgValue, bool isSystemCfg, string? description = null
    )
    {
        Check.NotNullOrWhiteSpace(cfgValue, nameof(cfgValue));
        Check.Length(cfgValue, nameof(cfgValue), SystemConfigurationConsts.CfgValueMaxLength);
        Check.Length(description, nameof(description), SystemConfigurationConsts.DescriptionMaxLength);

        var systemConfiguration = await _systemConfigurationRepository.GetAsync(id);

        systemConfiguration.CfgValue = cfgValue;

        return await _systemConfigurationRepository.UpdateAsync(systemConfiguration);
    }

}