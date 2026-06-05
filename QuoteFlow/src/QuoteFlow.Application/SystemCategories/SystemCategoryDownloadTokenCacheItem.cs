using System;

namespace QuoteFlow.SystemCategories;

[Serializable]
public class SystemCategoryDownloadTokenCacheItem
{
    public string Token { get; set; } = null!;
}