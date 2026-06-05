using System;

namespace QuoteFlow.StockManagements;
public class StockOfSO
{
    public string? SONo { get; set; }
    public string? StockName { get; set; }
    public string? DONo { get; set; }
    public string? Buyer { get; set; }
    public int? Qty { get; set; } = 0;
    public string? CreatedBy { get; set; }
    public DateTime? Created { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? Modified { get; set; }
}
