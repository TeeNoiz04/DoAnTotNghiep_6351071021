using QuoteFlow.Materials.MaterialApprovalRequestDetails.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.Materials.MaterialApprovalRequestDetails;

public interface IMaterialApprovalRequestDetailRepository : IRepository<MaterialApprovalRequestDetail, Guid>
{
    Task<List<MaterialApprovalRequestDetail>> GetListAsync(
        Guid approvalId,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        Guid approvalId,
        CancellationToken cancellationToken = default);

    Task<List<MaterialApprovalRequestDetail>> GetListByApprovalIdAsync(
     Guid approvalId,
     CancellationToken cancellationToken = default);

    Task<List<T>> GetListAsync<T>(
        MaterialDetailFilterParams filterParams,
        Expression<Func<MaterialApprovalRequestDetail, T>> selector,
        CancellationToken cancellationToken = default);

    Task<List<MaterialApprovalRequestDetail>> GetListAsync(
        MaterialDetailFilterParams filterParams,
        CancellationToken cancellationToken = default
    );

    Task ActionAsync(
        Guid materialApprovalId,
        string action,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Bulk insert approval request details using SqlBulkCopy for optimal performance.
    /// </summary>
    Task BulkInsertAsync(List<MaterialApprovalRequestDetail> details, CancellationToken cancellationToken = default);
    Task BulkInsertM3UAsync(List<MaterialApprovalRequestDetail> details, CancellationToken cancellationToken = default);
}