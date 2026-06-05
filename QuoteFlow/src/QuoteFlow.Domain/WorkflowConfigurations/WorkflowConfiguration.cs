using JetBrains.Annotations;
using QuoteFlow.Shared.Models;
using QuoteFlow.WorkflowApprovers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp;

namespace QuoteFlow.WorkflowConfigurations;

public class WorkflowConfiguration : ExtendedAuditedAggregateRoot<Guid>
{
    public Guid Id { get; set; }
    [NotNull]
    public virtual string WorkflowType { get; set; }

    public virtual short WorkflowLevel { get; set; }

    [NotNull]
    public virtual string WorkflowRole { get; set; }

    [CanBeNull]
    public virtual string? Condition { get; set; }

    [CanBeNull]
    public virtual string? Note { get; set; }

    [NotMapped]
    public virtual string? Approvers { get; set; }

    [NotMapped]

    public List<WorkflowApprover> WorkflowApprovers { get; set; }

    public WorkflowConfiguration()
    {

    }

    public WorkflowConfiguration(Guid id, string workflowType, short workflowLevel, string workflowRole, string? condition = null, string? note = null)
    {

        Id = id;
        Check.NotNull(workflowType, nameof(workflowType));
        Check.Length(workflowType, nameof(workflowType), WorkflowConfigurationConsts.WorkflowTypeMaxLength, 0);
        Check.NotNull(workflowRole, nameof(workflowRole));
        Check.Length(workflowRole, nameof(workflowRole), WorkflowConfigurationConsts.WorkflowRoleMaxLength, 0);
        Check.Length(condition, nameof(condition), WorkflowConfigurationConsts.ConditionMaxLength, 0);
        Check.Length(note, nameof(note), WorkflowConfigurationConsts.NoteMaxLength, 0);
        WorkflowType = workflowType;
        WorkflowLevel = workflowLevel;
        WorkflowRole = workflowRole;
        Condition = condition;
        Note = note;
    }

}