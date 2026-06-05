using QuoteFlow.Shared;
using QuoteFlow.Suppliers;
using System;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.SupplierBUs;

public class SupplierBUDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string SupplierBUCode { get; set; } = null!;
    public string? SupplierBURemarks { get; set; }
    public string? OrderMethod { get; set; }
    public string? POTemplate { get; set; }
    public string? Contact { get; set; }
    public string? Email { get; set; }
    public string? INCOTerm { get; set; }
    public string? PaymentTermCode { get; set; }
    public string? PaymentDescription { get; set; }
    public string? Currency { get; set; }
    public string? MaterialType { get; set; }
    public Guid? SupplierId { get; set; }
    public string? SupplierCode { get; set; }
    public string? SupplierShortName { get; set; }
    public string? SupplierAddress { get; set; }
    public int SortOrder { get; set; }
    public string? FASCMVendorCode { get; set; }
    public string? FASCMBuyerCode { get; set; }
    public string? FASCMConsigneeCode { get; set; }
    public string? FASCMSectionCode { get; set; }
    public string? FASCMPaymentTerm { get; set; }
    public string? FASCMFreightMethod { get; set; }
    public string? FASCMDeliveryTerms { get; set; }
    public string? FASCMPlaceOfDeliveryTerms { get; set; }
    public string? FASCMShippingMarkCode { get; set; }
    public bool? IsDeactive { get; set; }

    public SupplierDto? Supplier { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;

}