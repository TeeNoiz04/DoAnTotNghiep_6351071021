using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.PriceOffers;

public class SubmitProjectResultDto
{
    [Required]
    public string ResultStatus { get; set; } = null!; // "WON", "PRE_ORDER", or "LOST"

    public List<WinningCustomerPerChannelDto>? WinningCustomers { get; set; } // required for "WON" and "PRE_ORDER"

    [Required]
    public string Note { get; set; } = null!;
}

public class WinningCustomerPerChannelDto
{
    [Required]
    public string ChannelId { get; set; } = null!;

    [Required]
    public Guid CustomerId { get; set; }
}
