using System;

namespace QuoteFlow.Materials;
public class MaterialUpdateWithoutPriceImportDto
{
    public Guid? Id { get; set; }
    public string? MaterialCode { get; set; } //A*
    public string? ModelName { get; set; } //B*
    public DateTime? RegistrationDate { get; set; } //C
    public DateTime? ValidFrom { get; set; } //D
    public DateTime? ValidTo { get; set; } //E
    public string? Spec1 { get; set; } //F
    public string? Spec2 { get; set; } //G
    public string? Spec3 { get; set; } //H
    public string? Spec4 { get; set; } //I
    public string? DescriptionEN { get; set; } //J
    public string? DescriptionVN { get; set; } //K
    public string? Supplier { get; set; } //L
    public string? SupplierBU { get; set; } //M
    public Guid? SupplierBUId { get; set; }
    public string? Factory { get; set; } //N
    public string? MaterialType { get; set; } //O
    public string? Unit { get; set; } //P
    public string? MaterialGroup { get; set; } //Q
    //public Guid? MaterialGroupId { get; set; }
    public string? CountryOfOrigin { get; set; } //R
    public int? ReferenceLeadTime { get; set; } //S
    public int? WarrantyTime { get; set; } //T
    public string? Weight { get; set; } //U
    public string? Size { get; set; } //V
    public string? QRCode { get; set; } //W
    public int? StockWarning { get; set; } //X
    public int? StockQty { get; set; } //Y
    public string? ConcurrencyStamp { get; set; }
}
