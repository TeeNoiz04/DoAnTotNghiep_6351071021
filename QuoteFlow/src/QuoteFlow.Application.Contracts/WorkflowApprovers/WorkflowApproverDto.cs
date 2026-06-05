using QuoteFlow.Shared;
using System;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.WorkflowApprovers;

public class WorkflowApproverDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public Guid WFId { get; set; }
    public string Approver { get; set; } = null!;
    public string? Note { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;

}