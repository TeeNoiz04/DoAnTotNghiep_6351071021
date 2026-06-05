using System;

namespace QuoteFlow.Materials;

public class MaterialExcelDownloadDto
{
    public string DownloadToken { get; set; } = null!;

    public string? FilterText { get; set; }

    public string? GolfaCode { get; set; }
    public string? Model { get; set; }
    public string? SAPCode { get; set; }
    public string? DescriptionEN { get; set; }
    public string? DescriptionVN { get; set; }
    public string? DescriptionGroup { get; set; }
    public string? Spec1 { get; set; }
    public string? Spec2 { get; set; }
    public string? Spec3 { get; set; }
    public string? Spec4 { get; set; }
    public string? Unit { get; set; }
    public string? Origin { get; set; }
    public string? Kind { get; set; }
    public string? MaterialType { get; set; }
    public string? MaterialStatus { get; set; }
    public string? ProductHierarchy { get; set; }
    public string? MaterialGroup { get; set; }
    public Guid? Factory { get; set; }
    public Guid? Vendor { get; set; }
    public bool? EPA { get; set; }
    public int? LeadTimeMin { get; set; }
    public int? LeadTimeMax { get; set; }
    public int? MaxlotMin { get; set; }
    public int? MaxlotMax { get; set; }
    public int? StockValueWarningMin { get; set; }
    public int? StockValueWarningMax { get; set; }
    public string? HSCode { get; set; }
    public string? INCOTERMS { get; set; }
    public decimal? ImportDutyMin { get; set; }
    public decimal? ImportDutyMax { get; set; }
    public decimal? RefExchangeRateMin { get; set; }
    public decimal? RefExchangeRateMax { get; set; }
    public decimal? InputPriceMin { get; set; }
    public decimal? InputPriceMax { get; set; }
    public string? Currency { get; set; }
    public decimal? StandardPriceMin { get; set; }
    public decimal? StandardPriceMax { get; set; }
    public decimal? BuyerPrice1Min { get; set; }
    public decimal? BuyerPrice1Max { get; set; }
    public decimal? BuyerPrice2Min { get; set; }
    public decimal? BuyerPrice2Max { get; set; }
    public decimal? BuyerPrice3Min { get; set; }
    public decimal? BuyerPrice3Max { get; set; }
    public decimal? BuyerPrice4Min { get; set; }
    public decimal? BuyerPrice4Max { get; set; }
    public decimal? BuyerPrice5Min { get; set; }
    public decimal? BuyerPrice5Max { get; set; }
    public decimal? VATMin { get; set; }
    public decimal? VATMax { get; set; }
    public decimal? LandingCostMin { get; set; }
    public decimal? LandingCostMax { get; set; }
    public decimal? MarginAllowSaleMin { get; set; }
    public decimal? MarginAllowSaleMax { get; set; }
    public decimal? MarginAllowManagerMin { get; set; }
    public decimal? MarginAllowManagerMax { get; set; }
    public string? Note { get; set; }
    public string? MaterialClass { get; set; }

    public MaterialExcelDownloadDto()
    {

    }
}