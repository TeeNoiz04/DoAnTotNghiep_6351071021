using QuoteFlow.ApprovalHistories;
using QuoteFlow.ApprovalHistories.ParameterObjects;
using System;

namespace QuoteFlow.PriceOffers;

public class PriceOfferApprovalHistory : ApprovalHistory
{
    public Guid PriceOfferId { get; set; }
    public Guid? ImportGuid { get; set; }
    protected PriceOfferApprovalHistory() : base()
    {
    }
    public PriceOfferApprovalHistory(Guid id, Guid priceOfferId, ApprovalHistoryCreateParams createParams)
        : base(id, createParams)
    {
        PriceOfferId = priceOfferId;
        EntityType = EntityTypes.PriceOffer;
    }
    public PriceOfferApprovalHistory(Guid id, Guid? importGuid, Guid priceOfferId, ApprovalHistoryCreateParams createParams)
        : base(id, createParams)
    {
        PriceOfferId = priceOfferId;
        ImportGuid = importGuid;
        EntityType = EntityTypes.PriceOffer;
    }
}
