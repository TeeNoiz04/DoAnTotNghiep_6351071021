using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.DPOs;

public class ImportDPOInput
{
    [Required]
    public string? MaterialType { get; set; }

    [Required]
    public Guid BuyerId { get; set; }

    [Required]
    public Guid BuyerTypeId { get; set; }

    [Required]
    public DateTime ConfirmDate { get; set; }
}
