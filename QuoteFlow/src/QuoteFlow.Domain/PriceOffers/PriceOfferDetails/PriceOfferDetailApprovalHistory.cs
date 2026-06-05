using QuoteFlow.ApprovalHistories;
using QuoteFlow.ApprovalHistories.ParameterObjects;
using System;

namespace QuoteFlow.PriceOffers.PriceOfferDetails;

public class PriceOfferDetailApprovalHistory : ApprovalHistory
{
    public Guid PriceOfferDetailId { get; set; }

    public Guid? ImportGuid { get; set; }
    protected PriceOfferDetailApprovalHistory() : base()
    {
    }
    public PriceOfferDetailApprovalHistory(Guid id, Guid priceOfferDetailId, ApprovalHistoryCreateParams createParams)
        : base(id, createParams)
    {
        PriceOfferDetailId = priceOfferDetailId;
        EntityType = EntityTypes.PriceOfferDetail;
    }

    public PriceOfferDetailApprovalHistory(Guid id, Guid? importGuid, Guid priceOfferDetailId, ApprovalHistoryCreateParams createParams)
        : base(id, createParams)
    {
        PriceOfferDetailId = priceOfferDetailId;
        ImportGuid = importGuid;
        EntityType = EntityTypes.PriceOfferDetail;
    }
}
