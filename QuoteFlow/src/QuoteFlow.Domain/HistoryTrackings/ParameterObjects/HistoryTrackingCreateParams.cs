using System;

namespace QuoteFlow.HistoryTrackings.ParameterObjects;
public class HistoryTrackingCreateParams
{
    public string TrackingType { get; set; }
    public string Action { get; set; }
    public string? GolfaCode { get; set; }
    public string? ObjectId { get; set; }
    public string? Model { get; set; }
    public decimal? Qty { get; set; }
    public decimal? PreviousValue { get; set; }
    public decimal? NextValue { get; set; }
    public Guid? StockId { get; set; }
    public string? StockName { get; set; }
    public string? Note { get; set; }
    public string? BeforeChange { get; set; }
    public string? AfterChange { get; set; }
}
