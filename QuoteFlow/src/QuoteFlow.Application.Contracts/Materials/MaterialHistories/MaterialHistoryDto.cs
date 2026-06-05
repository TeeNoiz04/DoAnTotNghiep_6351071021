using QuoteFlow.Shared;
using System;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Materials.MaterialHistories;

public class MaterialHistoryDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public Guid MaterialId { get; set; }
    public string? Action { get; set; }
    public string? Note { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;

}