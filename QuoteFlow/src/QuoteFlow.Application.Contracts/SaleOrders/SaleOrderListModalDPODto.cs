using System;

namespace QuoteFlow.SaleOrders;
public class SaleOrderListModalDPODto
{
    public string? Status { get; set; }
    public string? SONo { get; set; }
    public string? SOSAPNo { get; set; }
    public string? DONo { get; set; }
    public string? StockName { get; set; }
    public decimal? Quantity { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? Modified { get; set; }
}
