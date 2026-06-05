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
    public string? SAPGroup { get; set; } //R
    public string? ProductHierarchy_Description { get; set; } //S
    public string? CountryOfOrigin { get; set; } //T
    public int? ReferenceLeadTime { get; set; } //U
    public int? WarrantyTime { get; set; } //V
    public string? InventoryCategory { get; set; } //W
    public string? CargoNote { get; set; } //x
    public string? Weight { get; set; } //Y
    public string? Size { get; set; } //Z
    public string? QRCode { get; set; } //AA
    public int? MaxLot { get; set; } //AB
    public int? StockWarning { get; set; } //AC
    public int? StockQty { get; set; } //AD
    public string? HSCode { get; set; } //AE
    public string? ConcurrencyStamp { get; set; }
}
