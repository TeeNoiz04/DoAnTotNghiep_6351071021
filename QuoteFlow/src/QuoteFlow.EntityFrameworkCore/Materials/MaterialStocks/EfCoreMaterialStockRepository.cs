using QuoteFlow.EntityFrameworkCore;
using QuoteFlow.Extensions;
using QuoteFlow.Helper;
using QuoteFlow.Materials.MaterialStocks.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.Materials.MaterialStocks;

public class EfCoreMaterialStockRepository : EfCoreRepository<QuoteFlowDbContext, MaterialStock, Guid>, IMaterialStockRepository
{
    public EfCoreMaterialStockRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<MaterialStock>> GetListAsync(
        MaterialStockFilterParams filterParams,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = (await GetQueryableAsync());

        query = ApplyFilter(query, filterParams);

        query = query.OrderBy(
            string.IsNullOrWhiteSpace(filterParams.Sorting)
                ? MaterialStockConsts.GetDefaultSorting(false)
                : filterParams.Sorting
        );

        return await query
            .PageBy(filterParams.SkipCount, filterParams.MaxResultCount)
            .ToListNoLockAsync(dbContext, cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        MaterialStockFilterParams filterParams,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = (await GetQueryableAsync());

        query = ApplyFilter(query, filterParams);
        return await query.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<MaterialStock> ApplyFilter(
        IQueryable<MaterialStock> query,
        MaterialStockFilterParams filterParams)
    {
        var golfaCodes = ConvertListToSearchTerms(filterParams.GolfaCodes);
        var models = ConvertListToSearchTerms(filterParams.Models);
        var materialType = filterParams.MaterialType;
        //var stockQty = filterParams.StockQty;
        //var onOderStock = filterParams.OnOderStock;
        return query
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.FilterText),
                e => e.GolfaCode!.Contains(filterParams.FilterText!) ||
                     e.Model!.Contains(filterParams.FilterText!) ||
                     e.Note!.Contains(filterParams.FilterText!))
            .WhereIf(filterParams.MaterialId.HasValue, e => e.MaterialId == filterParams.MaterialId)
            .WhereIf(filterParams.StockCategoryId.HasValue, e => e.StockCategoryId == filterParams.StockCategoryId)
            .WhereIf(!string.IsNullOrWhiteSpace(golfaCodes), QueryFilterHelper.BuildMultiFieldSearch<MaterialStock>(golfaCodes, e => e.GolfaCode))
            .WhereIf(!string.IsNullOrWhiteSpace(models), QueryFilterHelper.BuildMultiFieldSearch<MaterialStock>(models, e => e.Model))
            .WhereIf(filterParams.QtyMin.HasValue, e => e.Qty >= filterParams.QtyMin!.Value)
            .WhereIf(filterParams.QtyMax.HasValue, e => e.Qty <= filterParams.QtyMax!.Value)
            .WhereIf(filterParams.LockedMin.HasValue, e => e.Locked >= filterParams.LockedMin!.Value)
            .WhereIf(filterParams.LockedMax.HasValue, e => e.Locked <= filterParams.LockedMax!.Value)
            .WhereIf(filterParams.LockStockKeepingMin.HasValue, e => e.LockStockKeeping >= filterParams.LockStockKeepingMin!.Value)
            .WhereIf(filterParams.LockStockKeepingMax.HasValue, e => e.LockStockKeeping <= filterParams.LockStockKeepingMax!.Value)
            .WhereIf(filterParams.LockStockSOMin.HasValue, e => e.LockStockSO >= filterParams.LockStockSOMin!.Value)
            .WhereIf(filterParams.LockStockSOMax.HasValue, e => e.LockStockSO <= filterParams.LockStockSOMax!.Value)
            .WhereIf(filterParams.Available_QtyMin.HasValue, e => e.Available_Qty >= filterParams.Available_QtyMin!.Value)
            .WhereIf(filterParams.Available_QtyMax.HasValue, e => e.Available_Qty <= filterParams.Available_QtyMax!.Value)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.Note), e => e.Note.Contains(filterParams.Note))
            //.WhereIf(!string.IsNullOrWhiteSpace(materialType), e => e.Material.MaterialType == materialType)
            .ApplyMaterialTypeFilter(filterParams, e => e.Material.MaterialType);
    }

    private static string? ConvertListToSearchTerms(List<string>? listTerms)
    {
        if (listTerms?.Any() != true)
        {
            return null;
        }

        var validTerms = listTerms
            .Where(term => !string.IsNullOrWhiteSpace(term))
            .Select(term => term.Trim())
            .ToList();

        return validTerms.Count != 0 ? string.Join("\n", validTerms) : null;
    }

}