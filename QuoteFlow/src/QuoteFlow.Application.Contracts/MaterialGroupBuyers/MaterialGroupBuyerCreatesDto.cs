using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.MaterialGroupBuyers;
public class MaterialGroupBuyerCreatesDto
{
    [Required]
    public Guid BuyerId { get; set; }
    [StringLength(MaterialGroupBuyerConsts.BuyerShortNameMaxLength)]
    public string? BuyerShortName { get; set; }
    [Required]
    public List<MaterialGroupOfBuyerCreateDto> MaterialGroups { get; set; }
    public string? Note { get; set; }
}
