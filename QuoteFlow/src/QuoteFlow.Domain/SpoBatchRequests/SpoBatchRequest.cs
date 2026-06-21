using JetBrains.Annotations;
using QuoteFlow.Shared.Models;
using QuoteFlow.SpoBatchRequests.ParameterObject;
using QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails;
using System;
using System.Collections.Generic;

namespace QuoteFlow.SpoBatchRequests;

public class SpoBatchRequest : ExtendedAuditedAggregateRoot<Guid>
{
    [NotNull]
    public virtual string RequestNo { get; set; }

    [NotNull]
    public virtual string ImportType { get; set; }

    [CanBeNull]
    public virtual string? FileName { get; set; }

    [CanBeNull]
    public virtual string? Note { get; set; }

    [CanBeNull]
    public virtual string? Status { get; set; }
    public virtual bool? IsDeleted { get; set; } = false;

    public List<SpoBatchRequestDetail> SpoBatchRequestDetails { get; set; }

    protected SpoBatchRequest()
    {

    }

    public SpoBatchRequest(Guid id, string code, SpoBatchRequestCreateParams input)
    {


        Id = id;
        RequestNo = code;
        ImportType = input.ImportType;
        FileName = input.FileName;
        Note = input.Note;
        Status = input.Status;
        IsDeleted = false;
    }

}