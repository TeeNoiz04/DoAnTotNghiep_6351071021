using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.DPOs;
public class DPOLockShipmentItemUpdateDto
{
    [Required]
    [StringLength(50)]
    public string GolfaCode { get; set; } = null!;

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public int Qty { get; set; }

    [StringLength(4000)]
    public string? Note { get; set; }
}
