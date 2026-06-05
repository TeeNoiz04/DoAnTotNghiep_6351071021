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

namespace QuoteFlow.ApprovalRoutes;

public class EfCoreApprovalRouteRepository : EfCoreRepository<QuoteFlowDbContext, ApprovalRoute, Guid>, IApprovalRouteRepository
{
    public EfCoreApprovalRouteRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<ApprovalRoute>> GetListAsync(
        string? filterText = null,

        string? entityType = null,
        Guid? instanceId = null,
        int? stepSequenceMin = null,
        int? stepSequenceMax = null,
        string? approver = null,
        string? approverRoleCode = null,
        string? approverRoleName = null,
        DateTime? approvalDateMin = null,
        DateTime? approvalDateMax = null,
        string? notes = null,
        bool? isApproved = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetQueryableAsync()), filterText, entityType, instanceId, stepSequenceMin, stepSequenceMax, approver, approverRoleCode, approverRoleName, approvalDateMin, approvalDateMax, notes, isApproved);
        query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ApprovalRouteConsts.GetDefaultSorting(false) : sorting);
        return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(
        string? filterText = null,

        string? entityType = null,
        Guid? instanceId = null,
        int? stepSequenceMin = null,
        int? stepSequenceMax = null,
        string? approver = null,
        string? approverRoleCode = null,
        string? approverRoleName = null,
        DateTime? approvalDateMin = null,
        DateTime? approvalDateMax = null,
        string? notes = null,
        bool? isApproved = null,
        CancellationToken cancellationToken = default)
    {
        var query = ApplyFilter((await GetDbSetAsync()), filterText, entityType, instanceId, stepSequenceMin, stepSequenceMax, approver, approverRoleCode, approverRoleName, approvalDateMin, approvalDateMax, notes, isApproved);
        return await query.LongCountAsync(GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<ApprovalRoute> ApplyFilter(
        IQueryable<ApprovalRoute> query,
        string? filterText = null,

        string? entityType = null,
        Guid? instanceId = null,
        int? stepSequenceMin = null,
        int? stepSequenceMax = null,
        string? approver = null,
        string? approverRoleCode = null,
        string? approverRoleName = null,
        DateTime? approvalDateMin = null,
        DateTime? approvalDateMax = null,
        string? notes = null,
        bool? isApproved = null)
    {
        return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.EntityType!.Contains(filterText!) || e.Approver!.Contains(filterText!) || e.ApproverRoleCode!.Contains(filterText!) || e.ApproverRoleName!.Contains(filterText!) || e.Notes!.Contains(filterText!))

                .WhereIf(!string.IsNullOrWhiteSpace(entityType), e => e.EntityType.Contains(entityType))
                .WhereIf(instanceId.HasValue, e => e.InstanceId == instanceId)
                .WhereIf(stepSequenceMin.HasValue, e => e.StepSequence >= stepSequenceMin!.Value)
                .WhereIf(stepSequenceMax.HasValue, e => e.StepSequence <= stepSequenceMax!.Value)
                .WhereIf(!string.IsNullOrWhiteSpace(approver), e => e.Approver.Contains(approver))
                .WhereIf(!string.IsNullOrWhiteSpace(approverRoleCode), e => e.ApproverRoleCode.Contains(approverRoleCode))
                .WhereIf(!string.IsNullOrWhiteSpace(approverRoleName), e => e.ApproverRoleName.Contains(approverRoleName))
                .WhereIf(approvalDateMin.HasValue, e => e.ApprovalDate >= approvalDateMin!.Value)
                .WhereIf(approvalDateMax.HasValue, e => e.ApprovalDate <= approvalDateMax!.Value)
                .WhereIf(!string.IsNullOrWhiteSpace(notes), e => e.Notes.Contains(notes))
                .WhereIf(isApproved.HasValue, e => e.IsApproved == isApproved);
    }
}