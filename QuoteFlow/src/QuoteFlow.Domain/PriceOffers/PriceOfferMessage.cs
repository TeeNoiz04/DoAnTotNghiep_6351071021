using QuoteFlow.Messages;
using System;
using System.Collections.Generic;

namespace QuoteFlow.PriceOffers;
public class PriceOfferMessage : Message
{
    public Guid PriceOfferId { get; set; }

    protected PriceOfferMessage()
    {
    }

    public PriceOfferMessage(Guid id, Guid priceOfferId, string userName, string fullName, IEnumerable<string> sendToEmails, string comment)
        : base(id, userName, fullName, sendToEmails, comment)
    {
        PriceOfferId = priceOfferId;
    }
}
