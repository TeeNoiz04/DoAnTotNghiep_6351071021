using QuoteFlow.Materials.MaterialApprovalRequestDetails;
using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.Materials.ParameterObjects;
public class MaterialCreateParams
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
    [StringLength(MaterialConsts.ReferenceLeadTimeMaxLength)]
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
    //[StringLength(MaterialConsts.NoteMaxLength)]
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

    public MaterialCreateParams()
    {

    }
    public MaterialCreateParams(MaterialApprovalRequestDetail createParams)
    {
        GolfaCode = createParams.GolfaCode;
        Model = createParams.Model;
        ValidFrom = createParams.ValidFrom;
        ValidTo = createParams.ValidTo;
        SAP_Code = createParams.SAP_Code;
        Spec1 = createParams.Spec1;
        Spec2 = createParams.Spec2;
        Spec3 = createParams.Spec3;
        Spec4 = createParams.Spec4;
        Description_EN = createParams.Description_EN;
        Description_VN = createParams.Description_VN;
        MaterialType = createParams.MaterialType;
        Unit = createParams.Unit;
        Material_SEC_Classification = createParams.Material_SEC_Classification;
        Material_Group = createParams.Material_Group;
        SAPMatGroup = createParams.SAPMatGroup;
        Product_Hierarchy = createParams.Product_Hierarchy;
        ProductHierarchyDescription = createParams.ProductHierarchyDescription;
        CountryOfOrigin = createParams.CountryOfOrigin;
        ReferenceLeadTime = createParams.ReferenceLeadTime;
        WarrantyTime = createParams.WarrantyTime;
        InventoryCategory = createParams.InventoryCategory;
        Maxlot = createParams.Maxlot;
        StockWarning = createParams.StockWarning;
        VAT = createParams.VAT;
        HS_Code = createParams.HS_Code;
        SupplierBUId = createParams.SupplierBUId;
        SupplierBUCode = createParams.SupplierBUCode;
        Factory_Text = createParams.Factory_Text;
        Input_Price = createParams.Input_Price;
        InputCurrency = createParams.InputCurrency;
        INCOTERMS = createParams.INCOTERMS;
        EPA = createParams.EPA!.Value;
        ImportDuty = createParams.ImportDuty;
        AppliedExchangeRate = createParams.AppliedExchangeRate;
        LandedCost = createParams.LandedCost;
        MaxSalesOfferPrice = createParams.MaxSalesOfferPrice;
        MaxMangerOfferPrice = createParams.MaxMangerOfferPrice;
        Standard_Price = createParams.Standard_Price!.Value;
        SellingPrice1 = createParams.SellingPrice1;
        SellingPrice2 = createParams.SellingPrice2;
        SellingPrice3 = createParams.SellingPrice3;
        SellingPrice4 = createParams.SellingPrice4;
        SellingPrice5 = createParams.SellingPrice5;
        MaterialStatus = createParams.MaterialStatus;
        //DestinationDate = createParams.DestinationDate;
        RegistrationDate = createParams.RegistrationDate;
        //IndeactiveDate = createParams.IndeactiveDate;
        //Description_Group = createParams.Description_Group;
        //Origin = createParams.Origin;
        //Kind = createParams.Kind;
        //Factory = createParams.Factory;
        //Vendor = createParams.Vendor;
        //LeadTime = createParams.LeadTime;
        //RefExchangeRate = createParams.RefExchangeRate;
        Note = createParams.Note;
        //Source = createParams.Source;
        //Reason = createParams.Reason;
        //FinalDPOAcceptanceDate = createParams.FinalDPOAcceptanceDate;
        SupplierCode = createParams.SupplierCode;
        MaterialClass = createParams.MaterialClass;
        CargoNote = createParams.CargoNote;
        Weight = createParams.Weight;
        Size = createParams.Size;
        QRCode = createParams.QRCode;
    }
}
