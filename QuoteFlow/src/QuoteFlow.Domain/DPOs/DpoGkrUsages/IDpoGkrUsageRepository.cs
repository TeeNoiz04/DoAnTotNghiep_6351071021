using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.DPOs.DpoGkrUsages;

public interface IDpoGkrUsageRepository : IRepository<DpoGkrUsage, Guid>
{
    Task<List<DpoGkrUsage>> GetListAsync(
        string? filterText = null,
        Guid? gkrId = null,
        Guid? dpoId = null,
        decimal? gkrQtyMin = null,
        decimal? gkrQtyMax = null,
        decimal? dpoQtyMin = null,
        decimal? dpoQtyMax = null,
        decimal? gkrLockStockQtyMin = null,
        decimal? gkrLockStockQtyMax = null,
        decimal? dpoLockStockQtyMin = null,
        decimal? dpoLockStockQtyMax = null,
        decimal? gkrLockShipmentQtyMin = null,
        decimal? gkrLockShipmentQtyMax = null,
        decimal? dpoLockShipmentQtyMin = null,
        decimal? dpoLockShipmentQtyMax = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        string? filterText = null,
        Guid? gkrId = null,
        Guid? dpoId = null,
        decimal? gkrQtyMin = null,
        decimal? gkrQtyMax = null,
        decimal? dpoQtyMin = null,
        decimal? dpoQtyMax = null,
        decimal? gkrLockStockQtyMin = null,
        decimal? gkrLockStockQtyMax = null,
        decimal? dpoLockStockQtyMin = null,
        decimal? dpoLockStockQtyMax = null,
        decimal? gkrLockShipmentQtyMin = null,
        decimal? gkrLockShipmentQtyMax = null,
        decimal? dpoLockShipmentQtyMin = null,
        decimal? dpoLockShipmentQtyMax = null,
        CancellationToken cancellationToken = default
    );

    Task<DpoGkrUsage?> FindByGkrAndDpoAsync(
        Guid gkrId,
        Guid dpoId,
        CancellationToken cancellationToken = default
    );

    Task<List<DpoGkrUsage>> GetListByGkrIdAsync(
        Guid gkrId,
        CancellationToken cancellationToken = default
    );

    Task<List<DpoGkrUsage>> GetListByDpoIdAsync(
        Guid dpoId,
        CancellationToken cancellationToken = default
    );
}