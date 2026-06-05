using JetBrains.Annotations;
using QuoteFlow.Shared.Models;
using System;
using Volo.Abp;

namespace QuoteFlow.ApprovalRoutes;

public class ApprovalRoute : ExtendedAuditedAggregateRoot<Guid>
{

    [CanBeNull]
    public virtual string? EntityType { get; set; }

    public virtual Guid? InstanceId { get; set; }

    public virtual int StepSequence { get; set; }

    [CanBeNull]
    public virtual string? Approver { get; set; }

    [NotNull]
    public virtual string ApproverRoleCode { get; set; }

    [NotNull]
    public virtual string ApproverRoleName { get; set; }

    public virtual DateTime? ApprovalDate { get; set; }

    [CanBeNull]
    public virtual string? Notes { get; set; }

    public virtual bool IsApproved { get; set; }

    protected ApprovalRoute()
    {

    }

    public ApprovalRoute(Guid id, int stepSequence, string approverRoleCode, string approverRoleName, string entityType, Guid instanceId, string approver, string? notes = null)
    {

        Id = id;
        Check.NotNull(approverRoleCode, nameof(approverRoleCode));
        Check.Length(approverRoleCode, nameof(approverRoleCode), ApprovalRouteConsts.ApproverRoleCodeMaxLength, 0);
        Check.NotNull(approverRoleName, nameof(approverRoleName));
        Check.Length(approverRoleName, nameof(approverRoleName), ApprovalRouteConsts.ApproverRoleNameMaxLength, 0);
        Check.NotNull(entityType, nameof(entityType));
        Check.Length(entityType, nameof(entityType), ApprovalRouteConsts.EntityTypeMaxLength, 0);
        Check.NotNull(approver, nameof(approver));
        Check.Length(approver, nameof(approver), ApprovalRouteConsts.ApproverMaxLength, 0);
        Check.Length(notes, nameof(notes), ApprovalRouteConsts.NotesMaxLength, 0);

        StepSequence = stepSequence;
        ApproverRoleCode = approverRoleCode;
        ApproverRoleName = approverRoleName;
        IsApproved = false;
        EntityType = entityType;
        InstanceId = instanceId;
        Approver = approver;
        ApprovalDate = null;
        Notes = notes;
    }

    public ApprovalRoute(Guid id, int stepSequence, string approverRoleCode, string approverRoleName, bool isApproved, string? entityType = null, Guid? instanceId = null, string? approver = null, DateTime? approvalDate = null, string? notes = null)
    {

        Id = id;
        Check.NotNull(approverRoleCode, nameof(approverRoleCode));
        Check.Length(approverRoleCode, nameof(approverRoleCode), ApprovalRouteConsts.ApproverRoleCodeMaxLength, 0);
        Check.NotNull(approverRoleName, nameof(approverRoleName));
        Check.Length(approverRoleName, nameof(approverRoleName), ApprovalRouteConsts.ApproverRoleNameMaxLength, 0);
        Check.Length(entityType, nameof(entityType), ApprovalRouteConsts.EntityTypeMaxLength, 0);
        Check.Length(approver, nameof(approver), ApprovalRouteConsts.ApproverMaxLength, 0);
        Check.Length(notes, nameof(notes), ApprovalRouteConsts.NotesMaxLength, 0);
        StepSequence = stepSequence;
        ApproverRoleCode = approverRoleCode;
        ApproverRoleName = approverRoleName;
        IsApproved = isApproved;
        EntityType = entityType;
        InstanceId = instanceId;
        Approver = approver;
        ApprovalDate = approvalDate;
        Notes = notes;
    }

    public void Approve(DateTime approvalDate, string? note = null)
    {
        IsApproved = true;
        ApprovalDate = approvalDate;
        Notes = note;
    }
}