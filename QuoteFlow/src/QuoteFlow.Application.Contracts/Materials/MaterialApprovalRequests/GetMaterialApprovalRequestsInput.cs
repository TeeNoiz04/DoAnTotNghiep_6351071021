using System;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.Materials.MaterialApprovalRequests;

public class GetMaterialApprovalRequestsInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public string? ImportType { get; set; }
    public string? FileName { get; set; }
    public string? Note { get; set; }
    public string? Status { get; set; }
    public string? RequestNo { get; set; }
    public Guid? CurrentApprovalRouteInstanceId { get; set; }
    public int? CurrentApprovalStepSequenceMin { get; set; }
    public int? CurrentApprovalStepSequenceMax { get; set; }
    public string? CurrentApproval { get; set; }
    public string? CurrentApproverRoleCode { get; set; }
    public string? CurrentApproverRoleName { get; set; }

    public GetMaterialApprovalRequestsInput()
    {

    }
}