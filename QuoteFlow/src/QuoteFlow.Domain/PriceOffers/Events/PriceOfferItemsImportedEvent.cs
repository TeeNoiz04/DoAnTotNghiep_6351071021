using System;

namespace QuoteFlow.PriceOffers.Events;

public record PriceOfferItemsImportedEvent(Guid PriceOfferId, Guid ImportGuid, string? Comment);
