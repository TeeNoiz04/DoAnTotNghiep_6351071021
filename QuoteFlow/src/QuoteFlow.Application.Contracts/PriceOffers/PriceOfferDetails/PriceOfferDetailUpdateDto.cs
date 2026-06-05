using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.PriceOffers.PriceOfferDetails;

public class PriceOfferDetailUpdateDto
{
    [Required]
    public Guid PriceOfferId { get; set; }
    [Required]
    [StringLength(PriceOfferDetailConsts.GolfaCodeMaxLength)]
    public string GolfaCode { get; set; } = null!;
    [Required]
    [StringLength(PriceOfferDetailConsts.ModelNameMaxLength)]
    public string ModelName { get; set; } = null!;
    [StringLength(PriceOfferDetailConsts.SpecialSpec1MaxLength)]
    public string? SpecialSpec1 { get; set; }
    [StringLength(PriceOfferDetailConsts.SpecialSpec2MaxLength)]
    public string? SpecialSpec2 { get; set; }
    public decimal? DpoUsed { get; set; }
    [Required]
    public decimal Qty { get; set; }
    [Required]
    public decimal StandardPrice { get; set; }
    [Required]
    public decimal StandardAmount { get; set; }
    public decimal? BuyerPrice { get; set; }
    public decimal? RequestedAmount { get; set; }
    public decimal? RequestedDiscountRatio { get; set; }
    public decimal? PriceToCustomer { get; set; }
    [Required]
    public decimal MEVNOfferPrice { get; set; }
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
    [Required]
    public Guid ImportGuid { get; set; }

}