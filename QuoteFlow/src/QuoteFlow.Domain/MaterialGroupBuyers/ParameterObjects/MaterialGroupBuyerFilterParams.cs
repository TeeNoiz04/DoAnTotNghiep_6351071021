using System;

namespace QuoteFlow.MaterialGroupBuyers.ParameterObjects;
public class MaterialGroupBuyerFilterParams
{
    public Guid? MaterialGroupId { get; set; }
    public string? MaterialGroupCode { get; set; }
    public Guid? BuyerId { get; set; }
    public string? BuyerShortName { get; set; }
    public string? Note { get; set; }
    public string? Sorting { get; set; } = null;
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = int.MaxValue;
}
