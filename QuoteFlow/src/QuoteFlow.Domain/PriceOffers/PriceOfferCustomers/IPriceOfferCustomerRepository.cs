using QuoteFlow.PriceOffers.PriceOfferCustomers.ParameterObject;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.PriceOffers.PriceOfferCustomers;

public interface IPriceOfferCustomerRepository : IRepository<PriceOfferCustomer, Guid>
{
    Task<List<PriceOfferCustomer>> GetListAsync(
        PriceOfferCustomerFilterParams filterParams,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        PriceOfferCustomerFilterParams filterParams,
        CancellationToken cancellationToken = default);
}