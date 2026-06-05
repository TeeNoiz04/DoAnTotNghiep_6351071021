using System;

namespace QuoteFlow.Shared;

public class SupplierPOLookupDto
{
    public string? SupplierCode { get; set; }
    public Guid? SupplierId { get; set; }
    public string? SupplierBUCode { get; set; }
    public Guid? SupplierBUId { get; set; }
}
