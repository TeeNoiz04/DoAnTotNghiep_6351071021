using JetBrains.Annotations;
using QuoteFlow.Shared.Models;
using QuoteFlow.SupplierBUs.ParameterObjects;
using QuoteFlow.Suppliers;
using System;
using Volo.Abp;

namespace QuoteFlow.SupplierBUs;

public class SupplierBU : ExtendedAuditedAggregateRoot<Guid>
{
    [NotNull]
    public virtual string SupplierBUCode { get; set; }

    [CanBeNull]
    public virtual string? SupplierBURemarks { get; set; }

    [CanBeNull]
    public virtual string? OrderMethod { get; set; }

    [CanBeNull]
    public virtual string? POTemplate { get; set; }

    [CanBeNull]
    public virtual string? Contact { get; set; }

    [CanBeNull]
    public virtual string? Email { get; set; }

    [CanBeNull]
    public virtual string? INCOTerm { get; set; }

    [CanBeNull]
    public virtual string? PaymentTermCode { get; set; }

    [CanBeNull]
    public virtual string? PaymentDescription { get; set; }

    [CanBeNull]
    public virtual string? Currency { get; set; }

    [CanBeNull]
    public virtual string? MaterialType { get; set; }

    public virtual Guid? SupplierId { get; set; }

    [CanBeNull]
    public virtual string? SupplierCode { get; set; }

    [CanBeNull]
    public virtual string? SupplierShortName { get; set; }

    [CanBeNull]
    public virtual string? SupplierAddress { get; set; }

    public virtual int SortOrder { get; set; }

    [CanBeNull]
    public virtual string? FASCMVendorCode { get; set; }

    [CanBeNull]
    public virtual string? FASCMBuyerCode { get; set; }

    [CanBeNull]
    public virtual string? FASCMConsigneeCode { get; set; }

    [CanBeNull]
    public virtual string? FASCMSectionCode { get; set; }

    [CanBeNull]
    public virtual string? FASCMPaymentTerm { get; set; }

    [CanBeNull]
    public virtual string? FASCMFreightMethod { get; set; }

    [CanBeNull]
    public virtual string? FASCMDeliveryTerms { get; set; }

    [CanBeNull]
    public virtual string? FASCMPlaceOfDeliveryTerms { get; set; }

    [CanBeNull]
    public virtual string? FASCMShippingMarkCode { get; set; }

    public virtual bool? IsDeactive { get; set; }
    public virtual Supplier? Supplier { get; set; }

    protected SupplierBU()
    {

    }

    public SupplierBU(Guid id, string supplierBUCode, int sortOrder, string? supplierBURemarks = null, string? orderMethod = null, string? pOTemplate = null, string? contact = null, string? email = null, string? iNCOTerm = null, string? paymentTermCode = null, string? paymentDescription = null, string? currency = null, string? materialType = null, Guid? supplierId = null, string? supplierCode = null, string? supplierShortName = null, string? supplierAddress = null, string? fASCMVendorCode = null, string? fASCMBuyerCode = null, string? fASCMConsigneeCode = null, string? fASCMSectionCode = null, string? fASCMPaymentTerm = null, string? fASCMFreightMethod = null, string? fASCMDeliveryTerms = null, string? fASCMPlaceOfDeliveryTerms = null, string? fASCMShippingMarkCode = null, bool? isDeactive = false)
    {

        Id = id;
        Check.NotNull(supplierBUCode, nameof(supplierBUCode));
        Check.Length(supplierBUCode, nameof(supplierBUCode), SupplierBUConsts.SupplierBUCodeMaxLength, 0);
        Check.Length(supplierBURemarks, nameof(supplierBURemarks), SupplierBUConsts.SupplierBURemarksMaxLength, 0);
        Check.Length(orderMethod, nameof(orderMethod), SupplierBUConsts.OrderMethodMaxLength, 0);
        Check.Length(pOTemplate, nameof(pOTemplate), SupplierBUConsts.POTemplateMaxLength, 0);
        Check.Length(contact, nameof(contact), SupplierBUConsts.ContactMaxLength, 0);
        Check.Length(email, nameof(email), SupplierBUConsts.EmailMaxLength, 0);
        Check.Length(iNCOTerm, nameof(iNCOTerm), SupplierBUConsts.INCOTermMaxLength, 0);
        Check.Length(paymentTermCode, nameof(paymentTermCode), SupplierBUConsts.PaymentTermCodeMaxLength, 0);
        Check.Length(paymentDescription, nameof(paymentDescription), SupplierBUConsts.PaymentDescriptionMaxLength, 0);
        Check.Length(currency, nameof(currency), SupplierBUConsts.CurrencyMaxLength, 0);
        Check.Length(materialType, nameof(materialType), SupplierBUConsts.MaterialTypeMaxLength, 0);
        Check.Length(supplierCode, nameof(supplierCode), SupplierBUConsts.SupplierCodeMaxLength, 0);
        Check.Length(supplierShortName, nameof(supplierShortName), SupplierBUConsts.SupplierShortNameMaxLength, 0);
        Check.Length(supplierAddress, nameof(supplierAddress), SupplierBUConsts.SupplierAddressMaxLength, 0);
        Check.Length(fASCMVendorCode, nameof(fASCMVendorCode), SupplierBUConsts.FASCMVendorCodeMaxLength, 0);
        Check.Length(fASCMBuyerCode, nameof(fASCMBuyerCode), SupplierBUConsts.FASCMBuyerCodeMaxLength, 0);
        Check.Length(fASCMConsigneeCode, nameof(fASCMConsigneeCode), SupplierBUConsts.FASCMConsigneeCodeMaxLength, 0);
        Check.Length(fASCMSectionCode, nameof(fASCMSectionCode), SupplierBUConsts.FASCMSectionCodeMaxLength, 0);
        Check.Length(fASCMPaymentTerm, nameof(fASCMPaymentTerm), SupplierBUConsts.FASCMPaymentTermMaxLength, 0);
        Check.Length(fASCMFreightMethod, nameof(fASCMFreightMethod), SupplierBUConsts.FASCMFreightMethodMaxLength, 0);
        Check.Length(fASCMDeliveryTerms, nameof(fASCMDeliveryTerms), SupplierBUConsts.FASCMDeliveryTermsMaxLength, 0);
        Check.Length(fASCMPlaceOfDeliveryTerms, nameof(fASCMPlaceOfDeliveryTerms), SupplierBUConsts.FASCMPlaceOfDeliveryTermsMaxLength, 0);
        Check.Length(fASCMShippingMarkCode, nameof(fASCMShippingMarkCode), SupplierBUConsts.FASCMShippingMarkCodeMaxLength, 0);
        SupplierBUCode = supplierBUCode;
        SortOrder = sortOrder;
        SupplierBURemarks = supplierBURemarks;
        OrderMethod = orderMethod;
        POTemplate = pOTemplate;
        Contact = contact;
        Email = email;
        INCOTerm = iNCOTerm;
        PaymentTermCode = paymentTermCode;
        PaymentDescription = paymentDescription;
        Currency = currency;
        MaterialType = materialType;
        SupplierId = supplierId;
        SupplierCode = supplierCode;
        SupplierShortName = supplierShortName;
        SupplierAddress = supplierAddress;
        FASCMVendorCode = fASCMVendorCode;
        FASCMBuyerCode = fASCMBuyerCode;
        FASCMConsigneeCode = fASCMConsigneeCode;
        FASCMSectionCode = fASCMSectionCode;
        FASCMPaymentTerm = fASCMPaymentTerm;
        FASCMFreightMethod = fASCMFreightMethod;
        FASCMDeliveryTerms = fASCMDeliveryTerms;
        FASCMPlaceOfDeliveryTerms = fASCMPlaceOfDeliveryTerms;
        FASCMShippingMarkCode = fASCMShippingMarkCode;
        IsDeactive = isDeactive ?? false;
    }

    public SupplierBU(Guid id, SupplierBUCreateParams createParams)
    {
        Id = id;

        SupplierBUCode = createParams.SupplierBUCode;
        SortOrder = createParams.SortOrder;
        SupplierBURemarks = createParams.SupplierBURemarks;
        OrderMethod = createParams.OrderMethod;
        POTemplate = createParams.POTemplate;
        Contact = createParams.Contact;
        Email = createParams.Email;
        INCOTerm = createParams.INCOTerm;
        PaymentTermCode = createParams.PaymentTermCode;
        PaymentDescription = createParams.PaymentDescription;
        Currency = createParams.Currency;
        MaterialType = createParams.MaterialType;
        SupplierId = createParams.SupplierId;
        SupplierCode = createParams.SupplierCode;
        SupplierShortName = createParams.SupplierShortName;
        SupplierAddress = createParams.SupplierAddress;
        FASCMVendorCode = createParams.FASCMVendorCode;
        FASCMBuyerCode = createParams.FASCMBuyerCode;
        FASCMConsigneeCode = createParams.FASCMConsigneeCode;
        FASCMSectionCode = createParams.FASCMSectionCode;
        FASCMPaymentTerm = createParams.FASCMPaymentTerm;
        FASCMFreightMethod = createParams.FASCMFreightMethod;
        FASCMDeliveryTerms = createParams.FASCMDeliveryTerms;
        FASCMPlaceOfDeliveryTerms = createParams.FASCMPlaceOfDeliveryTerms;
        FASCMShippingMarkCode = createParams.FASCMShippingMarkCode;
        IsDeactive = createParams.IsDeactive;
    }


}