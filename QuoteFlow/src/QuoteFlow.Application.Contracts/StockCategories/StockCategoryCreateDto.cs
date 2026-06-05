using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.StockCategories;

public class StockCategoryCreateDto
{
    [Required]
    [StringLength(StockCategoryConsts.StockCodeMaxLength)]
    public string StockCode { get; set; } = null!;
    [Required]
    [StringLength(StockCategoryConsts.StockNameMaxLength)]
    public string StockName { get; set; } = null!;
    [StringLength(StockCategoryConsts.SAPCodeMaxLength)]
    public string? SAPCode { get; set; }
    public bool? MainStock { get; set; }
    public bool? FOC { get; set; }
    public bool? DamagedStock { get; set; }
    public int? SortOrder { get; set; }
    public bool? IsDeactive { get; set; }
    [StringLength(StockCategoryConsts.NoteMaxLength)]
    public string? Note { get; set; }
}