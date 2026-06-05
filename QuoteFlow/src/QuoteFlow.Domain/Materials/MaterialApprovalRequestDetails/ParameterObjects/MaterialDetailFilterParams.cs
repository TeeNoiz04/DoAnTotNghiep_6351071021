namespace QuoteFlow.Materials.MaterialApprovalRequestDetails.ParameterObjects;

public class MaterialDetailFilterParams
{
    public string? GolfaCode { get; set; }
    public string? Model { get; set; }
    public string? SAPCode { get; set; }
    public string? MaterialType { get; set; }
    public string? MaterialGroup { get; set; }
    public string? SupplierBU { get; set; }
    public string? Supplier { get; set; }
    public string? Sorting { get; set; } = null;
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = int.MaxValue;
}
