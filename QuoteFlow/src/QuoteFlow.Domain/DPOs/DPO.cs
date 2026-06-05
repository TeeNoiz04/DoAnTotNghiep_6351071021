using JetBrains.Annotations;
using QuoteFlow.DPOs.DPODetails;
using QuoteFlow.DPOs.ParameterObjects;
using QuoteFlow.GICs;
using QuoteFlow.Shared.Interfaces;
using QuoteFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;

namespace QuoteFlow.DPOs;

public class DPO : ExtendedFullAuditedAggregateRoot<Guid>, IApprovable
{
    [CanBeNull]
    public virtual string? DPONo { get; set; }

    [CanBeNull]
    public virtual string? DPOType { get; set; }

    [CanBeNull]
    public virtual string? GICType { get; set; }

    [CanBeNull]
    public virtual string? MaterialType { get; set; }

    [CanBeNull]
    public virtual string? CostCenter { get; set; }

    [CanBeNull]
    public virtual string? Status { get; set; }

    public virtual Guid? BuyerTypeId { get; set; }

    public virtual Guid? BuyerId { get; set; }

    [CanBeNull]
    public virtual string? BuyerShortName { get; set; }

    public virtual string? BuyerTypeDescription { get; set; }

    public virtual DateTime? OrderDate { get; set; }

    public virtual decimal TotalAmount { get; set; }

    public virtual decimal TotalAmountIncludeExtraFee { get; set; }

    [CanBeNull]
    public virtual string? Remark { get; set; }

    [CanBeNull]
    public virtual string? FileName { get; set; }

    [CanBeNull]
    public virtual string? ReferenceDoc { get; set; }

    [CanBeNull]
    public virtual DateTime? ReferenceDocDate { get; set; }

    [CanBeNull]
    public virtual string? GICProcess { get; set; }

    [CanBeNull]
    public virtual string? LinkedDpoNo { get; set; }

    [CanBeNull]
    public virtual Guid? LinkedDpoId { get; set; }

    [CanBeNull]
    public virtual string? LinkedNote { get; set; }

    [CanBeNull]
    public virtual DateTime? ExpirationDate { get; set; }

    [CanBeNull]
    public Guid? CurrentApprovalRouteInstanceId { get; set; }

    [CanBeNull]
    public int? CurrentApprovalStepSequence { get; set; }

    [CanBeNull]
    public string? CurrentApproverRoleCode { get; set; }

    [CanBeNull]
    public string? CurrentApproverRoleName { get; set; }

    [CanBeNull]
    public string? Reason { get; set; }

    [CanBeNull]
    public string? SalePicUsername { get; set; }

    [CanBeNull]
    public string? SalePicFullName { get; set; }

    [CanBeNull]
    public Guid? SalePicTeamId { get; set; }

    public ICollection<DPODetail> Details { get; set; } = [];
    public virtual ICollection<DPOApprovalHistory> ApprovalHistories { get; set; } = [];
    public virtual ICollection<DPOMessage> Messages { get; set; } = [];

    // GKR only
    public virtual ICollection<GKRApprovalRoute> ApprovalRoutes { get; set; } = [];


    public bool IsConfirmed => Status == QuoteFlowStatuses.DPO.Confirmed;
    public bool IsLockedStock => Status == QuoteFlowStatuses.DPO.LockedStock;
    public bool IsInProgress => Status == QuoteFlowStatuses.InProgress;
    public bool IsCancelled => Status == QuoteFlowStatuses.Cancelled;
    public bool IsSubmitted => Status == QuoteFlowStatuses.GKR.Submitted;


    protected DPO()
    {

    }

    public DPO(Guid id, DPOCreateParams createParams)
    {
        Id = id;
        DPONo = createParams.DPONo;
        TotalAmount = createParams.TotalAmount;
        TotalAmountIncludeExtraFee = createParams.TotalAmount;
        MaterialType = createParams.MaterialType;
        CostCenter = createParams.CostCenter;
        BuyerId = createParams.BuyerId;
        BuyerTypeId = createParams.BuyerTypeId;
        BuyerShortName = createParams.BuyerShortName;
        BuyerTypeDescription = createParams.BuyerTypeDescription;
        OrderDate = createParams.OrderDate;
        Remark = createParams.Remark;
        FileName = createParams.FileName;

        // On FAP app, default status is 'Confirmed'
        DPOType = DPOTypes.DPO;
        Status = QuoteFlowStatuses.DPO.Confirmed;
    }

    public DPO(Guid id, GICCreateParams createParams)
    {
        Id = id;
        DPONo = createParams.DPONo;
        TotalAmount = createParams.TotalAmount;
        TotalAmountIncludeExtraFee = createParams.TotalAmount;
        GICType = createParams.GICType;
        MaterialType = createParams.MaterialType;
        CostCenter = createParams.CostCenter;
        BuyerId = createParams.BuyerId;
        BuyerTypeId = createParams.BuyerTypeId;
        BuyerShortName = createParams.BuyerShortName;
        BuyerTypeDescription = createParams.BuyerTypeDescription;
        OrderDate = createParams.OrderDate;
        Remark = createParams.Remark;
        FileName = createParams.FileName;
        ReferenceDoc = createParams.ReferenceDoc;
        ReferenceDocDate = createParams.ReferenceDocDate;
        GICProcess = createParams.GICProcess;

        // On FAP app, default status is 'Confirmed'
        DPOType = DPOTypes.GIC;
        Status = QuoteFlowStatuses.DPO.Confirmed;
    }

    public DPO(Guid id, GKRCreateParams createParams)
    {
        Id = id;
        DPONo = createParams.DPONo;
        TotalAmount = createParams.TotalAmount;
        TotalAmountIncludeExtraFee = createParams.TotalAmount;
        MaterialType = createParams.MaterialType;
        BuyerId = createParams.BuyerId;
        BuyerTypeId = createParams.BuyerTypeId;
        BuyerShortName = createParams.BuyerShortName;
        BuyerTypeDescription = createParams.BuyerTypeDescription;
        OrderDate = createParams.OrderDate;
        ExpirationDate = createParams.ExpirationDate;
        Remark = createParams.Remark;
        FileName = createParams.FileName;
        Reason = createParams.Reason;
        SalePicUsername = createParams.SalePicUsername;
        SalePicFullName = createParams.SalePicFullName;
        SalePicTeamId = createParams.SalePicTeamId;

        // On FAP app, default status is 'Confirmed'
        DPOType = DPOTypes.GKR;
        Status = QuoteFlowStatuses.DPO.Submitted;
    }

    public void Cancel(List<Guid> dpoDetailIds)
    {
        //if (Status != QuoteFlowStatuses.InProgress)
        //{
        //    throw new BusinessException(QuoteFlowDomainErrorCodes.OnlyInProgressCanBeCancelled);
        //}
        var detailsToCancel = Details.Where(d => dpoDetailIds.Contains(d.Id)).ToList();

        foreach (var detail in detailsToCancel)
        {
            detail.Cancel();
        }

        // Check if all details are cancelled, then cancel the DPO
        if (Details.All(d => d.Status == QuoteFlowStatuses.Cancelled))
        {
            Status = QuoteFlowStatuses.Cancelled;
        }

        else if (
            Details.All(d => d.Status == QuoteFlowStatuses.Cancelled || d.Status == QuoteFlowStatuses.Closed)
            && Details.Any(d => d.Status == QuoteFlowStatuses.Closed)
        )
        {
            Status = QuoteFlowStatuses.Closed;
        }


        // reduce dpo totalamount
        TotalAmount = CalculateTotalAmount();

        TotalAmountIncludeExtraFee = CalculateTotalAmount(includeExtraFee: true);
    }

    public void RecordAction(DPOApprovalHistory history)
    {
        ApprovalHistories ??= [];
        ApprovalHistories.Add(history);
    }

    public void ConfirmLockStock()
    {
        if (Status != QuoteFlowStatuses.DPO.Confirmed)
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.DPO.OnlyConfirmedDPOCanBeLockedStock);
        }

        if ((!string.IsNullOrWhiteSpace(GICType) && GICType == GICTypeCodes.WriteOff) || Details.All(d => d.NeedDelivery == 0))
        {
            Status = QuoteFlowStatuses.InProgress;
        }
        else
        {
            Status = QuoteFlowStatuses.DPO.LockedStock;
        }
    }

    public void ConfirmLockOnOrder()
    {
        if (Status != QuoteFlowStatuses.DPO.LockedStock)
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.DPO.OnlyLockedStockDPOCanBeInProgress);
        }

        Status = QuoteFlowStatuses.InProgress;
    }

    public void Confirm()
    {
        if (Status == QuoteFlowStatuses.DPO.Submitted || Status == QuoteFlowStatuses.DPO.Confirmed)
        {

            Status = QuoteFlowStatuses.DPO.Confirmed;
        }
        else
            throw new BusinessException(QuoteFlowDomainErrorCodes.DPO.OnlySubmittedDPOCanBeApproved);


    }

    public void Approve(DateTime actionDate, string? note = null, bool isLastStep = false)
    {
        if (!IsSubmitted)
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.GKR.OnlySubmittedCanBeApproved);
        }

        if (isLastStep)
        {
            Status = QuoteFlowStatuses.GKR.Confirmed;
        }
        else
        {
            Status = QuoteFlowStatuses.GKR.Submitted;
        }

        var latestUnapprovedStep = GetLatestUnapprovedStep();
        foreach (var route in ApprovalRoutes.Where(x => x.StepSequence <= latestUnapprovedStep.StepSequence && !x.IsApproved))
        {
            route.Approve(actionDate, note);
        }
    }

    public void Reject()
    {
        if (Status != QuoteFlowStatuses.DPO.Submitted)
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.DPO.OnlySubmittedDPOCanBeRejected);
        }

        Status = QuoteFlowStatuses.Rejected;

        // Reject all details as well
        foreach (var detail in Details)
        {
            detail.Reject();
        }
    }

    public void AddMessage(DPOMessage message)
    {
        Messages ??= [];
        Messages.Add(message);
    }

    public decimal CalculateTotalAmount(bool includeExtraFee = false)
    {
        var details = Details.Where(d => d.Status != QuoteFlowStatuses.Cancelled).ToList();
        return includeExtraFee
            ? details.Sum(d => d.AmountIncludeExtraFee ?? 0)
            : details.Sum(d => d.Amount ?? 0);
    }

    public GKRApprovalRoute GetLatestUnapprovedStep()
    {
        if (ApprovalRoutes == null || ApprovalRoutes.Count == 0)
        {
            throw new BusinessException(QuoteFlowDomainErrorCodes.NoApprovalRouteFound)
                .WithData("entityId", Id);
        }

        var latestUnapproved = ApprovalRoutes
            .Where(x => !x.IsApproved)
            .MinBy(x => x.StepSequence)
            ?? throw new BusinessException(QuoteFlowDomainErrorCodes.NoUnapprovedStepFound)
                .WithData("entityId", Id);

        return latestUnapproved;
    }
}