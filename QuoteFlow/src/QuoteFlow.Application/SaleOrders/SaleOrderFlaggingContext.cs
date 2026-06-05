using QuoteFlow.Shared.Flagging;
using System.Collections.Generic;

namespace QuoteFlow.SaleOrders;

public class SaleOrderFlaggingContext : BaseFlaggingContext
{

    public SaleOrderFlaggingContext()
    {
    }

    public SaleOrderFlaggingContext(string currentUsername, Dictionary<string, object>? additionalData = null)
        : base(currentUsername, [], additionalData)
    {
    }

    public SaleOrderFlaggingContext(string currentUsername, IEnumerable<string> roles, Dictionary<string, object>? additionalData = null)
        : base(currentUsername, roles, additionalData)
    {
    }
}