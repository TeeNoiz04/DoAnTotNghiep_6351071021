using QuoteFlow.Shared;
using QuoteFlow.StockTracingDetails;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.StockTracings;

public class StockTracingDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string? FileName { get; set; }
    public ReportType ReportType { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? Note { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;

    public List<StockTracingDetailDto> Details { get; set; } = new();

}