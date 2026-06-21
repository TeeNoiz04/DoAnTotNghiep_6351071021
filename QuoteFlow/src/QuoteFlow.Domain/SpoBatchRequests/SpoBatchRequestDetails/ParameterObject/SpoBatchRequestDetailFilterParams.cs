using System;

namespace QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails.Params;

public class SpoBatchRequestDetailFilterParams
{
    public Guid? RequestId { get; set; }
    public string? SPOCode { get; set; }
    public string? GolfaCode { get; set; }
    public string? Action { get; set; }
    public DateTime? ActionDateMin { get; set; }
    public DateTime? ActionDateMax { get; set; }
    public string? Note { get; set; }
    public string? Sorting { get; set; } = null;
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = int.MaxValue;
}
