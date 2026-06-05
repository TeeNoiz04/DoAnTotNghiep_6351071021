using System;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.ApprovalHistories;

public class GetApprovalHistoriesInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public Guid? EntityId { get; set; }
    public string? EntityType { get; set; }
    public string? ApproverRoleCode { get; set; }
    public string? ApproverRoleName { get; set; }
    public string? ApproverUsername { get; set; }
    public string? ApproverFullName { get; set; }
    public string? Action { get; set; }
    public DateTime? ActionDateMin { get; set; }
    public DateTime? ActionDateMax { get; set; }
    public string? Note { get; set; }
    public bool? IsLastApprovalInCurrentWorkflow { get; set; }

    public GetApprovalHistoriesInput()
    {

    }
}