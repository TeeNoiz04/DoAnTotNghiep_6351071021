using JetBrains.Annotations;
using QuoteFlow.DistributorTargets.ParameterObjects;
using QuoteFlow.Shared.Models;
using System;
using Volo.Abp;

namespace QuoteFlow.DistributorTargets;

public class DistributorTarget : ExtendedAuditedAggregateRoot<Guid>
{
    public virtual Guid? BuyerTypeId { get; set; }

    public virtual Guid BuyerId { get; set; }

    [CanBeNull]
    public virtual string? BuyerCode { get; set; }
    [CanBeNull]
    public virtual string? BuyerName { get; set; }

    [NotNull]
    public virtual string MaterialType { get; set; }

    public virtual int? FinanceYear { get; set; }

    public virtual decimal? FirstFYTarget { get; set; }

    public virtual decimal? SecondFYTarget { get; set; }

    [CanBeNull]
    public virtual string? Note { get; set; }

    protected DistributorTarget()
    {

    }

    public DistributorTarget(Guid id, Guid buyerId, string materialType, Guid? buyerTypeId = null, string? buyerCode = null, int? financeYear = null, decimal? firstFYTarget = null, decimal? secondFYTarget = null, string? note = null, string? buyerName = null)
    {

        Id = id;
        Check.NotNull(materialType, nameof(materialType));
        Check.Length(materialType, nameof(materialType), DistributorTargetConsts.MaterialTypeMaxLength, 0);
        Check.Length(buyerCode, nameof(buyerCode), DistributorTargetConsts.BuyerCodeMaxLength, 0);
        BuyerId = buyerId;
        MaterialType = materialType;
        BuyerTypeId = buyerTypeId;
        BuyerCode = buyerCode;
        BuyerName = buyerName;
        FinanceYear = financeYear;
        FirstFYTarget = firstFYTarget;
        SecondFYTarget = secondFYTarget;
        Note = note;
    }

    public DistributorTarget(Guid id, DistributorTargetCreateParams createParams)
    {
        Id = id;
        BuyerId = createParams.BuyerId;
        MaterialType = createParams.MaterialType;
        BuyerTypeId = createParams.BuyerTypeId;
        BuyerCode = createParams.BuyerCode;
        BuyerName = createParams.BuyerName;
        FinanceYear = createParams.FinanceYear;
        FirstFYTarget = createParams.FirstFYTarget;
        SecondFYTarget = createParams.SecondFYTarget;
        Note = createParams.Note;
    }
}