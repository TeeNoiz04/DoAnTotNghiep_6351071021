using QuoteFlow.DPOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace QuoteFlow.Dashboards;

public interface IDashboardAppService : IApplicationService
{
    Task<DPOStatusSummaryDto> GetDPOStatusSummaryAsync(GetDPOsInput input);


    Task<List<SaleResultMaterialGroupDto>> SaleResultByMaterialGroupAsync(int fy);
    Task<List<SaleResultBuyerDto>> SaleResultByBuyerAsync(int fy);

    Task<List<SaleResultPODto>> POResultAsync(int fy);
    Task<List<SaleResultBaseDto>> SaleResultBaseAsync(int fy);

    Task<List<ApprovalDashboardItemDto>> GetApprovalDashboardAsync();
}