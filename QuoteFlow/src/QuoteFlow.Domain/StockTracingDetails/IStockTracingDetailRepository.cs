using QuoteFlow.StockTracingDetails.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.StockTracingDetails;

public interface IStockTracingDetailRepository : IRepository<StockTracingDetail, Guid>
{
    Task<List<StockTracingDetail>> GetListAsync(
        StockTracingDetailFilterParams filterParams,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        StockTracingDetailFilterParams filterParams,
        CancellationToken cancellationToken = default);

    Task BulkInsertAsync(
        List<StockTracingDetail> details,
        CancellationToken cancellationToken = default);
}