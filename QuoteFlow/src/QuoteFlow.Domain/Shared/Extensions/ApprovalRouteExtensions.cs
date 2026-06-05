using QuoteFlow.Shared.Interfaces;
using QuoteFlow.Shared.Models;

namespace QuoteFlow.Shared.Extensions;

public static class ApprovalRouteExtensions
{
    public static bool IsInProgress(this IHasStatus entity) =>
        entity.Status == QuoteFlowStatuses.InProgress;

    public static bool IsApproved(this IHasStatus entity) =>
        entity.Status == QuoteFlowStatuses.Approved;

    public static bool IsCancelled(this IHasStatus entity) =>
        entity.Status == QuoteFlowStatuses.Cancelled;

    public static bool IsVerifying(this IHasStatus entity) =>
        entity.Status == QuoteFlowStatuses.Verifying;

    public static bool IsRejected(this IHasStatus entity) =>
        entity.Status == QuoteFlowStatuses.Rejected;

    public static bool IsDraft(this IHasStatus entity) =>
        entity.Status == QuoteFlowStatuses.Draft;

    public static bool IsClosed(this IHasStatus entity) =>
        entity.Status == QuoteFlowStatuses.Closed;

    public static bool IsSubmittable(this IHasStatus entity) =>
        entity.IsDraft() || entity.IsCancelled() || entity.IsRejected();


    public static void SetCurrentRoute(this IHasApprovalRoute entity, CurrentApprovalRouteParams? currentRoute)
    {
        if (currentRoute == null)
        {
            entity.CurrentApprovalRouteInstanceId = null;
            entity.CurrentApprovalStepSequence = null;
            entity.CurrentApproverRoleCode = null;
            entity.CurrentApproverRoleName = null;
        }
        else
        {
            entity.CurrentApprovalRouteInstanceId = currentRoute.InstanceId;
            entity.CurrentApprovalStepSequence = currentRoute.StepSequence;
            entity.CurrentApproverRoleCode = currentRoute.ApproverRoleCode;
            entity.CurrentApproverRoleName = currentRoute.ApproverRoleName;
        }
    }
}
