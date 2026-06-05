using System;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.SaleOrders.SaleOrderDetails;

public class GetSaleOrderDetailsInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

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

    public GetSaleOrderDetailsInput()
    {

    }
}