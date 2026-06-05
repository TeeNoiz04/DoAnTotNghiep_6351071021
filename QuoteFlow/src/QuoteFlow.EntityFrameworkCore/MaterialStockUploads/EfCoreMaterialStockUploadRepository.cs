using QuoteFlow.EntityFrameworkCore;
using QuoteFlow.Helper;
using QuoteFlow.MaterialStockUploadDetails;
using QuoteFlow.MaterialStockUploads.ParameterObjects;
using QuoteFlow.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.MaterialStockUploads;

public class EfCoreMaterialStockUploadRepository : EfCoreRepository<QuoteFlowDbContext, MaterialStockUpload, Guid>, IMaterialStockUploadRepository
{
    public EfCoreMaterialStockUploadRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<MaterialStockUpload>> GetListAsync(
    MaterialStockUploadFilterParams filterParams,
    CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetQueryableAsync()), filterParams);
        query = query.OrderBy(string.IsNullOrWhiteSpace(filterParams.Sorting)
            ? MaterialStockUploadConsts.GetDefaultSorting(false)
            : filterParams.Sorting);

        return await query
            .PageBy(filterParams.SkipCount, filterParams.MaxResultCount)
            .ToListNoLockAsync(dbContext, cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        MaterialStockUploadFilterParams filterParams,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetDbSetAsync()), filterParams);
        return await query.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<MaterialStockUpload> ApplyFilter(
        IQueryable<MaterialStockUpload> query,
        MaterialStockUploadFilterParams filterParams)
    {
        var golfas = filterParams.GolfaCode;
        var model = filterParams.Model;
        return query
            .Where(x => x.Status == QuoteFlowStatuses.Closed)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.ImportType),
                e => e.ImportType.Contains(filterParams.ImportType))


            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.ApprovalStatus),
                e => e.Status.Contains(filterParams.ApprovalStatus))
            //.WhereIf(!string.IsNullOrWhiteSpace(filterParams.GolfaCode),
            //    e => e.MaterialStockUploadDetails.Any(x => x.MaterialCode.Contains(filterParams.GolfaCode)))
            .WhereIf(!string.IsNullOrWhiteSpace(golfas),
             QueryFilterHelper.BuildNestedCollectionSearch<MaterialStockUpload, MaterialStockUploadDetail>(
                    golfas,
                    e => e.MaterialStockUploadDetails,
                    d => d.MaterialCode
                ))
            .WhereIf(!string.IsNullOrWhiteSpace(model),
             QueryFilterHelper.BuildNestedCollectionSearch<MaterialStockUpload, MaterialStockUploadDetail>(
                    model,
                    e => e.MaterialStockUploadDetails,
                    d => d.Model
                ));


    }
    public override async Task<MaterialStockUpload> GetAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = await GetQueryableAsync();
        var result = await query
            .Include(x => x.MaterialStockUploadDetails)
            .Where(x => x.Id == id)
            .FirstOrDefaultNoLockAsync(dbContext, GetCancellationToken(cancellationToken))
            ?? throw new EntityNotFoundException(typeof(MaterialStockUpload), id);

        return result;
    }

    public async Task<string?> GetLatestCodeAsync(
        string prefix,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync();
        var latestCode = await query
            .Where(p => p.RequestNo!.StartsWith(prefix))
            .OrderByDescending(p => p.CreationTime)
            .Select(p => p.RequestNo)
            .FirstOrDefaultAsync(cancellationToken);

        return latestCode;
    }

}