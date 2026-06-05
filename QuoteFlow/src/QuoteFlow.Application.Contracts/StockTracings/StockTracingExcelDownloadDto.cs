using System;

namespace QuoteFlow.StockTracings;

public class StockTracingExcelDownloadDto
{
    public string DownloadToken { get; set; } = null!;

    public string? FilterText { get; set; }

    public string? FileName { get; set; }
    public ReportType? ReportType { get; set; }
    public DateTime? FromDateMin { get; set; }
    public DateTime? FromDateMax { get; set; }
    public DateTime? ToDateMin { get; set; }
    public DateTime? ToDateMax { get; set; }
    public string? Note { get; set; }

    public StockTracingExcelDownloadDto()
    {

    }
}