namespace QuoteFlow.Shared;

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayCode { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
}