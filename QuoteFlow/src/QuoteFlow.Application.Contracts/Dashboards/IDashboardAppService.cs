using QuoteFlow.DPOs;
using QuoteFlow.GICs;
using QuoteFlow.GKRs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace QuoteFlow.Dashboards;

public interface IDashboardAppService : IApplicationService
{
    Task<DPOStatusSummaryDto> GetDPOStatusSummaryAsync(GetDPOsInput input);
    Task<GICStatusSummaryDto> GetGICStatusSummaryAsync(GetGICsInput input);
    Task<GKRStatusSummaryDto> GetGKRStatusSummaryAsync(GetGKRsInput input);


    Task<List<SaleResultMaterialGroupDto>> SaleResultByMaterialGroupAsync(int fy);
    Task<List<SaleResultBuyerDto>> SaleResultByBuyerAsync(int fy);

    Task<List<SaleResultPODto>> POResultAsync(int fy);
    Task<List<SaleResultBaseDto>> SaleResultBaseAsync(int fy);

    Task<List<ApprovalDashboardItemDto>> GetApprovalDashboardAsync();
}