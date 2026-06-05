using QuoteFlow.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.MaterialStockUploadDetails;

public class EfCoreMaterialStockUploadDetailRepository : EfCoreRepository<QuoteFlowDbContext, MaterialStockUploadDetail, Guid>, IMaterialStockUploadDetailRepository
{
    public EfCoreMaterialStockUploadDetailRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<MaterialStockUploadDetail>> GetListAsync(
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
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetQueryableAsync()), filterText, requestId, materialCode, model, storage, storageDestination, qtyMin, qtyMax, refDoc, remark);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? MaterialStockUploadDetailConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListNoLockAsync(dbContext, cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
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
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetDbSetAsync()), filterText, requestId, materialCode, model, storage, storageDestination, qtyMin, qtyMax, refDoc, remark);
        return await query.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<MaterialStockUploadDetail> ApplyFilter(
        IQueryable<MaterialStockUploadDetail> query,
        string? filterText = null,
        Guid? requestId = null,
        string? materialCode = null,
        string? model = null,
        string? storage = null,
        string? storageDestination = null,
        decimal? qtyMin = null,
        decimal? qtyMax = null,
        string? refDoc = null,
        string? remark = null)
    {
        return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.MaterialCode!.Contains(filterText!) || e.Model!.Contains(filterText!) || e.Storage!.Contains(filterText!) || e.StorageDestination!.Contains(filterText!) || e.RefDoc!.Contains(filterText!) || e.Remark!.Contains(filterText!))
                .WhereIf(requestId.HasValue, e => e.RequestId == requestId)
                .WhereIf(!string.IsNullOrWhiteSpace(materialCode), e => e.MaterialCode.Contains(materialCode))
                .WhereIf(!string.IsNullOrWhiteSpace(model), e => e.Model.Contains(model))
                .WhereIf(!string.IsNullOrWhiteSpace(storage), e => e.Storage.Contains(storage))
                .WhereIf(!string.IsNullOrWhiteSpace(storageDestination), e => e.StorageDestination.Contains(storageDestination))
                .WhereIf(qtyMin.HasValue, e => e.Qty >= qtyMin!.Value)
                .WhereIf(qtyMax.HasValue, e => e.Qty <= qtyMax!.Value)
                .WhereIf(!string.IsNullOrWhiteSpace(refDoc), e => e.RefDoc.Contains(refDoc))
                .WhereIf(!string.IsNullOrWhiteSpace(remark), e => e.Remark.Contains(remark));
    }
}