using JetBrains.Annotations;
using QuoteFlow.Shared.Models;
using QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails.ParameterObject;
using System;

namespace QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails;

public class SpoBatchRequestDetail : ExtendedAuditedAggregateRoot<Guid>
{
    public virtual Guid RequestId { get; set; }

    [CanBeNull]
    public virtual string? SPOCode { get; set; }

    [CanBeNull]
    public virtual string? GolfaCode { get; set; }

    [CanBeNull]
    public virtual string? Action { get; set; }

    public virtual DateTime? ActionDate { get; set; }

    [CanBeNull]
    public virtual string? Note { get; set; }
    public virtual string? Status { get; set; }
    public virtual bool? IsDeleted { get; set; } = false;
    protected SpoBatchRequestDetail()
    {

    }

    public SpoBatchRequestDetail(
     Guid id,
     SpoBatchRequestDetailCreateParams input)
    {
        Id = id;

        RequestId = input.RequestId;
        SPOCode = input.SPOCode;
        GolfaCode = input.GolfaCode;
        Action = input.Action;
        ActionDate = input.ActionDate;
        Note = input.Note;
        IsDeleted = false;
        Status = "NOT_START";
    }


}