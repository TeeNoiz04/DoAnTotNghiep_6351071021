using System;

namespace QuoteFlow.SystemCategories;

public class SystemCategoryExcelDownloadDto
{
    public string DownloadToken { get; set; } = null!;

    public string? FilterText { get; set; }

    public Guid? ParentId { get; set; }
    public string? Code { get; set; }
    public string? Description { get; set; }
    public decimal? ValueMin { get; set; }
    public decimal? ValueMax { get; set; }
    public string? CategoryType { get; set; }
    public string? Note { get; set; }
    public bool? IsDeactive { get; set; }
    public int? SortOrderMin { get; set; }
    public int? SortOrderMax { get; set; }

    public SystemCategoryExcelDownloadDto()
    {

    }
}