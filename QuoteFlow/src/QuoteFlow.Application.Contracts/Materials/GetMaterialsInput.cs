using System;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.Materials;

public class GetMaterialsInput : PagedAndSortedResultRequestDto
{
    public string? GolfaCodes { get; set; }
    public string? Models { get; set; }
    public string? SAPCode { get; set; }
    public string? MaterialType { get; set; }
    public string? MaterialGroup { get; set; }
    public string? Supplier { get; set; }
    public Guid? SupplierBUId { get; set; }
    public string? SupplierBU { get; set; }
    public string? MaterialStatus { get; set; }
    public int? StockQty { get; set; }
    public int? OnOrderStock { get; set; }
    public bool? IsDeactive { get; set; }
    public Guid? StockId { get; set; }

    public GetMaterialsInput()
    {

    }
}