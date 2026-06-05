using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.MaterialGroupBuyers.ParameterObjects;
public class MaterialGroupBuyerCreateParams
{
    public Guid? MaterialGroupId { get; set; }
    [StringLength(MaterialGroupBuyerConsts.MaterialGroupCodeMaxLength)]
    public string? MaterialGroupCode { get; set; }
    [Required]
    public Guid BuyerId { get; set; }
    [StringLength(MaterialGroupBuyerConsts.BuyerShortNameMaxLength)]
    public string? BuyerShortName { get; set; }
    public string? Note { get; set; }

}
