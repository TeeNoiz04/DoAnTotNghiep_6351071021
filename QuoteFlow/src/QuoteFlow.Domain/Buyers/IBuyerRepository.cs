using QuoteFlow.Buyers.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.Buyers;

public interface IBuyerRepository : IRepository<Buyer, Guid>
{
    Task<List<Buyer>> GetListAsync(
        BuyerFilterParams filterParams,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        BuyerFilterParams filterParams,
        CancellationToken cancellationToken = default);
    Task<List<Buyer>> GetBuyersNotAssignedToMaterialGroupAsync();
    Task<Buyer> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
}
