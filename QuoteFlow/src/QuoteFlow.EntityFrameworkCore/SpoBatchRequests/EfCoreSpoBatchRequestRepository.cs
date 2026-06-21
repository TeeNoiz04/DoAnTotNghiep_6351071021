using Dapper;
using QuoteFlow.EntityFrameworkCore;
using QuoteFlow.Helper;
using QuoteFlow.SaleOrders;
using QuoteFlow.SpoBatchRequests.ParameterObject;
using QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.SpoBatchRequests;

public class EfCoreSpoBatchRequestRepository : EfCoreRepository<QuoteFlowDbContext, SpoBatchRequest, Guid>, ISpoBatchRequestRepository
{
    public EfCoreSpoBatchRequestRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<SpoBatchRequest>> GetListAsync(
    SpoBatchRequestFilterParams input,
    CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter(await GetQueryableAsync(), input);

        query = query.OrderBy(
            string.IsNullOrWhiteSpace(input.Sorting)
                ? SpoBatchRequestConsts.GetDefaultSorting(false)
                : input.Sorting
        );

        return await query
            .PageBy(input.SkipCount, input.MaxResultCount)
            .ToListNoLockAsync(dbContext, cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        SpoBatchRequestFilterParams input,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter(await GetDbSetAsync(), input);
        return await query.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<SpoBatchRequest> ApplyFilter(
        IQueryable<SpoBatchRequest> query,
        SpoBatchRequestFilterParams input)
    {
        return query
            //.Where(x => x.Status == QuoteFlowStatuses.Closed)
            .Where(x => x.IsDeleted != true)
            .WhereIf(!string.IsNullOrWhiteSpace(input.RequestNo), e => e.RequestNo.Contains(input.RequestNo))
            .WhereIf(!string.IsNullOrWhiteSpace(input.ImportType), e => e.ImportType.Contains(input.ImportType))
            .WhereIf(!string.IsNullOrWhiteSpace(input.FileName), e => e.FileName.Contains(input.FileName))
            .WhereIf(!string.IsNullOrWhiteSpace(input.Note), e => e.Note.Contains(input.Note))
            .WhereIf(!string.IsNullOrWhiteSpace(input.Status), e => e.Status.Contains(input.Status))
            .WhereIf(!string.IsNullOrWhiteSpace(input.SPOCode), QueryFilterHelper.BuildNestedCollectionSearch<SpoBatchRequest, SpoBatchRequestDetail>(input.SPOCode, e => e.SpoBatchRequestDetails, d => d.SPOCode))
            .WhereIf(!string.IsNullOrWhiteSpace(input.GolfaCode), QueryFilterHelper.BuildNestedCollectionSearch<SpoBatchRequest, SpoBatchRequestDetail>(input.GolfaCode, e => e.SpoBatchRequestDetails, d => d.GolfaCode));
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

    public async Task GetBatchUpdateAsync(
     Guid requestId,
     CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();


        var parameters = new DynamicParameters();
        parameters.Add("@prRequestId", requestId);


        await connection.ExecuteAsync(
            "usp_TimerJob_SPO_BatchUpdate",
            parameters,
            transaction: dbContext.Database.CurrentTransaction?.GetDbTransaction(),
            commandType: CommandType.StoredProcedure
        );


    }

    public override async Task<SpoBatchRequest> GetAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = await GetQueryableAsync();

        var result = await query

            .Include(x => x.SpoBatchRequestDetails
            .Where(d => d.IsDeleted != true))
        .Where(p => p.Id == id && p.IsDeleted != true)
            .FirstOrDefaultNoLockAsync(dbContext, GetCancellationToken(cancellationToken))
            ?? throw new EntityNotFoundException(typeof(SaleOrder), id);



        return result;
    }
}