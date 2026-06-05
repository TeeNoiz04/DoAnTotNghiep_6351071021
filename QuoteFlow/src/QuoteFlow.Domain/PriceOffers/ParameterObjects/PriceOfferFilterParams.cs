using QuoteFlow.BuyerAccess;
using System;
using System.Collections.Generic;

namespace QuoteFlow.PriceOffers.ParameterObjects;

public class PriceOfferFilterParams : IBuyerRestrictable, IMaterialTypeRestrictable, ILocationRestrictable
{
    public string? FilterText { get; set; }
    public string? PriceOfferType { get; set; }
    public string? MaterialType { get; set; }
    public Guid? BuyerId { get; set; }
    public string? PriceOfferCode { get; set; }
    public string? CustomerTaxCode { get; set; }
    public string? CustomerName { get; set; }
    public string? ApprovalStatus { get; set; }
    public string? ProjectResultStatus { get; set; }
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
    public bool RelatedToMe { get; set; } = false;
    public string? ProjectName { get; set; }

    // IBuyerRestrictable implementation
    public List<Guid> RestrictedBuyerIds { get; set; } = [];
    public List<string> RestrictedMaterialTypes { get; set; } = [];
    public Guid? LocationId { get; set; }
    public List<Guid> RestrictedLocationIds { get; set; } = [];
}
