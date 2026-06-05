using QuoteFlow.ApprovalHistories;
using QuoteFlow.ApprovalRoutes;
using System;

namespace QuoteFlow.PriceOffers;

public class PriceOfferApprovalRoute : ApprovalRoute
{
    public Guid PriceOfferId { get; set; }

    public PriceOfferApprovalRoute() : base() { }

    public PriceOfferApprovalRoute(Guid priceOfferId, Guid id, int stepSequence, string approverRoleCode, string approverRoleName, Guid instanceId, string approver, string? notes = null)
        : base(id, stepSequence, approverRoleCode, approverRoleName, false, EntityTypes.PriceOffer, instanceId, approver, null, notes)
    {
        PriceOfferId = priceOfferId;
    }
}
