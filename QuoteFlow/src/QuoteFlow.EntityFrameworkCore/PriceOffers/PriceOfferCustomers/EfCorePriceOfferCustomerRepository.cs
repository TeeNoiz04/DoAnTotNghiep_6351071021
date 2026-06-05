using QuoteFlow.EntityFrameworkCore;
using QuoteFlow.PriceOffers.PriceOfferCustomers.ParameterObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.PriceOffers.PriceOfferCustomers;

public class EfCorePriceOfferCustomerRepository : EfCoreRepository<QuoteFlowDbContext, PriceOfferCustomer, Guid>, IPriceOfferCustomerRepository
{
    public EfCorePriceOfferCustomerRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<PriceOfferCustomer>> GetListAsync(
        PriceOfferCustomerFilterParams filterParams,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter(await GetQueryableAsync(), filterParams);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? PriceOfferCustomerConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListNoLockAsync(dbContext, cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        PriceOfferCustomerFilterParams filterParams,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter(await GetDbSetAsync(), filterParams);
        return await query.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<PriceOfferCustomer> ApplyFilter(
        IQueryable<PriceOfferCustomer> query,
        PriceOfferCustomerFilterParams filterParams)
    {
        var filterText = filterParams.FilterText;
        var priceOfferId = filterParams.PriceOfferId;
        var saleChannel = filterParams.SaleChannel;
        var customerId = filterParams.CustomerId;
        var customerTaxCode = filterParams.CustomerTaxCode;
        var customerName = filterParams.CustomerName;
        var customerAddress = filterParams.CustomerAddress;
        var customerNationality = filterParams.CustomerNationality;
        var customerType = filterParams.CustomerType;
        var note = filterParams.Note;

        return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.SaleChannel!.Contains(filterText!) || e.CustomerTaxCode!.Contains(filterText!) || e.CustomerName!.Contains(filterText!) || e.CustomerAddress!.Contains(filterText!) || e.CustomerNationality!.Contains(filterText!) || e.CustomerType!.Contains(filterText!) || e.Note!.Contains(filterText!))
                .WhereIf(priceOfferId.HasValue, e => e.PriceOfferId == priceOfferId)
                .WhereIf(!string.IsNullOrWhiteSpace(saleChannel), e => !string.IsNullOrEmpty(e.SaleChannel) && e.SaleChannel.Contains(saleChannel!))
                .WhereIf(customerId.HasValue, e => e.CustomerId == customerId)
                .WhereIf(!string.IsNullOrWhiteSpace(customerTaxCode), e => !string.IsNullOrEmpty(e.CustomerTaxCode) && e.CustomerTaxCode.Contains(customerTaxCode!))
                .WhereIf(!string.IsNullOrWhiteSpace(customerName), e => !string.IsNullOrEmpty(e.CustomerName) && e.CustomerName.Contains(customerName!))
                .WhereIf(!string.IsNullOrWhiteSpace(customerAddress), e => !string.IsNullOrEmpty(e.CustomerAddress) && e.CustomerAddress.Contains(customerAddress!))
                .WhereIf(!string.IsNullOrWhiteSpace(customerNationality), e => !string.IsNullOrEmpty(e.CustomerNationality) && e.CustomerNationality.Contains(customerNationality!))
                .WhereIf(!string.IsNullOrWhiteSpace(customerType), e => !string.IsNullOrEmpty(e.CustomerType) && e.CustomerType.Contains(customerType!))
                .WhereIf(!string.IsNullOrWhiteSpace(note), e => !string.IsNullOrEmpty(e.Note) && e.Note.Contains(note!));
    }
}