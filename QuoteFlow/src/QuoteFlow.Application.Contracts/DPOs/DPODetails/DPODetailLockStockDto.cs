using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.DPOs.DPODetails;

public class DPODetailLockStockDto
{
    [Required]
    public string GolfaCode { get; set; } = null!;

    [Required]
    public Guid StockCategoryId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int LockQty { get; set; }

    [StringLength(500)]
    public string? Note { get; set; }
}