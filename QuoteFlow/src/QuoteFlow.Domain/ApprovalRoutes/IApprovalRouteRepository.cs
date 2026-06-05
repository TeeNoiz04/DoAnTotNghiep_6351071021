using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.ApprovalRoutes;

public interface IApprovalRouteRepository : IRepository<ApprovalRoute, Guid>
{
    Task<List<ApprovalRoute>> GetListAsync(
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
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
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
        CancellationToken cancellationToken = default);
}