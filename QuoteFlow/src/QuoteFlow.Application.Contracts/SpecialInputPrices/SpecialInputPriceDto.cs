using QuoteFlow.Shared;
using QuoteFlow.SupplierBUs;
using QuoteFlow.Suppliers;
using System;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.SpecialInputPrices;

public class SpecialInputPriceDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string AccountNo { get; set; } = null!;
    public string AccountName { get; set; } = null!;
    public string? ProjectName { get; set; }
    public string? MaterialType { get; set; }
    public Guid? SupplierId { get; set; }
    public Guid? SupplierBUId { get; set; }
    public string? Currency { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public string Status { get; set; } = null!;
    public string? Note { get; set; }
    public decimal TotalAmount { get; set; } = 0;
    public virtual SupplierBUDto? SupplierBU { get; set; }

    public virtual SupplierDto? Supplier { get; set; }
    public string ConcurrencyStamp { get; set; } = null!;

}