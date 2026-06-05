using QuoteFlow.Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.StockTracings.ParameterObjects;

public class StockTracingUpdateParams : BaseUpdateParams
{
    [Required]
    public ReportType ReportType { get; set; }

    [StringLength(StockTracingConsts.FileNameMaxLength)]
    public string? FileName { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    [StringLength(QuoteFlowSharedConsts.NoteMaxLength)]
    public string? Note { get; set; }

}
