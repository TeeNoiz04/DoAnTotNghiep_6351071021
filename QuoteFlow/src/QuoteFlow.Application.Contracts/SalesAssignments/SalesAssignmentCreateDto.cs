using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.SalesAssignments;

public class SalesAssignmentCreateDto
{
    [Required]
    [StringLength(SalesAssignmentConsts.MaterialTypeMaxLength)]
    public string MaterialType { get; set; } = null!;

    [Required]
    public Guid LocationId { get; set; }

    [Required]
    public Guid BuyerId { get; set; }

    [Required]
    public Guid BuyerTypeId { get; set; }

    [StringLength(QuoteFlowSharedConsts.NoteMaxLength)]
    public string? Note { get; set; }
    [StringLength(SalesAssignmentConsts.BuyerShortNameMaxLength)]
    public string? BuyerShortName { get; set; }

    [Required]
    public List<Shared.UserLookupCreateDto> Users { get; set; } = null!;
}
