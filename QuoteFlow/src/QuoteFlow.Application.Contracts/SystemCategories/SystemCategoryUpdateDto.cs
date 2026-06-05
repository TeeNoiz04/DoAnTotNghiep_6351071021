using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.SystemCategories;

public class SystemCategoryUpdateDto : IHasConcurrencyStamp
{
    public Guid? ParentId { get; set; }
    [Required]
    [StringLength(SystemCategoryConsts.DescriptionMaxLength)]
    public string Description { get; set; } = null!;
    [Required]
    [StringLength(SystemCategoryConsts.CategoryTypeMaxLength)]
    public string CategoryType { get; set; } = null!;
    public decimal? Value { get; set; }
    [StringLength(QuoteFlowSharedConsts.NoteMaxLength)]
    public string? Note { get; set; }
    public bool IsDeactive { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}