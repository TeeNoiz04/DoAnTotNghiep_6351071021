using QuoteFlow.EntityFrameworkCore;
using QuoteFlow.Suppliers.ParameterObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.Suppliers;

public class EFCoreSupplierRepository : EfCoreRepository<QuoteFlowDbContext, Supplier, Guid>, ISupplierRepository
{
    public EFCoreSupplierRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
       : base(dbContextProvider)
    {

    }
    public async Task<long> GetCountAsync(SupplierFilterParams filterParams, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetDbSetAsync()), filterParams);
        return await query.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }

    public async Task<List<Supplier>> GetListAsync(SupplierFilterParams filterParams, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetQueryableAsync()), filterParams);
        query = query.OrderBy(string.IsNullOrWhiteSpace(filterParams.Sorting) ? SupplierConsts.GetDefaultSorting(false) : filterParams.Sorting);
        return await query.PageBy(filterParams.SkipCount, filterParams.MaxResultCount).ToListNoLockAsync(dbContext, cancellationToken);
    }
    protected virtual IQueryable<Supplier> ApplyFilter(
        IQueryable<Supplier> query,
        SupplierFilterParams filterParams
     )
    {
        var filterText = filterParams.FilterText;
        var supplierCode = filterParams.SupplierCode;
        var shortName = filterParams.ShortName;
        var fullName = filterParams.FullName;
        var taxCode = filterParams.TaxCode;
        var address = filterParams.Address;
        var isDeactive = filterParams.IsDeactive;

        return query
            .WhereIf(!string.IsNullOrWhiteSpace(filterText),
                e => (e.SupplierCode ?? "").Contains(filterText!) || (e.ShortName ?? "").Contains(filterText!) || (e.FullName ?? "").Contains(filterText!) || (e.TaxCode ?? "").Contains(filterText!) || (e.Address ?? "").Contains(filterText!))
            .WhereIf(!string.IsNullOrWhiteSpace(supplierCode), e => e.SupplierCode == supplierCode)
            .WhereIf(!string.IsNullOrWhiteSpace(shortName), e => e.ShortName == shortName)
            .WhereIf(!string.IsNullOrWhiteSpace(fullName), e => e.FullName == fullName)
            .WhereIf(!string.IsNullOrWhiteSpace(taxCode), e => e.TaxCode == taxCode)
            .WhereIf(!string.IsNullOrWhiteSpace(address), e => e.Address == address)
            .WhereIf(isDeactive.HasValue, e => e.IsDeactive == isDeactive);
    }

}
