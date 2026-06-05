using System;

namespace QuoteFlow.StockTracingDetails;

[Serializable]
public class StockTracingDetailDownloadTokenCacheItem
{
    public string Token { get; set; } = null!;
}