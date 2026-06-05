using QuoteFlow.Dashboards;
using QuoteFlow.WorkflowConfigurations.ParameterObject;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.WorkflowConfigurations;

public interface IWorkflowConfigurationRepository : IRepository<WorkflowConfiguration, Guid>
{
    Task<List<WorkflowConfiguration>> GetListAsync(
        WorkflowFilterParams filter,
        CancellationToken cancellationToken = default
    );

    Task<long> GetCountAsync(
        WorkflowFilterParams filter,
        CancellationToken cancellationToken = default);

    Task<List<DashboardSaleResult>> SaleResultByMaterialGroup(int fy);
    Task<List<DashboardSaleResult>> SaleResultByBuyer(int fy);
    Task<List<DashboardSaleResult>> POResultByBuyer(int fy);

    Task<List<DashboardSaleResult>> SaleResultBase(int fy);

    Task<List<DashboardApprovalItem>> GetApprovalDashboard(string username, bool hasPermissionConfirmedDPO);
}