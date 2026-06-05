using QuoteFlow.Shared;
using QuoteFlow.SpecialInputPrices.SpecialInputPriceDetails;
using QuoteFlow.SupplierBUs;
using QuoteFlow.Suppliers;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.SpecialInputPrices;
public class SpecialInputPriceWithDetailDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
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
    public virtual ICollection<SpecialInputPriceDetailDto>? Details { get; set; }

    public virtual SupplierDto? Supplier { get; set; }
    public string ConcurrencyStamp { get; set; } = null!;
}

