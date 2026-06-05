using QuoteFlow.StockCategories.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.StockCategories;

public interface IStockCategoryRepository : IRepository<StockCategory, Guid>
{
    Task<List<StockCategory>> GetListAsync(
        StockCategoryFilterParams filterParams,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        StockCategoryFilterParams filterParams,
        CancellationToken cancellationToken = default);
}