using QuoteFlow.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.CustomerPICs;

public class EfCoreCustomerPICRepository : EfCoreRepository<QuoteFlowDbContext, CustomerPIC, Guid>, ICustomerPICRepository
{
    public EfCoreCustomerPICRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<CustomerPIC>> GetListAsync(
        string? filterText = null,
        Guid? keyAccountId = null,
        string? pICName = null,
        string? pIC_Phone = null,
        string? pIC_Email = null,
        string? pIC_JobTitle = null,
        string? remark = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetQueryableAsync()), filterText, keyAccountId, pICName, pIC_Phone, pIC_Email, pIC_JobTitle, remark);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? CustomerPICConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListNoLockAsync(dbContext, cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        string? filterText = null,
        Guid? keyAccountId = null,
        string? pICName = null,
        string? pIC_Phone = null,
        string? pIC_Email = null,
        string? pIC_JobTitle = null,
        string? remark = null,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetDbSetAsync()), filterText, keyAccountId, pICName, pIC_Phone, pIC_Email, pIC_JobTitle, remark);
        return await query.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<CustomerPIC> ApplyFilter(
        IQueryable<CustomerPIC> query,
        string? filterText = null,
        Guid? keyAccountId = null,
        string? pICName = null,
        string? pIC_Phone = null,
        string? pIC_Email = null,
        string? pIC_JobTitle = null,
        string? remark = null)
    {
        return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.PICName!.Contains(filterText!) || e.PIC_Phone!.Contains(filterText!) || e.PIC_Email!.Contains(filterText!) || e.PIC_JobTitle!.Contains(filterText!) || e.Remark!.Contains(filterText!))
                .WhereIf(keyAccountId.HasValue, e => e.KeyAccountId == keyAccountId)
                .WhereIf(!string.IsNullOrWhiteSpace(pICName), e => e.PICName.Contains(pICName))
                .WhereIf(!string.IsNullOrWhiteSpace(pIC_Phone), e => e.PIC_Phone.Contains(pIC_Phone))
                .WhereIf(!string.IsNullOrWhiteSpace(pIC_Email), e => e.PIC_Email.Contains(pIC_Email))
                .WhereIf(!string.IsNullOrWhiteSpace(pIC_JobTitle), e => e.PIC_JobTitle.Contains(pIC_JobTitle))
                .WhereIf(!string.IsNullOrWhiteSpace(remark), e => e.Remark.Contains(remark));
    }
}