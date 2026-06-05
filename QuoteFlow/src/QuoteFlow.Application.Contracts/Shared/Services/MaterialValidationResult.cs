using System.Collections.Generic;
using System.Linq;

namespace QuoteFlow.Shared.Services;

/// <summary>
/// Result of material validation containing errors grouped by type
/// </summary>
/// <typeparam name="T">Type of the validated items</typeparam>
public class MaterialValidationResult<T>
{
    /// <summary>
    /// All validation errors
    /// </summary>
    public List<MaterialValidationError<T>> Errors { get; set; } = new();

    /// <summary>
    /// Items that failed because combination was not found
    /// </summary>
    public List<MaterialValidationError<T>> CombinationNotFoundErrors =>
        Errors.Where(e => e.ErrorType == MaterialValidationErrorType.CombinationNotFound).ToList();

    /// <summary>
    /// Items that failed due to standard price mismatch
    /// </summary>
    public List<MaterialValidationError<T>> StandardPriceMismatchErrors =>
        Errors.Where(e => e.ErrorType == MaterialValidationErrorType.StandardPriceMismatch).ToList();

    /// <summary>
    /// Items that failed because material is discontinued
    /// </summary>
    public List<MaterialValidationError<T>> MaterialDiscontinuedErrors =>
        Errors.Where(e => e.ErrorType == MaterialValidationErrorType.MaterialDeactivated).ToList();

    /// <summary>
    /// Whether there are any validation errors
    /// </summary>
    public bool HasErrors => Errors.Any();

    /// <summary>
    /// Total number of validation errors
    /// </summary>
    public int ErrorCount => Errors.Count;
}
