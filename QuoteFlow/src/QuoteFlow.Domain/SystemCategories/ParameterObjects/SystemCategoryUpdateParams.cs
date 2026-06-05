using QuoteFlow.Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.SystemCategories.ParameterObjects;

public class SystemCategoryUpdateParams : BaseUpdateParams
{
    [Required]
    [StringLength(SystemCategoryConsts.DescriptionMaxLength)]
    public string Description { get; set; } = null!;

    [Required]
    [StringLength(SystemCategoryConsts.CategoryTypeMaxLength)]
    public string CategoryType { get; set; } = null!;

    [Required]
    public bool IsDeactive { get; set; }

    public Guid? ParentId { get; set; }

    public decimal? Value { get; set; }

    [StringLength(QuoteFlowSharedConsts.NoteMaxLength)]
    public string? Note { get; set; }


}
