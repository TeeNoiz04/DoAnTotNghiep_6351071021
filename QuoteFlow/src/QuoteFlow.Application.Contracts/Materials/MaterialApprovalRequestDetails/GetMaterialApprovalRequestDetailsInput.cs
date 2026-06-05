using System;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.Materials.MaterialApprovalRequestDetails;

public class GetMaterialApprovalRequestDetailsInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public Guid? MaterialApprovalId { get; set; }
    public string? GolfaCode { get; set; }
    public string? Model { get; set; }
    public DateTime? ValidFromMin { get; set; }
    public DateTime? ValidFromMax { get; set; }
    public DateTime? ValidToMin { get; set; }
    public DateTime? ValidToMax { get; set; }
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
    public int? WarrantyTimeMin { get; set; }
    public int? WarrantyTimeMax { get; set; }
    public string? InventoryCategory { get; set; }
    public int? MaxlotMin { get; set; }
    public int? MaxlotMax { get; set; }
    public int? StockWarningMin { get; set; }
    public int? StockWarningMax { get; set; }
    public decimal? VATMin { get; set; }
    public decimal? VATMax { get; set; }
    public string? HS_Code { get; set; }
    public Guid? SupplierBUId { get; set; }
    public string? SupplierBUCode { get; set; }
    public string? Factory_Text { get; set; }
    public decimal? Input_PriceMin { get; set; }
    public decimal? Input_PriceMax { get; set; }
    public string? InputCurrency { get; set; }
    public string? INCOTERMS { get; set; }
    public bool? EPA { get; set; }
    public decimal? ImportDutyMin { get; set; }
    public decimal? ImportDutyMax { get; set; }
    public decimal? AppliedExchangeRateMin { get; set; }
    public decimal? AppliedExchangeRateMax { get; set; }
    public decimal? LandedCostMin { get; set; }
    public decimal? LandedCostMax { get; set; }
    public decimal? MaxSalesOfferPriceMin { get; set; }
    public decimal? MaxSalesOfferPriceMax { get; set; }
    public decimal? MaxMangerOfferPriceMin { get; set; }
    public decimal? MaxMangerOfferPriceMax { get; set; }
    public decimal? Standard_PriceMin { get; set; }
    public decimal? Standard_PriceMax { get; set; }
    public decimal? SellingPrice1Min { get; set; }
    public decimal? SellingPrice1Max { get; set; }
    public decimal? SellingPrice2Min { get; set; }
    public decimal? SellingPrice2Max { get; set; }
    public decimal? SellingPrice3Min { get; set; }
    public decimal? SellingPrice3Max { get; set; }
    public decimal? SellingPrice4Min { get; set; }
    public decimal? SellingPrice4Max { get; set; }
    public decimal? SellingPrice5Min { get; set; }
    public decimal? SellingPrice5Max { get; set; }
    public string? MaterialStatus { get; set; }
    //public DateTime? DestinationDateMin { get; set; }
    //public DateTime? DestinationDateMax { get; set; }
    public DateTime? RegistrationDateMin { get; set; }
    public DateTime? RegistrationDateMax { get; set; }
    //public DateTime? IndeactiveDateMin { get; set; }
    //public DateTime? IndeactiveDateMax { get; set; }
    //public string? Description_Group { get; set; }

    public GetMaterialApprovalRequestDetailsInput()
    {

    }
}