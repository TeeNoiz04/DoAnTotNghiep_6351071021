using JetBrains.Annotations;
using QuoteFlow.Materials.MaterialApprovalRequestDetails.ParameterObjects;
using QuoteFlow.Shared.Models;
using System;

namespace QuoteFlow.Materials.MaterialApprovalRequestDetails;

public class MaterialApprovalRequestDetail : ExtendedAuditedAggregateRoot<Guid>
{
    public virtual Guid MaterialApprovalId { get; set; }

    [NotNull]
    public virtual string GolfaCode { get; set; }

    [NotNull]
    public virtual string Model { get; set; }
    public virtual DateTime? RegistrationDate { get; set; }

    public virtual DateTime? ValidFrom { get; set; }

    public virtual DateTime? ValidTo { get; set; }

    [CanBeNull]
    public virtual string? SAP_Code { get; set; }

    [CanBeNull]
    public virtual string? Spec1 { get; set; }

    [CanBeNull]
    public virtual string? Spec2 { get; set; }

    [CanBeNull]
    public virtual string? Spec3 { get; set; }

    [CanBeNull]
    public virtual string? Spec4 { get; set; }

    [CanBeNull]
    public virtual string? Description_EN { get; set; }

    [CanBeNull]
    public virtual string? Description_VN { get; set; }

    [CanBeNull]
    public virtual string? MaterialType { get; set; }

    [CanBeNull]
    public virtual string? Unit { get; set; }

    [CanBeNull]
    public virtual string? Material_SEC_Classification { get; set; }

    public virtual string? Material_Group { get; set; }

    [CanBeNull]
    public virtual string? SAPMatGroup { get; set; }

    [CanBeNull]
    public virtual string? Product_Hierarchy { get; set; }

    [CanBeNull]
    public virtual string? ProductHierarchyDescription { get; set; }

    [CanBeNull]
    public virtual string? CountryOfOrigin { get; set; }

    [CanBeNull]
    public virtual int? ReferenceLeadTime { get; set; }

    public virtual int? WarrantyTime { get; set; }

    [CanBeNull]
    public virtual string? InventoryCategory { get; set; }

    public virtual int? Maxlot { get; set; }

    public virtual int? StockWarning { get; set; }

    public virtual decimal? VAT { get; set; }

    [CanBeNull]
    public virtual string? HS_Code { get; set; }

    public virtual Guid? SupplierBUId { get; set; }

    [CanBeNull]
    public virtual string? SupplierBUCode { get; set; }

    [CanBeNull]
    public virtual string? Factory_Text { get; set; }

    public virtual decimal? Input_Price { get; set; }

    public virtual string? InputCurrency { get; set; }

    [CanBeNull]
    public virtual string? INCOTERMS { get; set; }

    public virtual bool? EPA { get; set; }

    public virtual decimal? ImportDuty { get; set; }

    public virtual decimal? AppliedExchangeRate { get; set; }

    public virtual decimal? LandedCost { get; set; }

    public virtual decimal? MaxSalesOfferPrice { get; set; }

    public virtual decimal? MaxMangerOfferPrice { get; set; }

    public virtual decimal? Standard_Price { get; set; }

    public virtual decimal? SellingPrice1 { get; set; }

    public virtual decimal? SellingPrice2 { get; set; }

    public virtual decimal? SellingPrice3 { get; set; }

    public virtual decimal? SellingPrice4 { get; set; }

    public virtual decimal? SellingPrice5 { get; set; }

    //[NotNull]
    public virtual string? MaterialStatus { get; set; }

    [CanBeNull]
    public virtual string? Note { get; set; }
    public virtual string? Source { get; set; }
    [CanBeNull]
    public virtual string? Reason { get; set; }
    [CanBeNull]
    public virtual DateTime? FinalDPOAcceptanceDate { get; set; }
    [CanBeNull]
    public virtual string? SupplierCode { get; set; }
    [CanBeNull]
    public virtual string? MaterialClass { get; set; }
    [CanBeNull]
    public virtual DateTime? ActionDate { get; set; }
    [CanBeNull]
    public virtual string? Action { get; set; }
    [CanBeNull]
    public virtual string? FactoryRefDoc { get; set; }
    [CanBeNull]
    public virtual string? CargoNote { get; set; }
    [CanBeNull]
    public virtual string? Weight { get; set; }
    [CanBeNull]
    public virtual string? Size { get; set; }
    [CanBeNull]
    public virtual string? QRCode { get; set; }

    //public SystemCategory? InputCurrencyCategory { get; set; }
    //public SystemCategory? MaterialGroupCategory { get; set; }

    protected MaterialApprovalRequestDetail()
    {

    }

    public MaterialApprovalRequestDetail(Guid id, Guid materialApprovalId, string golfaCode, string model, bool ePA, decimal standard_Price, string materialStatus, DateTime? validFrom = null, DateTime? validTo = null, string? sAP_Code = null, string? spec1 = null, string? spec2 = null, string? spec3 = null, string? spec4 = null, string? description_EN = null, string? description_VN = null, string? materialType = null, string? unit = null, string? material_SEC_Classification = null, string? material_Group = null, string? sAPMatGroup = null, string? product_Hierarchy = null, string? productHierarchyDescription = null, string? countryOfOrigin = null, int? referenceLeadTime = null, int? warrantyTime = null, string? inventoryCategory = null, int? maxlot = null, int? stockWarning = null, decimal? vAT = null, string? hS_Code = null, Guid? supplierBUId = null, string? supplierBUCode = null, string? factory_Text = null, decimal? input_Price = null, string? inputCurrency = null, string? iNCOTERMS = null, decimal? importDuty = null, decimal? appliedExchangeRate = null, decimal? landedCost = null, decimal? maxSalesOfferPrice = null, decimal? maxMangerOfferPrice = null, decimal? sellingPrice1 = null, decimal? sellingPrice2 = null, decimal? sellingPrice3 = null, decimal? sellingPrice4 = null, decimal? sellingPrice5 = null, DateTime? registrationDate = null, string? note = null, string? source = null, string? reason = null, DateTime? finalDPOAcceptanceDate = null, string? supplierCode = null, string? cargoNote = null, string? weight = null, string? size = null, string? qrCode = null)
    {

        Id = id;

        MaterialApprovalId = materialApprovalId;
        GolfaCode = golfaCode;
        Model = model;
        EPA = ePA;
        Standard_Price = standard_Price;
        MaterialStatus = "DRAFT";
        ValidFrom = validFrom;
        ValidTo = validTo;
        SAP_Code = sAP_Code;
        Spec1 = spec1;
        Spec2 = spec2;
        Spec3 = spec3;
        Spec4 = spec4;
        Description_EN = description_EN;
        Description_VN = description_VN;
        MaterialType = materialType;
        Unit = unit;
        Material_SEC_Classification = material_SEC_Classification;
        Material_Group = material_Group;
        SAPMatGroup = sAPMatGroup;
        Product_Hierarchy = product_Hierarchy;
        ProductHierarchyDescription = productHierarchyDescription;
        CountryOfOrigin = countryOfOrigin;
        ReferenceLeadTime = referenceLeadTime;
        WarrantyTime = warrantyTime;
        InventoryCategory = inventoryCategory;
        Maxlot = maxlot;
        StockWarning = stockWarning;
        VAT = vAT;
        HS_Code = hS_Code;
        SupplierBUId = supplierBUId;
        SupplierBUCode = supplierBUCode;
        Factory_Text = factory_Text;
        Input_Price = input_Price;
        InputCurrency = inputCurrency;
        INCOTERMS = iNCOTERMS;
        ImportDuty = importDuty;
        AppliedExchangeRate = appliedExchangeRate;
        LandedCost = landedCost;
        MaxSalesOfferPrice = maxSalesOfferPrice;
        MaxMangerOfferPrice = maxMangerOfferPrice;
        SellingPrice1 = sellingPrice1;
        SellingPrice2 = sellingPrice2;
        SellingPrice3 = sellingPrice3;
        SellingPrice4 = sellingPrice4;
        SellingPrice5 = sellingPrice5;
        //DestinationDate = destinationDate;
        RegistrationDate = registrationDate;
        //IndeactiveDate = indeactiveDate;
        //Description_Group = description_Group;
        //Origin = origin;
        //Kind = kind;
        //Factory = factory;
        //Vendor = vendor;
        //LeadTime = leadTime;
        //RefExchangeRate = refExchangeRate;
        Note = note;
        Source = source;
        Reason = reason;
        FinalDPOAcceptanceDate = finalDPOAcceptanceDate;
        SupplierCode = supplierCode;
        CargoNote = cargoNote;
        Weight = weight;
        Size = size;
        QRCode = qrCode;
    }

    public MaterialApprovalRequestDetail(Guid id, MaterialApprovalRequestDetailCreateParams createParams)
    {
        Id = id;
        MaterialApprovalId = createParams.MaterialApprovalId;
        GolfaCode = createParams.GolfaCode;
        Model = createParams.Model;
        EPA = createParams.EPA;
        Standard_Price = createParams.Standard_Price;
        MaterialStatus = createParams.MaterialStatus;
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
        ImportDuty = createParams.ImportDuty;
        AppliedExchangeRate = createParams.AppliedExchangeRate;
        LandedCost = createParams.LandedCost;
        MaxSalesOfferPrice = createParams.MaxSalesOfferPrice;
        MaxMangerOfferPrice = createParams.MaxMangerOfferPrice;
        SellingPrice1 = createParams.SellingPrice1;
        SellingPrice2 = createParams.SellingPrice2;
        SellingPrice3 = createParams.SellingPrice3;
        SellingPrice4 = createParams.SellingPrice4;
        SellingPrice5 = createParams.SellingPrice5;
        //DestinationDate = createParams.DestinationDate;
        RegistrationDate = createParams.RegistrationDate;
        //IndeactiveDate = createParams.IndeactiveDate;
        //Description_Group = createParams.Description_Group;
        //Origin = createParams.Origin;
        //Kind = createParams.Kind;
        //Factory = createParams.Factory;
        Factory_Text = createParams.Factory_Text;
        //Vendor = createParams.Vendor;
        //LeadTime = createParams.LeadTime;
        //RefExchangeRate = createParams.RefExchangeRate;
        Note = createParams.Note;
        Source = createParams.Source;
        Reason = createParams.Reason;
        FinalDPOAcceptanceDate = createParams.FinalDPOAcceptanceDate;
        SupplierCode = createParams.SupplierCode;
        SupplierBUId = createParams.SupplierBUId;
        MaterialClass = createParams.MaterialClass;
        ActionDate = createParams.ActionDate;
        Source = createParams.Source;
        Reason = createParams.Reason;
        FactoryRefDoc = createParams.FactoryRefDoc;
        Action = createParams.Action;
        CargoNote = createParams.CargoNote;
        Weight = createParams.Weight;
        Size = createParams.Size;
        QRCode = createParams.QRCode;
    }


}