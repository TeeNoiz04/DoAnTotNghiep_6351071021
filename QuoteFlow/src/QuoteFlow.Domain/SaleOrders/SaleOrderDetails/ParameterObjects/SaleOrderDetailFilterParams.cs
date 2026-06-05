using System;

namespace QuoteFlow.SaleOrders.SaleOrderDetails.ParameterObjects;
public class SaleOrderDetailFilterParams
{
    public Guid? SaleOrderId { get; set; }
    public Guid? DPODetailId { get; set; }
    public string? StatusCode { get; set; }
    public string? GolfaCode { get; set; }
    public int? QtyMin { get; set; }
    public int? QtyMax { get; set; }
    public decimal? PriceMin { get; set; }
    public decimal? PriceMax { get; set; }
    public decimal? AmountMin { get; set; }
    public decimal? AmountMax { get; set; }
    public decimal? VATMin { get; set; }
    public decimal? VATMax { get; set; }
    public Guid? StockCategoryId { get; set; }
    public string? Note { get; set; }
    public Guid? LockStockId { get; set; }

    public string? Sorting { get; set; } = null;
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = int.MaxValue;

}
