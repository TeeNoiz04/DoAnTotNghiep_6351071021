using QuoteFlow.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.WorkflowApprovers;

public class EfCoreWorkflowApproverRepository : EfCoreRepository<QuoteFlowDbContext, WorkflowApprover, Guid>, IWorkflowApproverRepository
{
    public EfCoreWorkflowApproverRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<WorkflowApprover>> GetListAsync(
        string? filterText = null,
        Guid? wFId = null,
        string? approver = null,
        string? note = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetQueryableAsync()), filterText, wFId, approver, note);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? WorkflowApproverConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListNoLockAsync(dbContext, cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        string? filterText = null,
        Guid? wFId = null,
        string? approver = null,
        string? note = null,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetDbSetAsync()), filterText, wFId, approver, note);
        return await query.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<WorkflowApprover> ApplyFilter(
        IQueryable<WorkflowApprover> query,
        string? filterText = null,
        Guid? wFId = null,
        string? approver = null,
        string? note = null)
    {
        return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Approver!.Contains(filterText!) || e.Note!.Contains(filterText!))
                .WhereIf(wFId.HasValue, e => e.WFId == wFId)
                .WhereIf(!string.IsNullOrWhiteSpace(approver), e => e.Approver.Contains(approver))
                .WhereIf(!string.IsNullOrWhiteSpace(note), e => e.Note.Contains(note));
    }
}