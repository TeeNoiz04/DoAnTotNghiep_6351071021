using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Materials.MaterialGroups;

public class MaterialGroupUpdateDto : IHasConcurrencyStamp
{
    [Required]
    [StringLength(50)]
    public string Code { get; set; } = null!;
    [Required]
    [StringLength(255)]
    public string Name { get; set; } = null!;
    public Guid? Parent { get; set; }
    [Required]
    public int SortOrder { get; set; }
    [StringLength(255)]
    public string? Note { get; set; }
    [Required]
    public bool IsDeActive { get; set; }
    [StringLength(MaterialGroupConsts.MaterialTypeMaxLength)]
    public string? MaterialType { get; set; }
    [StringLength(255)]
    public string? MaterialGroupPSI { get; set; }
    [Required]
    public bool AllowKeyAccount { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}