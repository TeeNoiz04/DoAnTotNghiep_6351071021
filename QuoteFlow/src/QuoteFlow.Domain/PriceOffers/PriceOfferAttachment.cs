using QuoteFlow.ApprovalHistories;
using QuoteFlow.Attachments;
using QuoteFlow.Attachments.ParameterObjects;
using System;

namespace QuoteFlow.PriceOffers;
public class PriceOfferAttachment : Attachment
{
    public Guid PriceOfferId { get; set; }

    protected PriceOfferAttachment() : base()
    {

    }

    public PriceOfferAttachment(Guid id, Guid priceOfferId, AttachmentCreateParams createParams) : base(id, createParams)
    {
        PriceOfferId = priceOfferId;
        AttachCode = "PP";
        AttachName = EntityTypes.PriceOffer;
    }
}
