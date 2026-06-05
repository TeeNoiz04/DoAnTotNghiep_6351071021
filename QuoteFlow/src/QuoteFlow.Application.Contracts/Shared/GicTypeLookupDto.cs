using System.Collections.Generic;

namespace QuoteFlow.Shared;

public class GicTypeLookupDto<TKey>
{
    public TKey Id { get; set; } = default!;
    public string DisplayCode { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public bool HasProcess { get; set; }
    public List<LookupDto<TKey>> Processes { get; set; } = new();
}