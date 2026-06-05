using System;

namespace QuoteFlow.PriceOffers.Events;

public record PriceOfferCreatedEvent(
    Guid PriceOfferId,
    bool ForceSubmit = false
);