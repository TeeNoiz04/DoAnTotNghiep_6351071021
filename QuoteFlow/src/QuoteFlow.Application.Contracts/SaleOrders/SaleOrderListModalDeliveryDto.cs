using System;

namespace QuoteFlow.SaleOrders;
public class SaleOrderListModalDeliveryDto
{
    public string? SONo { get; set; }
    public string? SOSAPNo { get; set; }
    public string? InvoiceNo { get; set; }
    public string? StockName { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public decimal? Quantity { get; set; }
    public string? Note { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? Modified { get; set; }
}
