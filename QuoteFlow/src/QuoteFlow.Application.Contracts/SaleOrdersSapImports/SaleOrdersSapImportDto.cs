using QuoteFlow.Shared;
using System;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.SaleOrdersSapImports;

public class SaleOrdersSapImportDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string? SONo { get; set; }
    public string? DONo { get; set; }
    public DateTime? DODate { get; set; }
    public string? DONote { get; set; }
    public string? SOSAPNo { get; set; }
    public string? DOSAPNo { get; set; }
    public string? BillingNo { get; set; }
    public string? InvoiceNo { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public bool? Deleted { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;

}