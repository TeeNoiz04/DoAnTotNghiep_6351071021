using JetBrains.Annotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.SaleOrdersSapImports.ParameterObjects;
public class SaleOrderSapImportUpdateParams
{
    public string? MaterialCode { get; set; }
    public string? ModelName { get; set; }
    public string? SOType { get; set; }
    [StringLength(SaleOrdersSapImportConsts.SONoMaxLength)]
    public string? SONo { get; set; }
    [StringLength(SaleOrdersSapImportConsts.SOSAPNoMaxLength)]
    public string? SOSAPNo { get; set; }
    [StringLength(SaleOrdersSapImportConsts.DOSAPNoMaxLength)]
    public string? DOSAPNo { get; set; }
    [StringLength(SaleOrdersSapImportConsts.BillingNoMaxLength)]
    public string? BillingNo { get; set; }
    [StringLength(SaleOrdersSapImportConsts.InvoiceNoMaxLength)]
    public string? InvoiceNo { get; set; }
    public DateTime? InvoiceDate { get; set; }
    [StringLength(SaleOrdersSapImportConsts.NoteMaxLength)]
    public string? Note { get; set; }
    [StringLength(SaleOrdersSapImportConsts.FileNameMaxLength)]
    public string? FileName { get; set; }
    public Guid? ImportKey { get; set; }
    public bool? IsDeleted { get; set; }
    [CanBeNull]
    public virtual string? GICNo { get; set; }
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
    public virtual string? Disposed { get; set; }
    public string ConcurrencyStamp { get; set; } = null!;
}
