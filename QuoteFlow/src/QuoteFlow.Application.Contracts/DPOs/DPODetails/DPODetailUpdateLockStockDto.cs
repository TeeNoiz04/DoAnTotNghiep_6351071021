using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.DPOs.DPODetails;

public class DPODetailUpdateLockStockDto
{
    [Required]
    public Guid StockCategoryId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int OldQty { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int NewQty { get; set; }

    [Required]
    [StringLength(500)]
    public string Note { get; set; } = null!;
}