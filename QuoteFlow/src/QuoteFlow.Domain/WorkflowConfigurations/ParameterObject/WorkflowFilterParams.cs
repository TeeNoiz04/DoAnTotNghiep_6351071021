namespace QuoteFlow.WorkflowConfigurations.ParameterObject;
public class WorkflowFilterParams
{
    public string? FilterText { get; set; }

    public string? WorkflowType { get; set; }
    public short? WorkflowLevel { get; set; }
    public short? WorkflowLevelMin { get; set; }
    public short? WorkflowLevelMax { get; set; }
    public string? WorkflowRole { get; set; }
    public string? Condition { get; set; }
    public string? Note { get; set; }
    public string? Sorting { get; set; } = null;
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = int.MaxValue;
}
