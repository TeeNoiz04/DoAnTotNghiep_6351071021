using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.Materials.MaterialApprovalRequests;

public class MaterialApprovalRequestCreateDto
{
    [Required]
    [StringLength(MaterialApprovalRequestConsts.ImportTypeMaxLength)]
    public string ImportType { get; set; } = null!;
    [StringLength(MaterialApprovalRequestConsts.FileNameMaxLength)]
    public string? FileName { get; set; }
    [StringLength(MaterialApprovalRequestConsts.NoteMaxLength)]
    public string? Note { get; set; }
    [StringLength(MaterialApprovalRequestConsts.StatusMaxLength)]
    public string? Status { get; set; }
    [Required]
    [StringLength(MaterialApprovalRequestConsts.RequestNoMaxLength)]
    public string RequestNo { get; set; } = null!;
    public Guid? CurrentApprovalRouteInstanceId { get; set; }
    public int? CurrentApprovalStepSequence { get; set; }
    [StringLength(MaterialApprovalRequestConsts.CurrentApprovalMaxLength)]
    public string? CurrentApproval { get; set; }
    [StringLength(MaterialApprovalRequestConsts.CurrentApproverRoleCodeMaxLength)]
    public string? CurrentApproverRoleCode { get; set; }
    [StringLength(MaterialApprovalRequestConsts.CurrentApproverRoleNameMaxLength)]
    public string? CurrentApproverRoleName { get; set; }
}