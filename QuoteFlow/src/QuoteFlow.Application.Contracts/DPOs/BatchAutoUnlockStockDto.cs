using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.DPOs;

public class BatchAutoUnlockStockDto
{
    [Required]
    [MinLength(1, ErrorMessage = "At least one DPO detail ID is required")]
    public List<Guid> DpoDetailIds { get; set; } = new();
}
