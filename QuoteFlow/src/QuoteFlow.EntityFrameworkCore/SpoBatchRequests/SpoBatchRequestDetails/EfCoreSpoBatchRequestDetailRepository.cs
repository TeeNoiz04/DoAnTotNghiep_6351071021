using QuoteFlow.EntityFrameworkCore;
using QuoteFlow.SpoBatchRequestDetails;
using QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails;
public class EfCoreSpoBatchRequestDetailRepository
    : EfCoreRepository<QuoteFlowDbContext, SpoBatchRequestDetail, Guid>, ISpoBatchRequestDetailRepository
{
    public EfCoreSpoBatchRequestDetailRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public virtual async Task<List<SpoBatchRequestDetail>> GetListAsync(
        SpoBatchRequestDetailFilterParams input,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter(await GetQueryableAsync(), input);

        query = query.OrderBy(
            string.IsNullOrWhiteSpace(input.Sorting)
                ? SpoBatchRequestDetailConsts.GetDefaultSorting(false)
                : input.Sorting
        );

        return await query
            .PageBy(input.SkipCount, input.MaxResultCount)
            .ToListNoLockAsync(dbContext, cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        SpoBatchRequestDetailFilterParams input,
        CancellationToken cancellationToken = default)
    {

        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter(await GetDbSetAsync(), input);
        return await query.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<SpoBatchRequestDetail> ApplyFilter(
        IQueryable<SpoBatchRequestDetail> query,
        SpoBatchRequestDetailFilterParams input)
    {
        return query
            .Where(x => x.IsDeleted != true)
            .WhereIf(input.RequestId.HasValue, e => e.RequestId == input.RequestId)
            .WhereIf(!string.IsNullOrWhiteSpace(input.SPOCode), e => e.SPOCode.Contains(input.SPOCode))
            .WhereIf(!string.IsNullOrWhiteSpace(input.GolfaCode), e => e.GolfaCode.Contains(input.GolfaCode))
            .WhereIf(!string.IsNullOrWhiteSpace(input.Action), e => e.Action.Contains(input.Action))
            .WhereIf(input.ActionDateMin.HasValue, e => e.ActionDate >= input.ActionDateMin.Value)
            .WhereIf(input.ActionDateMax.HasValue, e => e.ActionDate <= input.ActionDateMax.Value)
            .WhereIf(!string.IsNullOrWhiteSpace(input.Note), e => e.Note.Contains(input.Note));
    }
}
