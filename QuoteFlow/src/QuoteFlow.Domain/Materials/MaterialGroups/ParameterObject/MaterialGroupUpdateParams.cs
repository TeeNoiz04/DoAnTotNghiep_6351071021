using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Materials.MaterialGroups.ParameterObject;

public class MaterialGroupUpdateParams : IHasConcurrencyStamp
{
    [Required]
    [StringLength(MaterialGroupConsts.CodeMaxLength)]
    public string Code { get; set; } = null!;
    [Required]
    [StringLength(MaterialGroupConsts.NameMaxLength)]
    public string Name { get; set; } = null!;
    public Guid? Parent { get; set; }
    [Required]
    public int SortOrder { get; set; }
    [StringLength(MaterialGroupConsts.NoteMaxLength)]
    public string? Note { get; set; }
    [Required]
    public bool IsDeActive { get; set; }
    [StringLength(MaterialGroupConsts.MaterialTypeMaxLength)]
    public string? MaterialType { get; set; }
    [StringLength(MaterialGroupConsts.MaterialGroupPSIMaxLength)]
    public string? MaterialGroupPSI { get; set; }
    [Required]
    public bool AllowKeyAccount { get; set; }
    public string? ConcurrencyStamp { get; set; } = null!;
}
