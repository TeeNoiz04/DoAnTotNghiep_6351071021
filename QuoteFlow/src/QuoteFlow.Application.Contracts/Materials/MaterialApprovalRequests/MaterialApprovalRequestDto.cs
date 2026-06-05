using QuoteFlow.Materials.MaterialApprovalRequestDetails;
using QuoteFlow.Shared;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Materials.MaterialApprovalRequests;

public class MaterialApprovalRequestDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string ImportType { get; set; } = null!;
    public string? FileName { get; set; }
    public string? Note { get; set; }
    public string? Status { get; set; }
    public string RequestNo { get; set; } = null!;
    public Guid? CurrentApprovalRouteInstanceId { get; set; }
    public int? CurrentApprovalStepSequence { get; set; }
    public string? CurrentApproverRoleCode { get; set; }
    public string? CurrentApproverRoleName { get; set; }
    public List<MaterialApprovalRequestDetailDto>? MaterialApprovalDetails { get; set; }
    public List<MaterialApprovalRequestHistoryDto>? MaterialHistories { get; set; }
    public List<MaterialApprovalRequestRouteDto>? MaterialRoutes { get; set; }
    public MaterialFlagsDto Flags { get; set; } = null!;

    public string ConcurrencyStamp { get; set; } = null!;


}