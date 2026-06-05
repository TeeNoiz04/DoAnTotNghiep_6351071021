using System;

namespace QuoteFlow.DistributorTargets;

[Serializable]
public class DistributorTargetDownloadTokenCacheItem
{
    public string Token { get; set; } = null!;
}