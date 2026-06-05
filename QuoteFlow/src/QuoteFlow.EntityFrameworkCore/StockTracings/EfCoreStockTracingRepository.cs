using QuoteFlow.EntityFrameworkCore;
using QuoteFlow.StockTracings.ParameterObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.StockTracings;

public class EfCoreStockTracingRepository : EfCoreRepository<QuoteFlowDbContext, StockTracing, Guid>, IStockTracingRepository
{
    public EfCoreStockTracingRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<StockTracing> GetWithDetailsAsync(Guid id)
    {
        var dbContext = await GetDbContextAsync();

        var query = dbContext.StockTracings
            .Where(x => x.Id == id)
            .Include(x => x.Details
                .Where(d => d.RowNo > 0)
                .OrderBy(d => d.CreationTime)
                .Take(10000));

        var entity = await query.FirstOrDefaultNoLockAsync(dbContext);

        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(StockTracing), id);
        }

        return entity;
    }

    public virtual async Task<StockTracing> GetNoDetailsAsync(Guid id)
    {
        var dbContext = await GetDbContextAsync();

        var query = dbContext.StockTracings
            .AsNoTracking()
            .Where(x => x.Id == id);

        var entity = await query.FirstOrDefaultNoLockAsync(dbContext);
        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(StockTracing), id);
        }

        return entity;
    }

    public virtual async Task<List<StockTracing>> GetListAsync(
        StockTracingFilterParams filterParams,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetQueryableAsync()), filterParams);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? StockTracingConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListNoLockAsync(dbContext, cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        StockTracingFilterParams filterParams,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetDbSetAsync()), filterParams);
        return await query.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<StockTracing> ApplyFilter(
        IQueryable<StockTracing> query,
        StockTracingFilterParams filterParams)
    {
        var filterText = filterParams.FilterText;
        var fileName = filterParams.FileName;
        var reportType = filterParams.ReportType;
        var fromDateMin = filterParams.FromDateMin;
        var fromDateMax = filterParams.FromDateMax;
        var toDateMin = filterParams.ToDateMin;
        var toDateMax = filterParams.ToDateMax;
        var note = filterParams.Note;

        return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.FileName!.Contains(filterText!) || e.Note!.Contains(filterText!))
                .WhereIf(!string.IsNullOrWhiteSpace(fileName), e => !string.IsNullOrWhiteSpace(e.FileName) && e.FileName.Contains(fileName!))
                .WhereIf(reportType != ReportType.None && reportType.HasValue, e => e.ReportType == reportType)
                .WhereIf(fromDateMin.HasValue, e => e.FromDate!.Value.Date >= fromDateMin!.Value.Date)
                .WhereIf(fromDateMax.HasValue, e => e.FromDate!.Value.Date <= fromDateMax!.Value.Date)
                .WhereIf(toDateMin.HasValue, e => e.ToDate >= toDateMin!.Value)
                .WhereIf(toDateMax.HasValue, e => e.ToDate <= toDateMax!.Value)
                .WhereIf(!string.IsNullOrWhiteSpace(note), e => !string.IsNullOrWhiteSpace(e.Note) && e.Note.Contains(note!));
    }

}