using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Materials.MaterialHistories.ParameterObjects;
public class MaterialHistoryUpdateParams : IHasConcurrencyStamp
{
    [Required]
    public Guid MaterialId { get; set; }
    [StringLength(MaterialHistoryConsts.ActionMaxLength)]
    public string? Action { get; set; }
    [StringLength(MaterialHistoryConsts.NoteMaxLength)]
    public string? Note { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}
