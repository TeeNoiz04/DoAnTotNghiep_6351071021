using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.DistributorTargets;

public class DistributorTargetCreateDto
{
    public Guid? BuyerTypeId { get; set; }
    [Required]
    public Guid BuyerId { get; set; }
    [StringLength(DistributorTargetConsts.BuyerCodeMaxLength)]
    public string? BuyerCode { get; set; }

    public string? BuyerName { get; set; }
    [Required]
    [StringLength(DistributorTargetConsts.MaterialTypeMaxLength)]
    public string MaterialType { get; set; } = null!;
    public int? FinanceYear { get; set; }
    public decimal? FirstFYTarget { get; set; }
    public decimal? SecondFYTarget { get; set; }
    public string? Note { get; set; }
}