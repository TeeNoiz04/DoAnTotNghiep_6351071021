using QuoteFlow.DPOs.DPODetails.ParameterObjects;
using QuoteFlow.SystemCategories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.DPOs.ParameterObjects;

public class GICCreateParams
{
    public Guid? Id { get; set; }

    [Required]
    [StringLength(DPOConsts.DPONoMaxLength)]
    public string DPONo { get; set; } = null!;

    [Required]
    [StringLength(DPOConsts.DPOTypeMaxLength)]
    public string GICType { get; set; } = null!;

    [StringLength(DPOConsts.MaterialTypeMaxLength)]
    public string? MaterialType { get; set; }

    [StringLength(DPOConsts.CostCenterMaxLength)]
    public string? CostCenter { get; set; }

    public Guid? BuyerTypeId { get; set; }

    public Guid? BuyerId { get; set; }

    [StringLength(DPOConsts.BuyerShortNameMaxLength)]
    public string? BuyerShortName { get; set; }

    [StringLength(SystemCategoryConsts.DescriptionMaxLength)]
    public string? BuyerTypeDescription { get; set; }

    public DateTime? OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    [StringLength(DPOConsts.RemarkMaxLength)]
    public string? Remark { get; set; }

    [StringLength(DPOConsts.FileNameMaxLength)]
    public string? FileName { get; set; }

    [StringLength(DPOConsts.ReferenceDocMaxLength)]
    public virtual string? ReferenceDoc { get; set; }

    public virtual DateTime? ReferenceDocDate { get; set; }
    [Required]
    [StringLength(DPOConsts.GICProcessMaxLength)]
    public virtual string GICProcess { get; set; } = null!;

    public ICollection<DPODetailCreateParams>? Details { get; set; }
}
