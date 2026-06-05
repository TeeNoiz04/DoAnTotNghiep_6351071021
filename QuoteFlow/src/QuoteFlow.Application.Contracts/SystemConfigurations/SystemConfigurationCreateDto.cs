using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.SystemConfigurations;

public class SystemConfigurationCreateDto
{
    [Required]
    [StringLength(SystemConfigurationConsts.CfgKeyMaxLength)]
    public string CfgKey { get; set; } = null!;
    [Required]
    [StringLength(SystemConfigurationConsts.CfgValueMaxLength)]
    public string CfgValue { get; set; } = null!;
    [StringLength(SystemConfigurationConsts.DescriptionMaxLength)]
    public string? Description { get; set; }
    [StringLength(SystemConfigurationConsts.CfgKeyMaxLength)]
    public string? CfgType { get; set; }
    public bool IsSystemCfg { get; set; } = false;
}