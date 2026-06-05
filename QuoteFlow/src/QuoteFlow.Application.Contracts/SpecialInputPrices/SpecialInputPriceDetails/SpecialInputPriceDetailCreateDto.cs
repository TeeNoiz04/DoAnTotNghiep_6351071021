using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.SpecialInputPrices.SpecialInputPriceDetails;

public class SpecialInputPriceDetailCreateDto
{
    public Guid? SpecialInputPriceId { get; set; }
    [StringLength(SpecialInputPriceDetailConsts.MaterialCodeMaxLength)]
    public string? MaterialCode { get; set; }
    [StringLength(SpecialInputPriceDetailConsts.ModelMaxLength)]
    public string? Model { get; set; }
    [StringLength(SpecialInputPriceDetailConsts.Spec1MaxLength)]
    public string? Spec1 { get; set; }
    public int? LimitQty { get; set; }
    public decimal InputPrice { get; set; }
    public decimal LandedCost { get; set; }
    public int Used { get; set; } = 0;
    [StringLength(SpecialInputPriceDetailConsts.NoteMaxLength)]
    public string? Note { get; set; }
}