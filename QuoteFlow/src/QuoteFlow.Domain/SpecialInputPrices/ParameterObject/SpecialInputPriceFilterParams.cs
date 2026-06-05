using System;
using System.Collections.Generic;

namespace QuoteFlow.SpecialInputPrices.ParameterObject;
public class SpecialInputPriceFilterParams
{
    public string? FilterText { get; set; }

    public string? AccountNo { get; set; }
    public string? AccountName { get; set; }
    public List<string>? Materials { get; set; }
    public List<string>? Models { get; set; }
    public string? ProjectName { get; set; }
    public DateTime? ValidFromMin { get; set; }
    public DateTime? ValidFromMax { get; set; }
    public DateTime? ValidToMin { get; set; }
    public DateTime? ValidToMax { get; set; }
    public string? Status { get; set; }
    public string? Note { get; set; }
    public string? Sorting { get; set; } = null;
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = int.MaxValue;
}
