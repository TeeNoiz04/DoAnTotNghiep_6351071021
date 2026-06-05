using JetBrains.Annotations;
using QuoteFlow.SaleOrders.ParameterObjects;
using QuoteFlow.SaleOrders.SaleOrderDetails;
using QuoteFlow.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuoteFlow.SaleOrders;

public class SaleOrder : ExtendedAuditedAggregateRoot<Guid>
{
    [NotNull]
    public virtual string SONo { get; set; }

    [CanBeNull]
    public virtual string? SOSAPNo { get; set; }

    [CanBeNull]
    public virtual string? MaterialType { get; set; }

    public virtual Guid? BuyerId { get; set; }

    public virtual string? BuyerType { get; set; }

    [CanBeNull]
    public virtual string? BuyerCode { get; set; }

    [CanBeNull]
    public virtual string? BuyerName { get; set; }

    public virtual DateTime? OrderDate { get; set; }

    [CanBeNull]
    public virtual string? StatusCode { get; set; }

    public virtual Guid? StockCategoryId { get; set; }

    public virtual decimal? SO_VAT { get; set; }

    [CanBeNull]
    public virtual string? Note { get; set; }

    public virtual bool IsDeleted { get; set; }

    public virtual string? SAPDONo { get; set; }
    public virtual string? SAPInvoice { get; set; }
    public virtual DateTime? SAPInvoiceDate { get; set; }
    public virtual bool? DeliveryConfirmed { get; set; }
    public virtual string? SAPBillingNo { get; set; }
    public virtual DateTime? SAPDeliveryDate { get; set; }

    public virtual string? SOType { get; set; }
    public virtual string? GICType { get; set; }
    public virtual string? GICProcess { get; set; }


    public virtual string? GICGivNo { get; set; }
    public virtual DateTime? GICGivDate { get; set; }

    public virtual bool? CompletelyClosed { get; set; }


    [NotMapped]
    public virtual decimal TotalAmount { get; set; }
    public virtual ICollection<SaleOrderDetail> SaleOrderDetails { get; set; }

    [NotMapped]
    public virtual bool IsInProgress => StatusCode == QuoteFlowStatuses.InProgress;

    [NotMapped]
    public virtual bool IsDraft => StatusCode == QuoteFlowStatuses.Draft;

    [NotMapped]
    public virtual bool IsClosed => StatusCode == QuoteFlowStatuses.Closed;

    public ICollection<SOHistory>? SOHistories { get; set; }

    public SaleOrder()
    {

    }

    public SaleOrder(Guid id)
    {
        Id = id;
    }

    public void RecordAction(SOHistory history)
    {
        SOHistories ??= [];

        SOHistories.Add(history);
    }

    public SaleOrder(Guid id, string soNo, SaleOrderCreateParams createParams)
    {
        Id = id;


        SONo = soNo;
        SOSAPNo = createParams.SOSAPNo;
        MaterialType = createParams.MaterialType;
        BuyerId = createParams.BuyerId;
        BuyerCode = createParams.BuyerCode;
        BuyerName = createParams.BuyerName;
        OrderDate = createParams.OrderDate;
        StatusCode = QuoteFlowStatuses.Draft;
        StockCategoryId = createParams.StockCategoryId;
        SO_VAT = createParams.SO_VAT;
        Note = createParams.Note;
        BuyerType = createParams.BuyerType;
        IsDeleted = false;
        SAPBillingNo = createParams.SAPBillingNo;
        SAPDeliveryDate = createParams.SAPDeliveryDate;
        SAPDONo = createParams.SAPDONo;
        SAPInvoice = createParams.SAPInvoice;
        SAPInvoiceDate = createParams.SAPInvoiceDate;
        DeliveryConfirmed = createParams.DeliveryConfirmed;
        SOType = createParams.SOType;
        GICType = createParams.GICType;
        GICProcess = createParams.GICProcess;

        GICGivNo = createParams.GICGivNo;
        GICGivDate = createParams.GICGivDate;
        CompletelyClosed = createParams.CompletelyClosed;

    }
}