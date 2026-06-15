using System;

namespace QuoteFlow.SupplierBUs;

[Serializable]
public class SupplierBUDownloadTokenCacheItem
{
    public string Token { get; set; } = null!;
}