using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.StockManagements;

public class GetStockHistoriesInput : IValidatableObject
{
    [Required]
    public string StockCode { get; set; } = null!;

    [Required]
    public string GolfaCode { get; set; } = null!;

    [Required]
    public DateTime ActionFrom { get; set; }

    public DateTime? ActionTo { get; set; }

    public string? Note { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        const int MaxPastYears = 3;

        if (ActionTo.HasValue && ActionFrom.Date > ActionTo.Value.Date)
        {
            yield return new ValidationResult("Action From cannot be later than Action To.",
                new[] { nameof(ActionFrom), nameof(ActionTo) });
        }

        if (ActionTo.HasValue && ActionFrom.Date.AddYears(MaxPastYears) < ActionTo.Value.Date)
        {
            yield return new ValidationResult("Please select a date range within three years.",
                new[] { nameof(ActionTo) });
        }
        else if (!ActionTo.HasValue && ActionFrom.Date.AddYears(MaxPastYears) < DateTime.Now.Date)
        {
            yield return new ValidationResult("Please select a date range within three years.",
                new[] { nameof(ActionFrom) });
        }
    }
}
