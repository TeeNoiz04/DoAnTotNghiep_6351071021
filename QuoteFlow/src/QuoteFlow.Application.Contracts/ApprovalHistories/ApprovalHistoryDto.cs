using QuoteFlow.Shared;
using System;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.ApprovalHistories;

public class ApprovalHistoryDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string? EntityType { get; set; }
    public string? ApproverRoleCode { get; set; }
    public string? ApproverRoleName { get; set; }
    public string? ApproverUsername { get; set; }
    public string? ApproverFullName { get; set; }
    public string Action { get; set; } = null!;
    public DateTime ActionDate { get; set; }
    public string? Note { get; set; }
    public bool IsLastApprovalInCurrentWorkflow { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;

}