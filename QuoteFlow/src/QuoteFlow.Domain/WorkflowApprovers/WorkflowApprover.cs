using JetBrains.Annotations;
using QuoteFlow.Shared.Models;
using System;
using Volo.Abp;

namespace QuoteFlow.WorkflowApprovers;

public class WorkflowApprover : ExtendedAuditedAggregateRoot<Guid>
{
    public virtual Guid WFId { get; set; }

    [NotNull]
    public virtual string Approver { get; set; }

    [CanBeNull]
    public virtual string? Note { get; set; }

    protected WorkflowApprover()
    {

    }

    public WorkflowApprover(Guid id, Guid wFId, string approver, string? note = null)
    {

        Id = id;
        Check.NotNull(approver, nameof(approver));
        Check.Length(approver, nameof(approver), WorkflowApproverConsts.ApproverMaxLength, 0);
        Check.Length(note, nameof(note), WorkflowApproverConsts.NoteMaxLength, 0);
        WFId = wFId;
        Approver = approver;
        Note = note;
    }

}