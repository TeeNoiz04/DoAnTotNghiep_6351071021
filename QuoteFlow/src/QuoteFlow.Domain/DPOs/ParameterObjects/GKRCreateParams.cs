using QuoteFlow.DPOs.DPODetails.ParameterObjects;
using QuoteFlow.SystemCategories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.DPOs.ParameterObjects;

public class GKRCreateParams
{
    public Guid? Id { get; set; }

    [Required]
    [StringLength(DPOConsts.DPONoMaxLength)]
    public string DPONo { get; set; } = null!;

    [StringLength(DPOConsts.MaterialTypeMaxLength)]
    public string? MaterialType { get; set; }

    public Guid? BuyerTypeId { get; set; }
    public Guid? BuyerId { get; set; }

    [StringLength(DPOConsts.BuyerShortNameMaxLength)]
    public string? BuyerShortName { get; set; }

    [StringLength(SystemCategoryConsts.DescriptionMaxLength)]
    public string? BuyerTypeDescription { get; set; }

    [Required]
    public DateTime? OrderDate { get; set; }

    [Required]
    public DateTime? ExpirationDate { get; set; }

    [Required]
    public decimal TotalAmount { get; set; }

    [StringLength(DPOConsts.RemarkMaxLength)]
    public string? Remark { get; set; }

    [StringLength(DPOConsts.FileNameMaxLength)]
    public string? FileName { get; set; }

    [StringLength(4000)]
    public string? Reason { get; set; }

    [StringLength(500)]
    public string? SalePicUsername { get; set; }

    [StringLength(500)]
    public string? SalePicFullName { get; set; }

    public Guid? SalePicTeamId { get; set; }

    public ICollection<DPODetailCreateParams>? Details { get; set; }
}
