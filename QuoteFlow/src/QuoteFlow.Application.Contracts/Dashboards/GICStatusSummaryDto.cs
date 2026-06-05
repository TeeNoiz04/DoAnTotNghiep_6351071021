namespace QuoteFlow.Dashboards;

public class GICStatusSummaryDto
{
    public long Confirmed { get; set; }
    public long LockedStock { get; set; }
    public long Cancelled { get; set; }
    public long InProgress { get; set; }
    public long Closed { get; set; }
}