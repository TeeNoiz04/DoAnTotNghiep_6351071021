using QuoteFlow.EntityFrameworkCore;
using QuoteFlow.Helper;
using QuoteFlow.Materials;
using QuoteFlow.SupplierBUs.ParameterObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.SupplierBUs;

public class EfCoreSupplierBURepository : EfCoreRepository<QuoteFlowDbContext, SupplierBU, Guid>, ISupplierBURepository
{
    public EfCoreSupplierBURepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<SupplierBU>> GetListAsync(
    SupplierBUFilterParams filterParams,
    CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter(await GetQueryableAsync(), filterParams);
        query = query.Include(x => x.Supplier);
        query = query.OrderBy(string.IsNullOrWhiteSpace(filterParams.Sorting)
            ? SupplierBUConsts.GetDefaultSorting(false)
            : filterParams.Sorting);

        return await query
            .PageBy(filterParams.SkipCount, filterParams.MaxResultCount)
            .ToListNoLockAsync(dbContext, cancellationToken);
    }


    public virtual async Task<long> GetCountAsync(
    SupplierBUFilterParams filterParams,
    CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter(await GetDbSetAsync(), filterParams);
        return await query.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }


    protected virtual IQueryable<SupplierBU> ApplyFilter(
        IQueryable<SupplierBU> query,
        SupplierBUFilterParams filterParams)
    {
        return query
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.SupplierBUCode), QueryFilterHelper.BuildMultiFieldSearch<SupplierBU>(filterParams.SupplierBUCode, e => e.SupplierBUCode))
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.SupplierBURemarks), e => e.SupplierBURemarks == filterParams.SupplierBURemarks)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.OrderMethod), e => e.OrderMethod == filterParams.OrderMethod)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.POTemplate), e => e.POTemplate == filterParams.POTemplate)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.Contact), e => e.Contact == filterParams.Contact)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.Email), e => e.Email == filterParams.Email)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.INCOTerm), e => e.INCOTerm == filterParams.INCOTerm)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.PaymentTermCode), e => e.PaymentTermCode == filterParams.PaymentTermCode)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.PaymentDescription), e => e.PaymentDescription == filterParams.PaymentDescription)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.Currency), e => e.Currency == filterParams.Currency)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.MaterialType), e => e.MaterialType == filterParams.MaterialType)
            .WhereIf(filterParams.SupplierId.HasValue, e => e.SupplierId == filterParams.SupplierId)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.SupplierCode), e => e.SupplierCode == filterParams.SupplierCode)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.SupplierShortName), e => e.SupplierShortName == filterParams.SupplierShortName)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.SupplierAddress), e => e.SupplierAddress == filterParams.SupplierAddress)
            .WhereIf(filterParams.SortOrder.HasValue, e => e.SortOrder == filterParams.SortOrder.Value)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.FASCMVendorCode), e => e.FASCMVendorCode == filterParams.FASCMVendorCode)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.FASCMBuyerCode), e => e.FASCMBuyerCode == filterParams.FASCMBuyerCode)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.FASCMConsigneeCode), e => e.FASCMConsigneeCode == filterParams.FASCMConsigneeCode)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.FASCMSectionCode), e => e.FASCMSectionCode == filterParams.FASCMSectionCode)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.FASCMPaymentTerm), e => e.FASCMPaymentTerm == filterParams.FASCMPaymentTerm)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.FASCMFreightMethod), e => e.FASCMFreightMethod == filterParams.FASCMFreightMethod)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.FASCMDeliveryTerms), e => e.FASCMDeliveryTerms == filterParams.FASCMDeliveryTerms)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.FASCMPlaceOfDeliveryTerms), e => e.FASCMPlaceOfDeliveryTerms == filterParams.FASCMPlaceOfDeliveryTerms)
            .WhereIf(!string.IsNullOrWhiteSpace(filterParams.FASCMShippingMarkCode), e => e.FASCMShippingMarkCode == filterParams.FASCMShippingMarkCode)
            .WhereIf(filterParams.IsDeactive.HasValue, e => e.IsDeactive == filterParams.IsDeactive.Value);
    }

    public virtual async Task<List<T>> GetListAsync<T>(
        SupplierBUFilterParams filterParams,
        Expression<Func<SupplierBU, T>> selector,
        CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetQueryableAsync()), filterParams);

        query = query.OrderBy(string.IsNullOrWhiteSpace(filterParams.Sorting)
            ? MaterialConsts.GetDefaultSorting(false)
            : filterParams.Sorting);
        var resultQuery = query
            .PageBy(filterParams.SkipCount, filterParams.MaxResultCount)
            .Select(selector).AsQueryable();

        var result = await resultQuery.ToListAsync(cancellationToken);

        return result;
    }

}