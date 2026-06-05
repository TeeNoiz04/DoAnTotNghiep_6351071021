using QuoteFlow.WorkflowApprovers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.WorkflowConfigurations;

public class WorkflowConfigurationUpdateDto : IHasConcurrencyStamp
{

    [Required]
    [StringLength(WorkflowConfigurationConsts.WorkflowRoleMaxLength)]
    public string WorkflowRole { get; set; } = null!;
    [StringLength(WorkflowConfigurationConsts.ConditionMaxLength)]
    public string? Note { get; set; }

    public List<WorkflowApproverUpdateDto>? Approvers { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}