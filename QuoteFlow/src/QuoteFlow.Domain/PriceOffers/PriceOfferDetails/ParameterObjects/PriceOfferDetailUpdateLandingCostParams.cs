using System;

namespace QuoteFlow.PriceOffers.PriceOfferDetails.ParameterObjects;

public class PriceOfferDetailUpdateLandingCostParams
{
    public Guid Id { get; set; }
    public int? Qty { get; set; }
    public decimal? LandingCost { get; set; }
    public decimal? MEVNOfferPrice { get; set; }
    public string? ConcurrencyStamp { get; set; }
}
