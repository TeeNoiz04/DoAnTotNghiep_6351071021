using QuoteFlow.Shared.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuoteFlow.Shared.Models;


public class ExtendedFullAuditedAggregateRoot<TKey> : ExtendedAuditedAggregateRoot<TKey>, IExtendedFullAuditedObject
{
    public Guid? DeleterId { get; set; }

    public string? DeleterUsername { get; set; }

    public string? DeleterName { get; set; }

    public DateTime? DeletionTime { get; set; }

    public bool IsDeleted { get; set; }

    [NotMapped]
    public bool ForceDelete { get; set; }
}