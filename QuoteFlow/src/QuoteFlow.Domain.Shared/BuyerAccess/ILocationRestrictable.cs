using System;
using System.Collections.Generic;

namespace QuoteFlow.BuyerAccess;

public interface ILocationRestrictable
{
    Guid? LocationId { get; set; }
    List<Guid> RestrictedLocationIds { get; set; }
}
