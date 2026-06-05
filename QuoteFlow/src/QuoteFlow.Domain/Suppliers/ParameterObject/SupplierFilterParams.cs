namespace QuoteFlow.Suppliers.ParameterObject;

public class SupplierFilterParams
{
    public string? FilterText { get; set; }
    public string? SupplierCode { get; set; }
    public string? ShortName { get; set; }
    public string? FullName { get; set; }
    public string? TaxCode { get; set; }
    public string? Address { get; set; }
    public bool? IsDeactive { get; set; } = null;
    public string? Sorting { get; set; } = null;
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = int.MaxValue;
}
