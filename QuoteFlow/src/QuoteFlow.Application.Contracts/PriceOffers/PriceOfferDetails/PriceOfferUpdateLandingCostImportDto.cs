using System;

namespace QuoteFlow.PriceOffers.PriceOfferDetails;

public class PriceOfferUpdateLandingCostImportDto
{
    public string? GolfaCode { get; set; }
    public string? ModelName { get; set; }
    public decimal? SaleOfferPrice { get; set; }
    public int? Qty { get; set; }
    public decimal? LandingCost { get; set; }
    public decimal? NewSaleOfferPrice { get; set; }
    // Internal properties for storing validation results
    public Guid? MaterialId { get; set; }
    public Guid? PriceOfferDetailId { get; set; }
    public Guid? PriceOfferId { get; set; }
}
