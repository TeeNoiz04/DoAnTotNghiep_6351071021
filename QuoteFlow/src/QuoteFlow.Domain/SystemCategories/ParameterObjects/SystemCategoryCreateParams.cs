using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.SystemCategories.ParameterObjects;

public class SystemCategoryCreateParams
{
    [Required]
    [StringLength(SystemCategoryConsts.CodeMaxLength)]
    public string Code { get; set; } = null!;

    [Required]
    [StringLength(SystemCategoryConsts.DescriptionMaxLength)]
    public string Description { get; set; } = null!;

    [Required]
    [StringLength(SystemCategoryConsts.CategoryTypeMaxLength)]
    public string CategoryType { get; set; } = null!;

    [Required]
    public bool IsDeactive { get; set; } = false;

    public Guid? ParentId { get; set; }

    public decimal? Value { get; set; }

    [StringLength(QuoteFlowSharedConsts.NoteMaxLength)]
    public string? Note { get; set; }

}
