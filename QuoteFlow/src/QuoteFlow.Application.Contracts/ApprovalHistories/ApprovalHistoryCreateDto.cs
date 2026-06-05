using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.ApprovalHistories;

public class ApprovalHistoryCreateDto
{
    [Required]
    public Guid EntityId { get; set; }
    [StringLength(ApprovalHistoryConsts.EntityTypeMaxLength)]
    public string? EntityType { get; set; }
    [StringLength(ApprovalHistoryConsts.ApproverRoleCodeMaxLength)]
    public string? ApproverRoleCode { get; set; }
    public string? ApproverRoleName { get; set; }
    [StringLength(ApprovalHistoryConsts.ApproverUsernameMaxLength)]
    public string? ApproverUsername { get; set; }
    [StringLength(ApprovalHistoryConsts.ApproverFullNameMaxLength)]
    public string? ApproverFullName { get; set; }
    [Required]
    public string Action { get; set; } = null!;
    [Required]
    public DateTime ActionDate { get; set; }
    [StringLength(ApprovalHistoryConsts.NoteMaxLength)]
    public string? Note { get; set; }
    [Required]
    public bool IsLastApprovalInCurrentWorkflow { get; set; }
}