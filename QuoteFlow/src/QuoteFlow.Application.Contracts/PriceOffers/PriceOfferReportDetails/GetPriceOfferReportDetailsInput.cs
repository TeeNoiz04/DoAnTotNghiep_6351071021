using System;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.PriceOffers.PriceOfferReportDetails;
public class GetPriceOfferReportDetailsInput : PagedAndSortedResultRequestDto
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public string? GolfaCode { get; set; }
    public string? ModelName { get; set; }
    public string? Buyer { get; set; }
    public string? PriceOfferCode { get; set; }
    public string? PriceOfferName { get; set; }
    public string? MaterialGroup { get; set; }


}
