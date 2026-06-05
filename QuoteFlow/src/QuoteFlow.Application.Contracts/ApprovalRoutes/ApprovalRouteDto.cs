using QuoteFlow.Shared;
using System;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.ApprovalRoutes;

public class ApprovalRouteDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public Guid? EntityId { get; set; }
    public string? EntityType { get; set; }
    public Guid? InstanceId { get; set; }
    public int StepSequence { get; set; }
    public string? Approver { get; set; }
    public string ApproverRoleCode { get; set; } = null!;
    public string ApproverRoleName { get; set; } = null!;
    public DateTime? ApprovalDate { get; set; }
    public string? Notes { get; set; }
    public bool IsApproved { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;

}