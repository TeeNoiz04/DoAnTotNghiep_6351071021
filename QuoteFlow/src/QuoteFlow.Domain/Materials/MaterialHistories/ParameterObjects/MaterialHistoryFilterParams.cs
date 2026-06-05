using System;

namespace QuoteFlow.Materials.MaterialHistories.ParameterObjects;
public class MaterialHistoryFilterParams
{
    public string? FilterText { get; set; }

    public Guid? MaterialId { get; set; }
    public string? Action { get; set; }
    public string? Note { get; set; }

    public string? Sorting { get; set; } = null;
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = int.MaxValue;
}
