using QuoteFlow.Materials.MaterialHistories.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.Materials.MaterialHistories;

public interface IMaterialHistoryRepository : IRepository<MaterialHistory, Guid>
{
    Task<List<MaterialHistory>> GetListAsync(
        MaterialHistoryFilterParams filterParams,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
       MaterialHistoryFilterParams filterParams,
        CancellationToken cancellationToken = default);
}