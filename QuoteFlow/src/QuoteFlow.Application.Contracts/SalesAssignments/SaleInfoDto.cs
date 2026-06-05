using QuoteFlow.Shared;
using System.Collections.Generic;

namespace QuoteFlow.SalesAssignments;

public class SaleInfoDto
{
    public string UserName { get; set; } = null!;
    public string? FullName { get; set; }
    public List<LookupDto<System.Guid>> Buyers { get; set; } = new();
    public List<LookupDto<System.Guid>> BuyerTypes { get; set; } = new();
    public List<string> MaterialTypes { get; set; } = new();
    public List<LookupDto<System.Guid>> Locations { get; set; } = new();
}
