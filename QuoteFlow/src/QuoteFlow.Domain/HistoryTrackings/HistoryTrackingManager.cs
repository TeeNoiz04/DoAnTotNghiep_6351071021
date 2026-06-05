using JetBrains.Annotations;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace QuoteFlow.HistoryTrackings;

public class HistoryTrackingManager : DomainService
{
    protected IHistoryTrackingRepository _historyTrackingRepository;

    public HistoryTrackingManager(IHistoryTrackingRepository historyTrackingRepository)
    {
        _historyTrackingRepository = historyTrackingRepository;
    }

    public virtual async Task<HistoryTracking> CreateAsync(
    string trackingType, string action, string? golfaCode = null, string? objectId = null, string? model = null, decimal? qty = null, decimal? previousValue = null, decimal? nextValue = null, Guid? stockId = null, string? stockName = null, string? note = null)
    {
        Check.NotNullOrWhiteSpace(trackingType, nameof(trackingType));
        Check.Length(trackingType, nameof(trackingType), HistoryTrackingConsts.TrackingTypeMaxLength);
        Check.NotNullOrWhiteSpace(action, nameof(action));
        Check.Length(action, nameof(action), HistoryTrackingConsts.ActionMaxLength);
        Check.NotNullOrWhiteSpace(golfaCode, nameof(golfaCode));

        Check.Length(objectId, nameof(objectId), HistoryTrackingConsts.ObjectIdMaxLength);
        Check.Length(model, nameof(model), HistoryTrackingConsts.ModelMaxLength);
        Check.Length(stockName, nameof(stockName), HistoryTrackingConsts.StockNameMaxLength);
        Check.Length(note, nameof(note), HistoryTrackingConsts.NoteMaxLength);

        var historyTracking = new HistoryTracking(
         GuidGenerator.Create(),
         trackingType, action, golfaCode, objectId, model, qty, previousValue, nextValue, stockId, stockName, note
         );

        return await _historyTrackingRepository.InsertAsync(historyTracking);
    }

    public virtual async Task<HistoryTracking> UpdateAsync(
        Guid id,
        string trackingType, string action, string? golfaCode = null, string? objectId = null, string? model = null, decimal? qty = null, decimal? previousValue = null, decimal? nextValue = null, Guid? stockId = null, string? stockName = null, string? note = null, [CanBeNull] string? concurrencyStamp = null
    )
    {
        Check.NotNullOrWhiteSpace(trackingType, nameof(trackingType));
        Check.Length(trackingType, nameof(trackingType), HistoryTrackingConsts.TrackingTypeMaxLength);
        Check.NotNullOrWhiteSpace(action, nameof(action));
        Check.Length(action, nameof(action), HistoryTrackingConsts.ActionMaxLength);
        Check.NotNullOrWhiteSpace(golfaCode, nameof(golfaCode));

        Check.Length(objectId, nameof(objectId), HistoryTrackingConsts.ObjectIdMaxLength);
        Check.Length(model, nameof(model), HistoryTrackingConsts.ModelMaxLength);
        Check.Length(stockName, nameof(stockName), HistoryTrackingConsts.StockNameMaxLength);
        Check.Length(note, nameof(note), HistoryTrackingConsts.NoteMaxLength);

        var historyTracking = await _historyTrackingRepository.GetAsync(id);

        historyTracking.TrackingType = trackingType;
        historyTracking.Action = action;
        historyTracking.GolfaCode = golfaCode;
        historyTracking.ObjectId = objectId;
        historyTracking.Model = model;
        historyTracking.Qty = qty;
        historyTracking.PreviousValue = previousValue;
        historyTracking.NextValue = nextValue;
        historyTracking.StockId = stockId;
        historyTracking.StockName = stockName;
        historyTracking.Note = note;

        historyTracking.SetConcurrencyStampIfNotNull(concurrencyStamp);
        return await _historyTrackingRepository.UpdateAsync(historyTracking);
    }

}