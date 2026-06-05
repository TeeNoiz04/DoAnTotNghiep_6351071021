using QuoteFlow.PriceOffers.PriceOfferDetails.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.PriceOffers.PriceOfferDetails;

public interface IPriceOfferDetailRepository : IRepository<PriceOfferDetail, Guid>
{
    Task<List<PriceOfferDetail>> GetListAsync(
        PriceOfferDetailFilterParams filterParams,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        PriceOfferDetailFilterParams filterParams,
        CancellationToken cancellationToken = default);
}