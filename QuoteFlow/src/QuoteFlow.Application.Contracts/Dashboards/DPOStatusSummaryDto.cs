namespace QuoteFlow.Dashboards;

public class DPOStatusSummaryDto
{
    public long Submitted { get; set; }
    public long Confirmed { get; set; }
    public long LockedStock { get; set; }
    public long Cancelled { get; set; }
    public long Closed { get; set; }
    public long InProgress { get; set; }
}