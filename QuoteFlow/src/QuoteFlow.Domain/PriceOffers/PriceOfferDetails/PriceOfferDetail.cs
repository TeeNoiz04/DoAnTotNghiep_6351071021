using JetBrains.Annotations;
using QuoteFlow.PriceOffers.PriceOfferDetails.ParameterObjects;
using QuoteFlow.Shared.Extensions;
using QuoteFlow.Shared.Interfaces;
using QuoteFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Volo.Abp;

namespace QuoteFlow.PriceOffers.PriceOfferDetails;

public class PriceOfferDetail : ExtendedAuditedAggregateRoot<Guid>, IHasStatus
{
    public virtual int RowNo { get; set; }
    public virtual Guid PriceOfferId { get; set; }

    [NotNull]
    public virtual string GolfaCode { get; set; }

    [NotNull]
    public virtual string ModelName { get; set; }

    [CanBeNull]
    public virtual string? SpecialSpec1 { get; set; }

    [CanBeNull]
    public virtual string? SpecialSpec2 { get; set; }

    public virtual decimal? DpoUsed { get; set; }

    public virtual decimal Qty { get; set; }

    public virtual decimal StandardPrice { get; set; }

    public virtual decimal StandardAmount { get; set; }

    public virtual decimal? BuyerPrice { get; set; }

    public virtual decimal? RequestedAmount { get; set; }

    public virtual decimal? RequestedDiscountRatio { get; set; }

    public virtual decimal? PriceToCustomer { get; set; }

    public virtual decimal MEVNOfferPrice { get; set; }

    [CanBeNull]
    public virtual string? CompetitorBrand { get; set; }

    [CanBeNull]
    public virtual string? CompetitorModel { get; set; }

    public virtual decimal? CompetitorPrice { get; set; }

    public virtual decimal? LandingCost { get; set; }

    public virtual decimal? InputPrice { get; set; }

    [CanBeNull]
    public virtual string? InputCurrency { get; set; }

    public virtual decimal? ManagerMargin { get; set; }

    public virtual decimal? PriceOfferDetailMargin { get; set; }

    [CanBeNull]
    public virtual string? AccountCode { get; set; }

    [CanBeNull]
    public virtual string? Note { get; set; }

    public virtual Guid ImportGuid { get; set; }

    public virtual string? Status { get; set; }

    [CanBeNull]
    public virtual decimal? MaxSalesOfferPrice { get; set; }

    [CanBeNull]
    public virtual decimal? MaxMangerOfferPrice { get; set; }

    [CanBeNull]
    public virtual decimal? ActualDiscountRatio { get; set; }

    [NotMapped]
    public virtual decimal? MEVNOfferAmount => MEVNOfferPrice * Qty;

    [NotMapped]
    public virtual decimal? DpoUsedAmount => (DpoUsed ?? 0) * MEVNOfferPrice;

    [NotMapped]
    public virtual string? ProjectResult => PriceOffer?.ProjectResultStatus;

    #region Navigation Properties
    public virtual PriceOffer? PriceOffer { get; set; }
    public virtual ICollection<PriceOfferDetailApprovalHistory> ApprovalHistories { get; set; } = [];
    #endregion
    protected PriceOfferDetail()
    {

    }

    public PriceOfferDetail(Guid id, PriceOfferDetailCreateParams createParams)
    {
        Id = id;
        RowNo = createParams.RowNo;
        PriceOfferId = createParams.PriceOfferId;
        GolfaCode = createParams.GolfaCode;
        ModelName = createParams.ModelName;
        Qty = createParams.Qty;
        StandardPrice = createParams.StandardPrice;
        StandardAmount = createParams.StandardAmount;
        MEVNOfferPrice = createParams.MEVNOfferPrice;
        ImportGuid = createParams.ImportGuid;
        SpecialSpec1 = createParams.SpecialSpec1;
        SpecialSpec2 = createParams.SpecialSpec2;
        DpoUsed = createParams.DpoUsed;
        BuyerPrice = createParams.BuyerPrice;
        RequestedAmount = createParams.RequestedAmount;
        RequestedDiscountRatio = createParams.RequestedDiscountRatio;
        PriceToCustomer = createParams.PriceToCustomer;
        CompetitorBrand = createParams.CompetitorBrand;
        CompetitorModel = createParams.CompetitorModel;
        CompetitorPrice = createParams.CompetitorPrice;
        LandingCost = createParams.LandingCost;
        InputPrice = createParams.InputPrice;
        InputCurrency = createParams.InputCurrency;
        ManagerMargin = createParams.ManagerMargin;
        PriceOfferDetailMargin = createParams.PriceOfferDetailMargin;
        AccountCode = createParams.AccountCode;
        Note = createParams.Note;

        Status = QuoteFlowStatuses.Verifying;
    }

    public void Submit()
    {
        if (!this.IsDraft() && !this.IsCancelled() && !this.IsVerifying() && !this.IsRejected())
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.InvalidStatusForSubmission);
        }

        Status = QuoteFlowStatuses.InProgress;
    }

    public void Approve(bool isLastStep = false)
    {
        if (!this.IsInProgress())
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.OnlyInProgressCanBeApproved);
        }

        if (isLastStep)
        {
            Status = QuoteFlowStatuses.Approved;
        }
        else
        {
            Status = QuoteFlowStatuses.InProgress;
        }
    }

    public void Reject()
    {
        if (!this.IsInProgress())
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.OnlyInProgressCanBeRejected);
        }
        else
        {
            Status = QuoteFlowStatuses.Rejected;
        }
    }

    public void Cancel()
    {
        if (!this.IsInProgress())
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.OnlyInProgressCanBeCancelled);
        }
        else
        {
            Status = QuoteFlowStatuses.Cancelled;
        }
    }

    public void Close()
    {
        if (!this.IsApproved())
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.OnlyApprovedCanBeClosed);
        }
        else
        {
            Status = QuoteFlowStatuses.Closed;
        }
    }

    public void RecordAction(PriceOfferDetailApprovalHistory history)
    {
        ApprovalHistories ??= [];

        ApprovalHistories.Add(history);
    }

    public void Merge(PriceOfferDetail detail)
    {
        if (detail == null)
        {
            throw new ArgumentNullException(nameof(detail), "Cannot merge with a null detail.");
        }

        var additionalQty = detail.Qty;
        if (Qty + additionalQty < 0)
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.InvalidQtyToMerge)
                .WithData("qty", Qty)
                .WithData("additionalQty", additionalQty);
        }

        Qty += detail.Qty;

        ApprovalHistories ??= [];
        if (detail.ApprovalHistories != null)
        {
            ApprovalHistories = [.. ApprovalHistories.Concat(detail.ApprovalHistories)];
        }
    }

    public void MarkAsMerged()
    {
        //if (!this.IsInProgress())
        //{
        //    throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.OnlyInProgressItemCanBeMerged);
        //}

        Status = QuoteFlowStatuses.PriceOfferDetail.Merged;
    }

    public bool IsMerged()
    {
        return Status == QuoteFlowStatuses.PriceOfferDetail.Merged;
    }

    public void UsedByDPO(int qty)
    {
        if (qty <= 0)
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.InvalidDpoUsedQuantity)
                .WithData("qty", qty);
        }
        var newDPOUsed = (DpoUsed ?? 0) + qty;
        if (newDPOUsed > Qty)
        {
            var availableQty = Qty - (DpoUsed ?? 0);
            throw new BusinessException(QuoteFlowDomainErrorCodes.PriceOffer.DpoUsedExceedsAvailableQuantity)
                .WithData("qtyToAdd", qty)
                .WithData("availableQty", availableQty);
        }

        DpoUsed = newDPOUsed;
    }
}