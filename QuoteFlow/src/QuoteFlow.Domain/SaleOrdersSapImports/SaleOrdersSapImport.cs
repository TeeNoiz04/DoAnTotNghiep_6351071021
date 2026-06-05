using JetBrains.Annotations;
using QuoteFlow.SaleOrdersSapImports.ParameterObjects;
using QuoteFlow.Shared.Models;
using System;

namespace QuoteFlow.SaleOrdersSapImports;

public class SaleOrdersSapImport : ExtendedAuditedAggregateRoot<Guid>
{
    [CanBeNull]
    public virtual string? MaterialCode { get; set; }
    [CanBeNull]
    public virtual string? ModelName { get; set; }
    [CanBeNull]
    public virtual string? SOType { get; set; }
    [CanBeNull]
    public virtual string? SONo { get; set; }

    [CanBeNull]
    public virtual string? SOSAPNo { get; set; }
    [CanBeNull]
    public virtual string? DOSAPNo { get; set; }
    [CanBeNull]
    public virtual string? BillingNo { get; set; }
    [CanBeNull]
    public virtual string? InvoiceNo { get; set; }

    public virtual DateTime? InvoiceDate { get; set; }
    [CanBeNull]
    public virtual string? Note { get; set; }
    [CanBeNull]
    public virtual string? FileName { get; set; }
    [CanBeNull]
    public virtual Guid? ImportKey { get; set; }
    [CanBeNull]
    public virtual bool? IsDeleted { get; set; }
    [CanBeNull]
    public virtual decimal? GICLandingCost { get; set; }
    [CanBeNull]
    public virtual decimal? GICAmountLandingCost { get; set; }
    [CanBeNull]
    public virtual string? GICPORNo { get; set; }
    [CanBeNull]
    public virtual string? GICPRNo { get; set; }
    [CanBeNull]
    public virtual string? GICGivNo { get; set; }
    [CanBeNull]
    public virtual DateTime? GICGivDate { get; set; }
    [CanBeNull]
    public virtual string? GICSalesPIC { get; set; }
    [CanBeNull]
    public virtual string? GICLocation { get; set; }
    [CanBeNull]
    public virtual string? GICReservationNo { get; set; }
    [CanBeNull]
    public virtual string? GICAssetClass { get; set; }
    [CanBeNull]
    public virtual string? GICMainAssetCode { get; set; }

    [CanBeNull]
    public virtual string? GICSubAssetCode { get; set; }
    [CanBeNull]
    public virtual string? GICAssetName { get; set; }


    [CanBeNull]
    public virtual string? GICNo { get; set; }
    [CanBeNull]
    public virtual string? Disposed { get; set; }

    protected SaleOrdersSapImport()
    {

    }

    public SaleOrdersSapImport(
        Guid id,
        string? materialCode = null,
        string? modelName = null,
        string? sOType = null,
        string? sONo = null,
        string? sOSAPNo = null,
        string? dOSAPNo = null,
        string? billingNo = null,
        string? invoiceNo = null,
        DateTime? invoiceDate = null,
        string? note = null,
        string? fileName = null,
        Guid? importKey = null,
        bool? isDeleted = null,
        decimal? gicLandingCost = null,
        decimal? gicAmountLandingCost = null,
        string? gicPORNo = null,
        string? gicPRNo = null,
        string? gicGivNo = null,
        DateTime? gicGivDate = null,
        string? gicSalesPIC = null,
        string? gicLocation = null,
        string? gicReservationNo = null,
        string? gicAssetClass = null,
        string? gicMainAssetCode = null,
        string? gicSubAssetCode = null,
        string? gicAssetName = null)
    {
        Id = id;
        MaterialCode = materialCode;
        ModelName = modelName;
        SOType = sOType;

        SONo = sONo;
        SOSAPNo = sOSAPNo;
        DOSAPNo = dOSAPNo;
        BillingNo = billingNo;
        InvoiceNo = invoiceNo;
        InvoiceDate = invoiceDate;
        Note = note;
        FileName = fileName;
        ImportKey = importKey;
        IsDeleted = isDeleted;

        GICLandingCost = gicLandingCost;
        GICAmountLandingCost = gicAmountLandingCost;
        GICPORNo = gicPORNo;
        GICPRNo = gicPRNo;
        GICGivNo = gicGivNo;
        GICGivDate = gicGivDate;
        GICSalesPIC = gicSalesPIC;
        GICLocation = gicLocation;
        GICReservationNo = gicReservationNo;
        GICAssetClass = gicAssetClass;
        GICMainAssetCode = gicMainAssetCode;
        GICSubAssetCode = gicSubAssetCode;
        GICAssetName = gicAssetName;
    }


    public SaleOrdersSapImport(Guid id, SaleOrderSapImportCreateParams createParams)
    {
        Id = id;

        MaterialCode = createParams.MaterialCode;
        ModelName = createParams.ModelName;
        SOType = createParams.SOType;

        SONo = createParams.SONo;
        SOSAPNo = createParams.SOSAPNo;
        DOSAPNo = createParams.DOSAPNo;
        BillingNo = createParams.BillingNo;
        InvoiceNo = createParams.InvoiceNo;
        InvoiceDate = createParams.InvoiceDate;
        Note = createParams.Note;
        FileName = createParams.FileName;
        ImportKey = createParams.ImportKey;
        IsDeleted = createParams.IsDeleted;


        GICLandingCost = createParams.GICLandingCost;
        GICAmountLandingCost = createParams.GICAmountLandingCost;
        GICPORNo = createParams.GICPORNo;
        GICPRNo = createParams.GICPRNo;
        GICGivNo = createParams.GICGivNo;
        GICGivDate = createParams.GICGivDate;
        GICSalesPIC = createParams.GICSalesPIC;
        GICLocation = createParams.GICLocation;
        GICReservationNo = createParams.GICReservationNo;
        GICAssetClass = createParams.GICAssetClass;
        GICMainAssetCode = createParams.GICMainAssetCode;
        GICSubAssetCode = createParams.GICSubAssetCode;
        GICAssetName = createParams.GICAssetName;
        GICNo = createParams.GICNo;
        Disposed = createParams.Disposed;
    }



}