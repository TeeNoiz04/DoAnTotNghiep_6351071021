using Volo.Abp.Application.Dtos;

namespace QuoteFlow.SystemConfigurations;

public class GetSystemConfigurationsInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public string? CfgKey { get; set; }
    public string? CfgValue { get; set; }
    public string? Description { get; set; }
    public bool? IsSystemCfg { get; set; }

    public string? CfgType { get; set; }

    public GetSystemConfigurationsInput()
    {

    }
}