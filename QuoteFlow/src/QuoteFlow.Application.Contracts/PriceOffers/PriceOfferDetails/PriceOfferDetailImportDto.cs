namespace QuoteFlow.PriceOffers.PriceOfferDetails;

public class PriceOfferDetailImportDto
{
    public int RowNo { get; set; }
    public string? GolfaCode { get; set; } = null!; // Material Code
    public string? ModelName { get; set; } = null!;
    public string? SpecialSpec1 { get; set; }
    public string? SpecialSpec2 { get; set; }
    //public decimal? DpoUsed { get; set; }
    public decimal? Qty { get; set; }
    public decimal? StandardPrice { get; set; }
    public decimal? StandardAmount { get; set; }
    public decimal? BuyerPrice { get; set; }
    public decimal? RequestedAmount { get; set; }
    public decimal? RequestedDiscountRatio { get; set; }
    public decimal? PriceToCustomer { get; set; }
    public decimal? MEVNOfferPrice { get; set; }
    public decimal? MEVNOfferAmount { get; set; }
    public string? CompetitorBrand { get; set; }
    public string? CompetitorModel { get; set; }
    public decimal? CompetitorPrice { get; set; }
    //public decimal? LandingCost { get; set; } // Get From Materials by Material Code => LandedCost
    //public decimal? ManagerMargin { get; set; } // ...
    //public decimal? PriceOfferDetailMargin { get; set; } // based on doc to calculate this value
}
