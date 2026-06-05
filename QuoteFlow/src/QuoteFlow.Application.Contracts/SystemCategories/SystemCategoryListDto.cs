using QuoteFlow.Shared;
using System;

namespace QuoteFlow.SystemCategories;

public class SystemCategoryListDto : ExtendedAuditedEntityDto<Guid>
{
    public string Code { get; set; } = null!;
    public string? Description { get; set; }
    public string CategoryType { get; set; } = null!;
    public decimal? Value { get; set; }
    public bool IsDeactive { get; set; } = false;
    public string? Note { get; set; }
}
