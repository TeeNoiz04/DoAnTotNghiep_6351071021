using System;

namespace QuoteFlow.DPOs;

public class DPOProcessingReportDto
{

    public string? DPONo { get; set; }
    public string? GolfaCode { get; set; }
    public string? Model { get; set; }
    public string? Spec1 { get; set; }
    public string? Material_Group { get; set; }
    public string? MaterialType { get; set; }
    public string? BuyerTypeDescription { get; set; }
    public string? BuyerShortName { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? RequestedETA { get; set; }
    public string? SPOCode { get; set; }
    public string? CustomerName { get; set; }
    public string? Status { get; set; }
    public decimal? Qty { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? Extrafee { get; set; }
    public decimal? AmountIncludeExtrafee { get; set; }
    public decimal? Delivered { get; set; }
    public decimal? Delivery_Price { get; set; }
    public decimal? DeliveryOrder_Amount { get; set; }
    public decimal? Remain_Qty { get; set; }
    public decimal? Remain_Amount { get; set; }
}
