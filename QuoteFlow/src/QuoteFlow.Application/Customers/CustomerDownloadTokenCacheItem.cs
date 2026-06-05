using System;

namespace QuoteFlow.Customers;

[Serializable]
public class CustomerDownloadTokenCacheItem
{
    public string Token { get; set; } = null!;
}