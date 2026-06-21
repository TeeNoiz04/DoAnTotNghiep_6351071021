namespace QuoteFlow.SpoBatchRequests.ParameterObject;

public class SpoBatchRequestFilterParams
{

    public string? RequestNo { get; set; }
    public string? ImportType { get; set; }
    public string? FileName { get; set; }
    public string? Note { get; set; }
    public string? Status { get; set; }
    public string? SPOCode { get; set; }
    public string? GolfaCode { get; set; }
    public string? Sorting { get; set; } = null;
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = int.MaxValue;
}
