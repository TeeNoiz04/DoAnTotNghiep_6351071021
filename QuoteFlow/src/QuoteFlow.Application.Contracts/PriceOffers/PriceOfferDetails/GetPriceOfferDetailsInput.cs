using System;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.PriceOffers.PriceOfferDetails;

public class GetPriceOfferDetailsInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public string? GolfaCode { get; set; }
    public string? ModelName { get; set; }
    public string? SpecialSpec1 { get; set; }
    public string? SpecialSpec2 { get; set; }
    public decimal? DpoUsedMin { get; set; }
    public decimal? DpoUsedMax { get; set; }
    public decimal? QtyMin { get; set; }
    public decimal? QtyMax { get; set; }
    public decimal? StandardPriceMin { get; set; }
    public decimal? StandardPriceMax { get; set; }
    public decimal? StandardAmountMin { get; set; }
    public decimal? StandardAmountMax { get; set; }
    public decimal? BuyerPriceMin { get; set; }
    public decimal? BuyerPriceMax { get; set; }
    public decimal? RequestedAmountMin { get; set; }
    public decimal? RequestedAmountMax { get; set; }
    public decimal? RequestedDiscountRatioMin { get; set; }
    public decimal? RequestedDiscountRatioMax { get; set; }
    public decimal? PriceToCustomerMin { get; set; }
    public decimal? PriceToCustomerMax { get; set; }
    public decimal? MEVNOfferPriceMin { get; set; }
    public decimal? MEVNOfferPriceMax { get; set; }
    public string? CompetitorBrand { get; set; }
    public string? CompetitorModel { get; set; }
    public decimal? CompetitorPriceMin { get; set; }
    public decimal? CompetitorPriceMax { get; set; }
    public decimal? LandingCostMin { get; set; }
    public decimal? LandingCostMax { get; set; }
    public decimal? InputPriceMin { get; set; }
    public decimal? InputPriceMax { get; set; }
    public string? InputCurrency { get; set; }
    public decimal? ManagerMarginMin { get; set; }
    public decimal? ManagerMarginMax { get; set; }
    public decimal? PriceOfferDetailMarginMin { get; set; }
    public decimal? PriceOfferDetailMarginMax { get; set; }
    public string? AccountCode { get; set; }
    public string? Note { get; set; }
    public Guid? ImportGuid { get; set; }

    public GetPriceOfferDetailsInput()
    {

    }
}