using System;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.StockManagements;
public class GetStockManagementsListInput : PagedAndSortedResultRequestDto
{
    public string? SupplierCode { get; set; }
    public string? SupplierBUCode { get; set; }
    public string? MaterialType { get; set; }
    public string? GolfaCode { get; set; }
    public string? Model { get; set; }
    public string? MaterialGroup { get; set; }
    public Guid? StockCategoryId { get; set; }
    public int? GreaterStockQty { get; set; }
    public int? GreaterOnOrderStockQty { get; set; }
    public string? Status { get; set; }
}
