using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.DPOs;

public class DPOLockStockAutoDto
{
    [Required]
    public Guid DPOId { get; set; }

    [Required]
    public Guid StockCategoryId { get; set; }
}