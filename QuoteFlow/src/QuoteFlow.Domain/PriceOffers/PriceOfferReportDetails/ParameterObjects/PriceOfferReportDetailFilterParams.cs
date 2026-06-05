using System;

namespace QuoteFlow.PriceOffers.PriceOfferReportDetails.ParameterObjects;
public class PriceOfferReportDetailFilterParams
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }

    public string? GolfaCode { get; set; }
    public string? ModelName { get; set; }
    public string? Buyer { get; set; }
    public string? PriceOfferCode { get; set; }
    public string? PriceOfferName { get; set; }
    public string? MaterialGroup { get; set; }
    public bool HasFullBuyerAccess { get; set; }
    public bool HasStrategicPriceAccess { get; set; }
    public string UserName { get; set; } = string.Empty;
}
