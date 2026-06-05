using QuoteFlow.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.ApprovalHistories;

public class EfCoreApprovalHistoryRepository : EfCoreRepository<QuoteFlowDbContext, ApprovalHistory, Guid>, IApprovalHistoryRepository
{
    public EfCoreApprovalHistoryRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<ApprovalHistory>> GetListAsync(
        string? filterText = null,

        string? entityType = null,
        string? approverRoleCode = null,
        string? approverRoleName = null,
        string? approverUsername = null,
        string? approverFullName = null,
        string? action = null,
        DateTime? actionDateMin = null,
        DateTime? actionDateMax = null,
        string? note = null,
        bool? isLastApprovalInCurrentWorkflow = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetQueryableAsync()), filterText, entityType, approverRoleCode, approverRoleName, approverUsername, approverFullName, action, actionDateMin, actionDateMax, note, isLastApprovalInCurrentWorkflow);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ApprovalHistoryConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        string? filterText = null,

        string? entityType = null,
        string? approverRoleCode = null,
        string? approverRoleName = null,
        string? approverUsername = null,
        string? approverFullName = null,
        string? action = null,
        DateTime? actionDateMin = null,
        DateTime? actionDateMax = null,
        string? note = null,
        bool? isLastApprovalInCurrentWorkflow = null,
        CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetDbSetAsync()), filterText, entityType, approverRoleCode, approverRoleName, approverUsername, approverFullName, action, actionDateMin, actionDateMax, note, isLastApprovalInCurrentWorkflow);
        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<ApprovalHistory> ApplyFilter(
        IQueryable<ApprovalHistory> query,
        string? filterText = null,

        string? entityType = null,
        string? approverRoleCode = null,
        string? approverRoleName = null,
        string? approverUsername = null,
        string? approverFullName = null,
        string? action = null,
        DateTime? actionDateMin = null,
        DateTime? actionDateMax = null,
        string? note = null,
        bool? isLastApprovalInCurrentWorkflow = null)
    {
        return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.EntityType!.Contains(filterText!) || e.ApproverRoleCode!.Contains(filterText!) || e.ApproverRoleName!.Contains(filterText!) || e.ApproverUsername!.Contains(filterText!) || e.ApproverFullName!.Contains(filterText!) || e.Action!.Contains(filterText!) || e.Note!.Contains(filterText!))

                .WhereIf(!string.IsNullOrWhiteSpace(entityType), e => e.EntityType.Contains(entityType))
                .WhereIf(!string.IsNullOrWhiteSpace(approverRoleCode), e => e.ApproverRoleCode.Contains(approverRoleCode))
                .WhereIf(!string.IsNullOrWhiteSpace(approverRoleName), e => e.ApproverRoleName.Contains(approverRoleName))
                .WhereIf(!string.IsNullOrWhiteSpace(approverUsername), e => e.ApproverUsername.Contains(approverUsername))
                .WhereIf(!string.IsNullOrWhiteSpace(approverFullName), e => e.ApproverFullName.Contains(approverFullName))
                .WhereIf(!string.IsNullOrWhiteSpace(action), e => e.Action.Contains(action))
                .WhereIf(actionDateMin.HasValue, e => e.ActionDate >= actionDateMin!.Value)
                .WhereIf(actionDateMax.HasValue, e => e.ActionDate <= actionDateMax!.Value)
                .WhereIf(!string.IsNullOrWhiteSpace(note), e => e.Note.Contains(note))
                .WhereIf(isLastApprovalInCurrentWorkflow.HasValue, e => e.IsLastApprovalInCurrentWorkflow == isLastApprovalInCurrentWorkflow);
    }
}