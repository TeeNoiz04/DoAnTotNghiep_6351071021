using System;

namespace QuoteFlow.PriceOffers.Events;

public record PriceOfferSubmittedEvent(Guid PriceOfferId, DateTime SubmittedDate, string? Action);
