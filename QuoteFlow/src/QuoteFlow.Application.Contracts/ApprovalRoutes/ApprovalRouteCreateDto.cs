using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.ApprovalRoutes;

public class ApprovalRouteCreateDto
{
    public Guid? EntityId { get; set; }
    [StringLength(ApprovalRouteConsts.EntityTypeMaxLength)]
    public string? EntityType { get; set; }
    public Guid? InstanceId { get; set; }
    [Required]
    public int StepSequence { get; set; }
    [StringLength(ApprovalRouteConsts.ApproverMaxLength)]
    public string? Approver { get; set; }
    [Required]
    [StringLength(ApprovalRouteConsts.ApproverRoleCodeMaxLength)]
    public string ApproverRoleCode { get; set; } = null!;
    [Required]
    [StringLength(ApprovalRouteConsts.ApproverRoleNameMaxLength)]
    public string ApproverRoleName { get; set; } = null!;
    public DateTime? ApprovalDate { get; set; }
    [StringLength(ApprovalRouteConsts.NotesMaxLength)]
    public string? Notes { get; set; }
    [Required]
    public bool IsApproved { get; set; }
}