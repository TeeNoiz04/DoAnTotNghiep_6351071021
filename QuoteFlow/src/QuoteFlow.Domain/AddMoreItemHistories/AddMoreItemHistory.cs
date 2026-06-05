using JetBrains.Annotations;
using QuoteFlow.PriceOffers.PriceOfferDetails.ParameterObjects;
using QuoteFlow.Shared.Models;
using System;

namespace QuoteFlow.AddMoreItemHistories;
public class AddMoreItemHistory : ExtendedAuditedAggregateRoot<Guid>
{
    [CanBeNull]
    public virtual Guid? ImportGuid { get; set; }

    [CanBeNull]
    public virtual string? MaterialCode { get; set; }

    [CanBeNull]
    public virtual string? Model { get; set; }

    [CanBeNull]
    public virtual string? Spec1 { get; set; }

    [CanBeNull]
    public virtual string? Spec2 { get; set; }

    [CanBeNull]
    public virtual int? Qty { get; set; }

    [CanBeNull]
    public virtual decimal? StandardPriceToDist { get; set; }

    [CanBeNull]
    public virtual decimal? StandardAmount { get; set; }

    [CanBeNull]
    public virtual decimal? DistRequestedPrice { get; set; }

    [CanBeNull]
    public virtual decimal? RequestedAmount { get; set; }

    [CanBeNull]
    public virtual decimal? RequestedDiscount { get; set; }

    [CanBeNull]
    public virtual decimal? PriceToCustomer { get; set; }

    [CanBeNull]
    public virtual decimal? PriceOffer { get; set; }


    [CanBeNull]
    public virtual string? CometiorBrand { get; set; }

    [CanBeNull]
    public virtual string? CompetiorModel { get; set; }

    [CanBeNull]
    public virtual decimal? CompetiorPrice { get; set; }

    protected AddMoreItemHistory()
    {

    }

    public AddMoreItemHistory(Guid id, Guid importGuid, PriceOfferDetailCreateParams createParams)
    {
        Id = id;
        ImportGuid = importGuid;
        MaterialCode = createParams.GolfaCode;
        Model = createParams.ModelName;
        Spec1 = createParams.SpecialSpec1;
        Spec2 = createParams.SpecialSpec2;
        Qty = (int?)createParams.Qty;
        StandardPriceToDist = createParams.StandardPrice;
        StandardAmount = createParams.StandardAmount;
        DistRequestedPrice = createParams.BuyerPrice;
        RequestedAmount = createParams.RequestedAmount;
        RequestedDiscount = createParams.RequestedDiscountRatio;
        PriceToCustomer = createParams.PriceToCustomer;
        PriceOffer = createParams.MEVNOfferPrice;
        CometiorBrand = createParams.CompetitorBrand;
        CompetiorModel = createParams.CompetitorModel;
        CompetiorPrice = createParams.CompetitorPrice;

    }
}
