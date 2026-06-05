namespace QuoteFlow.SpecialInputPrices;
public class SpecialInputPriceDetailImportDto
{
    public int? No { get; set; }
    public string? AccountNo { get; set; }

    public string? MaterialCode { get; set; }
    public string? ModelName { get; set; }
    public string? Spec { get; set; }
    public int? LimitQty { get; set; }
    public decimal? SpecialInputPrice { get; set; }
    public decimal? LandedCost { get; set; }
}
