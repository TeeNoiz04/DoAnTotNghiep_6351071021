using QuoteFlow.EntityFrameworkCore;
using QuoteFlow.SaleOrdersSapImports.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.SaleOrdersSapImports;

public class EfCoreSaleOrdersSapImportRepository : EfCoreRepository<QuoteFlowDbContext, SaleOrdersSapImport, Guid>, ISaleOrdersSapImportRepository
{
    public EfCoreSaleOrdersSapImportRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<SaleOrdersSapImport>> GetListAsync(
        SaleOrderSapImportFilterParams filterParams,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetQueryableAsync()), filterParams);
        query = query.OrderBy(string.IsNullOrWhiteSpace(filterParams.Sorting) ? SaleOrdersSapImportConsts.GetDefaultSorting(false) : filterParams.Sorting);
        return await query.PageBy(filterParams.SkipCount, filterParams.MaxResultCount).ToListNoLockAsync(dbContext, cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        SaleOrderSapImportFilterParams filterParams,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetDbSetAsync()), filterParams);
        return await query.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<SaleOrdersSapImport> ApplyFilter(
    IQueryable<SaleOrdersSapImport> query,
    SaleOrderSapImportFilterParams filterParams
    )
    {
        var filterText = filterParams.FilterText;
        var sONo = filterParams.SONo;
        //var dONote = filterParams.DONote;
        var sOSAPNo = filterParams.SOSAPNo;
        var dOSAPNo = filterParams.DOSAPNo;
        var billingNo = filterParams.BillingNo;
        var invoiceNo = filterParams.InvoiceNo;
        var invoiceDateMin = filterParams.InvoiceDateMin;
        var invoiceDateMax = filterParams.InvoiceDateMax;
        var note = filterParams.Note;
        var fileName = filterParams.FileName;
        var importKey = filterParams.ImportKey;
        var isDeleted = filterParams.IsDeleted;

        return query
            .WhereIf(!string.IsNullOrWhiteSpace(filterText), e =>
                (e.SONo ?? "").Contains(filterText!) ||
                //(e.DONo ?? "").Contains(filterText!) ||
                //(e.DONote ?? "").Contains(filterText!) ||
                (e.SOSAPNo ?? "").Contains(filterText!) ||
                (e.DOSAPNo ?? "").Contains(filterText!) ||
                (e.BillingNo ?? "").Contains(filterText!) ||
                (e.InvoiceNo ?? "").Contains(filterText!) ||
                (e.Note ?? "").Contains(filterText!) ||
                (e.FileName ?? "").Contains(filterText!)
            )
            .WhereIf(!string.IsNullOrWhiteSpace(sONo), e => e.SONo.Contains(sONo))
            //.WhereIf(!string.IsNullOrWhiteSpace(dONote), e => e.DONote.Contains(dONote))
            .WhereIf(!string.IsNullOrWhiteSpace(sOSAPNo), e => e.SOSAPNo.Contains(sOSAPNo))
            .WhereIf(!string.IsNullOrWhiteSpace(dOSAPNo), e => e.DOSAPNo.Contains(dOSAPNo))
            .WhereIf(!string.IsNullOrWhiteSpace(billingNo), e => e.BillingNo.Contains(billingNo))
            .WhereIf(!string.IsNullOrWhiteSpace(invoiceNo), e => e.InvoiceNo.Contains(invoiceNo))
            .WhereIf(invoiceDateMin.HasValue, e => e.InvoiceDate >= invoiceDateMin.Value)
            .WhereIf(invoiceDateMax.HasValue, e => e.InvoiceDate <= invoiceDateMax.Value)
            .WhereIf(!string.IsNullOrWhiteSpace(note), e => e.Note.Contains(note))
            .WhereIf(!string.IsNullOrWhiteSpace(fileName), e => e.FileName.Contains(fileName))
            //.WhereIf(!string.IsNullOrWhiteSpace(importKey), e => e.ImportKey == importKey)
            .WhereIf(isDeleted.HasValue, e => e.IsDeleted == isDeleted.Value);
    }

}
