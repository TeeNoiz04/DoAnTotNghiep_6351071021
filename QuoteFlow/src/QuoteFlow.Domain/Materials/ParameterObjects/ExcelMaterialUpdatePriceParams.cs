using System;

namespace QuoteFlow.Materials.ParameterObjects;

public class ExcelMaterialUpdatePriceParams
{
    public Guid? Id { get; set; }
    public string GolfaCode { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string? Spec1 { get; set; }
    public string? MaterialGroup { get; set; }
    public string? MaterialType { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public decimal? Input_Price { get; set; }
    //public Guid? Material_GroupId { get; set; }
    public string? InputCurrency { get; set; }
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
    public string ConcurrencyStamp { get; set; } = null!;
}

