using QuoteFlow.Materials.MaterialStocks;
using QuoteFlow.Shared;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.StockManagements;
public class StockManagementDto : ExtendedAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string GolfaCode { get; set; } = null!;
    public string Model { get; set; } = null!;
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    [JsonPropertyName("sap_Code")]
    public string? SAP_Code { get; set; }
    public string? Spec1 { get; set; }
    public string? Spec2 { get; set; }
    public string? Spec3 { get; set; }
    public string? Spec4 { get; set; }
    public string? Description_EN { get; set; }
    public string? Description_VN { get; set; }
    public string? MaterialType { get; set; }
    public string? Unit { get; set; }
    public string? Material_SEC_Classification { get; set; }
    public string? Material_Group { get; set; }
    public string? SAPMatGroup { get; set; }
    public string? Product_Hierarchy { get; set; }
    public string? ProductHierarchyDescription { get; set; }
    public string? CountryOfOrigin { get; set; }
    public string? ReferenceLeadTime { get; set; }
    public int? WarrantyTime { get; set; }
    public string? InventoryCategory { get; set; }
    public int? Maxlot { get; set; }
    public int? StockWarning { get; set; }
    public decimal? VAT { get; set; }
    public string? HS_Code { get; set; }
    public Guid? SupplierBUId { get; set; }
    public string? SupplierBUCode { get; set; }
    public string? Factory_Text { get; set; }
    public decimal? Input_Price { get; set; }
    public string? InputCurrency { get; set; }
    public string? INCOTERMS { get; set; }
    public bool EPA { get; set; }
    public decimal? ImportDuty { get; set; }
    public decimal? AppliedExchangeRate { get; set; }
    public decimal? LandedCost { get; set; }
    public decimal? MaxSalesOfferPrice { get; set; }
    public decimal? MaxMangerOfferPrice { get; set; }
    public decimal Standard_Price { get; set; }
    public decimal? SellingPrice1 { get; set; }
    public decimal? SellingPrice2 { get; set; }
    public decimal? SellingPrice3 { get; set; }
    public decimal? SellingPrice4 { get; set; }
    public decimal? SellingPrice5 { get; set; }
    public string MaterialStatus { get; set; } = null!;
    //public DateTime? DestinationDate { get; set; }
    public DateTime? RegistrationDate { get; set; }
    //public DateTime? IndeactiveDate { get; set; }
    //public string? Description_Group { get; set; }
    //public string? Origin { get; set; }
    //public string? Kind { get; set; }
    //public Guid? Factory { get; set; }
    //public Guid? Vendor { get; set; }
    //public int? LeadTime { get; set; }
    //public decimal? RefExchangeRate { get; set; }
    public string? Note { get; set; }
    //public virtual string? Source { get; set; }
    //public virtual string? Reason { get; set; }
    //public virtual DateTime? FinalDPOAcceptanceDate { get; set; }
    public virtual string? SupplierCode { get; set; }
    public List<MaterialStockDto>? MaterialStock { get; set; }
    public string ConcurrencyStamp { get; set; } = null!;

}