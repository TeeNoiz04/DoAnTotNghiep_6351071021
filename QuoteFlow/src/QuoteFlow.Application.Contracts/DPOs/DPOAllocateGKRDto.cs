using QuoteFlow.Shared;
using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.DPOs;

public class DPOAllocateGKRDto : NoteMetadataDto
{
    [Required]
    public Guid DPOId { get; set; }

    [Required]
    public Guid GKRId { get; set; }
}
