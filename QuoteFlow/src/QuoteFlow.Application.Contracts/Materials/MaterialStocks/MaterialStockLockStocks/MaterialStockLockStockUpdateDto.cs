using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.Materials.MaterialStocks.MaterialStockLockStocks;

public class MaterialStockLockStockUpdateDto
{
    [Required]
    [StringLength(MaterialStockLockStockConsts.GolfaCodeMaxLength)]
    public string GolfaCode { get; set; } = null!;
    public Guid? DPODetailId { get; set; }
    public Guid? StockCategoryId { get; set; }
    public int Qty { get; set; }
    [StringLength(MaterialStockLockStockConsts.NoteMaxLength)]
    public string? Note { get; set; }
    public int ReleasedLock { get; set; }

}