using QuoteFlow.Shared.Interfaces;
using System;

namespace QuoteFlow.Shared;


public class ExtendedFullAuditedEntityDto<TKey> : ExtendedAuditedEntityDto<TKey>, IExtendedFullAuditedObject
{
    public Guid? DeleterId { get; set; }

    public string? DeleterUsername { get; set; }

    public string? DeleterName { get; set; }

    public DateTime? DeletionTime { get; set; }

    public bool IsDeleted { get; set; }
    public bool ForceDelete { get; set; }
}