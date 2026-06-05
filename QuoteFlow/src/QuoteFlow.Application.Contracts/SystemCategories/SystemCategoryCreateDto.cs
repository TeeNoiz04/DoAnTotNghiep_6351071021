using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.SystemCategories;

public class SystemCategoryCreateDto
{
    public Guid? ParentId { get; set; }
    [Required]
    [StringLength(SystemCategoryConsts.CodeMaxLength)]
    public string Code { get; set; } = null!;
    [Required]
    [StringLength(SystemCategoryConsts.DescriptionMaxLength)]
    public string Description { get; set; } = null!;
    public decimal? Value { get; set; }
    [Required]
    [StringLength(SystemCategoryConsts.CategoryTypeMaxLength)]
    public string CategoryType { get; set; } = null!;
    [StringLength(QuoteFlowSharedConsts.NoteMaxLength)]
    public string? Note { get; set; }

}