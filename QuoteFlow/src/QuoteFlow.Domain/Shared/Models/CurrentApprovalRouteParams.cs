using System;

namespace QuoteFlow.Shared.Models;

public class CurrentApprovalRouteParams
{
    public Guid? InstanceId { get; protected set; } = null;
    public int? StepSequence { get; protected set; } = null;
    public string? ApproverRoleCode { get; protected set; } = null;
    public string? ApproverRoleName { get; protected set; } = null;

    public CurrentApprovalRouteParams(
        Guid? instanceId = null,
        int? stepSequence = null,
        string? approverRoleCode = null,
        string? approverRoleName = null
    )
    {
        InstanceId = instanceId;
        StepSequence = stepSequence;
        ApproverRoleCode = approverRoleCode;
        ApproverRoleName = approverRoleName;
    }

    protected CurrentApprovalRouteParams()
    {

    }
}
