using QuoteFlow.Shared;
using System;

namespace QuoteFlow.SystemConfigurations;

public class SystemConfigurationDto : ExtendedAuditedEntityDto<Guid>
{
    public string CfgKey { get; set; } = null!;
    public string CfgValue { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsSystemCfg { get; set; }
    public string? CfgType { get; set; }

}