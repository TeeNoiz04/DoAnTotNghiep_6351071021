using JetBrains.Annotations;
using QuoteFlow.Shared.Extensions;
using System;

namespace QuoteFlow.Emailing.EmailInfoModel;

public class ApprovalHistoryEmailInfo
{
    [CanBeNull]
    public virtual string? ApproverRoleName { get; set; }

    [CanBeNull]
    public virtual string? ApproverFullName { get; set; }

    public virtual string Action { get; set; }

    public virtual string ActionDate { get; set; }

    [CanBeNull]
    public virtual string? Note { get; set; }

    public ApprovalHistoryEmailInfo(
        string action,
        DateTime actionDate,
        string approverRoleName,
        string approverFullName,
        string? note = null
    )
    {
        Action = action;
        ActionDate = actionDate.ToStandardString();
        ApproverRoleName = approverRoleName;
        ApproverFullName = approverFullName;
        Note = note;
    }

    public ApprovalHistoryEmailInfo()
    {

    }
}
