using QuoteFlow.SpecialInputPrices.ParameterObject;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.SpecialInputPrices;

public interface ISpecialInputPriceRepository : IRepository<SpecialInputPrice, Guid>
{
    Task<List<SpecialInputPrice>> GetListAsync(
        string? filterText = null,
        string? accountNo = null,
        string? accountName = null,
        string? projectName = null,
        DateTime? validFromMin = null,
        DateTime? validFromMax = null,
        DateTime? validToMin = null,
        DateTime? validToMax = null,
        string? status = null,
        string? note = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        string? filterText = null,
        string? accountNo = null,
        string? accountName = null,
        string? projectName = null,
        DateTime? validFromMin = null,
        DateTime? validFromMax = null,
        DateTime? validToMin = null,
        DateTime? validToMax = null,
        string? status = null,
        string? note = null,
        CancellationToken cancellationToken = default);

    Task<List<SpecialInputPriceListAccountNo>> GetDetailsByMaterialCodeAsync(string materialCode, CancellationToken cancellationToken = default);

    Task<List<SpecialInputPrice>> GetListAsync(
       SpecialInputPriceFilterParams filterParams,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
       SpecialInputPriceFilterParams filterParams,
        CancellationToken cancellationToken = default);

    Task HardDeleteAsync(Guid id);
    Task<List<SpecialInputPrice>> GetListWithDetailAsync(
       SpecialInputPriceFilterParams filterParams,
        CancellationToken cancellationToken = default
    );
}