using System;

namespace QuoteFlow.DPOs;

public class GKRFilterParams
{
    public string? GKRNo { get; set; }
    public string? MaterialType { get; set; }

    public string? Status { get; set; }
    public Guid? BuyerTypeId { get; set; }
    public Guid? BuyerId { get; set; }
    public string? BuyerShortName { get; set; }
    public DateTime? OrderDateMin { get; set; }
    public DateTime? OrderDateMax { get; set; }
    public decimal? TotalAmountMin { get; set; }
    public decimal? TotalAmountMax { get; set; }

    // Specific for GKR
    public string? LinkedDPONo { get; set; }

    // Additional filters specific to GKR - GIC
    public string? GICType { get; set; }
    public string? GICProcess { get; set; }
    public string? CostCenter { get; set; }

    // Details related
    public string? MaterialGroup { get; set; }
    public string? MaterialCode { get; set; }
    public string? ModelName { get; set; }
    public string? SpecialPriceCode { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerTaxCode { get; set; }
    public string? PONo { get; set; }
    public string? SupplierCode { get; set; }
    public string? Username { get; set; }
}
