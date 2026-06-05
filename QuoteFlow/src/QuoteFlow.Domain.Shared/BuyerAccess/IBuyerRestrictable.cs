using System;
using System.Collections.Generic;

namespace QuoteFlow.BuyerAccess;

public interface IBuyerRestrictable
{
    Guid? BuyerId { get; set; }
    List<Guid> RestrictedBuyerIds { get; set; }
}