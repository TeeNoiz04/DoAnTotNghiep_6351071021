namespace QuoteFlow.CfgDiscountRatios.ParameterObjects;
public class CfgDiscountRatioFilterParams
{
    public string? FilterText { get; set; }

    public string? Approval_Type { get; set; }
    public string? Product_Type { get; set; }
    public string? AccountClassify { get; set; }
    public string? KAType { get; set; }
    public decimal? Value_MinMin { get; set; }
    public decimal? Value_MinMax { get; set; }
    public decimal? Value_MaxMin { get; set; }
    public decimal? Value_MaxMax { get; set; }
    public decimal? DiscountRatioMin { get; set; }
    public decimal? DiscountRatioMax { get; set; }
    public string? Note { get; set; }

    public int MaxResultCount { get; set; }
    public string? Sorting { get; set; }
    public int SkipCount { get; set; }
}
