using QuoteFlow.Shared;
using System;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.SpecialInputPrices.SpecialInputPriceDetails;

public class SpecialInputPriceDetailDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public Guid? SpecialInputPriceId { get; set; }
    public string? MaterialCode { get; set; }
    public decimal? Standard { get; set; }
    public string? Model { get; set; }
    public string? Spec1 { get; set; }
    public int? LimitQty { get; set; }
    public decimal InputPrice { get; set; }
    public decimal LandedCost { get; set; }
    public int Used { get; set; }
    public string? Note { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;

}