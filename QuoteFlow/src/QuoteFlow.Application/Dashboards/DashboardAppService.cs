using QuoteFlow.BuyerAccess;
using QuoteFlow.DPOs;
using QuoteFlow.DPOs.ParameterObjects;
using QuoteFlow.GICs;
using QuoteFlow.GKRs;
using QuoteFlow.Permissions;
using QuoteFlow.Shared.Models;
using QuoteFlow.WorkflowConfigurations;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;

namespace QuoteFlow.Dashboards;

[RemoteService(IsEnabled = false)]
public class DashboardAppService : QuoteFlowAppService, IDashboardAppService
{
    protected IDPORepository _dpoRepository;
    protected IWorkflowConfigurationRepository _workflowConfigurationRepository;
    protected IBuyerAccessService _buyerAccessService;
    protected IGICPermissionService _gicPermissionService;

    public DashboardAppService(IDPORepository dpoRepository, IWorkflowConfigurationRepository workflowConfigurationRepository, IBuyerAccessService buyerAccessService, IGICPermissionService gicPermissionService)
    {
        _dpoRepository = dpoRepository;
        _workflowConfigurationRepository = workflowConfigurationRepository;
        _buyerAccessService = buyerAccessService;
        _gicPermissionService = gicPermissionService;
    }

    public virtual async Task<DPOStatusSummaryDto> GetDPOStatusSummaryAsync(GetDPOsInput input)
    {
        var summary = new DPOStatusSummaryDto();
        var filterParams = ObjectMapper.Map<GetDPOsInput, DPOFilterParams>(input);
        var buyerAccess = await _buyerAccessService.GetBuyerAccessAsync();
        filterParams.ApplyBuyerRestrictions(buyerAccess);
        filterParams.ApplyMaterialTypeRestrictions(buyerAccess);
        var statusCounts = await _dpoRepository.GetGroupedCountAsync(filterParams);

        var submittedCount = statusCounts.FirstOrDefault(x => x.Status == QuoteFlowStatuses.DPO.Submitted)?.Count ?? 0;
        var confirmedCount = statusCounts.FirstOrDefault(x => x.Status == QuoteFlowStatuses.DPO.Confirmed)?.Count ?? 0;
        var lockedStockCount = statusCounts.FirstOrDefault(x => x.Status == QuoteFlowStatuses.DPO.LockedStock)?.Count ?? 0;
        var cancelledCount = statusCounts.FirstOrDefault(x => x.Status == QuoteFlowStatuses.Cancelled)?.Count ?? 0;
        var closedCount = statusCounts.FirstOrDefault(x => x.Status == QuoteFlowStatuses.Closed)?.Count ?? 0;
        var inProgressCount = statusCounts.FirstOrDefault(x => x.Status == QuoteFlowStatuses.InProgress)?.Count ?? 0;

        summary.Submitted = submittedCount;
        summary.Confirmed = confirmedCount;
        summary.LockedStock = lockedStockCount;
        summary.Cancelled = cancelledCount;
        summary.Closed = closedCount;
        summary.InProgress = inProgressCount;

        return summary;
    }

    public virtual async Task<GICStatusSummaryDto> GetGICStatusSummaryAsync(GetGICsInput input)
    {
        var summary = new GICStatusSummaryDto();
        var filterParams = ObjectMapper.Map<GetGICsInput, GICFilterParams>(input);
        filterParams.RestrictedGICTypes = await _gicPermissionService.GetRestrictedGICTypes();

        var statusCounts = await _dpoRepository.GetGICGroupedCountAsync(filterParams);

        var confirmingCount = statusCounts.FirstOrDefault(x => x.Status == QuoteFlowStatuses.GIC.Confirmed)?.Count ?? 0;
        var lockStockCount = statusCounts.FirstOrDefault(x => x.Status == QuoteFlowStatuses.GIC.LockedStock)?.Count ?? 0;
        var inProgressCount = statusCounts.FirstOrDefault(x => x.Status == QuoteFlowStatuses.InProgress)?.Count ?? 0;
        var closedCount = statusCounts.FirstOrDefault(x => x.Status == QuoteFlowStatuses.Closed)?.Count ?? 0;
        var cancelledCount = statusCounts.FirstOrDefault(x => x.Status == QuoteFlowStatuses.Cancelled)?.Count ?? 0;

        summary.Confirmed = confirmingCount;
        summary.LockedStock = lockStockCount;
        summary.InProgress = inProgressCount;
        summary.Closed = closedCount;
        summary.Cancelled = cancelledCount;

        return summary;
    }


    public virtual async Task<GKRStatusSummaryDto> GetGKRStatusSummaryAsync(GetGKRsInput input)
    {
        var summary = new GKRStatusSummaryDto();
        var filterParams = ObjectMapper.Map<GetGKRsInput, GKRFilterParams>(input);
        var buyerAccess = await _buyerAccessService.GetBuyerAccessAsync();
        //filterParams.ApplyBuyerRestrictions(buyerAccess);
        //filterParams.ApplyMaterialTypeRestrictions(buyerAccess);
        var statusCounts = await _dpoRepository.GetGKRGroupedCountAsync(filterParams);

        var submittedCount = statusCounts.FirstOrDefault(x => x.Status == QuoteFlowStatuses.DPO.Submitted)?.Count ?? 0;
        var confirmedCount = statusCounts.FirstOrDefault(x => x.Status == QuoteFlowStatuses.DPO.Confirmed)?.Count ?? 0;
        var lockedStockCount = statusCounts.FirstOrDefault(x => x.Status == QuoteFlowStatuses.DPO.LockedStock)?.Count ?? 0;
        var cancelledCount = statusCounts.FirstOrDefault(x => x.Status == QuoteFlowStatuses.Cancelled)?.Count ?? 0;
        var closedCount = statusCounts.FirstOrDefault(x => x.Status == QuoteFlowStatuses.Closed)?.Count ?? 0;
        var inProgressCount = statusCounts.FirstOrDefault(x => x.Status == QuoteFlowStatuses.InProgress)?.Count ?? 0;

        summary.Submitted = submittedCount;
        summary.Confirmed = confirmedCount;
        summary.LockedStock = lockedStockCount;
        summary.Cancelled = cancelledCount;
        summary.Closed = closedCount;
        summary.InProgress = inProgressCount;

        return summary;
    }

    public async Task<List<SaleResultBuyerDto>> SaleResultByBuyerAsync(int fy)
    {
        var items = await _workflowConfigurationRepository.SaleResultByBuyer(fy);
        return ObjectMapper.Map<List<DashboardSaleResult>, List<SaleResultBuyerDto>>(items);
    }

    public async Task<List<SaleResultMaterialGroupDto>> SaleResultByMaterialGroupAsync(int fy)
    {
        var items = await _workflowConfigurationRepository.SaleResultByMaterialGroup(fy);
        return ObjectMapper.Map<List<DashboardSaleResult>, List<SaleResultMaterialGroupDto>>(items);
    }

    public async Task<List<SaleResultPODto>> POResultAsync(int fy)
    {
        var items = await _workflowConfigurationRepository.POResultByBuyer(fy);
        return ObjectMapper.Map<List<DashboardSaleResult>, List<SaleResultPODto>>(items);
    }
    public async Task<List<SaleResultBaseDto>> SaleResultBaseAsync(int fy)
    {
        var items = await _workflowConfigurationRepository.SaleResultBase(fy);
        return ObjectMapper.Map<List<DashboardSaleResult>, List<SaleResultBaseDto>>(items);
    }

    public virtual async Task<List<ApprovalDashboardItemDto>> GetApprovalDashboardAsync()
    {
        var username = CurrentUser?.UserName ?? string.Empty;
        var hasPermissionConfirmedDPO = await AuthorizationService.IsGrantedAsync(QuoteFlowPermissions.MovingOrders.DPOs.ConfirmReject);

        var items = await _workflowConfigurationRepository.GetApprovalDashboard(username, hasPermissionConfirmedDPO);
        return ObjectMapper.Map<List<DashboardApprovalItem>, List<ApprovalDashboardItemDto>>(items);
    }

}
