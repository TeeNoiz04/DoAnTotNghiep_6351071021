using System;

namespace QuoteFlow.PriceOffers.PriceOfferReportGenerals.ParameterObjects;
public class PriceOfferReportGeneralFilterParams
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public string? Buyer { get; set; }
    public string? CustomerName { get; set; }
    public string? PriceOfferCode { get; set; }
    public string? PriceOfferName { get; set; }
    public string? Location { get; set; }
    public string? Status { get; set; }
    public string? MaterialType { get; set; }
    public decimal? OrderMin { get; set; }
    public decimal? OrderMax { get; set; }
    public bool HasFullBuyerAccess { get; set; }
    public bool HasStrategicPriceAccess { get; set; }
    public string UserName { get; set; } = string.Empty;
}
