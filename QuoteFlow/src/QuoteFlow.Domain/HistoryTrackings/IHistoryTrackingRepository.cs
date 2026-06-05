using QuoteFlow.HistoryTrackings.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.HistoryTrackings;

public interface IHistoryTrackingRepository : IRepository<HistoryTracking, Guid>
{
    Task<List<HistoryTracking>> GetListAsync(
        HistoryTrackingFilterParams filterParams,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        HistoryTrackingFilterParams filterParams,
        CancellationToken cancellationToken = default);
}