using System;
using System.Collections.Generic;

namespace QuoteFlow.DPOs.ParameterObjects;

public class GICFilterParams
{
    public string? FilterText { get; set; }

    public string? GicNo { get; set; }

    public string? GicType { get; set; }

    public string? MaterialCode { get; set; }
    public string? ModelName { get; set; }

    public string? GicProcess { get; set; }

    public string? MaterialType { get; set; }

    public string? CostCenter { get; set; }

    public string? Status { get; set; }

    public Guid? BuyerTypeId { get; set; }

    public Guid? BuyerId { get; set; }

    public string? BuyerShortName { get; set; }

    public DateTime? OrderDateMin { get; set; }

    public DateTime? OrderDateMax { get; set; }

    public decimal? TotalAmountMin { get; set; }

    public decimal? TotalAmountMax { get; set; }

    public string? Remark { get; set; }

    public string? FileName { get; set; }

    /// <summary>
    /// Empty list means no restriction, otherwise restrict to the specified GIC types.
    /// </summary>
    public List<string> RestrictedGICTypes { get; set; } = [];
}
