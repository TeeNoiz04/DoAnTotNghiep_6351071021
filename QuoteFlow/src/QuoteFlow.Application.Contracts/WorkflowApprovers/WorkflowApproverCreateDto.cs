using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.WorkflowApprovers;

public class WorkflowApproverCreateDto
{
    [Required]
    public Guid WFId { get; set; }
    [Required]
    [StringLength(WorkflowApproverConsts.ApproverMaxLength)]
    public string Approver { get; set; } = null!;
    [StringLength(WorkflowApproverConsts.NoteMaxLength)]
    public string? Note { get; set; }
}