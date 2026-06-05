using QuoteFlow.Shared;
using System;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.AddMoreItemHistories;
public class AddMoreItemHistoryDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public virtual Guid? ImportGuid { get; set; }

    public virtual string? MaterialCode { get; set; }

    public virtual string? Model { get; set; }

    public virtual string? Spec1 { get; set; }

    public virtual string? Spec2 { get; set; }

    public virtual int? Qty { get; set; }

    public virtual decimal? StandardPriceToDist { get; set; }

    public virtual decimal? StandardAmount { get; set; }

    public virtual decimal? DistRequestedPrice { get; set; }

    public virtual decimal? RequestedAmount { get; set; }

    public virtual decimal? RequestedDiscount { get; set; }

    public virtual decimal? PriceToCustomer { get; set; }

    public virtual decimal? PriceOffer { get; set; }


    public virtual string? CometiorBrand { get; set; }

    public virtual string? CompetiorModel { get; set; }

    public virtual decimal? CompetiorPrice { get; set; }
    public string ConcurrencyStamp { get; set; } = null!;
}
