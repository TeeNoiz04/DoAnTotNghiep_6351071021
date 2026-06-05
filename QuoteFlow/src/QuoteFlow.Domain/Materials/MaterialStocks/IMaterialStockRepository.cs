using QuoteFlow.Materials.MaterialStocks.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.Materials.MaterialStocks;

public interface IMaterialStockRepository : IRepository<MaterialStock, Guid>
{
    Task<List<MaterialStock>> GetListAsync(
        MaterialStockFilterParams filterParams,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        MaterialStockFilterParams filterParams,
        CancellationToken cancellationToken = default);
}