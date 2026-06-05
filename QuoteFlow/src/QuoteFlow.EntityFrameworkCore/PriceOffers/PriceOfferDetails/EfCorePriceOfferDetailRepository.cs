using QuoteFlow.EntityFrameworkCore;
using QuoteFlow.PriceOffers.PriceOfferDetails.ParameterObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.PriceOffers.PriceOfferDetails;

public class EfCorePriceOfferDetailRepository : EfCoreRepository<QuoteFlowDbContext, PriceOfferDetail, Guid>, IPriceOfferDetailRepository
{
    public EfCorePriceOfferDetailRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<PriceOfferDetail>> GetListAsync(
        PriceOfferDetailFilterParams filterParams,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter(await GetQueryableAsync(), filterParams)
            .Include(pd => pd.ApprovalHistories.OrderByDescending(x => x.CreationTime))
            .AsQueryable();

        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? PriceOfferDetailConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListNoLockAsync(dbContext, cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        PriceOfferDetailFilterParams filterParams,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter(await GetDbSetAsync(), filterParams);
        return await query.CountNoLockAsync(dbContext, cancellationToken);
    }

    protected virtual IQueryable<PriceOfferDetail> ApplyFilter(
        IQueryable<PriceOfferDetail> query,
        PriceOfferDetailFilterParams filterParams)
    {
        var filterText = filterParams.FilterText;
        var priceOfferId = filterParams.PriceOfferId;
        var status = filterParams.Status;
        var golfaCode = filterParams.GolfaCode;
        var modelName = filterParams.ModelName;
        var specialSpec1 = filterParams.SpecialSpec1;
        var specialSpec2 = filterParams.SpecialSpec2;
        var dpoUsedMin = filterParams.DpoUsedMin;
        var dpoUsedMax = filterParams.DpoUsedMax;
        var qtyMin = filterParams.QtyMin;
        var qtyMax = filterParams.QtyMax;
        var standardPriceMin = filterParams.StandardPriceMin;
        var standardPriceMax = filterParams.StandardPriceMax;
        var standardAmountMin = filterParams.StandardAmountMin;
        var standardAmountMax = filterParams.StandardAmountMax;
        var buyerPriceMin = filterParams.BuyerPriceMin;
        var buyerPriceMax = filterParams.BuyerPriceMax;
        var requestedAmountMin = filterParams.RequestedAmountMin;
        var requestedAmountMax = filterParams.RequestedAmountMax;
        var requestedDiscountRatioMin = filterParams.RequestedDiscountRatioMin;
        var requestedDiscountRatioMax = filterParams.RequestedDiscountRatioMax;
        var priceToCustomerMin = filterParams.PriceToCustomerMin;
        var priceToCustomerMax = filterParams.PriceToCustomerMax;
        var mEVNOfferPriceMin = filterParams.MEVNOfferPriceMin;
        var mEVNOfferPriceMax = filterParams.MEVNOfferPriceMax;
        var competitorBrand = filterParams.CompetitorBrand;
        var competitorModel = filterParams.CompetitorModel;
        var competitorPriceMin = filterParams.CompetitorPriceMin;
        var competitorPriceMax = filterParams.CompetitorPriceMax;
        var landingCostMin = filterParams.LandingCostMin;
        var landingCostMax = filterParams.LandingCostMax;
        var inputPriceMin = filterParams.InputPriceMin;
        var inputPriceMax = filterParams.InputPriceMax;
        var inputCurrency = filterParams.InputCurrency;
        var managerMarginMin = filterParams.ManagerMarginMin;
        var managerMarginMax = filterParams.ManagerMarginMax;
        var priceOfferDetailMarginMin = filterParams.PriceOfferDetailMarginMin;
        var priceOfferDetailMarginMax = filterParams.PriceOfferDetailMarginMax;
        var accountCode = filterParams.AccountCode;
        var note = filterParams.Note;
        var importGuid = filterParams.ImportGuid;
        var excludedStatuses = filterParams.ExcludedStatuses;

        return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.GolfaCode!.Contains(filterText!) || e.ModelName!.Contains(filterText!) || e.SpecialSpec1!.Contains(filterText!) || e.SpecialSpec2!.Contains(filterText!) || e.CompetitorBrand!.Contains(filterText!) || e.CompetitorModel!.Contains(filterText!) || e.InputCurrency!.Contains(filterText!) || e.AccountCode!.Contains(filterText!) || e.Note!.Contains(filterText!))
                .WhereIf(priceOfferId.HasValue, e => e.PriceOfferId == priceOfferId)
                .WhereIf(!string.IsNullOrWhiteSpace(status), e => e.Status == status)
                .WhereIf(!string.IsNullOrWhiteSpace(golfaCode), e => e.GolfaCode.Contains(golfaCode))
                .WhereIf(!string.IsNullOrWhiteSpace(modelName), e => e.ModelName.Contains(modelName))
                .WhereIf(!string.IsNullOrWhiteSpace(specialSpec1), e => e.SpecialSpec1.Contains(specialSpec1))
                .WhereIf(!string.IsNullOrWhiteSpace(specialSpec2), e => e.SpecialSpec2.Contains(specialSpec2))
                .WhereIf(dpoUsedMin.HasValue, e => e.DpoUsed >= dpoUsedMin!.Value)
                .WhereIf(dpoUsedMax.HasValue, e => e.DpoUsed <= dpoUsedMax!.Value)
                .WhereIf(qtyMin.HasValue, e => e.Qty >= qtyMin!.Value)
                .WhereIf(qtyMax.HasValue, e => e.Qty <= qtyMax!.Value)
                .WhereIf(standardPriceMin.HasValue, e => e.StandardPrice >= standardPriceMin!.Value)
                .WhereIf(standardPriceMax.HasValue, e => e.StandardPrice <= standardPriceMax!.Value)
                .WhereIf(standardAmountMin.HasValue, e => e.StandardAmount >= standardAmountMin!.Value)
                .WhereIf(standardAmountMax.HasValue, e => e.StandardAmount <= standardAmountMax!.Value)
                .WhereIf(buyerPriceMin.HasValue, e => e.BuyerPrice >= buyerPriceMin!.Value)
                .WhereIf(buyerPriceMax.HasValue, e => e.BuyerPrice <= buyerPriceMax!.Value)
                .WhereIf(requestedAmountMin.HasValue, e => e.RequestedAmount >= requestedAmountMin!.Value)
                .WhereIf(requestedAmountMax.HasValue, e => e.RequestedAmount <= requestedAmountMax!.Value)
                .WhereIf(requestedDiscountRatioMin.HasValue, e => e.RequestedDiscountRatio >= requestedDiscountRatioMin!.Value)
                .WhereIf(requestedDiscountRatioMax.HasValue, e => e.RequestedDiscountRatio <= requestedDiscountRatioMax!.Value)
                .WhereIf(priceToCustomerMin.HasValue, e => e.PriceToCustomer >= priceToCustomerMin!.Value)
                .WhereIf(priceToCustomerMax.HasValue, e => e.PriceToCustomer <= priceToCustomerMax!.Value)
                .WhereIf(mEVNOfferPriceMin.HasValue, e => e.MEVNOfferPrice >= mEVNOfferPriceMin!.Value)
                .WhereIf(mEVNOfferPriceMax.HasValue, e => e.MEVNOfferPrice <= mEVNOfferPriceMax!.Value)
                .WhereIf(!string.IsNullOrWhiteSpace(competitorBrand), e => e.CompetitorBrand.Contains(competitorBrand))
                .WhereIf(!string.IsNullOrWhiteSpace(competitorModel), e => e.CompetitorModel.Contains(competitorModel))
                .WhereIf(competitorPriceMin.HasValue, e => e.CompetitorPrice >= competitorPriceMin!.Value)
                .WhereIf(competitorPriceMax.HasValue, e => e.CompetitorPrice <= competitorPriceMax!.Value)
                .WhereIf(landingCostMin.HasValue, e => e.LandingCost >= landingCostMin!.Value)
                .WhereIf(landingCostMax.HasValue, e => e.LandingCost <= landingCostMax!.Value)
                .WhereIf(inputPriceMin.HasValue, e => e.InputPrice >= inputPriceMin!.Value)
                .WhereIf(inputPriceMax.HasValue, e => e.InputPrice <= inputPriceMax!.Value)
                .WhereIf(!string.IsNullOrWhiteSpace(inputCurrency), e => e.InputCurrency.Contains(inputCurrency))
                .WhereIf(managerMarginMin.HasValue, e => e.ManagerMargin >= managerMarginMin!.Value)
                .WhereIf(managerMarginMax.HasValue, e => e.ManagerMargin <= managerMarginMax!.Value)
                .WhereIf(priceOfferDetailMarginMin.HasValue, e => e.PriceOfferDetailMargin >= priceOfferDetailMarginMin!.Value)
                .WhereIf(priceOfferDetailMarginMax.HasValue, e => e.PriceOfferDetailMargin <= priceOfferDetailMarginMax!.Value)
                .WhereIf(!string.IsNullOrWhiteSpace(accountCode), e => e.AccountCode.Contains(accountCode))
                .WhereIf(!string.IsNullOrWhiteSpace(note), e => e.Note.Contains(note))
                .WhereIf(importGuid.HasValue, e => e.ImportGuid == importGuid)
                .WhereIf(excludedStatuses.Count > 0, e => !excludedStatuses.Contains(e.Status));
    }
}