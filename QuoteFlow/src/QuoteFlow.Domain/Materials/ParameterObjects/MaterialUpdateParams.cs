using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Materials.ParameterObjects;
public class MaterialUpdateParams : IHasConcurrencyStamp
{
    [Required]
    [StringLength(MaterialConsts.GolfaCodeMaxLength)]
    public string GolfaCode { get; set; } = null!;
    [Required]
    [StringLength(MaterialConsts.ModelMaxLength)]
    public string Model { get; set; } = null!;
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    [StringLength(MaterialConsts.SAP_CodeMaxLength)]
    public string? SAP_Code { get; set; }
    [StringLength(MaterialConsts.Spec1MaxLength)]
    public string? Spec1 { get; set; }
    [StringLength(MaterialConsts.Spec2MaxLength)]
    public string? Spec2 { get; set; }
    [StringLength(MaterialConsts.Spec3MaxLength)]
    public string? Spec3 { get; set; }
    [StringLength(MaterialConsts.Spec4MaxLength)]
    public string? Spec4 { get; set; }
    [StringLength(MaterialConsts.Description_ENMaxLength)]
    public string? Description_EN { get; set; }
    [StringLength(MaterialConsts.Description_VNMaxLength)]
    public string? Description_VN { get; set; }
    [StringLength(MaterialConsts.MaterialTypeMaxLength)]
    public string? MaterialType { get; set; }
    [StringLength(MaterialConsts.UnitMaxLength)]
    public string? Unit { get; set; }
    [StringLength(MaterialConsts.Material_SEC_ClassificationMaxLength)]
    public string? Material_SEC_Classification { get; set; }
    public string? Material_Group { get; set; }
    [StringLength(MaterialConsts.SAPMatGroupMaxLength)]
    public string? SAPMatGroup { get; set; }
    [StringLength(MaterialConsts.Product_HierarchyMaxLength)]
    public string? Product_Hierarchy { get; set; }
    [StringLength(MaterialConsts.ProductHierarchyDescriptionMaxLength)]
    public string? ProductHierarchyDescription { get; set; }
    [StringLength(MaterialConsts.CountryOfOriginMaxLength)]
    public string? CountryOfOrigin { get; set; }
    public int? ReferenceLeadTime { get; set; }
    public int? WarrantyTime { get; set; }
    [StringLength(MaterialConsts.InventoryCategoryMaxLength)]
    public string? InventoryCategory { get; set; }
    public int? Maxlot { get; set; }
    public int? StockWarning { get; set; }
    public decimal? VAT { get; set; }
    [StringLength(MaterialConsts.HS_CodeMaxLength)]
    public string? HS_Code { get; set; }
    public Guid? SupplierBUId { get; set; }

    public string? SupplierBUCode { get; set; }

    public string? Factory_Text { get; set; }
    public decimal? Input_Price { get; set; }
    [StringLength(MaterialConsts.InputCurrencyMaxLength)]
    public string? InputCurrency { get; set; }
    [StringLength(MaterialConsts.INCOTERMSMaxLength)]
    public string? INCOTERMS { get; set; }
    [Required]
    public bool EPA { get; set; }
    public decimal? ImportDuty { get; set; }
    public decimal? AppliedExchangeRate { get; set; }
    public decimal? LandedCost { get; set; }
    public decimal? MaxSalesOfferPrice { get; set; }
    public decimal? MaxMangerOfferPrice { get; set; }
    [Required]
    public decimal Standard_Price { get; set; }
    public decimal? SellingPrice1 { get; set; }
    public decimal? SellingPrice2 { get; set; }
    public decimal? SellingPrice3 { get; set; }
    public decimal? SellingPrice4 { get; set; }
    public decimal? SellingPrice5 { get; set; }
    [Required]
    [StringLength(MaterialConsts.MaterialStatusMaxLength)]
    public string MaterialStatus { get; set; } = null!;
    //public DateTime? DestinationDate { get; set; }
    public DateTime? RegistrationDate { get; set; }
    //public DateTime? IndeactiveDate { get; set; }
    //[StringLength(MaterialConsts.Description_GroupMaxLength)]
    //public string? Description_Group { get; set; }
    //[StringLength(MaterialConsts.OriginMaxLength)]
    //public string? Origin { get; set; }
    //[StringLength(MaterialConsts.KindMaxLength)]
    //public string? Kind { get; set; }
    //public Guid? Factory { get; set; }
    //public Guid? Vendor { get; set; }
    //public int? LeadTime { get; set; }
    //public decimal? RefExchangeRate { get; set; }
    [StringLength(MaterialConsts.NoteMaxLength)]
    public string? Note { get; set; }
    //[StringLength(MaterialConsts.SourceMaxLength)]
    //public virtual string? Source { get; set; }
    //[StringLength(MaterialConsts.ReasonMaxLength)]
    //public virtual string? Reason { get; set; }
    //public virtual DateTime? FinalDPOAcceptanceDate { get; set; }
    [StringLength(MaterialConsts.SupplierCodeMaxLength)]
    public virtual string? SupplierCode { get; set; }
    [StringLength(MaterialConsts.MaterialClassMaxLength)]
    public virtual string? MaterialClass { get; set; }
    [StringLength(MaterialConsts.CargoNoteMaxLength)]
    public virtual string? CargoNote { get; set; }
    [StringLength(MaterialConsts.WeightMaxLength)]
    public virtual string? Weight { get; set; }
    [StringLength(MaterialConsts.SizeMaxLength)]
    public virtual string? Size { get; set; }
    [StringLength(MaterialConsts.QRCodeMaxLength)]
    public virtual string? QRCode { get; set; }
    public string ConcurrencyStamp { get; set; } = null!;
}
