using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.WorkflowApprovers;

public class WorkflowApproverUpdateDto
{
    [Required]
    public Guid WFId { get; set; }
    [Required]
    [StringLength(WorkflowApproverConsts.ApproverMaxLength)]
    public string Approver { get; set; } = null!;

}