using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.DPOs;

public class DPOLockStockAutoV2Dto
{
    [Required]
    public Guid StockCategoryId { get; set; }

    [Required]
    public List<Guid> DPODetailIds { get; set; } = new();
}