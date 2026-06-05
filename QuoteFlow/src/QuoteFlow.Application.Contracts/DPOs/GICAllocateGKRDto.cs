using QuoteFlow.Shared;
using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.DPOs;
public class GICAllocateGKRDto : NoteMetadataDto
{
    [Required]
    public Guid GICId { get; set; }

    [Required]
    public Guid GKRId { get; set; }
}
