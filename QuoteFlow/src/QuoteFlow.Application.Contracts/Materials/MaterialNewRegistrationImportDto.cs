using System;

namespace QuoteFlow.Materials;
public class MaterialNewRegistrationImportDto
{
    public string? MaterialCode { get; set; } //D*
    public string? ModelName { get; set; } //E*
    public DateTime? RegistrationDate { get; set; } //A
    public DateTime? ValidFrom { get; set; } //B*
    public DateTime? ValidTo { get; set; } //C*
    public string? Spec1 { get; set; } //F
    public string? Spec2 { get; set; } //G
    public string? Spec3 { get; set; } //H
    public string? Spec4 { get; set; } //I
    public string? DescriptionEN { get; set; } //J*
    public string? DescriptionVN { get; set; } //K
    public string? MaterialType { get; set; } //L*
    public string? Unit { get; set; } //M*
    public string? MaterialClass { get; set; } //N*
    public string? MaterialSECClassification { get; set; } //O*
    public string? MaterialGroup { get; set; } //P*
    public string? ProductHierarchy { get; set; } //Q
    public string? CountryOfOrigin { get; set; } //R
    public int? ReferenceLeadTime { get; set; } //S
    public int WarrantyTime { get; set; } //T*
    public string? Weight { get; set; } //U
    public string? Size { get; set; } //V
    public string? QRCode { get; set; } //W
    public int? StockWarning { get; set; } //X
    public decimal? VAT { get; set; } //Y
    public string? Supplier { get; set; } //Z*
    public Guid? SupplierId { get; set; }
    public string? SupplierBU { get; set; } //AA*
    public Guid? SupplierBUId { get; set; }
    public string? Factory { get; set; } //AB*
    public decimal InputPrice { get; set; } //AC*
    public string? InputCurrency { get; set; } //AD*
    public Guid? InputCurrencyId { get; set; }
    public string? Incoterms { get; set; } //AE*
    public bool EPA { get; set; } //AF*
    public decimal? ImportDuty { get; set; } //AG*
    public decimal ExchangeRate { get; set; } //AH*
    public decimal LandedCost { get; set; } //AI*
    public decimal MaxSaleOfferPrice { get; set; } //AJ*
    public decimal MaxManagerOfferPrice { get; set; } //AK*
    public decimal StandardPrice { get; set; } //AL*
    public decimal? SellingPrice1 { get; set; } //AM
    public decimal? SellingPrice2 { get; set; } //AN
    public decimal? SellingPrice3 { get; set; } //AO
    public decimal? SellingPrice4 { get; set; } //AP
    public decimal? SellingPrice5 { get; set; } //AQ
}
