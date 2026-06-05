using System;

namespace QuoteFlow.StockTracings;

[Serializable]
public class StockTracingDownloadTokenCacheItem
{
    public string Token { get; set; } = null!;
}