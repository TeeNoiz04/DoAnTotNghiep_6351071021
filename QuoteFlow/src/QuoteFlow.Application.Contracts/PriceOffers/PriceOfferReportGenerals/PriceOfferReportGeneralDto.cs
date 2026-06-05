using System;

namespace QuoteFlow.PriceOffers.PriceOfferReportGenerals;
public class PriceOfferReportGeneralDto
{
    public string? BuyerType { get; set; }
    public string? MaterialType { get; set; }
    public string? SPOType { get; set; }
    public string? ProjectName { get; set; }
    public DateTime? CreationTime { get; set; }
    public string? ApprovalStatus { get; set; }
    public string? ProjectResultStatus { get; set; }
    public string? CompetitorBrand { get; set; }
    public decimal? TotalMEVNOfferAmount { get; set; }
    public decimal? TotalStandardAmount { get; set; }
    public decimal? SPO_DiscountRatio { get; set; }
    public DateTime? CloseDate { get; set; }
    public string? Country { get; set; }
    public string? Province { get; set; }

    public string? Trading_Customer { get; set; }
    public string? Trading_TaxCode { get; set; }
    public string? PanelBuilder_Customer { get; set; }
    public string? PanelBuilder_TaxCode { get; set; }
    public string? MEContractor_Customer { get; set; }
    public string? MEContractor_TaxCode { get; set; }
    public string? MainContractor_Customer { get; set; }
    public string? MainContractor_TaxCode { get; set; }
    public string? SIOEM_Customer { get; set; }
    public string? SIOEM_TaxCode { get; set; }
    public string? InvestorEU_Customer { get; set; }
    public string? InvestorEU_TaxCode { get; set; }

}
