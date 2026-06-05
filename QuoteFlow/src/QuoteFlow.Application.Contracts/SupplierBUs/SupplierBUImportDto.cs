using System;

namespace QuoteFlow.SupplierBUs;
public class SupplierBUImportDto
{
    public int? No { get; set; } // A.
    public string? SupplierBU { get; set; } // B
    public string? SupplierBURemarks { get; set; } // C
    public string? OrderMethod { get; set; } //D
    public string? POTemplate { get; set; } //E
    public string? Contact { get; set; } //F
    public string? Email { get; set; } //G
    public string? IncoTerm { get; set; } //H
    public string? PaymentTermCode { get; set; } // I
    public string? PaymentDescription { get; set; } // J
    public string? Currency { get; set; } // K
    public string? MaterialType { get; set; } // L
    public string? SAPCode { get; set; } // M
    public Guid? SupplierID { get; set; }
    public string? SupplierCode { get; set; } // N
    public string? SupplierAddress { get; set; } // O
    public string? FASCMVendorCode { get; set; } // P
    public string? FASCMBuyerCode { get; set; } // Q
    public string? FASCMConsigneeCode { get; set; } // R
    public string? FASCMSectionCode { get; set; } // S
    public string? FASCMPaymentTerm { get; set; } // T
    public string? FASCMFreightMethod { get; set; } // U
    public string? FASCMDeliveryTerms { get; set; } // V
    public string? FASCMPlaceOfDeliveryTerms { get; set; } // W
    public string? FASCMShippingMarkCode { get; set; } // X
    public bool? IsUpdate { get; set; } = false;
    public Guid? IdUpdate { get; set; } = null;
    public string? ConcurrencyStamp { get; set; } = null;
}

