using System;

namespace QuoteFlow.PriceOffers.Events;

public record PriceOfferActionedEvent(
    Guid PriceOfferId,
    string Action,
    string ActionerUsername,
    DateTime ActionedAt,
    string? Comment = null
);
