using System;

namespace QuoteFlow.DPOs;

[Serializable]
public class DPODownloadTokenCacheItem
{
    public string Token { get; set; } = null!;
}