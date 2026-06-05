using System;

namespace QuoteFlow.Materials;

[Serializable]
public class MaterialDownloadTokenCacheItem
{
    public string Token { get; set; } = null!;
}