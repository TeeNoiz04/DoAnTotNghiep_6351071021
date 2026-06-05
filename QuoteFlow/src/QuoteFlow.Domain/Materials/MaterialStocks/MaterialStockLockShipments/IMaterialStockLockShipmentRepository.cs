using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.Materials.MaterialStocks.MaterialStockLockShipments;

public interface IMaterialStockLockShipmentRepository : IRepository<MaterialStockLockShipment, Guid>
{
    Task<List<MaterialStockLockShipment>> GetListAsync(
        string? filterText = null,
        string? golfaCode = null,
        int? lockOnOrderMin = null,
        int? lockOnOrderMax = null,
        int? stockOnOrderMin = null,
        int? stockOnOrderMax = null,
        string? note = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        string? filterText = null,
        string? golfaCode = null,
        int? lockOnOrderMin = null,
        int? lockOnOrderMax = null,
        int? stockOnOrderMin = null,
        int? stockOnOrderMax = null,
        string? note = null,
        CancellationToken cancellationToken = default);
}