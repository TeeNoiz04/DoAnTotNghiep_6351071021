using System;

namespace QuoteFlow.SystemCategories;

public class SystemCategoryExcelDto
{
    public Guid? ParentId { get; set; }
    public string Code { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal? Value { get; set; }
    public string CategoryType { get; set; } = null!;
    public string? Note { get; set; }
    public bool IsDeactive { get; set; }
    public int SortOrder { get; set; }
}