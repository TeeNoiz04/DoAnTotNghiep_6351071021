using JetBrains.Annotations;
using QuoteFlow.DPOs.DPODetails;
using QuoteFlow.SaleOrderDetails;
using QuoteFlow.SaleOrders.SaleOrderDetails.ParameterObjects;
using QuoteFlow.Shared.Models;
using QuoteFlow.StockCategories;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp;

namespace QuoteFlow.SaleOrders.SaleOrderDetails;

public class SaleOrderDetail : ExtendedAuditedAggregateRoot<Guid>
{
    [NotMapped]
    public virtual int? No { get; set; }
    public virtual Guid SaleOrderId { get; set; }

    public virtual Guid? DPODetailId { get; set; }

    [CanBeNull]
    public virtual string? StatusCode { get; set; }

    [CanBeNull]
    public virtual string? GolfaCode { get; set; }

    public virtual int? Qty { get; set; }

    public virtual decimal? Price { get; set; }

    public virtual decimal? Amount { get; set; }

    public virtual decimal? VAT { get; set; }

    public virtual Guid? StockCategoryId { get; set; }

    [CanBeNull]
    public virtual string? Note { get; set; }
    [CanBeNull]
    public virtual string? Extrafee_Note { get; set; }

    public virtual Guid? LockStockId { get; set; }

    public virtual decimal? Extrafee { get; set; }
    public virtual decimal? AmountIncludeExtrafee { get; set; }

    public virtual bool IsDeleted { get; set; }
    public virtual decimal? SAPLandingCost { get; set; }
    public virtual decimal? SAPAmountLandingCost { get; set; }
    public virtual string? GICPorNo { get; set; }
    public virtual string? GICPrNo { get; set; }
    public virtual string? GICSalePIC { get; set; }
    public virtual string? GICLocation { get; set; }
    public virtual string? GICReservationNo { get; set; }

    public virtual string? GICGivNo { get; set; }
    public virtual DateTime? GICGivDate { get; set; }
    public virtual string? ChangeNote { get; set; }
    public virtual string? Disposed { get; set; }
    public virtual DPODetail? DPODetail { get; set; }
    public virtual StockCategory? StockCategory { get; set; }

    protected SaleOrderDetail()
    {

    }

    public SaleOrderDetail(Guid id, Guid saleOrderId, Guid? dPODetailId = null, string? statusCode = null, string? golfaCode = null, int? qty = null, decimal? price = null, decimal? amount = null, decimal? vAT = null, Guid? stockCategoryId = null, string? note = null, string? extrafee_Note = null, Guid? lockStockId = null)
    {

        Id = id;
        Check.Length(statusCode, nameof(statusCode), SaleOrderDetailConsts.StatusCodeMaxLength, 0);
        Check.Length(golfaCode, nameof(golfaCode), SaleOrderDetailConsts.GolfaCodeMaxLength, 0);
        Check.Length(note, nameof(note), SaleOrderDetailConsts.NoteMaxLength, 0);
        SaleOrderId = saleOrderId;
        DPODetailId = dPODetailId;
        StatusCode = statusCode;
        GolfaCode = golfaCode;
        Qty = qty;
        Price = price;
        Amount = amount;
        VAT = vAT;
        StockCategoryId = stockCategoryId;
        Note = note;
        Extrafee_Note = extrafee_Note;
        LockStockId = lockStockId;
    }
    public SaleOrderDetail(Guid id, SaleOrderDetailCreateParams createParams)
    {
        Id = id;


        SaleOrderId = createParams.SaleOrderId;
        DPODetailId = createParams.DPODetailId;
        StatusCode = createParams.StatusCode;
        GolfaCode = createParams.GolfaCode;
        Qty = createParams.Qty;
        Price = createParams.Price;


        Amount = createParams.Amount;

        VAT = createParams.VAT;
        StockCategoryId = createParams.StockCategoryId;
        Note = createParams.Note;
        Extrafee_Note = createParams.Extrafee_Note;
        LockStockId = createParams.LockStockId;

        Extrafee = createParams.Extrafee;
        AmountIncludeExtrafee = createParams.AmountIncludeExtrafee;
        SAPLandingCost = createParams.SAPLandingCost;
        SAPAmountLandingCost = createParams.SAPAmountLandingCost;
        GICPorNo = createParams.GICPorNo;
        GICPrNo = createParams.GICPrNo;
        GICSalePIC = createParams.GICSalePIC;
        GICLocation = createParams.GICLocation;
        GICReservationNo = createParams.GICReservationNo;

        GICGivDate = createParams.GICGivDate;
        GICGivNo = createParams.GICGivNo;
        Disposed = createParams.Disposed;
        IsDeleted = false;
    }

}