using JetBrains.Annotations;
using QuoteFlow.DPOs.DPODetails.ParameterObjects;
using QuoteFlow.Shared.Interfaces;
using QuoteFlow.Shared.Models;
using System;
using System.Collections.Generic;

namespace QuoteFlow.DPOs.DPODetails;

public class DPODetail : ExtendedFullAuditedAggregateRoot<Guid>, IHasStatus
{
    public virtual Guid DPOId { get; set; }

    [NotNull]
    public virtual string? Status { get; set; }

    public virtual int? RowNo { get; set; }

    [NotNull]
    public virtual string GolfaCode { get; set; }

    [CanBeNull]
    public virtual string? Model { get; set; }

    [CanBeNull]
    public virtual string? Spec1 { get; set; }

    [CanBeNull]
    public virtual string? Spec2 { get; set; }

    public virtual int? Qty { get; set; }

    public virtual decimal? UnitPrice { get; set; }

    public virtual decimal? LandedCost { get; set; }

    public virtual decimal? Amount { get; set; }
    public virtual decimal? AmountIncludeExtraFee { get; set; }

    public virtual DateTime? RequestedETA { get; set; }

    public virtual Guid? SPOId { get; set; }

    [CanBeNull]
    public virtual string? SPOCode { get; set; }

    [CanBeNull]
    public virtual Guid? CustomerId { get; set; }

    [CanBeNull]
    public virtual string? CustomerTaxCode { get; set; }

    [CanBeNull]
    public virtual string? CustomerName { get; set; }

    [CanBeNull]
    public virtual string? CustomerType { get; set; }

    [CanBeNull]
    public virtual string? CustomerIndustry { get; set; }

    public virtual int? LockStock { get; set; }

    public virtual int? LockStockSO { get; set; }

    public virtual int? LockShipment { get; set; }

    public virtual int? Delivered { get; set; }

    public virtual int? NeedDelivery { get; set; }

    [CanBeNull]
    public virtual string? Note { get; set; }

    [CanBeNull]
    public virtual string? OrderReason { get; set; }

    [CanBeNull]
    public virtual string? AccountNo { get; set; }

    [CanBeNull]
    public virtual decimal? Extrafee { get; set; }

    [CanBeNull]
    public virtual decimal? ExtrafeeUsedInSO { get; set; }

    [CanBeNull]
    public virtual decimal? ExtrafeeAvailable { get; set; }

    [CanBeNull]
    public virtual string? ExtrafeeNote { get; set; }
    [CanBeNull]
    public virtual string? ConfirmNoted { get; set; }

    // Additional fields specific to Warranty GICs
    [CanBeNull]
    public string? DamagedProduct { get; set; }

    [CanBeNull]
    public string? ProductSerialNo { get; set; }

    [CanBeNull]
    public string? MEVNSellingInvoiceNo { get; set; }

    [CanBeNull]
    public decimal? DPOUsed { get; set; } // GKR - DPOUsed qty / GKR Qty

    public DPO DPO { get; set; } = null!;
    public virtual ICollection<DPODetailApprovalHistory> ApprovalHistories { get; set; } = [];

    protected DPODetail()
    {

    }



    public DPODetail(Guid id, DPODetailCreateParams createParams)
    {
        Id = id;
        DPOId = createParams.DPOId;
        Status = QuoteFlowStatuses.InProgress;
        RowNo = createParams.RowNo;
        GolfaCode = createParams.GolfaCode;
        Model = createParams.Model;
        Spec1 = createParams.Spec1;
        Spec2 = createParams.Spec2;
        Qty = createParams.Qty;
        UnitPrice = createParams.UnitPrice;
        LandedCost = createParams.LandedCost;
        Amount = createParams.Amount;
        AmountIncludeExtraFee = createParams.Amount;
        RequestedETA = createParams.RequestedETA;
        SPOId = createParams.SPOId;
        SPOCode = createParams.SPOCode;
        CustomerId = createParams.CustomerId;
        CustomerTaxCode = createParams.CustomerTaxCode;
        CustomerName = createParams.CustomerName;
        CustomerType = createParams.CustomerType;
        CustomerIndustry = createParams.CustomerIndustry;
        LockStock = createParams.LockStock;
        LockStockSO = createParams.LockStockSO;
        LockShipment = createParams.LockShipment;
        Delivered = createParams.Delivered;
        NeedDelivery = createParams.NeedDelivery;
        Note = createParams.Note;
        OrderReason = createParams.OrderReason;
        AccountNo = createParams.AccountNo;
        Extrafee = 0;
        ExtrafeeUsedInSO = 0;
        ExtrafeeAvailable = 0;
        ExtrafeeNote = null;
        ConfirmNoted = createParams.ConfirmNoted;

        // Additional fields specific to Warranty GICs
        DamagedProduct = createParams.DamagedProduct;
        ProductSerialNo = createParams.ProductSerialNo;
        MEVNSellingInvoiceNo = createParams.MEVNSellingInvoiceNo;
    }

    public void Cancel()
    {
        //if (Status != QuoteFlowStatuses.InProgress)
        //{
        //    throw new BusinessException(QuoteFlowDomainErrorCodes.OnlyInProgressCanBeCancelled);
        //}

        Status = QuoteFlowStatuses.Cancelled;
    }

    public void RecordAction(DPODetailApprovalHistory history)
    {
        ApprovalHistories ??= [];
        ApprovalHistories.Add(history);
    }

    public bool IsInProgress() => Status == QuoteFlowStatuses.InProgress;

    public void SetInProgress()
    {
        Status = QuoteFlowStatuses.InProgress;
    }

    public void Reject()
    {
        Status = QuoteFlowStatuses.Rejected;
    }
}