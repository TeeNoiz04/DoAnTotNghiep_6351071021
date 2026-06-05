using QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails.Params;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails;

public interface ISpoBatchRequestDetailRepository : IRepository<SpoBatchRequestDetail, Guid>
{
    Task<List<SpoBatchRequestDetail>> GetListAsync(
     SpoBatchRequestDetailFilterParams input,
     CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        SpoBatchRequestDetailFilterParams input,

        CancellationToken cancellationToken = default
    );

}