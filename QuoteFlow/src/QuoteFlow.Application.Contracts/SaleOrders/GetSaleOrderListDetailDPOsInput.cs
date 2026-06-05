using System;

namespace QuoteFlow.SaleOrders;
public class GetSaleOrderListDetailDPOsInput
{
    public Guid BuyerId { get; set; }
    public string MaterialType { get; set; } = null!;
    public decimal? VAT { get; set; } = null;
    public Guid StockCategoryId { get; set; }
}
