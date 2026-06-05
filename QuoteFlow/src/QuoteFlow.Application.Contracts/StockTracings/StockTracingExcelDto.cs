using System;

namespace QuoteFlow.StockTracings;

public class StockTracingExcelDto
{
    public string? FileName { get; set; }
    public ReportType ReportType { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? Note { get; set; }
}