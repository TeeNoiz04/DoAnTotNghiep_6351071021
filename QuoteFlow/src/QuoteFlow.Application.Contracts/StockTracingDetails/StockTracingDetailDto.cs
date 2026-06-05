using QuoteFlow.Shared;
using QuoteFlow.StockTracings;
using System;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.StockTracingDetails;

public class StockTracingDetailDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public Guid StockTracingId { get; set; }
    public ReportType ReportType { get; set; }
    public int? RowNo { get; set; }
    public string? PackingListCode { get; set; }
    public string? CheckListCode { get; set; }
    public DateTime? DateEntered { get; set; }
    public string? Stock { get; set; }
    public string? BU { get; set; }
    public string? Customer { get; set; }
    public string? Category { get; set; }
    public string? GIV { get; set; }
    public string? Invoice { get; set; }
    public string? SKUCode { get; set; }
    public string? SKUName { get; set; }
    public string? Quality { get; set; }
    public string? Warranty { get; set; }
    public string? Unit { get; set; }
    public string? Series { get; set; }
    public string? OriginCode { get; set; }
    public DateTime? ProductionDate { get; set; }
    public string? Location { get; set; }
    public string? GolfaCode { get; set; }
    public string? Note { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;

}