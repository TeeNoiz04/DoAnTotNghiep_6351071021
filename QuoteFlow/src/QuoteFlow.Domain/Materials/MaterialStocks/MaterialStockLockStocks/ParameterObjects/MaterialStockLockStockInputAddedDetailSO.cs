using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.Materials.MaterialStocks.MaterialStockLockStocks.ParameterObjects;
public class MaterialStockLockStockInputAddedDetailSO
{
    [Required]
    public Guid BuyerId { get; set; }

    [Required]
    public Guid StockCategoryId { get; set; }
    [Required]
    public decimal VAT { get; set; }
    public string? Sorting { get; set; } = null;
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = int.MaxValue;
}
