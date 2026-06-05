using System;

namespace QuoteFlow.HistoryTrackings.ParameterObjects;

public class HistoryTrackingFilterParams
{
    public string? FilterText { get; set; }

    public string? TrackingType { get; set; }

    public string? Action { get; set; }

    public string? ObjectId { get; set; }

    public string? GolfaCode { get; set; }

    public string? Model { get; set; }

    public decimal? QtyMin { get; set; }

    public decimal? QtyMax { get; set; }

    public decimal? PreviousValueMin { get; set; }

    public decimal? PreviousValueMax { get; set; }

    public decimal? NextValueMin { get; set; }

    public decimal? NextValueMax { get; set; }

    public Guid? StockId { get; set; }

    public string? StockName { get; set; }

    public string? Note { get; set; }

    public DateTime? CreationTimeMin { get; set; }

    public DateTime? CreationTimeMax { get; set; }
}
