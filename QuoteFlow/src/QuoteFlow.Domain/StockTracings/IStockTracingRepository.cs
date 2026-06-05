using QuoteFlow.StockTracings.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.StockTracings;

public interface IStockTracingRepository : IRepository<StockTracing, Guid>
{
    Task<List<StockTracing>> GetListAsync(
        StockTracingFilterParams filterParams,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        StockTracingFilterParams filterParams,
        CancellationToken cancellationToken = default);

    Task<StockTracing> GetWithDetailsAsync(Guid id);
    Task<StockTracing> GetNoDetailsAsync(Guid id);
}