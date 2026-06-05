using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Shared.Extensions;

public static class ConcurrencyStampCheckExtension
{
    public static IEnumerable<ValidationResult> ValidateConcurrencyStamp<T>(this T entity, string? expectedConcurrencyStamp) where T : class, IHasConcurrencyStamp
    {
        if (entity.ConcurrencyStamp != expectedConcurrencyStamp)
        {
            yield return new ValidationResult(expectedConcurrencyStamp == null
                ? "The data has been modified by another user."
                : "The data has been modified by another user. Reload the data and try again.", new[] { nameof(entity.ConcurrencyStamp) });
        }

        yield break;
    }
}