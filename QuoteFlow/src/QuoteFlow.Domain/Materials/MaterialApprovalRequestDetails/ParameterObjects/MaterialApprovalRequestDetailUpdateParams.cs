using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Materials.MaterialApprovalRequestDetails.ParameterObjects;
public class MaterialApprovalRequestDetailUpdateParams : IHasConcurrencyStamp
{
    [Required]
    public Guid MaterialApprovalId { get; set; }
    [Required]
    [StringLength(MaterialApprovalRequestDetailConsts.GolfaCodeMaxLength)]
    public string GolfaCode { get; set; } = null!;
    [Required]
    [StringLength(MaterialApprovalRequestDetailConsts.ModelMaxLength)]
    public string Model { get; set; } = null!;
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    [StringLength(MaterialApprovalRequestDetailConsts.SAP_CodeMaxLength)]
    public string? SAP_Code { get; set; }
    [StringLength(MaterialApprovalRequestDetailConsts.Spec1MaxLength)]
    public string? Spec1 { get; set; }
    [StringLength(MaterialApprovalRequestDetailConsts.Spec2MaxLength)]
    public string? Spec2 { get; set; }
    [StringLength(MaterialApprovalRequestDetailConsts.Spec3MaxLength)]
    public string? Spec3 { get; set; }
    [StringLength(MaterialApprovalRequestDetailConsts.Spec4MaxLength)]
    public string? Spec4 { get; set; }
    [StringLength(MaterialApprovalRequestDetailConsts.Description_ENMaxLength)]
    public string? Description_EN { get; set; }
    [StringLength(MaterialApprovalRequestDetailConsts.Description_VNMaxLength)]
    public string? Description_VN { get; set; }
    [StringLength(MaterialApprovalRequestDetailConsts.MaterialTypeMaxLength)]
    public string? MaterialType { get; set; }
    [StringLength(MaterialApprovalRequestDetailConsts.UnitMaxLength)]
    public string? Unit { get; set; }
    [StringLength(MaterialApprovalRequestDetailConsts.Material_SEC_ClassificationMaxLength)]
    public string? Material_SEC_Classification { get; set; }
    public string? Material_Group { get; set; }
    [StringLength(MaterialApprovalRequestDetailConsts.SAPMatGroupMaxLength)]
    public string? SAPMatGroup { get; set; }
    [StringLength(MaterialApprovalRequestDetailConsts.Product_HierarchyMaxLength)]
    public string? Product_Hierarchy { get; set; }
    [StringLength(MaterialApprovalRequestDetailConsts.ProductHierarchyDescriptionMaxLength)]
    public string? ProductHierarchyDescription { get; set; }
    [StringLength(MaterialApprovalRequestDetailConsts.CountryOfOriginMaxLength)]
    public string? CountryOfOrigin { get; set; }
    public int? ReferenceLeadTime { get; set; }
    public int? WarrantyTime { get; set; }
    [StringLength(MaterialApprovalRequestDetailConsts.InventoryCategoryMaxLength)]
    public string? InventoryCategory { get; set; }
    public int? Maxlot { get; set; }
    public int? StockWarning { get; set; }
    public decimal? VAT { get; set; }
    [StringLength(MaterialApprovalRequestDetailConsts.HS_CodeMaxLength)]
    public string? HS_Code { get; set; }
    public Guid? SupplierBUId { get; set; }
    [StringLength(MaterialApprovalRequestDetailConsts.SupplierBUCodeMaxLength)]
    public string? SupplierBUCode { get; set; }
    [StringLength(MaterialApprovalRequestDetailConsts.Factory_TextMaxLength)]
    public string? Factory_Text { get; set; }
    public decimal? Input_Price { get; set; }
    [StringLength(MaterialApprovalRequestDetailConsts.InputCurrencyMaxLength)]
    public string? InputCurrency { get; set; }
    [StringLength(MaterialApprovalRequestDetailConsts.INCOTERMSMaxLength)]
    public string? INCOTERMS { get; set; }
    public bool? EPA { get; set; }
    public decimal? ImportDuty { get; set; }
    public decimal? AppliedExchangeRate { get; set; }
    public decimal? LandedCost { get; set; }
    public decimal? MaxSalesOfferPrice { get; set; }
    public decimal? MaxMangerOfferPrice { get; set; }
    public decimal? Standard_Price { get; set; }
    public decimal? SellingPrice1 { get; set; }
    public decimal? SellingPrice2 { get; set; }
    public decimal? SellingPrice3 { get; set; }
    public decimal? SellingPrice4 { get; set; }
    public decimal? SellingPrice5 { get; set; }
    //[Required]
    [StringLength(MaterialApprovalRequestDetailConsts.MaterialStatusMaxLength)]
    public string? MaterialStatus { get; set; }
    //public DateTime? DestinationDate { get; set; }
    public DateTime? RegistrationDate { get; set; }
    //public DateTime? IndeactiveDate { get; set; }
    [StringLength(MaterialApprovalRequestDetailConsts.Description_GroupMaxLength)]
    public string? Description_Group { get; set; }
    //[StringLength(MaterialApprovalRequestDetailConsts.OriginMaxLength)]
    //public string? Origin { get; set; }
    //[StringLength(MaterialApprovalRequestDetailConsts.KindMaxLength)]
    //public string? Kind { get; set; }
    //public Guid? Factory { get; set; }
    //public Guid? Vendor { get; set; }
    //public int? LeadTime { get; set; }
    //public decimal? RefExchangeRate { get; set; }
    [StringLength(MaterialApprovalRequestDetailConsts.NoteMaxLength)]
    public string? Note { get; set; }
    [StringLength(MaterialApprovalRequestDetailConsts.SourceMaxLength)]
    public virtual string? Source { get; set; }
    [StringLength(MaterialApprovalRequestDetailConsts.ReasonMaxLength)]
    public virtual string? Reason { get; set; }
    public virtual DateTime? FinalDPOAcceptanceDate { get; set; }
    [StringLength(MaterialApprovalRequestDetailConsts.SupplierCodeMaxLength)]
    public virtual string? SupplierCode { get; set; }
    [StringLength(MaterialApprovalRequestDetailConsts.MaterialClassMaxLength)]
    public virtual string? MaterialClass { get; set; }
    public virtual DateTime? ActionDate { get; set; }
    public virtual string? Action { get; set; }
    public virtual string? FactoryRefDoc { get; set; }
    [StringLength(MaterialApprovalRequestDetailConsts.CargoNoteMaxLength)]
    public virtual string? CargoNote { get; set; }
    [StringLength(MaterialApprovalRequestDetailConsts.WeightMaxLength)]
    public virtual string? Weight { get; set; }
    [StringLength(MaterialApprovalRequestDetailConsts.SizeMaxLength)]
    public virtual string? Size { get; set; }
    [StringLength(MaterialApprovalRequestDetailConsts.QRCodeMaxLength)]
    public virtual string? QRCode { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}
