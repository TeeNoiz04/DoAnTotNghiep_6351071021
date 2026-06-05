using QuoteFlow.MaterialStockUploads.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.MaterialStockUploads;

public interface IMaterialStockUploadRepository : IRepository<MaterialStockUpload, Guid>
{
    Task<List<MaterialStockUpload>> GetListAsync(
       MaterialStockUploadFilterParams filterParams,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
       MaterialStockUploadFilterParams filterParams,
        CancellationToken cancellationToken = default);

    Task<string?> GetLatestCodeAsync(
    string prefix,
    CancellationToken cancellationToken = default);
}