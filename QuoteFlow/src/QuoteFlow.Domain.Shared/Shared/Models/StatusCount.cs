namespace QuoteFlow.Shared.Models;

public class StatusCount
{
    public string Status { get; set; } = string.Empty;
    public long Count { get; set; }
    public StatusCount(string status, long count)
    {
        Status = status;
        Count = count;
    }
    public StatusCount()
    {

    }
}
