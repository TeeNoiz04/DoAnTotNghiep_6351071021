using QuoteFlow.EntityFrameworkCore;
using QuoteFlow.HistoryTrackings.ParameterObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.HistoryTrackings;

public class EfCoreHistoryTrackingRepository : EfCoreRepository<QuoteFlowDbContext, HistoryTracking, Guid>, IHistoryTrackingRepository
{
    public EfCoreHistoryTrackingRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<HistoryTracking>> GetListAsync(
        HistoryTrackingFilterParams filterParams,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetQueryableAsync()), filterParams);
        query = query.OrderByDescending(x => x.CreationTime);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        HistoryTrackingFilterParams filterParams,
        CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetDbSetAsync()), filterParams);
        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<HistoryTracking> ApplyFilter(
        IQueryable<HistoryTracking> query,
        HistoryTrackingFilterParams filterParams)
    {
        var filterText = filterParams.FilterText;
        var trackingType = filterParams.TrackingType;
        var action = filterParams.Action;
        var objectId = filterParams.ObjectId;
        var golfaCode = filterParams.GolfaCode;
        var model = filterParams.Model;
        var qtyMin = filterParams.QtyMin;
        var qtyMax = filterParams.QtyMax;
        var previousValueMin = filterParams.PreviousValueMin;
        var previousValueMax = filterParams.PreviousValueMax;
        var nextValueMin = filterParams.NextValueMin;
        var nextValueMax = filterParams.NextValueMax;
        var stockId = filterParams.StockId;
        var stockName = filterParams.StockName;
        var note = filterParams.Note;
        var creationTimeMin = filterParams.CreationTimeMin;
        var creationTimeMax = filterParams.CreationTimeMax;

        return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.TrackingType!.Contains(filterText!) || e.Action!.Contains(filterText!) || e.ObjectId!.Contains(filterText!) || e.GolfaCode!.Contains(filterText!) || e.Model!.Contains(filterText!) || e.StockName!.Contains(filterText!) || e.Note!.Contains(filterText!))
                .WhereIf(!string.IsNullOrWhiteSpace(trackingType), e => e.TrackingType.Contains(trackingType))
                .WhereIf(!string.IsNullOrWhiteSpace(action), e => e.Action.Contains(action))
                .WhereIf(!string.IsNullOrWhiteSpace(objectId), e => e.ObjectId.Contains(objectId))
                .WhereIf(!string.IsNullOrWhiteSpace(golfaCode), e => e.GolfaCode.Contains(golfaCode))
                .WhereIf(!string.IsNullOrWhiteSpace(model), e => e.Model.Contains(model))
                .WhereIf(qtyMin.HasValue, e => e.Qty >= qtyMin!.Value)
                .WhereIf(qtyMax.HasValue, e => e.Qty <= qtyMax!.Value)
                .WhereIf(previousValueMin.HasValue, e => e.PreviousValue >= previousValueMin!.Value)
                .WhereIf(previousValueMax.HasValue, e => e.PreviousValue <= previousValueMax!.Value)
                .WhereIf(nextValueMin.HasValue, e => e.NextValue >= nextValueMin!.Value)
                .WhereIf(nextValueMax.HasValue, e => e.NextValue <= nextValueMax!.Value)
                .WhereIf(stockId.HasValue, e => e.StockId == stockId)
                .WhereIf(!string.IsNullOrWhiteSpace(stockName), e => e.StockName.Contains(stockName))
                .WhereIf(!string.IsNullOrWhiteSpace(note), e => e.Note.Contains(note))
                .WhereIf(creationTimeMin.HasValue, e => e.CreationTime >= creationTimeMin.Value.Date)
                .WhereIf(creationTimeMax.HasValue, e => e.CreationTime <= creationTimeMax.Value.Date);
    }
}