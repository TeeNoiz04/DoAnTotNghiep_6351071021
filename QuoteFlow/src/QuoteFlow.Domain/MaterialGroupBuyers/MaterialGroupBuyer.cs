using JetBrains.Annotations;
using QuoteFlow.Buyers;
using QuoteFlow.MaterialGroupBuyers.ParameterObjects;
using QuoteFlow.Materials.MaterialGroups;
using QuoteFlow.Shared.Models;
using System;

namespace QuoteFlow.MaterialGroupBuyers;

public class MaterialGroupBuyer : ExtendedAuditedAggregateRoot<Guid>
{
    public virtual Guid? MaterialGroupId { get; set; }

    [CanBeNull]
    public virtual string? MaterialGroupCode { get; set; }

    public virtual Guid BuyerId { get; set; }

    [CanBeNull]
    public virtual string? BuyerShortName { get; set; }

    [CanBeNull]
    public virtual string? Note { get; set; }


    public virtual MaterialGroup? MaterialGroup { get; set; }
    public virtual Buyer? Buyer { get; set; }

    protected MaterialGroupBuyer()
    {

    }

    public MaterialGroupBuyer(Guid id, Guid buyerId, Guid? materialGroupId = null, string? materialGroupCode = null, string? buyerShortName = null, string? note = null)
    {
        Id = id;
        BuyerId = buyerId;
        MaterialGroupId = materialGroupId;
        MaterialGroupCode = materialGroupCode;
        BuyerShortName = buyerShortName;
        Note = note;
    }

    public MaterialGroupBuyer(Guid id, MaterialGroupBuyerCreateParams createParams)
    {
        Id = id;
        BuyerId = createParams.BuyerId;
        MaterialGroupId = createParams.MaterialGroupId;
        MaterialGroupCode = createParams.MaterialGroupCode;
        BuyerShortName = createParams.BuyerShortName;
        Note = createParams.Note;
    }

}