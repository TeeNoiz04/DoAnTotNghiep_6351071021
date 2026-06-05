using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.Materials.MaterialStocks;

public class GetMaterialStocksInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public Guid? MaterialId { get; set; }
    public Guid? StockCategoryId { get; set; }
    public List<string>? GolfaCodes { get; set; }
    public List<string>? Models { get; set; }
    public int? QtyMin { get; set; }
    public int? QtyMax { get; set; }
    public int? LockedMin { get; set; }
    public int? LockedMax { get; set; }
    public int? LockStockKeepingMin { get; set; }
    public int? LockStockKeepingMax { get; set; }
    public int? LockStockSOMin { get; set; }
    public int? LockStockSOMax { get; set; }
    public int? Available_QtyMin { get; set; }
    public int? Available_QtyMax { get; set; }
    public string? Note { get; set; }
    public string? MaterialType { get; set; }

    public GetMaterialStocksInput()
    {

    }
}