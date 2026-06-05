using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.PriceOffers.PriceOfferDetails.ParameterObjects;

public class PriceOfferDetailCreateParams
{
    public virtual int RowNo { get; set; }

    [Required]
    public Guid PriceOfferId { get; set; }

    [Required]
    [StringLength(PriceOfferDetailConsts.GolfaCodeMaxLength)]
    public string GolfaCode { get; set; }

    [Required]
    [StringLength(PriceOfferDetailConsts.ModelNameMaxLength)]
    public string ModelName { get; set; }

    [Required]
    public decimal Qty { get; set; }

    [Required]
    public decimal StandardPrice { get; set; }

    [Required]
    public decimal StandardAmount { get; set; }

    [Required]
    public decimal MEVNOfferPrice { get; set; }

    [Required]
    public Guid ImportGuid { get; set; }

    [StringLength(PriceOfferDetailConsts.SpecialSpec1MaxLength)]
    public string? SpecialSpec1 { get; set; }

    [StringLength(PriceOfferDetailConsts.SpecialSpec2MaxLength)]
    public string? SpecialSpec2 { get; set; }

    public decimal? DpoUsed { get; set; } = 0;

    public decimal? BuyerPrice { get; set; }

    public decimal? RequestedAmount { get; set; }

    public decimal? RequestedDiscountRatio { get; set; }

    public decimal? PriceToCustomer { get; set; }

    [StringLength(PriceOfferDetailConsts.CompetitorBrandMaxLength)]
    public string? CompetitorBrand { get; set; }

    [StringLength(PriceOfferDetailConsts.CompetitorModelMaxLength)]
    public string? CompetitorModel { get; set; }

    public decimal? CompetitorPrice { get; set; }

    public decimal? LandingCost { get; set; }

    public decimal? InputPrice { get; set; }

    [StringLength(PriceOfferDetailConsts.InputCurrencyMaxLength)]
    public string? InputCurrency { get; set; }

    public decimal? ManagerMargin { get; set; }

    public decimal? PriceOfferDetailMargin { get; set; }

    [StringLength(PriceOfferDetailConsts.AccountCodeMaxLength)]
    public string? AccountCode { get; set; }

    [StringLength(PriceOfferDetailConsts.NoteMaxLength)]
    public string? Note { get; set; }

    protected PriceOfferDetailCreateParams()
    {

    }

    public PriceOfferDetailCreateParams(
        int rowNo,
        Guid priceOfferId,
        string golfaCode,
        string modelName,
        decimal qty,
        decimal standardPrice,
        decimal standardAmount,
        decimal mevnOfferPrice,
        Guid importGuid,
        string? specialSpec1 = null,
        string? specialSpec2 = null,
        decimal? buyerPrice = null,
        decimal? requestedAmount = null,
        decimal? requestedDiscountRatio = null,
        decimal? priceToCustomer = null,
        string? competitorBrand = null,
        string? competitorModel = null,
        decimal? competitorPrice = null,
        decimal? landingCost = null,
        decimal? inputPrice = null,
        string? inputCurrency = null,
        decimal? managerMargin = null,
        decimal? priceOfferDetailMargin = null,
        string? accountCode = null,
        decimal? dpoUsed = null,
        string? note = null)
    {
        RowNo = rowNo;
        PriceOfferId = priceOfferId;
        GolfaCode = golfaCode;
        ModelName = modelName;
        Qty = qty;
        StandardPrice = standardPrice;
        StandardAmount = standardAmount;
        MEVNOfferPrice = mevnOfferPrice;
        ImportGuid = importGuid;

        SpecialSpec1 = specialSpec1;
        SpecialSpec2 = specialSpec2;
        DpoUsed = dpoUsed;
        BuyerPrice = buyerPrice;
        RequestedAmount = requestedAmount;
        RequestedDiscountRatio = requestedDiscountRatio;
        PriceToCustomer = priceToCustomer;
        CompetitorBrand = competitorBrand;
        CompetitorModel = competitorModel;
        CompetitorPrice = competitorPrice;
        LandingCost = landingCost;
        InputPrice = inputPrice;
        InputCurrency = inputCurrency;
        ManagerMargin = managerMargin;
        PriceOfferDetailMargin = priceOfferDetailMargin;
        AccountCode = accountCode;
        Note = note;
    }
}
