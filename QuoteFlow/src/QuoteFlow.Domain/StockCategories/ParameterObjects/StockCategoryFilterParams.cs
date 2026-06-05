namespace QuoteFlow.StockCategories.ParameterObjects;
public class StockCategoryFilterParams
{
    public string? FilterText { get; set; }

    public string? StockCode { get; set; }
    public string? StockName { get; set; }
    public bool? MainStock { get; set; }
    public bool? FOC { get; set; }
    public bool? DamagedStock { get; set; }
    public int? SortOrderMin { get; set; }
    public int? SortOrderMax { get; set; }
    public bool? IsDeactive { get; set; }
    public string? Note { get; set; }

    public string? Sorting { get; set; } = null;
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = int.MaxValue;
}
