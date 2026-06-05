using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Shared;

public class NoteMetadataDto : IHasConcurrencyStamp
{
    [StringLength(QuoteFlowSharedConsts.NoteMaxLength)]
    public string? Note { get; set; }

    [Required]
    public string ConcurrencyStamp { get; set; } = null!;
}
