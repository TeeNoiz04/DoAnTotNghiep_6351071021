using System;
using System.Collections.Generic;

namespace QuoteFlow.PriceOffers.PriceOfferDetails;

public class PriceOfferDetailCancelDto
{
    public List<Guid> PriceOfferDetailIds { get; set; } = new();
    public string Note { get; set; } = string.Empty;
}