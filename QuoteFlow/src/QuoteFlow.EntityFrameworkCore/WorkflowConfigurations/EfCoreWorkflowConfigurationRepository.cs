using Dapper;
using QuoteFlow.Dashboards;
using QuoteFlow.EntityFrameworkCore;
using QuoteFlow.WorkflowApprovers;
using QuoteFlow.WorkflowConfigurations.ParameterObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace QuoteFlow.WorkflowConfigurations;

public class EfCoreWorkflowConfigurationRepository : EfCoreRepository<QuoteFlowDbContext, WorkflowConfiguration, Guid>, IWorkflowConfigurationRepository
{
    public EfCoreWorkflowConfigurationRepository(IDbContextProvider<QuoteFlowDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<List<WorkflowConfiguration>> GetListAsync(
    WorkflowFilterParams filter,
    CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var workflowConfigs = dbContext.Set<WorkflowConfiguration>();
        var workflowApprovers = dbContext.Set<WorkflowApprover>();

        var query = ApplyFilter(
            await GetQueryableAsync(),
            filter
        );

        query = query.OrderBy(
            string.IsNullOrWhiteSpace(filter.Sorting)
                ? WorkflowConfigurationConsts.GetDefaultSorting(false)
                : filter.Sorting
        );

        var result = await (
            from wf in query
            join wa in workflowApprovers on wf.Id equals wa.WFId into wfGroup
            select new WorkflowConfiguration
            {
                Id = wf.Id,
                WorkflowType = wf.WorkflowType,
                WorkflowLevel = wf.WorkflowLevel,
                WorkflowRole = wf.WorkflowRole,
                Condition = wf.Condition,
                Note = wf.Note,
                CreationTime = wf.CreationTime,
                CreatorName = wf.CreatorName,
                LastModificationTime = wf.LastModificationTime,
                LastModifierName = wf.LastModifierName,
                ConcurrencyStamp = wf.ConcurrencyStamp,
                Approvers = wfGroup.Count() > 1
                    ? string.Join(", ", wfGroup.Select(x => x.Approver))
                    : wfGroup.Select(x => x.Approver).FirstOrDefault(),
                // map Approvers (NotMapped property)
                WorkflowApprovers = new List<WorkflowApprover>(wfGroup)
            }
        )
        .PageBy(filter.SkipCount, filter.MaxResultCount)
        .ToListNoLockAsync(dbContext, cancellationToken);

        return result;
    }


    public virtual async Task<long> GetCountAsync(
        WorkflowFilterParams filter,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var query = ApplyFilter((await GetDbSetAsync()), filter);
        return await query.CountNoLockAsync(dbContext, GetCancellationToken(cancellationToken));
    }

    protected virtual IQueryable<WorkflowConfiguration> ApplyFilter(
        IQueryable<WorkflowConfiguration> query,
       WorkflowFilterParams filter)
    {
        var filterText = filter.FilterText;
        var workflowType = filter.WorkflowType;
        var workflowLevel = filter.WorkflowLevel;
        var workflowLevelMin = filter.WorkflowLevelMin;
        var workflowLevelMax = filter.WorkflowLevelMax;
        var workflowRole = filter.WorkflowRole;
        var condition = filter.Condition;
        var note = filter.Note;

        return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.WorkflowType!.Contains(filterText!) || e.WorkflowRole!.Contains(filterText!) || e.Condition!.Contains(filterText!) || e.Note!.Contains(filterText!))
                .WhereIf(!string.IsNullOrWhiteSpace(workflowType), e => e.WorkflowType.Contains(workflowType))
                .WhereIf(workflowLevel.HasValue, e => e.WorkflowLevel == workflowLevel!.Value)
                .WhereIf(workflowLevelMin.HasValue, e => e.WorkflowLevel >= workflowLevelMin!.Value)
                .WhereIf(workflowLevelMax.HasValue, e => e.WorkflowLevel <= workflowLevelMax!.Value)
                .WhereIf(!string.IsNullOrWhiteSpace(workflowRole), e => e.WorkflowRole.Contains(workflowRole))
                .WhereIf(!string.IsNullOrWhiteSpace(condition), e => e.Condition.Contains(condition))
                .WhereIf(!string.IsNullOrWhiteSpace(note), e => e.Note.Contains(note));
    }

    public async Task<List<DashboardSaleResult>> SaleResultByMaterialGroup(int fy)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();
        var parameters = new DynamicParameters();

        parameters.Add("financeYear", fy);


        var result = await connection.QueryAsync<DashboardSaleResult>(
            "usp_Dashboard_SaleResult_ByMaterialGroup",
            parameters,
            commandType: CommandType.StoredProcedure

        );
        return result.ToList();
    }

    public async Task<List<DashboardSaleResult>> SaleResultByBuyer(int fy)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();
        var parameters = new DynamicParameters();

        parameters.Add("financeYear", fy);


        var result = await connection.QueryAsync<DashboardSaleResult>(
            "usp_Dashboard_SaleResult_ByBuyer",
            parameters,
            commandType: CommandType.StoredProcedure

        );
        return result.ToList();
    }

    public async Task<List<DashboardSaleResult>> POResultByBuyer(int fy)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();
        var parameters = new DynamicParameters();

        parameters.Add("financeYear", fy);


        var result = await connection.QueryAsync<DashboardSaleResult>(
            "usp_Dashboard_POResultBase",
            parameters,
            commandType: CommandType.StoredProcedure

        );
        return result.ToList();
    }

    public async Task<List<DashboardSaleResult>> SaleResultBase(int fy)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();
        var parameters = new DynamicParameters();

        parameters.Add("financeYear", fy);


        var result = await connection.QueryAsync<DashboardSaleResult>(
            "usp_Dashboard_SaleResultBase",
            parameters,
            commandType: CommandType.StoredProcedure

        );
        return result.ToList();
    }

    public async Task<List<DashboardApprovalItem>> GetApprovalDashboard(string username, bool hasPermissionConfirmedDPO)
    {
        var dbContext = await GetDbContextAsync();
        var connection = dbContext.Database.GetDbConnection();
        var parameters = new DynamicParameters();

        parameters.Add("username", username);
        parameters.Add("hasPermissionConfirmedDPO", hasPermissionConfirmedDPO);

        var result = await connection.QueryAsync<DashboardApprovalItem>(
            "usp_Dashboard_Approval",
            parameters,
            commandType: CommandType.StoredProcedure
        );
        return result.ToList();
    }

}