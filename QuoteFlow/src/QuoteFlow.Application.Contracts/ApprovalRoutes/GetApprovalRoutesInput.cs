using System;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.ApprovalRoutes;

public class GetApprovalRoutesInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public Guid? EntityId { get; set; }
    public string? EntityType { get; set; }
    public Guid? InstanceId { get; set; }
    public int? StepSequenceMin { get; set; }
    public int? StepSequenceMax { get; set; }
    public string? Approver { get; set; }
    public string? ApproverRoleCode { get; set; }
    public string? ApproverRoleName { get; set; }
    public DateTime? ApprovalDateMin { get; set; }
    public DateTime? ApprovalDateMax { get; set; }
    public string? Notes { get; set; }
    public bool? IsApproved { get; set; }

    public GetApprovalRoutesInput()
    {

    }
}