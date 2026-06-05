using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.Materials.MaterialHistories;

public class MaterialHistoryCreateDto
{
    [Required]
    public Guid MaterialId { get; set; }
    [StringLength(MaterialHistoryConsts.ActionMaxLength)]
    public string? Action { get; set; }
    [StringLength(MaterialHistoryConsts.NoteMaxLength)]
    public string? Note { get; set; }
}