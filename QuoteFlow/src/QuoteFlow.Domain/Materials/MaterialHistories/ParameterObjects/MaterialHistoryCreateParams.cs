using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.Materials.MaterialHistories.ParameterObjects;
public class MaterialHistoryCreateParams
{
    [Required]
    public Guid MaterialId { get; set; }
    [StringLength(MaterialHistoryConsts.ActionMaxLength)]
    public string? Action { get; set; }
    [StringLength(MaterialHistoryConsts.NoteMaxLength)]
    public string? Note { get; set; }
}
