namespace QuoteFlow.SpecialInputPrices;
using System;
using System.ComponentModel.DataAnnotations;

public class SpecialInputPriceImportInput
{
    [Required]
    public string AccountNo { get; set; } = null!;
    [Required]
    public string AccountName { get; set; } = null!;

    public string? ProjectName { get; set; }

    public DateTime? ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public string? Note { get; set; }
}
