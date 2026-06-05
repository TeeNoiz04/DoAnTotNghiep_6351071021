using System;

namespace QuoteFlow.SaleOrders.ParameterObjects;
public class SaleOrderAddedDetailDPOParams
{
    public Guid PrSOId { get; set; }
    public Guid DPODetailId { get; set; }
    public string? LockStockId { get; set; }
    public string MaterialCode { get; set; } = string.Empty;
    public int Qty { get; set; }
    public decimal Price { get; set; }
    public decimal? VAT { get; set; }
    public Guid StockCategoryId { get; set; }
    public decimal Extrafee { get; set; }
    public string Note { get; set; } = string.Empty;
    public string MaterialType { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string UserFullName { get; set; } = string.Empty;
}
