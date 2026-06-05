using System;

namespace QuoteFlow.StockManagements;
public class StockQtyDto
{
    public string? StockCode { get; set; }
    public string? StockName { get; set; }
    public string? GolfaCode { get; set; }
    public int? Qty { get; set; } = 0;

    public int? AvailableStock { get; set; } = 0;

    public string? CreatedBy { get; set; }
    public DateTime? Created { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? Modified { get; set; }
}
