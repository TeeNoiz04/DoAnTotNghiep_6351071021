using System;

namespace QuoteFlow.Materials;
public class MaterialNewRegistrationImportDto
{
    public string? MaterialCode { get; set; } //A* -> D
    public string? ModelName { get; set; } //B* ->E
    public DateTime? RegistrationDate { get; set; } //C -> A
    public DateTime? ValidFrom { get; set; } //D* -> B
    public DateTime? ValidTo { get; set; } //E* -> C
    public string? SAPCode { get; set; } //F
    public string? Spec1 { get; set; } //G
    public string? Spec2 { get; set; } //H
    public string? Spec3 { get; set; } //I
    public string? Spec4 { get; set; } //J
    public string? DescriptionEN { get; set; } //K*
    public string? DescriptionVN { get; set; } //L
    public string? MaterialType { get; set; } //M*
    //public Guid? MaterialTypeId { get; set; }
    //public string MaterialTypeCode { get; set; } = null!;
    public string? Unit { get; set; } //N*
    public string? MaterialClass { get; set; } //O*
    public string? MaterialSECClassification { get; set; } //P*
    public string? MaterialGroup { get; set; } //Q*
    //public Guid? MaterialGroupId { get; set; }
    public string? SAPGroup { get; set; } //R
    public string? ProductHierarchy { get; set; } //S
    public string? ProductHierarchy_Description { get; set; } //T
    public string? CountryOfOrigin { get; set; } //U
    public int? ReferenceLeadTime { get; set; } //V
    public int WarrantyTime { get; set; } //W*
    public string? InventoryCategory { get; set; } //X
    public string? CargoNote { get; set; } //Y
    public string? Weight { get; set; } //Z
    public string? Size { get; set; } //AA
    public string? QRCode { get; set; } //AB
    public int? MaxLot { get; set; } //AC
    public int? StockWarning { get; set; } //AD
    public decimal? VAT { get; set; } //AE
    public string? HSCode { get; set; } //AF
    public string? Supplier { get; set; } //AG*
    public Guid? SupplierId { get; set; }
    public string? SupplierBU { get; set; } //AH*
    public Guid? SupplierBUId { get; set; }
    public string? Factory { get; set; } //AI*
    //public Guid? FactoryId { get; set; }
    public decimal InputPrice { get; set; } //AJ*
    public string? InputCurrency { get; set; }//AK*
    public Guid? InputCurrencyId { get; set; }
    public string? Incoterms { get; set; } //AL*
    public bool EPA { get; set; } //AM*
    public decimal? ImportDuty { get; set; } //AN*
    public decimal ExchangeRate { get; set; } //AO*
    public decimal LandedCost { get; set; } //AP*
    public decimal MaxSaleOfferPrice { get; set; } //AQ*
    public decimal MaxManagerOfferPrice { get; set; } //AR*
    public decimal StandardPrice { get; set; } //AS*
    public decimal? SellingPrice1 { get; set; } //AT
    public decimal? SellingPrice2 { get; set; } //AU
    public decimal? SellingPrice3 { get; set; } //AV
    public decimal? SellingPrice4 { get; set; } //AW
    public decimal? SellingPrice5 { get; set; } //AX

}
