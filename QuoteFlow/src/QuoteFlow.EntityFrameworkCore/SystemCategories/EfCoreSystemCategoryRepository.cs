using QuoteFlow.EntityFrameworkCore;
using QuoteFlow.SystemCategories.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.SystemCategories;

public class EfCoreSystemCategoryRepository : EfCoreRepository<QuoteFlowDbContext, SystemCategory, Guid>, ISystemCategoryRepository
{
    public EfCoreSystemCategoryRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<SystemCategory>> GetListAsync(
       SystemCategoryFilterParams filterParams,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetQueryableAsync()), filterParams);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? SystemCategoryConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListNoLockAsync(dbContext, cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        SystemCategoryFilterParams filterParams,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetDbSetAsync()), filterParams);
        return await query.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }
    public virtual async Task<List<T>> GetListAsync<T>(
        SystemCategoryFilterParams filterParams,
        Expression<Func<SystemCategory, T>> selector,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetQueryableAsync()), filterParams);

        query = query.OrderBy(SystemCategoryConsts.GetDefaultSorting(false));
        var resultQuery = query.Select(selector).AsQueryable();

        var result = await resultQuery.ToListNoLockAsync(dbContext, cancellationToken);

        return result;
    }

    protected virtual IQueryable<SystemCategory> ApplyFilter(
    IQueryable<SystemCategory> query,
    SystemCategoryFilterParams filterParams
)
    {
        var filterText = filterParams.FilterText;
        var parentId = filterParams.ParentId;
        var code = filterParams.Code;
        var description = filterParams.Description;
        var valueMin = filterParams.ValueMin;
        var valueMax = filterParams.ValueMax;
        var categoryType = filterParams.CategoryType;
        var note = filterParams.Note;
        var isDeactive = filterParams.IsDeactive;
        var sortOrderMin = filterParams.SortOrderMin;
        var sortOrderMax = filterParams.SortOrderMax;

        return query
            .WhereIf(!string.IsNullOrWhiteSpace(filterText),
                e =>
                    (e.Code ?? "").Contains(filterText!) ||
                    (e.Description ?? "").Contains(filterText!) ||
                    (e.CategoryType ?? "").Contains(filterText!) ||
                    (e.Note ?? "").Contains(filterText!))
            .WhereIf(parentId.HasValue, e => e.ParentId == parentId)
            .WhereIf(!string.IsNullOrWhiteSpace(code), e => e.Code == code)
            .WhereIf(!string.IsNullOrWhiteSpace(description), e => e.Description == description)
            .WhereIf(valueMin.HasValue, e => e.Value >= valueMin!.Value)
            .WhereIf(valueMax.HasValue, e => e.Value <= valueMax!.Value)
            .WhereIf(!string.IsNullOrWhiteSpace(categoryType), e => e.CategoryType == categoryType)
            .WhereIf(!string.IsNullOrWhiteSpace(note), e => e.Note == note)
            .WhereIf(isDeactive.HasValue, e => e.IsDeactive == isDeactive)
            .WhereIf(sortOrderMin.HasValue, e => e.SortOrder >= sortOrderMin!.Value)
            .WhereIf(sortOrderMax.HasValue, e => e.SortOrder <= sortOrderMax!.Value);

    }
}