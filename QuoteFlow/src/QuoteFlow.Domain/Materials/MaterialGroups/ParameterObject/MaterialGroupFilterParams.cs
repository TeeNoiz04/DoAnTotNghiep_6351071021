using System;

namespace QuoteFlow.Materials.MaterialGroups.ParameterObject;

public class MaterialGroupFilterParams
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public Guid? Parent { get; set; }
    public int? MinSortOrder { get; set; }
    public int? MaxSortOrder { get; set; }
    public string? Note { get; set; }
    public bool? IsDeActive { get; set; }
    public string? MaterialType { get; set; }
    public string? MaterialGroupPSI { get; set; }
    public bool? AllowKeyAccount { get; set; }
    public string? Sorting { get; set; } = null;
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = int.MaxValue;
}
