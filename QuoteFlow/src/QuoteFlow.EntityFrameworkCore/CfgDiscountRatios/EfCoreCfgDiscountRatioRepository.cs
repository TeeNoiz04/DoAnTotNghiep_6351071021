using QuoteFlow.CfgDiscountRatios.ParameterObjects;
using QuoteFlow.EntityFrameworkCore;
using QuoteFlow.SystemCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.CfgDiscountRatios;

public class EfCoreCfgDiscountRatioRepository : EfCoreRepository<QuoteFlowDbContext, CfgDiscountRatio, Guid>, ICfgDiscountRatioRepository
{
    public EfCoreCfgDiscountRatioRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<CfgDiscountRatio>> GetListAsync(
CfgDiscountRatioFilterParams filterParams,
CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var categories = dbContext.Set<SystemCategory>();
        var baseQuery = await GetQueryableAsync();
        var filteredQuery = ApplyFilter(baseQuery, filterParams);

        var joinedQuery =
            from cfg in filteredQuery
            join sc in categories.Where(c => c.CategoryType == "Key_Account_Classify")
                on cfg.AccountClassify equals sc.Code into scGroup
            from sc in scGroup.DefaultIfEmpty()
            join scType in categories.Where(c => c.CategoryType == "Key_Account_Type")
                on sc.ParentId equals scType.Id into scTypeGroup
            from scType in scTypeGroup.DefaultIfEmpty()
            select new
            {
                Cfg = cfg,
                ScTypeCode = scType != null ? scType.Code : null,
                ScCode = sc != null ? sc.Code : null,
                ScSortOrder = sc != null ? (int?)sc.SortOrder : null
            };

        // Apply KAType filter after join
        if (!string.IsNullOrWhiteSpace(filterParams.KAType))
        {
            joinedQuery = joinedQuery.Where(x => x.ScTypeCode == filterParams.KAType);
        }

            // Sort ascending by SortOrder (nulls last)
            joinedQuery = joinedQuery
                .OrderBy(x => x.ScSortOrder.HasValue ? 0 : 1)
                .ThenBy(x => x.ScSortOrder ?? int.MaxValue)
                .ThenBy(x => x.Cfg.Value_Min);

        var results = await joinedQuery
            .PageBy(filterParams.SkipCount, filterParams.MaxResultCount)
            .ToListNoLockAsync(dbContext,cancellationToken);

        return results.Select(x =>
        {
            x.Cfg.KAType = x.ScTypeCode;
            return x.Cfg;
        }).ToList();
    }

    public virtual async Task<long> GetCountAsync(
        CfgDiscountRatioFilterParams filterParams,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var categories = dbContext.Set<SystemCategory>();
        var baseQuery = await GetQueryableAsync();
        var filteredQuery = ApplyFilter(baseQuery, filterParams);

        var joinedQuery =
            from cfg in filteredQuery
            join sc in categories.Where(c => c.CategoryType == "Key_Account_Classify")
                on cfg.AccountClassify equals sc.Code into scGroup
            from sc in scGroup.DefaultIfEmpty()
            join scType in categories.Where(c => c.CategoryType == "Key_Account_Type")
                on sc.ParentId equals scType.Id into scTypeGroup
            from scType in scTypeGroup.DefaultIfEmpty()
            select new
            {
                Cfg = cfg,
                ScTypeCode = scType != null ? scType.Code : null
            };

        // Apply KAType filter after join
        if (!string.IsNullOrWhiteSpace(filterParams.KAType))
        {
            joinedQuery = joinedQuery.Where(x => x.ScTypeCode == filterParams.KAType);
        }

        return await joinedQuery.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<CfgDiscountRatio> ApplyFilter(
        IQueryable<CfgDiscountRatio> query,
        CfgDiscountRatioFilterParams filterParams
        )
    {
        var filterText = filterParams.FilterText;
        var approval_Type = filterParams.Approval_Type;
        var product_Type = filterParams.Product_Type;
        var accountClassify = filterParams.AccountClassify;
        var value_MinMin = filterParams.Value_MinMin;
        var value_MinMax = filterParams.Value_MinMax;
        var value_MaxMin = filterParams.Value_MaxMin;
        var value_MaxMax = filterParams.Value_MaxMax;
        var discountRatioMin = filterParams.DiscountRatioMin;
        var discountRatioMax = filterParams.DiscountRatioMax;
        var note = filterParams.Note;
        return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Approval_Type!.Contains(filterText!) || e.Product_Type!.Contains(filterText!) || e.AccountClassify!.Contains(filterText!) || e.Note!.Contains(filterText!))
                .WhereIf(!string.IsNullOrWhiteSpace(approval_Type), e => e.Approval_Type.Contains(approval_Type))
                .WhereIf(!string.IsNullOrWhiteSpace(product_Type), e => e.Product_Type.Contains(product_Type))
                .WhereIf(!string.IsNullOrWhiteSpace(accountClassify), e => e.AccountClassify.Contains(accountClassify))
                .WhereIf(value_MinMin.HasValue, e => e.Value_Min >= value_MinMin!.Value)
                .WhereIf(value_MinMax.HasValue, e => e.Value_Min <= value_MinMax!.Value)
                .WhereIf(value_MaxMin.HasValue, e => e.Value_Max >= value_MaxMin!.Value)
                .WhereIf(value_MaxMax.HasValue, e => e.Value_Max <= value_MaxMax!.Value)
                .WhereIf(discountRatioMin.HasValue, e => e.DiscountRatio >= discountRatioMin!.Value)
                .WhereIf(discountRatioMax.HasValue, e => e.DiscountRatio <= discountRatioMax!.Value)
                .WhereIf(!string.IsNullOrWhiteSpace(note), e => e.Note.Contains(note));
    }
}