using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.WorkflowConfigurations;

public class WorkflowConfigurationCreateDto
{
    [Required]
    [StringLength(WorkflowConfigurationConsts.WorkflowTypeMaxLength)]
    public string WorkflowType { get; set; } = null!;
    [Required]
    public short WorkflowLevel { get; set; }
    [Required]
    [StringLength(WorkflowConfigurationConsts.WorkflowRoleMaxLength)]
    public string WorkflowRole { get; set; } = null!;
    [StringLength(WorkflowConfigurationConsts.ConditionMaxLength)]
    public string? Condition { get; set; }
    [StringLength(WorkflowConfigurationConsts.NoteMaxLength)]
    public string? Note { get; set; }
}