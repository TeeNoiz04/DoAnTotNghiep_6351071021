using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.SupplierBUs.ParameterObjects;
public class SupplierBUUpdateParams
{
    [Required]
    [StringLength(SupplierBUConsts.SupplierBUCodeMaxLength)]
    public string SupplierBUCode { get; set; } = null!;
    [StringLength(SupplierBUConsts.SupplierBURemarksMaxLength)]
    public string? SupplierBURemarks { get; set; }
    [StringLength(SupplierBUConsts.OrderMethodMaxLength)]
    public string? OrderMethod { get; set; }
    [StringLength(SupplierBUConsts.POTemplateMaxLength)]
    public string? POTemplate { get; set; }
    [StringLength(SupplierBUConsts.ContactMaxLength)]
    public string? Contact { get; set; }
    [StringLength(SupplierBUConsts.EmailMaxLength)]
    public string? Email { get; set; }
    [StringLength(SupplierBUConsts.INCOTermMaxLength)]
    public string? INCOTerm { get; set; }
    [StringLength(SupplierBUConsts.PaymentTermCodeMaxLength)]
    public string? PaymentTermCode { get; set; }
    [StringLength(SupplierBUConsts.PaymentDescriptionMaxLength)]
    public string? PaymentDescription { get; set; }
    [StringLength(SupplierBUConsts.CurrencyMaxLength)]
    public string? Currency { get; set; }
    [StringLength(SupplierBUConsts.MaterialTypeMaxLength)]
    public string? MaterialType { get; set; }
    public Guid? SupplierId { get; set; }
    [StringLength(SupplierBUConsts.SupplierCodeMaxLength)]
    public string? SupplierCode { get; set; }
    [StringLength(SupplierBUConsts.SupplierShortNameMaxLength)]
    public string? SupplierShortName { get; set; }
    [StringLength(SupplierBUConsts.SupplierAddressMaxLength)]
    public string? SupplierAddress { get; set; }
    [Required]
    public int SortOrder { get; set; }
    [StringLength(SupplierBUConsts.FASCMVendorCodeMaxLength)]
    public string? FASCMVendorCode { get; set; }
    [StringLength(SupplierBUConsts.FASCMBuyerCodeMaxLength)]
    public string? FASCMBuyerCode { get; set; }
    [StringLength(SupplierBUConsts.FASCMConsigneeCodeMaxLength)]
    public string? FASCMConsigneeCode { get; set; }
    [StringLength(SupplierBUConsts.FASCMSectionCodeMaxLength)]
    public string? FASCMSectionCode { get; set; }
    [StringLength(SupplierBUConsts.FASCMPaymentTermMaxLength)]
    public string? FASCMPaymentTerm { get; set; }
    [StringLength(SupplierBUConsts.FASCMFreightMethodMaxLength)]
    public string? FASCMFreightMethod { get; set; }
    [StringLength(SupplierBUConsts.FASCMDeliveryTermsMaxLength)]
    public string? FASCMDeliveryTerms { get; set; }
    [StringLength(SupplierBUConsts.FASCMPlaceOfDeliveryTermsMaxLength)]
    public string? FASCMPlaceOfDeliveryTerms { get; set; }
    [StringLength(SupplierBUConsts.FASCMShippingMarkCodeMaxLength)]
    public string? FASCMShippingMarkCode { get; set; }
    public bool? IsDeactive { get; set; } = false;

    public string ConcurrencyStamp { get; set; } = null!;
}
