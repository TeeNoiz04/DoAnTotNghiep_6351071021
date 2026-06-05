using QuoteFlow.MaterialGroupBuyers.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.MaterialGroupBuyers;

public interface IMaterialGroupBuyerRepository : IRepository<MaterialGroupBuyer, Guid>
{
    Task<List<MaterialGroupBuyer>> GetListAsync(
        MaterialGroupBuyerFilterParams filterParams,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        MaterialGroupBuyerFilterParams filterParams,
        CancellationToken cancellationToken = default);
}