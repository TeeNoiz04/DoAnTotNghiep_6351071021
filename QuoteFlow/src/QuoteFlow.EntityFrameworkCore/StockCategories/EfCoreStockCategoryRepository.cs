using QuoteFlow.EntityFrameworkCore;
using QuoteFlow.StockCategories.ParameterObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.StockCategories;

public class EfCoreStockCategoryRepository : EfCoreRepository<QuoteFlowDbContext, StockCategory, Guid>, IStockCategoryRepository
{
    public EfCoreStockCategoryRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<StockCategory>> GetListAsync(
     StockCategoryFilterParams filterParams,
     CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter(await GetQueryableAsync(), filterParams);
        query = query.OrderBy(string.IsNullOrWhiteSpace(filterParams.Sorting)
            ? StockCategoryConsts.GetDefaultSorting(false)
            : filterParams.Sorting);

        return await query
            .PageBy(filterParams.SkipCount, filterParams.MaxResultCount)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
    StockCategoryFilterParams filterParams,
    CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter(await GetDbSetAsync(), filterParams);
        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<StockCategory> ApplyFilter(
    IQueryable<StockCategory> query,
    StockCategoryFilterParams filterParams)
    {
        return query
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.FilterText),
                e => e.StockCode!.Contains(filterParams.FilterText!) ||
                     e.StockName!.Contains(filterParams.FilterText!) ||
                     e.Note!.Contains(filterParams.FilterText!))
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.StockCode),
                e => e.StockCode == (filterParams.StockCode))
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.StockName),
                e => e.StockName == (filterParams.StockName))
            .WhereIf(filterParams.MainStock.HasValue,
                e => e.MainStock == filterParams.MainStock)
            .WhereIf(filterParams.DamagedStock.HasValue,
                e => e.DamagedStock == filterParams.DamagedStock)
            .WhereIf(filterParams.FOC.HasValue,
                e => e.FOC == filterParams.FOC)
            .WhereIf(filterParams.SortOrderMin.HasValue,
                e => e.SortOrder >= filterParams.SortOrderMin.Value)
            .WhereIf(filterParams.SortOrderMax.HasValue,
                e => e.SortOrder <= filterParams.SortOrderMax.Value)
            .WhereIf(filterParams.IsDeactive.HasValue,
                e => e.IsDeactive == filterParams.IsDeactive)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.Note),
                e => e.Note.Contains(filterParams.Note));
    }

}