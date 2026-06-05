using JetBrains.Annotations;
using QuoteFlow.HistoryTrackings.ParameterObjects;
using QuoteFlow.Shared.Models;
using System;
using Volo.Abp;

namespace QuoteFlow.HistoryTrackings;

public class HistoryTracking : ExtendedAuditedAggregateRoot<Guid>
{
    [NotNull]
    public virtual string TrackingType { get; set; }

    [NotNull]
    public virtual string Action { get; set; }

    [CanBeNull]
    public virtual string? ObjectId { get; set; }
    [CanBeNull]
    public string? GolfaCode { get; set; }

    [CanBeNull]
    public virtual string? Model { get; set; }

    public virtual decimal? Qty { get; set; }

    public virtual decimal? PreviousValue { get; set; }

    public virtual decimal? NextValue { get; set; }

    public virtual Guid? StockId { get; set; }

    [CanBeNull]
    public virtual string? StockName { get; set; }

    [CanBeNull]
    public virtual string? Note { get; set; }
    [CanBeNull]
    public virtual string? BeforeChange { get; set; }

    [CanBeNull]
    public virtual string? AfterChange { get; set; }

    protected HistoryTracking()
    {

    }

    public HistoryTracking(Guid id, string trackingType, string action, string? golfaCode = null, string? objectId = null, string? model = null, decimal? qty = null, decimal? previousValue = null, decimal? nextValue = null, Guid? stockId = null, string? stockName = null, string? note = null)
    {

        Id = id;

        TrackingType = trackingType;
        Action = action;
        GolfaCode = golfaCode;
        ObjectId = objectId;
        Model = model;
        Qty = qty;
        PreviousValue = previousValue;
        NextValue = nextValue;
        StockId = stockId;
        StockName = stockName;
        Note = note;
    }

    public HistoryTracking(Guid id, HistoryTrackingCreateParams p)
    {
        Id = id;
        TrackingType = Check.NotNullOrWhiteSpace(p.TrackingType, nameof(p.TrackingType));
        Action = Check.NotNullOrWhiteSpace(p.Action, nameof(p.Action));

        GolfaCode = p.GolfaCode;
        ObjectId = p.ObjectId;
        Model = p.Model;
        Qty = p.Qty;
        PreviousValue = p.PreviousValue;
        NextValue = p.NextValue;
        StockId = p.StockId;
        StockName = p.StockName;
        Note = p.Note;
        BeforeChange = p.BeforeChange;
        AfterChange = p.AfterChange;
    }
}