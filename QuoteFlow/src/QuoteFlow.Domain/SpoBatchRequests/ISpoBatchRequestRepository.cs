using QuoteFlow.SpoBatchRequests.ParameterObject;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.SpoBatchRequests;

public interface ISpoBatchRequestRepository : IRepository<SpoBatchRequest, Guid>
{
    Task<List<SpoBatchRequest>> GetListAsync(
        SpoBatchRequestFilterParams filterParams,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        SpoBatchRequestFilterParams filterParams,
        CancellationToken cancellationToken = default);

    Task<string?> GetLatestCodeAsync(
      string prefix,
      CancellationToken cancellationToken = default);


    Task GetBatchUpdateAsync(
     Guid requestId,
     CancellationToken cancellationToken = default);

}