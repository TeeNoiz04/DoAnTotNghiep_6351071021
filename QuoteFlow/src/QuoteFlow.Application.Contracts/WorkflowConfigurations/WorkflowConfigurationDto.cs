using QuoteFlow.Shared;
using QuoteFlow.WorkflowApprovers;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.WorkflowConfigurations;

public class WorkflowConfigurationDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string WorkflowType { get; set; } = null!;
    public short WorkflowLevel { get; set; }
    public string WorkflowRole { get; set; } = null!;
    public string? Condition { get; set; }
    public string? Note { get; set; }

    public virtual string? Approvers { get; set; }



    public List<WorkflowApproverDto>? WorkflowApprovers { get; set; }
    public string ConcurrencyStamp { get; set; } = null!;

}