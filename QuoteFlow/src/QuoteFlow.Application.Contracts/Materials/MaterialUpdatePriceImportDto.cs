using System;

namespace QuoteFlow.Materials;
public class MaterialUpdatePriceImportDto
{
    public Guid? Id { get; set; }
    public string MaterialCode { get; set; } = null!;
    public string ModelName { get; set; } = null!;
    public string? Spec1 { get; set; }
    //public Guid? MaterialGroupId { get; set; }
    public string? MaterialType { get; set; }
    public string? MaterialGroup { get; set; }
    public DateTime? PriceValidFrom { get; set; }
    public DateTime? PriceValidTo { get; set; }
    public decimal? InputPrice { get; set; }
    //public Guid? CurrencyId { get; set; }
    public string? InputCurrency { get; set; }
    public string? Incoterms { get; set; }
    public bool? EPA { get; set; }
    //public string? Spec2 { get; set; }
    public decimal? ImportDuty { get; set; }
    public decimal? ExchangeRate { get; set; }
    public decimal? LandedCost { get; set; }
    public decimal? MaxSaleOfferPrice { get; set; }
    public decimal? MaxManagerOfferPrice { get; set; }
    public decimal? StandardPrice { get; set; }
    public decimal? SellingPrice1 { get; set; }
    public decimal? SellingPrice2 { get; set; }
    public decimal? SellingPrice3 { get; set; }
    public decimal? SellingPrice4 { get; set; }
    public decimal? SellingPrice5 { get; set; }
    public string ConcurrencyStamp { get; set; }
}
