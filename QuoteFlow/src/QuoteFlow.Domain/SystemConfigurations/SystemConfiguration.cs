using JetBrains.Annotations;
using QuoteFlow.Shared.Models;
using System;
using Volo.Abp;

namespace QuoteFlow.SystemConfigurations;

public class SystemConfiguration : ExtendedAuditedAggregateRoot<Guid>
{
    [NotNull]
    public virtual string CfgKey { get; set; }

    [NotNull]
    public virtual string CfgValue { get; set; }
    [CanBeNull]
    public virtual string? CfgType { get; set; }

    [CanBeNull]
    public virtual string? Description { get; set; }

    public virtual bool IsSystemCfg { get; set; }

    protected SystemConfiguration()
    {

    }

    public SystemConfiguration(Guid id, string cfgKey, string cfgValue, bool isSystemCfg, string? description = null, string? cfgType = null)
    {

        Id = id;
        Check.NotNull(cfgKey, nameof(cfgKey));
        Check.Length(cfgKey, nameof(cfgKey), SystemConfigurationConsts.CfgKeyMaxLength, 0);
        Check.NotNull(cfgValue, nameof(cfgValue));
        Check.Length(cfgValue, nameof(cfgValue), SystemConfigurationConsts.CfgValueMaxLength, 0);
        Check.Length(description, nameof(description), SystemConfigurationConsts.DescriptionMaxLength, 0);
        CfgKey = cfgKey;
        CfgValue = cfgValue;
        IsSystemCfg = isSystemCfg;
        Description = description;
        CfgType = cfgType;
    }

}