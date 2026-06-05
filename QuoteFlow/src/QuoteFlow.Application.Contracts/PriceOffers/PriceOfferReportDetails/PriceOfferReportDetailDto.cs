using System;

namespace QuoteFlow.PriceOffers.PriceOfferReportDetails;
public class PriceOfferReportDetailDto
{
    public string? Location { get; set; }
    public string? BuyerType { get; set; }
    public string? BuyerCode { get; set; }
    public string? MaterialType { get; set; }
    public string? SPOType { get; set; }
    public string? PriceOffer_Code { get; set; }
    public string? ProjectName { get; set; }
    public DateTime? CreationTime { get; set; }
    public string? ApprovalStatus { get; set; }
    public DateTime? SPOValidity_From { get; set; }
    public DateTime? SPOValidity_To { get; set; }
    public string? Country { get; set; }
    public string? Province { get; set; }
    public string? ProjectResultStatus { get; set; }
    public string? Material_Group { get; set; }
    public string? GolfaCode { get; set; }
    public string? ModelName { get; set; }
    public int Qty { get; set; }
    public decimal? MEVNOfferPrice { get; set; }
    public decimal SaleOfferAmount { get; set; }
    public decimal StandardPrice { get; set; }
    public decimal StandardAmount { get; set; }
    public decimal? ActualDiscountRatio { get; set; }
    public decimal? PriceOfferDetailMargin { get; set; }
    public int? DPO_UsedQty { get; set; }
    public decimal? DPO_UsedAmount { get; set; }
    public int? DPO_DeliveredQty { get; set; }
    public decimal? DPO_DeliveredAmount { get; set; }
    public int? DOP_BO_Qty { get; set; }
    public decimal? DOP_BO_Amount { get; set; }
    public int? SPO_Open_Qty { get; set; }
    public decimal? SPO_Open_Amount { get; set; }
}
