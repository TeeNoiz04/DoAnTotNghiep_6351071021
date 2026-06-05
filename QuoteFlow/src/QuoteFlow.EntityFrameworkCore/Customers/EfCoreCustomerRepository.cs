using QuoteFlow.Customers.ParameterObjects;
using QuoteFlow.EntityFrameworkCore;
using QuoteFlow.Helper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.Customers;

public class EfCoreCustomerRepository : EfCoreRepository<QuoteFlowDbContext, Customer, Guid>, ICustomerRepository
{
    public EfCoreCustomerRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<Customer>> GetListAsync(
        CustomerFilterParams filterParams,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetQueryableAsync()), filterParams);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? CustomerConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListNoLockAsync(dbContext, cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        CustomerFilterParams filterParams,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetDbSetAsync()), filterParams);
        return await query.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<Customer> ApplyFilter(
    IQueryable<Customer> query,
    CustomerFilterParams filterParams)
    {
        var filterText = filterParams.FilterText;
        var taxCode = filterParams.TaxCode;
        var customerName = filterParams.CustomerName;
        var customerShortName = filterParams.CustomerShortName;
        var address = filterParams.Address;
        var phone = filterParams.Phone;
        var province = filterParams.Province;
        var website = filterParams.Website;
        var deactive = filterParams.IsDeactive;
        var customerType = filterParams.CustomerType;
        var customerIndustry = filterParams.CustomerIndustry;
        var fromDate = filterParams.FromDate;
        var toDate = filterParams.ToDate;

        return query
            .WhereIf(!string.IsNullOrWhiteSpace(filterText),
                e => e.TaxCode!.Contains(filterText!) ||
                     e.CustomerName!.Contains(filterText!) ||
                     e.CustomerShortName!.Contains(filterText!) ||
                     e.Address!.Contains(filterText!) ||
                     e.Phone!.Contains(filterText!) ||
                     e.Note!.Contains(filterText!) ||
                     e.Province!.Contains(filterText!) ||
                     e.Website!.Contains(filterText!))

            .WhereIf(!string.IsNullOrWhiteSpace(taxCode),
                QueryFilterHelper.BuildMultiFieldSearch<Customer>(taxCode, e => e.TaxCode))
            .WhereIf(!string.IsNullOrWhiteSpace(customerName),
                QueryFilterHelper.BuildMultiFieldSearch<Customer>(customerName, e => e.CustomerName))
            .WhereIf(!string.IsNullOrWhiteSpace(customerShortName),
                QueryFilterHelper.BuildMultiFieldSearch<Customer>(customerShortName, e => e.CustomerShortName))
            .WhereIf(!string.IsNullOrWhiteSpace(address),
                QueryFilterHelper.BuildMultiFieldSearch<Customer>(address, e => e.Address))
            .WhereIf(!string.IsNullOrWhiteSpace(phone),
                QueryFilterHelper.BuildMultiFieldSearch<Customer>(phone, e => e.Phone))
            .WhereIf(!string.IsNullOrWhiteSpace(province),
                QueryFilterHelper.BuildMultiFieldSearch<Customer>(province, e => e.Province))
            .WhereIf(!string.IsNullOrWhiteSpace(website),
                QueryFilterHelper.BuildMultiFieldSearch<Customer>(website, e => e.Website))

            .WhereIf(!string.IsNullOrWhiteSpace(customerIndustry), e => e.CustomerIndustry == customerIndustry)
            .WhereIf(!string.IsNullOrWhiteSpace(customerType), e => e.CustomerType == customerType)
            .WhereIf(fromDate.HasValue, e => e.CreationTime >= fromDate)
            .WhereIf(toDate.HasValue, e => e.CreationTime <= toDate)
            .Where(e => e.IsDeactive == deactive);
    }   
}