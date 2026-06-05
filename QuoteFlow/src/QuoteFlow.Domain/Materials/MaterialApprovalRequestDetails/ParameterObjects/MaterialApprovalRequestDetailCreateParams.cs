using QuoteFlow.Materials.ParameterObjects;
using QuoteFlow.Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.Materials.MaterialApprovalRequestDetails.ParameterObjects;
public class MaterialApprovalRequestDetailCreateParams
{
    [Required]
    public Guid MaterialApprovalId { get; set; }
    [Required]
    [StringLength(MaterialApprovalRequestDetailConsts.GolfaCodeMaxLength)]
    public string GolfaCode { get; set; } = null!;
    [Required]
    [StringLength(MaterialApprovalRequestDetailConsts.ModelMaxLength)]
    public string Model { get; set; } = null!;
    public DateTime? RegistrationDate { get; set; }
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
    [StringLength(MaterialApprovalRequestDetailConsts.ReferenceLeadTimeMaxLength)]
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
    //[Required]
    public bool? EPA { get; set; }
    public decimal? ImportDuty { get; set; }
    public decimal? AppliedExchangeRate { get; set; }
    public decimal? LandedCost { get; set; }
    public decimal? MaxSalesOfferPrice { get; set; }
    public decimal? MaxMangerOfferPrice { get; set; }
    //[Required]
    public decimal? Standard_Price { get; set; }
    public decimal? SellingPrice1 { get; set; }
    public decimal? SellingPrice2 { get; set; }
    public decimal? SellingPrice3 { get; set; }
    public decimal? SellingPrice4 { get; set; }
    public decimal? SellingPrice5 { get; set; }

    [StringLength(MaterialApprovalRequestDetailConsts.MaterialStatusMaxLength)]
    public string MaterialStatus { get; set; }
    //public DateTime? DestinationDate { get; set; }

    //public DateTime? IndeactiveDate { get; set; }
    //[StringLength(MaterialApprovalRequestDetailConsts.Description_GroupMaxLength)]
    //public string? Description_Group { get; set; }
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
    public MaterialApprovalRequestDetailCreateParams()
    {

    }

    public MaterialApprovalRequestDetailCreateParams(Material material, Guid materialRequestId)
    {
        MaterialApprovalId = materialRequestId;
        GolfaCode = material.GolfaCode;
        Model = material.Model;
        ValidFrom = material.ValidFrom;
        ValidTo = material.ValidTo;
        SAP_Code = material.SAP_Code;
        Spec1 = material.Spec1;
        Spec2 = material.Spec2;
        Spec3 = material.Spec3;
        Spec4 = material.Spec4;
        Description_EN = material.Description_EN;
        Description_VN = material.Description_VN;
        MaterialType = material.MaterialType;
        Unit = material.Unit;
        Material_SEC_Classification = material.Material_SEC_Classification;
        Material_Group = material.Material_Group;
        SAPMatGroup = material.SAPMatGroup;
        Product_Hierarchy = material.Product_Hierarchy;
        ProductHierarchyDescription = material.ProductHierarchyDescription;
        CountryOfOrigin = material.CountryOfOrigin;
        ReferenceLeadTime = material.ReferenceLeadTime;
        WarrantyTime = material.WarrantyTime;
        InventoryCategory = material.InventoryCategory;
        Maxlot = material.Maxlot;
        StockWarning = material.StockWarning;
        VAT = material.VAT;
        HS_Code = material.HS_Code;
        SupplierBUId = material.SupplierBUId;
        SupplierBUCode = material.SupplierBUCode;
        Factory_Text = material.Factory_Text;
        Input_Price = material.Input_Price;
        InputCurrency = material.InputCurrency;
        INCOTERMS = material.INCOTERMS;
        EPA = material.EPA;
        ImportDuty = material.ImportDuty;
        AppliedExchangeRate = material.AppliedExchangeRate;
        LandedCost = material.LandedCost;
        MaxSalesOfferPrice = material.MaxSalesOfferPrice;
        MaxMangerOfferPrice = material.MaxMangerOfferPrice;
        Standard_Price = material.Standard_Price;
        SellingPrice1 = material.SellingPrice1;
        SellingPrice2 = material.SellingPrice2;
        SellingPrice3 = material.SellingPrice3;
        SellingPrice4 = material.SellingPrice4;
        SellingPrice5 = material.SellingPrice5;
        MaterialStatus = material.MaterialStatus;
        RegistrationDate = material.RegistrationDate;
        Note = material.Note;
        SupplierCode = material.SupplierCode;
        MaterialClass = material.MaterialClass;
        CargoNote = material.CargoNote;
        Weight = material.Weight;
        Size = material.Size;
        QRCode = material.QRCode;
    }

    public MaterialApprovalRequestDetailCreateParams(Material material, ExcelMaterialUpdatePriceParams update, Guid materialRequestId)
    {
        MaterialApprovalId = materialRequestId;
        GolfaCode = string.IsNullOrWhiteSpace(update.GolfaCode) ? material.GolfaCode : update.GolfaCode;
        Model = string.IsNullOrWhiteSpace(update.Model) ? material.Model : update.Model;

        ValidFrom = update.ValidFrom ?? material.ValidFrom;
        ValidTo = update.ValidTo ?? material.ValidTo;

        SAP_Code = material.SAP_Code;
        MaterialStatus = QuoteFlowStatuses.InProgress;
        Spec1 = update.Spec1 ?? material.Spec1;
        Spec2 = material.Spec2;
        Spec3 = material.Spec3;
        Spec4 = material.Spec4;

        Description_EN = material.Description_EN;
        Description_VN = material.Description_VN;

        MaterialType = update.MaterialType ?? material.MaterialType;
        Unit = material.Unit;
        Material_SEC_Classification = material.Material_SEC_Classification;

        Material_Group = update.MaterialGroup ?? material.Material_Group;
        SAPMatGroup = material.SAPMatGroup;

        Product_Hierarchy = material.Product_Hierarchy;
        ProductHierarchyDescription = material.ProductHierarchyDescription;
        CountryOfOrigin = material.CountryOfOrigin;
        ReferenceLeadTime = material.ReferenceLeadTime;
        WarrantyTime = material.WarrantyTime;
        InventoryCategory = material.InventoryCategory;
        Maxlot = material.Maxlot;
        StockWarning = material.StockWarning;
        VAT = material.VAT;
        HS_Code = material.HS_Code;
        SupplierBUId = material.SupplierBUId;
        SupplierBUCode = material.SupplierBUCode;
        Factory_Text = material.Factory_Text;

        Input_Price = update.Input_Price ?? material.Input_Price;
        InputCurrency = string.IsNullOrWhiteSpace(update.InputCurrency) ? material.InputCurrency : update.InputCurrency;
        INCOTERMS = string.IsNullOrWhiteSpace(update.INCOTERMS) ? material.INCOTERMS : update.INCOTERMS;

        EPA = update.EPA ?? material.EPA;
        ImportDuty = update.ImportDuty ?? material.ImportDuty;
        AppliedExchangeRate = update.AppliedExchangeRate ?? material.AppliedExchangeRate;
        LandedCost = update.LandedCost ?? material.LandedCost;

        MaxSalesOfferPrice = update.MaxSalesOfferPrice ?? material.MaxSalesOfferPrice;
        MaxMangerOfferPrice = update.MaxMangerOfferPrice ?? material.MaxMangerOfferPrice;

        Standard_Price = update.Standard_Price ?? material.Standard_Price;

        SellingPrice1 = update.SellingPrice1 ?? material.SellingPrice1;
        SellingPrice2 = update.SellingPrice2 ?? material.SellingPrice2;
        SellingPrice3 = update.SellingPrice3 ?? material.SellingPrice3;
        SellingPrice4 = update.SellingPrice4 ?? material.SellingPrice4;
        SellingPrice5 = update.SellingPrice5 ?? material.SellingPrice5;

        //MaterialStatus = material.MaterialStatus;
        RegistrationDate = material.RegistrationDate;
        Note = material.Note;
        SupplierCode = material.SupplierCode;
        MaterialClass = material.MaterialClass;

        CargoNote = material.CargoNote;
        Weight = material.Weight;
        Size = material.Size;
        QRCode = material.QRCode;
    }

    public MaterialApprovalRequestDetailCreateParams(ExcelMaterialUpdatePriceParams update, Guid materialRequestId)
    {
        MaterialApprovalId = materialRequestId;
        GolfaCode = update.GolfaCode;
        Model = update.Model;

        ValidFrom = update.ValidFrom;
        ValidTo = update.ValidTo;

        MaterialStatus = QuoteFlowStatuses.InProgress;
        Spec1 = update.Spec1;


        MaterialType = update.MaterialType;

        Material_Group = update.MaterialGroup;


        Input_Price = update.Input_Price;
        InputCurrency = update.InputCurrency;
        INCOTERMS = update.INCOTERMS;

        EPA = update.EPA;
        ImportDuty = update.ImportDuty;
        AppliedExchangeRate = update.AppliedExchangeRate;
        LandedCost = update.LandedCost;

        MaxSalesOfferPrice = update.MaxSalesOfferPrice;
        MaxMangerOfferPrice = update.MaxMangerOfferPrice;

        Standard_Price = update.Standard_Price;

        SellingPrice1 = update.SellingPrice1;
        SellingPrice2 = update.SellingPrice2;
        SellingPrice3 = update.SellingPrice3;
        SellingPrice4 = update.SellingPrice4;
        SellingPrice5 = update.SellingPrice5;

        //MaterialStatus = material.MaterialStatus;

    }

    //ExcelMaterialUpdateWithoutPrriceParams
    public MaterialApprovalRequestDetailCreateParams(
    ExcelMaterialUpdateWithoutPrriceParams update,
    Guid materialRequestId)
    {
        MaterialApprovalId = materialRequestId;

        GolfaCode = update.GolfaCode ?? string.Empty;
        Model = update.Model ?? string.Empty;
        RegistrationDate = update.RegistrationDate;
        ValidFrom = update.ValidFrom;
        ValidTo = update.ValidTo;

        Spec1 = update.Spec1;
        Spec2 = update.Spec2;
        Spec3 = update.Spec3;
        Spec4 = update.Spec4;

        Description_EN = update.Description_EN;
        Description_VN = update.Description_VN;

        SupplierCode = update.SupplierCode;
        SupplierBUCode = update.SupplierBUCode;
        SupplierBUId = update.SupplierBUId;
        Factory_Text = update.Factory_Text;


        MaterialType = update.MaterialType;
        Unit = update.Unit;
        Material_SEC_Classification = update.Material_SEC_Classification;
        Material_Group = update.Material_Group;
        SAPMatGroup = update.SAPMatGroup;
        ProductHierarchyDescription = update.ProductHierarchyDescription;
        CountryOfOrigin = update.CountryOfOrigin;

        ReferenceLeadTime = update.ReferenceLeadTime;
        WarrantyTime = update.WarrantyTime;
        InventoryCategory = update.InventoryCategory;
        Maxlot = update.Maxlot;
        StockWarning = update.StockWarning;

        HS_Code = update.HS_Code;
        CargoNote = update.CargoNote;
        Weight = update.Weight;
        Size = update.Size;
        QRCode = update.QRCode;

    }




    //update M4U
    public MaterialApprovalRequestDetailCreateParams(Material material, Guid materialRequestId, DateTime? finalDPOAcceptanceDate = null, DateTime? actionDate = null, string? action = null, string source = null!, string reason = null!, string? factoryRefDoc = null)
    {
        MaterialApprovalId = materialRequestId;
        GolfaCode = material.GolfaCode;
        Model = material.Model;
        ValidFrom = material.ValidFrom;
        ValidTo = material.ValidTo;
        SAP_Code = material.SAP_Code;
        Spec1 = material.Spec1;
        Spec2 = material.Spec2;
        Spec3 = material.Spec3;
        Spec4 = material.Spec4;
        Description_EN = material.Description_EN;
        Description_VN = material.Description_VN;
        MaterialType = material.MaterialType;
        Unit = material.Unit;
        Material_SEC_Classification = material.Material_SEC_Classification;
        Material_Group = material.Material_Group;
        SAPMatGroup = material.SAPMatGroup;
        Product_Hierarchy = material.Product_Hierarchy;
        ProductHierarchyDescription = material.ProductHierarchyDescription;
        CountryOfOrigin = material.CountryOfOrigin;
        ReferenceLeadTime = material.ReferenceLeadTime;
        WarrantyTime = material.WarrantyTime;
        InventoryCategory = material.InventoryCategory;
        Maxlot = material.Maxlot;
        StockWarning = material.StockWarning;
        VAT = material.VAT;
        HS_Code = material.HS_Code;
        SupplierBUId = material.SupplierBUId;
        SupplierBUCode = material.SupplierBUCode;
        Factory_Text = material.Factory_Text;
        Input_Price = material.Input_Price;
        InputCurrency = material.InputCurrency;
        INCOTERMS = material.INCOTERMS;
        EPA = material.EPA;
        ImportDuty = material.ImportDuty;
        AppliedExchangeRate = material.AppliedExchangeRate;
        LandedCost = material.LandedCost;
        MaxSalesOfferPrice = material.MaxSalesOfferPrice;
        MaxMangerOfferPrice = material.MaxMangerOfferPrice;
        Standard_Price = material.Standard_Price;
        SellingPrice1 = material.SellingPrice1;
        SellingPrice2 = material.SellingPrice2;
        SellingPrice3 = material.SellingPrice3;
        SellingPrice4 = material.SellingPrice4;
        SellingPrice5 = material.SellingPrice5;
        MaterialStatus = material.MaterialStatus;
        RegistrationDate = material.RegistrationDate;
        Note = material.Note;
        Source = source;
        Reason = reason;
        FinalDPOAcceptanceDate = finalDPOAcceptanceDate;
        SupplierCode = material.SupplierCode;
        MaterialClass = material.MaterialClass;
        ActionDate = actionDate;
        Action = action;
        FactoryRefDoc = factoryRefDoc;

    }

    public MaterialApprovalRequestDetailCreateParams(Guid materialRequestId, string golfaCode, string model, DateTime? finalDPOAcceptanceDate = null, DateTime? actionDate = null, string? action = null, string? source = null, string? reason = null, string? factoryRefDoc = null)
    {
        MaterialApprovalId = materialRequestId;
        GolfaCode = golfaCode;
        Model = model;
        FinalDPOAcceptanceDate = finalDPOAcceptanceDate;
        ActionDate = actionDate;
        Action = action;
        Source = source;
        Reason = reason;
        FactoryRefDoc = factoryRefDoc;
    }
    public MaterialApprovalRequestDetailCreateParams(Guid materialRequestId, string golfaCode, string model, string? inventoryCategory = null, int? stockWarning = null)
    {
        MaterialApprovalId = materialRequestId;
        GolfaCode = golfaCode;
        Model = model;
        InventoryCategory = inventoryCategory;
        StockWarning = stockWarning;
    }

    public MaterialApprovalRequestDetailCreateParams(Guid materialRequestId, string golfaCode, string model, int? referenceLeadTime = null, string? country = null, int? maxlot = null)
    {
        MaterialApprovalId = materialRequestId;
        GolfaCode = golfaCode;
        Model = model;
        ReferenceLeadTime = referenceLeadTime;
        CountryOfOrigin = country;
        Maxlot = maxlot;
    }
    public MaterialApprovalRequestDetailCreateParams(Guid materialRequestId, string golfaCode, string model, string? sapCode = null, string? description = null, string? productHierarchy = null, decimal? vat = null)
    {
        MaterialApprovalId = materialRequestId;
        GolfaCode = golfaCode;
        Model = model;
        SAP_Code = sapCode;
        Description_VN = description;
        Product_Hierarchy = productHierarchy;
        VAT = vat;
    }
}
