using Volo.Abp.Application.Dtos;

namespace QuoteFlow.Materials.MaterialStocks.MaterialStockLockShipments;

public class GetMaterialStockLockShipmentsInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public string? GolfaCode { get; set; }
    public int? LockOnOrderMin { get; set; }
    public int? LockOnOrderMax { get; set; }
    public int? StockOnOrderMin { get; set; }
    public int? StockOnOrderMax { get; set; }
    public string? Note { get; set; }

    public GetMaterialStockLockShipmentsInput()
    {

    }
}