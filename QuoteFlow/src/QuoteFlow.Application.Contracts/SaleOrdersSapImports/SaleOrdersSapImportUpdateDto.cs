using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.SaleOrdersSapImports;

public class SaleOrdersSapImportUpdateDto : IHasConcurrencyStamp
{
    [StringLength(SaleOrdersSapImportConsts.SONoMaxLength)]
    public string? SONo { get; set; }
    [StringLength(SaleOrdersSapImportConsts.DONoMaxLength)]
    public string? DONo { get; set; }
    public DateTime? DODate { get; set; }
    [StringLength(SaleOrdersSapImportConsts.DONoteMaxLength)]
    public string? DONote { get; set; }
    [StringLength(SaleOrdersSapImportConsts.SOSAPNoMaxLength)]
    public string? SOSAPNo { get; set; }
    [StringLength(SaleOrdersSapImportConsts.DOSAPNoMaxLength)]
    public string? DOSAPNo { get; set; }
    [StringLength(SaleOrdersSapImportConsts.BillingNoMaxLength)]
    public string? BillingNo { get; set; }
    [StringLength(SaleOrdersSapImportConsts.InvoiceNoMaxLength)]
    public string? InvoiceNo { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public bool? Deleted { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}