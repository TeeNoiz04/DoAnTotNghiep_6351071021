using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.SpecialInputPrices.SpecialInputPriceDetails;

public interface ISpecialInputPriceDetailRepository : IRepository<SpecialInputPriceDetail, Guid>
{
    Task<List<SpecialInputPriceDetail>> GetListAsync(
        string? filterText = null,
        Guid? specialInputPriceId = null,
        string? materialCode = null,
        string? model = null,
        string? spec1 = null,
        int? limitQtyMin = null,
        int? limitQtyMax = null,
        decimal? inputPriceMin = null,
        decimal? inputPriceMax = null,
        decimal? landedCostMin = null,
        decimal? landedCostMax = null,
        int? usedMin = null,
        int? usedMax = null,
        string? note = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        string? filterText = null,
        Guid? specialInputPriceId = null,
        string? materialCode = null,
        string? model = null,
        string? spec1 = null,
        int? limitQtyMin = null,
        int? limitQtyMax = null,
        decimal? inputPriceMin = null,
        decimal? inputPriceMax = null,
        decimal? landedCostMin = null,
        decimal? landedCostMax = null,
        int? usedMin = null,
        int? usedMax = null,
        string? note = null,
        CancellationToken cancellationToken = default);

    Task<decimal> GetTotalAmountAsync(Guid specialInputId, CancellationToken cancellationToken = default);
    Task<List<SpecialInputPriceDetail>> GetListWithDetailAsync(Guid specialInputId, CancellationToken cancellationToken = default);
}