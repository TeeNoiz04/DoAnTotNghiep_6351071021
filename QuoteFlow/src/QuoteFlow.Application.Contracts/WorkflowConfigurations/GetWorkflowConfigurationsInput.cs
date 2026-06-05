using Volo.Abp.Application.Dtos;

namespace QuoteFlow.WorkflowConfigurations;

public class GetWorkflowConfigurationsInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public string? WorkflowType { get; set; }
    public short? WorkflowLevel { get; set; }
    public short? WorkflowLevelMin { get; set; }
    public short? WorkflowLevelMax { get; set; }
    public string? WorkflowRole { get; set; }
    public string? Condition { get; set; }
    public string? Note { get; set; }

    public GetWorkflowConfigurationsInput()
    {

    }
}