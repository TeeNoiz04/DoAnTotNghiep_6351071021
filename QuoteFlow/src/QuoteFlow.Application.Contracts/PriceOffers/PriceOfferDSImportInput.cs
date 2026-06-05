using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.PriceOffers;
public class PriceOfferDSImportInput
{

    [Required]
    public Guid BuyerId { get; set; }

    [Required]
    public Guid BuyerTypeId { get; set; }

    [Required]
    public Guid LocationId { get; set; }

    public string? SalePIC { get; set; }
    public string? MaterialType { get; set; }

    public string? ProjectName { get; set; }

    [StringLength(QuoteFlowSharedConsts.NoteMaxLength)]
    public string? Note { get; set; }
}
