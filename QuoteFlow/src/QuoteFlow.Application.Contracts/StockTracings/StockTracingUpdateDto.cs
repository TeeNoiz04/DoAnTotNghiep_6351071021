using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.StockTracings;

public class StockTracingUpdateDto : IHasConcurrencyStamp
{
    [StringLength(StockTracingConsts.FileNameMaxLength)]
    public string? FileName { get; set; }
    public ReportType ReportType { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    [StringLength(QuoteFlowSharedConsts.NoteMaxLength)]
    public string? Note { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}