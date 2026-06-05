using QuoteFlow.BuyerAccess;
using System.Collections.Generic;

namespace QuoteFlow.Materials.ParameterObjects;
public class MaterialFilterParams : IMaterialTypeRestrictable
{
    public string? GolfaCodes { get; set; }
    public string? Models { get; set; }
    public string? SAPCode { get; set; }
    public string? MaterialType { get; set; }
    public string? MaterialGroup { get; set; }

    public bool? IsDeactive { get; set; }

    public string? SupplierBU { get; set; }
    public string? Supplier { get; set; }
    public string? MaterialStatus { get; set; }

    public int? StockQty { get; set; }
    public int? OnOderStock { get; set; }
    public bool? Deactive { get; set; }

    public string? Sorting { get; set; } = null;
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = int.MaxValue;
    public List<string> RestrictedMaterialTypes { get; set; } = [];
}
