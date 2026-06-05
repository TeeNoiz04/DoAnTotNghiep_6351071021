using QuoteFlow.Buyers;
using QuoteFlow.Materials.MaterialGroups;
using QuoteFlow.Shared;
using System;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.MaterialGroupBuyers;

public class MaterialGroupBuyerDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public Guid? MaterialGroupId { get; set; }
    public string? MaterialGroupCode { get; set; }
    public Guid BuyerId { get; set; }
    public string? BuyerShortName { get; set; }
    public string? Note { get; set; }

    public MaterialGroupDto? MaterialGroup { get; set; }
    public BuyerDto? Buyer { get; set; }
    public string ConcurrencyStamp { get; set; } = null!;

}