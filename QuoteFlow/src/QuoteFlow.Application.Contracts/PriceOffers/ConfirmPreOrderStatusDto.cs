using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.PriceOffers;

public class ConfirmPreOrderStatusDto
{
    [Required]
    public string ResultStatus { get; set; } = null!; // "WON" or "LOST" only

    [Required]
    public string Note { get; set; } = null!;
}