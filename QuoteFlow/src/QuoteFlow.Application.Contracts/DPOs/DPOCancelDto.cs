using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.DPOs;

public class DPOCancelDto
{
    [Required]
    public List<Guid> DPODetailIds { get; set; } = new();

    [Required]
    public string ConcurrencyStamp { get; set; } = null!;

    [Required]
    public string Note { get; set; } = null!;
}