using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.MaterialGroupBuyers;
public class MaterialGroupOfBuyerCreateDto
{
    public Guid? MaterialGroupId { get; set; }
    [StringLength(MaterialGroupBuyerConsts.MaterialGroupCodeMaxLength)]
    public string? MaterialGroupCode { get; set; }
}
