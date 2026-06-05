using QuoteFlow.Materials.MaterialApprovalRequests.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.Materials.MaterialApprovalRequests;

public interface IMaterialApprovalRequestRepository : IRepository<MaterialApprovalRequest, Guid>
{
    Task<List<MaterialApprovalRequest>> GetListAsync(
        MaterialApprovalRequestFilterParams filterParams,
        string? currenUser = null,
        CancellationToken cancellationToken = default
    );

    Task<List<MaterialApprovalRequest>> GetListPendingAsync(
        MaterialApprovalRequestFilterParams filterParams,
        string approverUsername,
        CancellationToken cancellationToken = default);

    Task<MaterialApprovalRequest> GetWithDetailAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    Task<MaterialApprovalRequest> GetWithDetailNoTrackingAsync(
       Guid id,
       CancellationToken cancellationToken = default
   );

    Task CreateApprovalRoute(
    MaterialApprovalWFRouteFilterParams filterParams,
    CancellationToken cancellationToken = default

    );

    Task<long> GetCountAsync(
       MaterialApprovalRequestFilterParams filterParams,
        string? currenUser = null,
        CancellationToken cancellationToken = default);

    Task<string?> GetLastestRequestNoAsync(string prefix);

    Task<List<MaterialApprovalRequestRoute>> GetListApprovalRoutesAsync(
        Guid? materialApprovalId
    );

    Task<List<T>> GetListAsync<T>(
        MaterialApprovalRequestFilterParams filterParams,
        Expression<Func<MaterialApprovalRequest, T>> selector,
        CancellationToken cancellationToken = default);
}