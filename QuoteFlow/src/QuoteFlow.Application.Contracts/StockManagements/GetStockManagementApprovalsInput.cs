using Volo.Abp.Application.Dtos;

namespace QuoteFlow.StockManagements;
public class GetStockManagementApprovalsInput : PagedAndSortedResultRequestDto
{
    public string? GolfaCode { get; set; }
    public string? Model { get; set; }
    public string? ImportType { get; set; }
    public string? ApprovalStatus { get; set; }
}
