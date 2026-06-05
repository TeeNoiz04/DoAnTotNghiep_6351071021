using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.ApprovalHistories;

public interface IApprovalHistoryRepository : IRepository<ApprovalHistory, Guid>
{
    Task<List<ApprovalHistory>> GetListAsync(
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
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
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
        CancellationToken cancellationToken = default);
}