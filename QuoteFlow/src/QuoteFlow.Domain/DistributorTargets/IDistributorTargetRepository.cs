using QuoteFlow.DistributorTargets.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.DistributorTargets;

public interface IDistributorTargetRepository : IRepository<DistributorTarget, Guid>
{
    Task<List<DistributorTarget>> GetListAsync(
       DistributorTargetFilterParams filterParams,
       CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        DistributorTargetFilterParams filterParams,
        CancellationToken cancellationToken = default);
}