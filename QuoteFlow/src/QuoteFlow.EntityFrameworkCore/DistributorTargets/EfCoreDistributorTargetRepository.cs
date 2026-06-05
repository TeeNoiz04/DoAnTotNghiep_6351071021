using QuoteFlow.DistributorTargets.ParameterObjects;
using QuoteFlow.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.DistributorTargets;

public class EfCoreDistributorTargetRepository : EfCoreRepository<QuoteFlowDbContext, DistributorTarget, Guid>, IDistributorTargetRepository
{
    public EfCoreDistributorTargetRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<DistributorTarget>> GetListAsync(
    DistributorTargetFilterParams filterParams,
    CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetQueryableAsync()), filterParams);
        query = query.OrderBy(string.IsNullOrWhiteSpace(filterParams.Sorting) ? DistributorTargetConsts.GetDefaultSorting(false) : filterParams.Sorting);
        return await query.PageBy(filterParams.SkipCount, filterParams.MaxResultCount).ToListNoLockAsync(dbContext, cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        DistributorTargetFilterParams filterParams,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetDbSetAsync()), filterParams);
        return await query.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<DistributorTarget> ApplyFilter(
        IQueryable<DistributorTarget> query,
        DistributorTargetFilterParams filterParams
        )
    {
        var filterText = filterParams.FilterText;
        var buyerTypeId = filterParams.BuyerTypeId;
        var buyerId = filterParams.BuyerId;
        var buyerCode = filterParams.BuyerCode;
        var buyerName = filterParams.BuyerName;
        var materialType = filterParams.MaterialType;
        var financeYearMin = filterParams.FinanceYearMin;
        var financeYearMax = filterParams.FinanceYearMax;
        var firstFYTargetMin = filterParams.FirstFYTargetMin;
        var firstFYTargetMax = filterParams.FirstFYTargetMax;
        var secondFYTargetMin = filterParams.SecondFYTargetMin;
        var secondFYTargetMax = filterParams.SecondFYTargetMax;
        var note = filterParams.Note;
        return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.BuyerName!.Contains(filterText!) || e.BuyerCode!.Contains(filterText!) || e.MaterialType!.Contains(filterText!) || e.Note!.Contains(filterText!))
                .WhereIf(buyerTypeId.HasValue, e => e.BuyerTypeId == buyerTypeId)
                .WhereIf(buyerId.HasValue, e => e.BuyerId == buyerId)
                .WhereIf(!string.IsNullOrWhiteSpace(buyerCode), e => e.BuyerCode.Contains(buyerCode))
                .WhereIf(!string.IsNullOrWhiteSpace(buyerName), e => e.BuyerName.Contains(buyerName))
                .WhereIf(!string.IsNullOrWhiteSpace(materialType), e => e.MaterialType.Contains(materialType))
                .WhereIf(financeYearMin.HasValue, e => e.FinanceYear >= financeYearMin!.Value)
                .WhereIf(financeYearMax.HasValue, e => e.FinanceYear == financeYearMax!.Value)
                .WhereIf(firstFYTargetMin.HasValue, e => e.FirstFYTarget >= firstFYTargetMin!.Value)
                .WhereIf(firstFYTargetMax.HasValue, e => e.FirstFYTarget <= firstFYTargetMax!.Value)
                .WhereIf(secondFYTargetMin.HasValue, e => e.SecondFYTarget >= secondFYTargetMin!.Value)
                .WhereIf(secondFYTargetMax.HasValue, e => e.SecondFYTarget <= secondFYTargetMax!.Value)
                .WhereIf(!string.IsNullOrWhiteSpace(note), e => e.Note.Contains(note));
    }
}