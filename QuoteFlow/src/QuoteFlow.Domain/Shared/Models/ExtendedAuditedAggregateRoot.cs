using JetBrains.Annotations;
using QuoteFlow.Shared.Interfaces;
using System;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Shared.Models;

public abstract class ExtendedAuditedAggregateRoot<TKey> : AggregateRoot<TKey>, IExtendedAuditedObject
{
    public virtual Guid? CreatorId { get; set; }

    [CanBeNull]
    public virtual string? CreatorUsername { get; set; }

    [CanBeNull]
    public virtual string? CreatorName { get; set; }

    [CanBeNull]
    public virtual DateTime? CreationTime { get; set; }


    public virtual Guid? LastModifierId { get; set; }

    [CanBeNull]
    public virtual string? LastModifierUsername { get; set; }

    [CanBeNull]
    public virtual string? LastModifierName { get; set; }

    [CanBeNull]
    public virtual DateTime? LastModificationTime { get; set; }
}