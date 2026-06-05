using System;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.HistoryTrackings;

public class HistoryTrackingDto : AuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string TrackingType { get; set; } = null!;
    public string Action { get; set; } = null!;
    public string? ObjectId { get; set; }
    public string? GolfaCode { get; set; }
    public string? Model { get; set; }
    public decimal? Qty { get; set; }
    public decimal? PreviousValue { get; set; }
    public decimal? NextValue { get; set; }
    public Guid? StockId { get; set; }
    public string? StockName { get; set; }
    public string? Note { get; set; }
    public string? CreatorUsername { get; set; }
    public string? BeforeChange { get; set; }
    public string? AfterChange { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;

}