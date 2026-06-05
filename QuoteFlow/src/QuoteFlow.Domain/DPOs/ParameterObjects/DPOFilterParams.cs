using QuoteFlow.BuyerAccess;
using System;
using System.Collections.Generic;

namespace QuoteFlow.DPOs.ParameterObjects;

public class DPOFilterParams : IBuyerRestrictable, IMaterialTypeRestrictable
{
    public string? FilterText { get; set; }

    public string? DPONo { get; set; }
    public string? MaterialCode { get; set; }
    public string? ModelName { get; set; }
    public string? PONo { get; set; }
    public string? CustomerName { get; set; }
    public DateTime? OrderDateMin { get; set; }
    public DateTime? OrderDateMax { get; set; }
    public Guid? BuyerId { get; set; }
    public string? MaterialType { get; set; }
    public string? SupplierId { get; set; }
    public string? SpecialPriceCode { get; set; }
    public string? MaterialGroup { get; set; }
    public string? TaxCode { get; set; }
    public string? SalesOrg { get; set; }
    public string? DPOType { get; set; }
    public string? DPOSubType { get; set; }
    public string? CostCenter { get; set; }
    public string? Status { get; set; }
    public Guid? BuyerTypeId { get; set; }
    public string? BuyerShortName { get; set; }
    public string? BuyerDescription { get; set; }
    public decimal? TotalAmountMin { get; set; }
    public decimal? TotalAmountMax { get; set; }
    public string? Remark { get; set; }
    public string? FileName { get; set; }
    public List<Guid> RestrictedBuyerIds { get; set; } = [];
    public List<string> RestrictedMaterialTypes { get; set; } = [];
}
