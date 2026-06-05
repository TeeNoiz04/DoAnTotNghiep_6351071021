using Dapper;
using QuoteFlow.EntityFrameworkCore;
using QuoteFlow.Materials.MaterialApprovalRequests.ParameterObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.Materials.MaterialApprovalRequests;

public class EfCoreMaterialApprovalRequestRepository : EfCoreRepository<QuoteFlowDbContext, MaterialApprovalRequest, Guid>, IMaterialApprovalRequestRepository
{
    public EfCoreMaterialApprovalRequestRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<MaterialApprovalRequest>> GetListAsync(
        MaterialApprovalRequestFilterParams filterParams,
        string? currenUser,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter(await GetQueryableAsync(), filterParams);
        query = query.Include(x => x.MaterialHistories
            .OrderByDescending(h => h.ActionDate));

        query = query.OrderBy(string.IsNullOrWhiteSpace(filterParams.Sorting) ? MaterialApprovalRequestConsts.GetDefaultSorting(false) : filterParams.Sorting);
        return await query.PageBy(filterParams.SkipCount, filterParams.MaxResultCount).ToListNoLockAsync(dbContext, cancellationToken);
    }

    public virtual async Task<List<MaterialApprovalRequest>> GetListPendingAsync(
        MaterialApprovalRequestFilterParams filterParams,
        string approverUsername,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter(await GetQueryableAsync(), filterParams);

        query = query
            .Include(x => x.MaterialRoutes)
            .Include(x => x.MaterialHistories.OrderByDescending(h => h.ActionDate))
             .Where(x =>
                x.CurrentApprovalRouteInstanceId != null &&

                x.MaterialRoutes!.Any(r => r.Approver == approverUsername &&
                r.IsApproved == false &&
                r.StepSequence == x.CurrentApprovalStepSequence &&
                r.InstanceId == x.CurrentApprovalRouteInstanceId));


        query = query.OrderBy(string.IsNullOrWhiteSpace(filterParams.Sorting) ? MaterialApprovalRequestConsts.GetDefaultSorting(false) : filterParams.Sorting);
        return await query.PageBy(filterParams.SkipCount, filterParams.MaxResultCount).ToListNoLockAsync(dbContext, cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        MaterialApprovalRequestFilterParams filterParams,
        string? currenUser = null,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter(await GetDbSetAsync(), filterParams);
        return await query.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<MaterialApprovalRequest> ApplyFilter(
        IQueryable<MaterialApprovalRequest> query,
        MaterialApprovalRequestFilterParams filterParams)
    {
        return query
                //.WhereIf(!string.IsNullOrWhiteSpace(currenUser), e => e.CreatorUsername != null && e.CreatorUsername.Contains(currenUser!))
                .WhereIf(!string.IsNullOrWhiteSpace(filterParams.ImportType), e => e.ImportType != null && e.ImportType.Contains(filterParams.ImportType!))
                .WhereIf(!string.IsNullOrWhiteSpace(filterParams.ApprovalStatus), e => e.Status != null && e.Status.Contains(filterParams.ApprovalStatus!))
                .WhereIf(!string.IsNullOrWhiteSpace(filterParams.GolfaCode), e => e.MaterialApprovalDetails != null && e.MaterialApprovalDetails.Any(x => x.GolfaCode.Contains(filterParams.GolfaCode!)))
                .WhereIf(!string.IsNullOrWhiteSpace(filterParams.Model), e => e.MaterialApprovalDetails != null && e.MaterialApprovalDetails.Any(x => x.Model.Contains(filterParams.Model!)));


    }

    public async Task<string?> GetLastestRequestNoAsync(string prefix)
    {
        var dbSet = await GetDbSetAsync();

        var requestNos = await dbSet
            .Where(r => r.RequestNo != null && r.RequestNo.StartsWith(prefix))
             .Select(r => new
             {
                 RequestNo = r.RequestNo!,
                 Last3Digits = Convert.ToInt32(r.RequestNo!.Substring(r.RequestNo.Length - 3))
             })
            .OrderByDescending(x => x.Last3Digits)
            .ToListAsync();

        var latest = requestNos.Select(x => x.RequestNo).FirstOrDefault();

        return latest;
    }




    public async Task<MaterialApprovalRequest> GetWithDetailAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = await GetQueryableAsync();
        var result = await query
            .Include(x => x.MaterialApprovalDetails)
            .Include(x => x.MaterialHistories.OrderByDescending(h => h.ActionDate))
            .Include(x => x.MaterialRoutes)
            .Where(x => x.Id == id)
            .FirstOrDefaultNoLockAsync(dbContext, cancellationToken);
        return result;
    }

    public async Task<MaterialApprovalRequest> GetWithDetailNoTrackingAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = await GetQueryableAsync();
        var result = await query
            .Include(x => x.MaterialApprovalDetails)
            .Include(x => x.MaterialHistories.OrderByDescending(h => h.ActionDate))
            .Include(x => x.MaterialRoutes)
            .AsNoTracking()
            .Where(x => x.Id == id)
            .FirstOrDefaultNoLockAsync(dbContext, cancellationToken);
        return result;
    }
    public async Task CreateApprovalRoute(MaterialApprovalWFRouteFilterParams filterParams, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();

        var parameters = new
        {
            requestId = filterParams.MaterialId,
            importType = filterParams.ImportType,
            note = filterParams.Note
        };

        await connection.ExecuteAsync(
            "usp_MaterialApproval_WF_CreateApprovalRoute",
            parameters,
            commandType: CommandType.StoredProcedure,
            transaction: await GetDbTransactionAsync()
        );
    }

    public virtual async Task<List<MaterialApprovalRequestRoute>> GetListApprovalRoutesAsync(
        Guid? materialApprovalId
    )
    {
        var query = await GetQueryableAsync();

        var approvalRoutes = query
            .Include(po => po.MaterialRoutes)
            .WhereIf(materialApprovalId.HasValue, po => po.Id == materialApprovalId!.Value)
            .SelectMany(po => po.MaterialRoutes)
            .ToList();

        return approvalRoutes;
    }

    public virtual async Task<List<T>> GetListAsync<T>(MaterialApprovalRequestFilterParams filterParams, Expression<Func<MaterialApprovalRequest, T>> selector, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetQueryableAsync()), filterParams);

        query = query.OrderBy(MaterialApprovalRequestConsts.GetDefaultSorting(false));
        var resultQuery = query
            .PageBy(filterParams.SkipCount, filterParams.MaxResultCount)
            .Select(selector).AsQueryable();

        var result = await resultQuery.ToListNoLockAsync(dbContext, cancellationToken);

        return result;
    }


}