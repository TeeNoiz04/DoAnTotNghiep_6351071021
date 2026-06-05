namespace QuoteFlow.Materials;

public class MaterialExcelDto
{
    public string GolfaCode { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string? SAPCode { get; set; }
    public string? DescriptionEN { get; set; }
    public string? DescriptionVN { get; set; }
    public string? DescriptionGroup { get; set; }
    public string? Spec1 { get; set; }
    public string? Spec2 { get; set; }
    public string? Spec3 { get; set; }
    public string? Spec4 { get; set; }
    public string? Unit { get; set; }
    public string? Origin { get; set; }
    public string? Kind { get; set; }
    public string? MaterialType { get; set; }
    public string MaterialStatus { get; set; } = null!;
    public string? ProductHierarchy { get; set; }
    public string? MaterialGroup { get; set; }
    //public Guid? Factory { get; set; }
    //public Guid? Vendor { get; set; }
    public bool EPA { get; set; }
    public int? LeadTime { get; set; }
    public int? Maxlot { get; set; }
    public int? StockValueWarning { get; set; }
    public string? HSCode { get; set; }
    public string? INCOTERMS { get; set; }
    public decimal? ImportDuty { get; set; }
    public decimal? RefExchangeRate { get; set; }
    public decimal? InputPrice { get; set; }
    public string? Currency { get; set; }
    public decimal StandardPrice { get; set; }
    public decimal? BuyerPrice1 { get; set; }
    public decimal? BuyerPrice2 { get; set; }
    public decimal? BuyerPrice3 { get; set; }
    public decimal? BuyerPrice4 { get; set; }
    public decimal? BuyerPrice5 { get; set; }
    public decimal? VAT { get; set; }
    public decimal? LandingCost { get; set; }
    public decimal? MarginAllowSale { get; set; }
    public decimal? MarginAllowManager { get; set; }
    public string? Note { get; set; }
    public string? MaterialClass { get; set; }
}