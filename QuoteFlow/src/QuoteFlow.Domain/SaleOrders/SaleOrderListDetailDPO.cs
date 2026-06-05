using System;

namespace QuoteFlow.SaleOrders;
public class SaleOrderListDetailDPO
{
    public string? DPONo { get; set; }
    public Guid DPODetail_Id { get; set; }
    public string? GolfaCode { get; set; }
    public string? Model { get; set; }
    public int Qty { get; set; }
    public decimal? VAT { get; set; }
    public string? StockName { get; set; }
    public decimal? UnitPrice { get; set; }
    public string? Note { get; set; }
    public decimal? Extrafee { get; set; }
    public string? Extrafee_Note { get; set; }
    public string? LockstockId { get; set; }
    public string? ConfirmedNote { get; set; }
}
