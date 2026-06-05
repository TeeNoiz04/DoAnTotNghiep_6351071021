using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.MaterialStockUploadDetails;

public interface IMaterialStockUploadDetailRepository : IRepository<MaterialStockUploadDetail, Guid>
{
    Task<List<MaterialStockUploadDetail>> GetListAsync(
        string? filterText = null,
        Guid? requestId = null,
        string? materialCode = null,
        string? model = null,
        string? storage = null,
        string? storageDestination = null,
        decimal? qtyMin = null,
        decimal? qtyMax = null,
        string? refDoc = null,
        string? remark = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        string? filterText = null,
        Guid? requestId = null,
        string? materialCode = null,
        string? model = null,
        string? storage = null,
        string? storageDestination = null,
        decimal? qtyMin = null,
        decimal? qtyMax = null,
        string? refDoc = null,
        string? remark = null,
        CancellationToken cancellationToken = default);
}