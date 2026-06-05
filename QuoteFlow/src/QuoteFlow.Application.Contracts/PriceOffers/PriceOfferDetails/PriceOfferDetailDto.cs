using QuoteFlow.ApprovalHistories;
using QuoteFlow.Shared;
using System;
using System.Collections.Generic;

namespace QuoteFlow.PriceOffers.PriceOfferDetails;

public class PriceOfferDetailDto : ExtendedAuditedEntityDto<Guid>
{
    public virtual int RowNo { get; set; }
    public Guid PriceOfferId { get; set; }
    public string GolfaCode { get; set; } = null!;
    public string ModelName { get; set; } = null!;
    public string? SpecialSpec1 { get; set; }
    public string? SpecialSpec2 { get; set; }
    public decimal? DpoUsed { get; set; }
    public decimal Qty { get; set; }
    public decimal StandardPrice { get; set; }
    public decimal StandardAmount { get; set; }
    public decimal? BuyerPrice { get; set; }
    public decimal? RequestedAmount { get; set; }
    public decimal? RequestedDiscountRatio { get; set; }
    public decimal? PriceToCustomer { get; set; }
    public decimal PriceToCustomerAmount { get; set; }
    public decimal MEVNOfferPrice { get; set; }
    public string? CompetitorBrand { get; set; }
    public string? CompetitorModel { get; set; }
    public decimal? CompetitorPrice { get; set; }
    public decimal? LandingCost { get; set; }
    public decimal? LandingCostAmount { get; set; }
    public decimal? InputPrice { get; set; }
    public string? InputCurrency { get; set; }
    public decimal? ManagerMargin { get; set; }
    public decimal? PriceOfferDetailMargin { get; set; }
    public string? AccountCode { get; set; }
    public string? Note { get; set; }
    public Guid ImportGuid { get; set; }
    public string? Status { get; set; }
    public virtual decimal? MaxSalesOfferPrice { get; set; }
    public virtual decimal? MaxMangerOfferPrice { get; set; }
    public virtual decimal? ActualDiscountRatio { get; set; }
    public virtual decimal? MEVNOfferAmount { get; set; }
    public ICollection<ApprovalHistoryDto> ApprovalHistories { get; set; } = [];
}