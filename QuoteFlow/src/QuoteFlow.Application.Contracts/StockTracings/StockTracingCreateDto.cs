using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.StockTracings;

public class StockTracingCreateDto
{
    [StringLength(StockTracingConsts.FileNameMaxLength)]
    public string? FileName { get; set; }
    public ReportType ReportType { get; set; } = ((ReportType[])Enum.GetValues(typeof(ReportType)))[0];
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    [StringLength(QuoteFlowSharedConsts.NoteMaxLength)]
    public string? Note { get; set; }
}