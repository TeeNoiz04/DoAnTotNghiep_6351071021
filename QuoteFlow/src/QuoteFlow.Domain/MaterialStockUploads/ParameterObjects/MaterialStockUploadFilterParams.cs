namespace QuoteFlow.MaterialStockUploads.ParameterObjects;
public class MaterialStockUploadFilterParams
{
    public string? GolfaCode { get; set; }
    public string? Model { get; set; }
    public string? ImportType { get; set; }
    public string? ApprovalStatus { get; set; }
    public string? Sorting { get; set; } = null;
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = int.MaxValue;
}
