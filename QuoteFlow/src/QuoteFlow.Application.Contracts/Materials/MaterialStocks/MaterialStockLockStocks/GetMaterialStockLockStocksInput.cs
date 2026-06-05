using System;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.Materials.MaterialStocks.MaterialStockLockStocks;

public class GetMaterialStockLockStocksInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public string? GolfaCode { get; set; }
    public Guid? DPODetailId { get; set; }
    public Guid? StockCategoryId { get; set; }
    public int? QtyMin { get; set; }
    public int? QtyMax { get; set; }
    public string? Note { get; set; }
    public int? ReleasedLockMin { get; set; }
    public int? ReleasedLockMax { get; set; }

    public GetMaterialStockLockStocksInput()
    {

    }
}