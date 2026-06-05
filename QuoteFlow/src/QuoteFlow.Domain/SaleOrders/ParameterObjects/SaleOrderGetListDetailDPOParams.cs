using System;

namespace QuoteFlow.SaleOrders.ParameterObjects;
public class SaleOrderGetListDetailDPOParams
{
    public string? FilterText { get; set; } = null;
    public Guid BuyerId { get; set; }
    public string MaterialType { get; set; } = null!;
    public decimal? VAT { get; set; } = null;
    public Guid StockCategoryId { get; set; }
}
