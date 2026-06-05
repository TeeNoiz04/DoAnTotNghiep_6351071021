using JetBrains.Annotations;
using QuoteFlow.Materials.MaterialApprovalRequestDetails;
using QuoteFlow.Materials.MaterialApprovalRequests.ParameterObjects;
using QuoteFlow.Shared.Extensions;
using QuoteFlow.Shared.Interfaces;
using QuoteFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;

namespace QuoteFlow.Materials.MaterialApprovalRequests;

public class MaterialApprovalRequest : ExtendedAuditedAggregateRoot<Guid>, IApprovable
{
    [NotNull]
    public virtual string ImportType { get; set; }

    [CanBeNull]
    public virtual string? FileName { get; set; }

    [CanBeNull]
    public virtual string? Note { get; set; }

    [CanBeNull]
    public virtual string? Status { get; set; }

    [NotNull]
    public virtual string RequestNo { get; set; }

    public virtual Guid? CurrentApprovalRouteInstanceId { get; set; }

    public virtual int? CurrentApprovalStepSequence { get; set; }

    [CanBeNull]
    public virtual string? CurrentApproverRoleCode { get; set; }

    [CanBeNull]
    public virtual string? CurrentApproverRoleName { get; set; }

    public ICollection<MaterialApprovalRequestDetail>? MaterialApprovalDetails { get; set; }
    public ICollection<MaterialApprovalRequestHistory>? MaterialHistories { get; set; }
    public ICollection<MaterialApprovalRequestRoute>? MaterialRoutes { get; set; }

    protected MaterialApprovalRequest()
    {

    }

    public MaterialApprovalRequest(Guid id, string requestNo, MaterialApprovalRequestCreateParams createParams)
    {
        Id = Id;
        ImportType = createParams.ImportType;
        RequestNo = requestNo;
        FileName = createParams.FileName;
        Note = createParams.Note;
        Status = "DRAFT";
    }

    public void RecordAction(MaterialApprovalRequestHistory history)
    {
        MaterialHistories ??= [];

        MaterialHistories.Add(history);
    }

    public virtual void Approved(bool lastStep, DateTime approveDate, string? note)
    {
        if (!this.IsInProgress())
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.OnlyInProgressCanBeRejected);
        }
        if (lastStep)
        {
            Status = QuoteFlowStatuses.Approved;
        }
        else
        {
            Status = QuoteFlowStatuses.InProgress;
            var latestUnapprovedStep = GetLatestUnapprovedStep();
            foreach (var route in MaterialRoutes.Where(x => x.StepSequence <= latestUnapprovedStep.StepSequence && !x.IsApproved))
            {
                route.Approve(approveDate, note);
            }
        }


    }
    public MaterialApprovalRequestRoute GetLatestUnapprovedStep()
    {
        if (MaterialRoutes == null || MaterialRoutes.Count == 0)
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.NoApprovalRouteFound)
                .WithData("entityId", Id);
        }

        var latestUnapproved = MaterialRoutes
            .Where(x => !x.IsApproved)
            .MinBy(x => x.StepSequence)
            ?? throw new BusinessException(QuoteFlowDomainErrorCodes.NoUnapprovedStepFound)
                .WithData("entityId", Id);

        return latestUnapproved;
    }
    public virtual void Submit()
    {
        if (!this.IsSubmittable())
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.InvalidStatusForSubmission);
        }

        Status = QuoteFlowStatuses.InProgress;
    }

    public virtual void Reject()
    {
        if (this.IsInProgress())
        {
            Status = QuoteFlowStatuses.Rejected;
        }
        else
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.OnlyInProgressCanBeRejected);

        }
    }

    public virtual void Cancel()
    {
        if (this.IsInProgress())
        {
            Status = QuoteFlowStatuses.Cancelled;
        }
        else
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.OnlyInProgressCanBeRejected);

        }
    }

    public void UpdateCurrentApprovalRoute(Guid? currrentApprovalRouteInstanceId = null, int? currentApprovalStepSequence = null, string? currentApproverRoleCode = null, string? currentApproverRoleName = null)
    {
        CurrentApprovalRouteInstanceId = currrentApprovalRouteInstanceId ?? null;

        CurrentApprovalStepSequence = currentApprovalStepSequence ?? null;
        CurrentApproverRoleCode = currentApproverRoleCode ?? null;
        CurrentApproverRoleName = currentApproverRoleName ?? null;

    }
}