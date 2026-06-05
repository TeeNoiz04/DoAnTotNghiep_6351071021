using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.DPOs;

public class DPOConfirmNoteDto
{
    [Required]
    public List<Guid> DPODetailIds { get; set; } = new();

    public string? Note { get; set; }
}