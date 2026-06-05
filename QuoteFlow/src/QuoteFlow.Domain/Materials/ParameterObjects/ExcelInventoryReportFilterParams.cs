namespace QuoteFlow.Materials.ParameterObjects;

public class ExcelInventoryReportFilterParams
{
    public string? MaterialCode { get; set; }
    public string? InventoryCategory { get; set; }
    public string? MaterialGroup { get; set; }
    public bool Export { get; set; }
    public string? Sorting { get; set; } = null;
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = int.MaxValue;
}
