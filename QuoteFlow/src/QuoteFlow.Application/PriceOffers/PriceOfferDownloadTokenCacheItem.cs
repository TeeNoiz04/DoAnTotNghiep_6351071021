using System;

namespace QuoteFlow.PriceOffers;

[Serializable]
public class PriceOfferDownloadTokenCacheItem
{
    public string Token { get; set; } = null!;
}