using JetBrains.Annotations;
using QuoteFlow.ApprovalHistories.ParameterObjects;
using QuoteFlow.Shared.Models;
using System;
using Volo.Abp;

namespace QuoteFlow.ApprovalHistories;

public class ApprovalHistory : ExtendedAuditedAggregateRoot<Guid>
{
    [CanBeNull]
    public virtual string? EntityType { get; set; }

    [CanBeNull]
    public virtual string? ApproverRoleCode { get; set; }

    [CanBeNull]
    public virtual string? ApproverRoleName { get; set; }

    [CanBeNull]
    public virtual string? ApproverUsername { get; set; }

    [CanBeNull]
    public virtual string? ApproverFullName { get; set; }

    [NotNull]
    public virtual string Action { get; set; }

    public virtual DateTime ActionDate { get; set; }

    [CanBeNull]
    public virtual string? Note { get; set; }

    public virtual bool IsLastApprovalInCurrentWorkflow { get; set; }

    protected ApprovalHistory()
    {

    }

    public ApprovalHistory(Guid id, string action, DateTime actionDate, bool isLastApprovalInCurrentWorkflow, string? entityType = null, string? approverRoleCode = null, string? approverRoleName = null, string? approverUsername = null, string? approverFullName = null, string? note = null)
    {

        Id = id;
        Check.NotNull(action, nameof(action));
        Check.Length(entityType, nameof(entityType), ApprovalHistoryConsts.EntityTypeMaxLength, 0);
        Check.Length(approverRoleCode, nameof(approverRoleCode), ApprovalHistoryConsts.ApproverRoleCodeMaxLength, 0);
        Check.Length(approverUsername, nameof(approverUsername), ApprovalHistoryConsts.ApproverUsernameMaxLength, 0);
        Check.Length(approverFullName, nameof(approverFullName), ApprovalHistoryConsts.ApproverFullNameMaxLength, 0);
        Check.Length(note, nameof(note), ApprovalHistoryConsts.NoteMaxLength, 0);
        Action = action;
        ActionDate = actionDate;
        IsLastApprovalInCurrentWorkflow = isLastApprovalInCurrentWorkflow;
        EntityType = entityType;
        ApproverRoleCode = approverRoleCode;
        ApproverRoleName = approverRoleName;
        ApproverUsername = approverUsername;
        ApproverFullName = approverFullName;
        Note = note;
    }

    public ApprovalHistory(Guid id, ApprovalHistoryCreateParams createParams)
    {
        Id = id;

        Action = createParams.Action;
        ActionDate = createParams.ActionDate;
        EntityType = createParams.EntityType;
        ApproverRoleCode = createParams.ApproverRoleCode ?? "Requester";
        ApproverRoleName = createParams.ApproverRoleName ?? "Requester";
        ApproverUsername = createParams.ApproverUsername;
        ApproverFullName = createParams.ApproverFullName;

        if (Action == HistoryActions.Cancelled || Action == HistoryActions.Rejected)
        {
            IsLastApprovalInCurrentWorkflow = true;
        }
        else
        {
            IsLastApprovalInCurrentWorkflow = createParams.IsLastApprovalInCurrentWorkflow;
        }

        IsLastApprovalInCurrentWorkflow = createParams.IsLastApprovalInCurrentWorkflow;
        Note = createParams.Note;
    }

}