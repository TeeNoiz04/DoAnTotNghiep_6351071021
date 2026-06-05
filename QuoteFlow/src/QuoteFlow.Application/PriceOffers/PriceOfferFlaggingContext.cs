using QuoteFlow.Shared.Flagging;
using System.Collections.Generic;

namespace QuoteFlow.PriceOffers;

public class PriceOfferFlaggingContext : BaseFlaggingContext
{

    public PriceOfferFlaggingContext()
    {
    }

    public PriceOfferFlaggingContext(string currentUsername, Dictionary<string, object>? additionalData = null)
        : base(currentUsername, [], additionalData)
    {
    }

    public PriceOfferFlaggingContext(string currentUsername, IEnumerable<string> roles, Dictionary<string, object>? additionalData = null)
        : base(currentUsername, roles, additionalData)
    {
    }
}
