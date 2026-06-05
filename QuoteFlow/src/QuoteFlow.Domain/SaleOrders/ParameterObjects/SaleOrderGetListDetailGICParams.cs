using System;

namespace QuoteFlow.SaleOrders.ParameterObjects;

public class SaleOrderGetListDetailGICParams
{
    public string GICType { get; set; } = null!;
    public string? GICProcess { get; set; }
    public Guid BuyerId { get; set; }
    public string MaterialType { get; set; } = null!;
    public decimal? VAT { get; set; } = null;
    public Guid StockCategoryId { get; set; }
}
