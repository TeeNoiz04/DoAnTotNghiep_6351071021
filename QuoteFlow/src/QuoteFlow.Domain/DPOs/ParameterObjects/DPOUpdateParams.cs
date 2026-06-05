using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.DPOs.ParameterObjects;
public class DPOUpdateParams : IHasConcurrencyStamp
{
    [StringLength(DPOConsts.DPONoMaxLength)]
    public string? DPONo { get; set; }

    public string? GICNo { get; set; }
    [StringLength(DPOConsts.DPOTypeMaxLength)]
    public string? DPOType { get; set; }

    public string? GICType { get; set; }
    [StringLength(DPOConsts.MaterialTypeMaxLength)]
    public string? MaterialType { get; set; }
    [StringLength(DPOConsts.CostCenterMaxLength)]
    public string? CostCenter { get; set; }
    [StringLength(DPOConsts.StatusMaxLength)]
    public string? Status { get; set; }
    public Guid? BuyerTypeId { get; set; }
    public Guid? BuyerId { get; set; }
    [StringLength(DPOConsts.BuyerShortNameMaxLength)]
    public string? BuyerShortName { get; set; }
    public string? BuyerTypeDescription { get; set; }
    public DateTime? OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    [StringLength(DPOConsts.RemarkMaxLength)]
    public string? Remark { get; set; }
    [StringLength(DPOConsts.FileNameMaxLength)]
    public string? FileName { get; set; }

    public virtual string? ReferenceDoc { get; set; }

    public virtual string? GICProcess { get; set; }
    public string ConcurrencyStamp { get; set; } = null!;
}