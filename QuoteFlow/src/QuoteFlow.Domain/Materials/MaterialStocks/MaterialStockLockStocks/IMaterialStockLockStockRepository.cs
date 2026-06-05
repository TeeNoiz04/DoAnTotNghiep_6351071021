using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.Materials.MaterialStocks.MaterialStockLockStocks;

public interface IMaterialStockLockStockRepository : IRepository<MaterialStockLockStock, Guid>
{
    Task<List<MaterialStockLockStock>> GetListAsync(
        string? filterText = null,
        string? golfaCode = null,
        Guid? dPODetailId = null,
        Guid? stockCategoryId = null,
        int? qtyMin = null,
        int? qtyMax = null,
        string? note = null,
        int? releasedLockMin = null,
        int? releasedLockMax = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        string? filterText = null,
        string? golfaCode = null,
        Guid? dPODetailId = null,
        Guid? stockCategoryId = null,
        int? qtyMin = null,
        int? qtyMax = null,
        string? note = null,
        int? releasedLockMin = null,
        int? releasedLockMax = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all unlockable lock stock records for a DPO detail (where ReleasedLock = 0)
    /// </summary>
    Task<List<MaterialStockLockStock>> GetUnlockableByDetailIdAsync(
        Guid dpoDetailId,
        CancellationToken cancellationToken = default);
}