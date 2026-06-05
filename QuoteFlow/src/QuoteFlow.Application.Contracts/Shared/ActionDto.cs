using QuoteFlow.Shared.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Shared;
public class ActionDto : IHasConcurrencyStamp, IValidatableObject
{
    [Required]
    public string Action { get; set; } = null!;

    public string? Comment { get; set; }

    [Required]
    public string ConcurrencyStamp { get; set; } = null!;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Action == HistoryActions.Rejected || Action == HistoryActions.Cancelled)
        {
            // required the comment when the action is rejected
            if (string.IsNullOrWhiteSpace(Comment))
            {
                yield return new ValidationResult("The Comment field is required for the this action.", [nameof(Comment)]);
            }
        }
    }
}