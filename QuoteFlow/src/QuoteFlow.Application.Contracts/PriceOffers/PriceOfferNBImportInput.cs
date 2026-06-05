using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.PriceOffers;

public class PriceOfferNBImportInput : IValidatableObject
{
    [Required]
    public Guid LocationId { get; set; }

    public string? SalePIC { get; set; }

    [Required]
    public DateTime CloseDate { get; set; }

    public string? MaterialType { get; set; }

    public string? ProjectName { get; set; }

    [Required]
    [StringLength(QuoteFlowSharedConsts.NoteMaxLength)]
    public string Note { get; set; } = null!;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CloseDate < DateTime.Today)
        {
            yield return new ValidationResult(
                "Close Date must be in the future.",
                new[] { nameof(CloseDate) });
        }

        // CloseDate not exceed today + 1 year
        else if (CloseDate > DateTime.Today.Date.AddYears(1))
        {
            yield return new ValidationResult(
                "Close date cannot exceed one year from today.",
                new[] { nameof(CloseDate) });
        }
    }
}
