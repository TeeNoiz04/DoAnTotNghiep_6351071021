using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.DPOs;

public class DPOAddExtraFeeDto : IValidatableObject
{
    [Required]
    public List<Guid> DPODetailIds { get; set; } = new();

    [Required]
    public decimal ExtraFee { get; set; }

    public string? ExtraFeeNote { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DPODetailIds == null || DPODetailIds.Count == 0)
        {
            yield return new ValidationResult(
                "At least one DPO Detail ID is required",
                new[] { nameof(DPODetailIds) }
            );
        }
    }
}