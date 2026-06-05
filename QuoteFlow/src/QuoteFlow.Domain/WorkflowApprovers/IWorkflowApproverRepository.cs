using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.WorkflowApprovers;

public interface IWorkflowApproverRepository : IRepository<WorkflowApprover, Guid>
{
    Task<List<WorkflowApprover>> GetListAsync(
        string? filterText = null,
        Guid? wFId = null,
        string? approver = null,
        string? note = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        string? filterText = null,
        Guid? wFId = null,
        string? approver = null,
        string? note = null,
        CancellationToken cancellationToken = default);
}