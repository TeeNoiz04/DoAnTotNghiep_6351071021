using System;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.PriceOffers;

public class AssignSpecialInputPriceDto : IHasConcurrencyStamp
{
    public Guid SpecialInputPriceId { get; set; }
    public string? Note { get; set; }
    public string ConcurrencyStamp { get; set; } = null!;
}
