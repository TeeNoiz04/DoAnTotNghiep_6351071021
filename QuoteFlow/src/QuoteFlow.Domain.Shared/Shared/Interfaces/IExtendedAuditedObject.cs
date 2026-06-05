using System;

namespace QuoteFlow.Shared.Interfaces;

public interface IExtendedAuditedObject
{
    Guid? CreatorId { get; set; }

    string? CreatorUsername { get; set; }

    string? CreatorName { get; set; }

    DateTime? CreationTime { get; set; }


    Guid? LastModifierId { get; set; }

    string? LastModifierUsername { get; set; }

    string? LastModifierName { get; set; }

    DateTime? LastModificationTime { get; set; }
}