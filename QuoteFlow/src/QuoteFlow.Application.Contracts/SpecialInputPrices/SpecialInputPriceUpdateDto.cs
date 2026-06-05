using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.SpecialInputPrices;

public class SpecialInputPriceUpdateDto : IHasConcurrencyStamp
{
    [Required]
    [StringLength(SpecialInputPriceConsts.AccountNoMaxLength)]
    public string AccountNo { get; set; } = null!;
    [Required]
    [StringLength(SpecialInputPriceConsts.AccountNameMaxLength)]
    public string AccountName { get; set; } = null!;
    [StringLength(SpecialInputPriceConsts.ProjectNameMaxLength)]
    public string? ProjectName { get; set; }
    [StringLength(SpecialInputPriceConsts.MaterialTypeMaxLength)]
    public string? MaterialType { get; set; }
    public Guid? SupplierId { get; set; }
    public Guid? SupplierBUId { get; set; }
    [StringLength(SpecialInputPriceConsts.CurrencyMaxLength)]
    public string? Currency { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    [Required]
    [StringLength(SpecialInputPriceConsts.StatusMaxLength)]
    public string Status { get; set; } = null!;
    [StringLength(SpecialInputPriceConsts.NoteMaxLength)]
    public string? Note { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}