using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.DPOs.DPODetails;

public class DPODetailUpdateDto : IHasConcurrencyStamp
{
    [Required]
    public Guid DPOId { get; set; }
    [Required]
    [StringLength(DPODetailConsts.StatusMaxLength)]
    public string Status { get; set; } = null!;
    [Required]
    [StringLength(DPODetailConsts.GolfaCodeMaxLength)]
    public string GolfaCode { get; set; } = null!;
    [StringLength(DPODetailConsts.ModelMaxLength)]
    public string? Model { get; set; }
    [StringLength(DPODetailConsts.Spec1MaxLength)]
    public string? Spec1 { get; set; }
    [StringLength(DPODetailConsts.Spec2MaxLength)]
    public string? Spec2 { get; set; }
    public int? Qty { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? Amount { get; set; }
    public DateTime? RequestedETA { get; set; }
    public Guid? SPOId { get; set; }
    [StringLength(DPODetailConsts.SPOCodeMaxLength)]
    public string? SPOCode { get; set; }
    [StringLength(DPODetailConsts.CustomerTaxCodeMaxLength)]
    public string? CustomerTaxCode { get; set; }
    [StringLength(DPODetailConsts.CustomerNameMaxLength)]
    public string? CustomerName { get; set; }
    public int? LockStock { get; set; }
    public int? LockStockSO { get; set; }
    public int? LockShipment { get; set; }
    public int? Delivered { get; set; }
    public int? NeedDelivery { get; set; }
    [StringLength(DPODetailConsts.NoteMaxLength)]
    public string? Note { get; set; }

    [StringLength(DPODetailConsts.NoteMaxLength)]
    public string? ConfirmNoted { get; set; }
    public string? OrderReason { get; set; }
    public string ConcurrencyStamp { get; set; } = null!;
}