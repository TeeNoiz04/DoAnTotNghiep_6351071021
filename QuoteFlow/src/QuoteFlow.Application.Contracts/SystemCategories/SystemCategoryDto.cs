using QuoteFlow.Shared;
using System;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.SystemCategories;

public class SystemCategoryDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public Guid? ParentId { get; set; }
    public string Code { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal? Value { get; set; }
    public string CategoryType { get; set; } = null!;
    public string? Note { get; set; }
    public bool IsDeactive { get; set; }
    public int SortOrder { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}