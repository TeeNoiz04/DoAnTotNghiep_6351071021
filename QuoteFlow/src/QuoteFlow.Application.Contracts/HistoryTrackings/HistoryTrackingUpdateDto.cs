using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.HistoryTrackings;

public class HistoryTrackingUpdateDto : IHasConcurrencyStamp
{
    [Required]
    [StringLength(HistoryTrackingConsts.TrackingTypeMaxLength)]
    public string TrackingType { get; set; } = null!;
    [Required]
    [StringLength(HistoryTrackingConsts.ActionMaxLength)]
    public string Action { get; set; } = null!;
    [StringLength(HistoryTrackingConsts.ObjectIdMaxLength)]
    public string? ObjectId { get; set; }

    [StringLength(HistoryTrackingConsts.GolfaCodeMaxLength)]
    public string? GolfaCode { get; set; }
    [StringLength(HistoryTrackingConsts.ModelMaxLength)]
    public string? Model { get; set; }
    public decimal? Qty { get; set; }
    public decimal? PreviousValue { get; set; }
    public decimal? NextValue { get; set; }
    public Guid? StockId { get; set; }
    [StringLength(HistoryTrackingConsts.StockNameMaxLength)]
    public string? StockName { get; set; }
    [StringLength(HistoryTrackingConsts.NoteMaxLength)]
    public string? Note { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}