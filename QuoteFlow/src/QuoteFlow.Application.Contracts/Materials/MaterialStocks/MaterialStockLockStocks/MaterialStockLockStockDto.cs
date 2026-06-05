using QuoteFlow.Shared;
using QuoteFlow.StockCategories;
using System;

namespace QuoteFlow.Materials.MaterialStocks.MaterialStockLockStocks;

public class MaterialStockLockStockDto : ExtendedFullAuditedEntityDto<Guid>
{
    public string GolfaCode { get; set; } = null!;
    public Guid? DPODetailId { get; set; }
    public Guid? StockCategoryId { get; set; }
    public int Qty { get; set; }
    public string? Note { get; set; }
    public int ReleasedLock { get; set; }

    public StockCategoryDto? StockCategory { get; set; }
}