using System;

namespace QuoteFlow.Shared.Interfaces;

public interface IHasApprovalRoute
{
    public Guid? CurrentApprovalRouteInstanceId { get; set; }
    public int? CurrentApprovalStepSequence { get; set; }
    public string? CurrentApproverRoleCode { get; set; }
    public string? CurrentApproverRoleName { get; set; }
}
