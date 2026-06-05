using Asp.Versioning;
using QuoteFlow.Dashboards;
using QuoteFlow.DPOs;
using QuoteFlow.GICs;
using QuoteFlow.GKRs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace QuoteFlow.Controllers.Dashboards;

[RemoteService]
[Area("app")]
[ControllerName("Dashboard")]
[Route("api/app/dashboard")]
public class DashboardController : AbpController, IDashboardAppService
{
    protected IDashboardAppService _dashboardAppService;

    public DashboardController(IDashboardAppService dashboardAppService)
    {
        _dashboardAppService = dashboardAppService;
    }

    [HttpGet]
    [Route("dpo-status-summary")]
    public virtual Task<DPOStatusSummaryDto> GetDPOStatusSummaryAsync(GetDPOsInput input)
    {
        return _dashboardAppService.GetDPOStatusSummaryAsync(input);
    }

    [HttpGet]
    [Route("gic-status-summary")]
    public virtual Task<GICStatusSummaryDto> GetGICStatusSummaryAsync(GetGICsInput input)
    {
        return _dashboardAppService.GetGICStatusSummaryAsync(input);
    }

    [HttpGet]
    [Route("gkr-status-summary")]
    public virtual Task<GKRStatusSummaryDto> GetGKRStatusSummaryAsync(GetGKRsInput input)
    {
        return _dashboardAppService.GetGKRStatusSummaryAsync(input);
    }

    [HttpGet]
    [Route("sale-result-by-buyer")]
    public Task<List<SaleResultBuyerDto>> SaleResultByBuyerAsync(int fy)
    {
        return _dashboardAppService.SaleResultByBuyerAsync(fy);
    }
    [HttpGet]
    [Route("sale-result-by-material-group")]
    public Task<List<SaleResultMaterialGroupDto>> SaleResultByMaterialGroupAsync(int fy)
    {
        return _dashboardAppService.SaleResultByMaterialGroupAsync(fy);
    }
    [HttpGet]
    [Route("po-result")]
    public Task<List<SaleResultPODto>> POResultAsync(int fy)
    {
        return _dashboardAppService.POResultAsync(fy);
    }

    [HttpGet]
    [Route("sale-result-base")]
    public Task<List<SaleResultBaseDto>> SaleResultBaseAsync(int fy)
    {
        return _dashboardAppService.SaleResultBaseAsync(fy);
    }

    [HttpGet]
    [Route("approval-dashboard")]
    public Task<List<ApprovalDashboardItemDto>> GetApprovalDashboardAsync()
    {
        return _dashboardAppService.GetApprovalDashboardAsync();
    }
}