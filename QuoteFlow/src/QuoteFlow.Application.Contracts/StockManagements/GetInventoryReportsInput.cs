using Volo.Abp.Application.Dtos;

namespace QuoteFlow.StockManagements;

public class GetInventoryReportsInput : PagedAndSortedResultRequestDto
{
    public string? MaterialCode { get; set; }
    public string? InventoryCategory { get; set; }
    public string? MaterialGroup { get; set; }

}
