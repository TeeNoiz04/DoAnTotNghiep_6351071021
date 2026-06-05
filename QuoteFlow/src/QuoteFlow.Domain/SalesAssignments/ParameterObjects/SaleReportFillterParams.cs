using System;

namespace QuoteFlow.SalesAssignments.ParameterObjects;
public class SaleReportFillterParams
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public DateTime? InvoiceFromDate { get; set; }
    public DateTime? InvoiceToDate { get; set; }
    public bool HasFullBuyerAccess { get; set; }
    public bool HasStrategicPriceAccess { get; set; }
    public string? UserName { get; set; }

    public string? Sorting { get; set; } = null;
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = int.MaxValue;
}
