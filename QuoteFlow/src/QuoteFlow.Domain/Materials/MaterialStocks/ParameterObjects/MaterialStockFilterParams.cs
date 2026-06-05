using QuoteFlow.BuyerAccess;
using System;
using System.Collections.Generic;

namespace QuoteFlow.Materials.MaterialStocks.ParameterObjects;
public class MaterialStockFilterParams : IMaterialTypeRestrictable
{
    public string? FilterText { get; set; }

    public Guid? MaterialId { get; set; }
    public Guid? StockCategoryId { get; set; }
    public List<string>? GolfaCodes { get; set; }
    public List<string>? Models { get; set; }
    public int? QtyMin { get; set; }
    public int? QtyMax { get; set; }
    public int? LockedMin { get; set; }
    public int? LockedMax { get; set; }
    public int? LockStockKeepingMin { get; set; }
    public int? LockStockKeepingMax { get; set; }
    public int? LockStockSOMin { get; set; }
    public int? LockStockSOMax { get; set; }
    public int? Available_QtyMin { get; set; }
    public int? Available_QtyMax { get; set; }
    public string? Note { get; set; }
    public string? MaterialType { get; set; }

    public string? Sorting { get; set; } = null;
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = int.MaxValue;

    public List<string> RestrictedMaterialTypes { get; set; } = [];
}