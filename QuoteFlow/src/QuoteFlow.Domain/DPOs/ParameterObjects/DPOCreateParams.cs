using QuoteFlow.DPOs.DPODetails.ParameterObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.DPOs.ParameterObjects;
public class DPOCreateParams
{
    public Guid? Id { get; set; }

    [Required]
    [StringLength(DPOConsts.DPONoMaxLength)]
    public string DPONo { get; set; } = null!;

    [StringLength(DPOConsts.MaterialTypeMaxLength)]
    public string? MaterialType { get; set; }
    [StringLength(DPOConsts.CostCenterMaxLength)]
    public string? CostCenter { get; set; }
    public Guid? BuyerTypeId { get; set; }
    public Guid? BuyerId { get; set; }
    public string? BuyerShortName { get; set; }
    public string? BuyerTypeDescription { get; set; }
    public DateTime? OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    [StringLength(DPOConsts.RemarkMaxLength)]
    public string? Remark { get; set; }
    [StringLength(DPOConsts.FileNameMaxLength)]
    public string? FileName { get; set; }

    public ICollection<DPODetailCreateParams>? Details { get; set; }
}
