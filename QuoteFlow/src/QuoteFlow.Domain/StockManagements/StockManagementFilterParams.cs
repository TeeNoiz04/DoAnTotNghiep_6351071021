using QuoteFlow.BuyerAccess;
using System;
using System.Collections.Generic;

namespace QuoteFlow.StockManagements;
public class StockManagementFilterParams : IMaterialTypeRestrictable
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

    public string? Sorting { get; set; } = null;
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = int.MaxValue;
    public List<string> RestrictedMaterialTypes { get; set; } = [];
}
