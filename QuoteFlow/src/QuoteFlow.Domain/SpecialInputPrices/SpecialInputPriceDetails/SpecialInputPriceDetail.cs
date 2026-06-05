using JetBrains.Annotations;
using QuoteFlow.Shared.Models;
using QuoteFlow.SpecialInputPrices.SpecialInputPriceDetails.ParameterObject;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp;

namespace QuoteFlow.SpecialInputPrices.SpecialInputPriceDetails;

public class SpecialInputPriceDetail : ExtendedFullAuditedAggregateRoot<Guid>
{
    public virtual Guid? SpecialInputPriceId { get; set; }
    [CanBeNull]
    public virtual string? AccountNo { get; set; }

    [CanBeNull]
    public virtual string? MaterialCode { get; set; }
    [NotMapped]
    public virtual decimal? Standard { get; set; }

    [CanBeNull]
    public virtual string? Model { get; set; }

    [CanBeNull]
    public virtual string? Spec1 { get; set; }

    public virtual int? LimitQty { get; set; }

    public virtual decimal InputPrice { get; set; }

    public virtual decimal LandedCost { get; set; }

    public virtual int Used { get; set; }

    [CanBeNull]
    public virtual string? Note { get; set; }

    public virtual SpecialInputPrice? SpecialInputPrice { get; set; }
    protected SpecialInputPriceDetail()
    {

    }

    public SpecialInputPriceDetail(Guid id)
    {
        Id = id;
    }

    public SpecialInputPriceDetail(Guid id, decimal inputPrice, decimal landedCost, int used, Guid? specialInputPriceId = null, string? materialCode = null, string? model = null, string? spec1 = null, int? limitQty = null, string? note = null, decimal? standard = null)
    {

        Id = id;
        Check.Length(materialCode, nameof(materialCode), SpecialInputPriceDetailConsts.MaterialCodeMaxLength, 0);
        Check.Length(model, nameof(model), SpecialInputPriceDetailConsts.ModelMaxLength, 0);
        Check.Length(spec1, nameof(spec1), SpecialInputPriceDetailConsts.Spec1MaxLength, 0);
        Check.Length(note, nameof(note), SpecialInputPriceDetailConsts.NoteMaxLength, 0);
        InputPrice = inputPrice;
        LandedCost = landedCost;
        Used = used;
        SpecialInputPriceId = specialInputPriceId;
        MaterialCode = materialCode;
        Model = model;
        Spec1 = spec1;
        LimitQty = limitQty;
        Note = note;
        Standard = standard;
    }
    public SpecialInputPriceDetail(Guid id, SpecialInputPriceDetailCreateParams createParams)
    {
        Id = Id;
        AccountNo = createParams.AccountNo;
        InputPrice = createParams.InputPrice;
        LandedCost = createParams.LandedCost;
        Used = createParams.Used;
        SpecialInputPriceId = createParams.SpecialInputPriceId;
        MaterialCode = createParams.MaterialCode;
        Model = createParams.Model;
        Spec1 = createParams.Spec1;
        LimitQty = createParams.LimitQty;
        Note = createParams.Note;
    }
}