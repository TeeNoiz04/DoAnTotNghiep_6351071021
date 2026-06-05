using System;

namespace QuoteFlow.DPOs.DPODetails;

[Serializable]
public class DPODetailDownloadTokenCacheItem
{
    public string Token { get; set; } = null!;
}