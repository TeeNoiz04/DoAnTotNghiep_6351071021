namespace QuoteFlow.Shared.Services;

/// <summary>
/// Represents a material validation error for a specific item
/// </summary>
/// <typeparam name="T">Type of the validated item</typeparam>
public class MaterialValidationError<T>
{
    /// <summary>
    /// The item that failed validation
    /// </summary>
    public T Item { get; set; } = default!;

    /// <summary>
    /// Type of validation error
    /// </summary>
    public MaterialValidationErrorType ErrorType { get; set; }

    /// <summary>
    /// Human-readable error message
    /// </summary>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// Provided material type for the failed validation
    /// </summary>
    public string? ProvidedMaterialType { get; set; }

    /// <summary>
    /// Expected material type for the failed validation
    /// </summary>
    public string? ExpectedMaterialType { get; set; }

    /// <summary>
    /// Golfa code for the failed validation
    /// </summary>
    public string? GolfaCode { get; set; }

    /// <summary>
    /// Model name for the failed validation
    /// </summary>
    public string? ModelName { get; set; }

    /// <summary>
    /// Expected standard price (for price mismatch errors)
    /// </summary>
    public decimal? ExpectedStandardPrice { get; set; }

    /// <summary>
    /// Provided standard price (for price mismatch errors)
    /// </summary>
    public decimal? ProvidedStandardPrice { get; set; }

    /// <summary>
    /// Expected specification 1 (for spec mismatch errors)
    /// </summary>
    public string? ExpectedSpec1 { get; set; }

    /// <summary>
    /// Provided specification 1 (for spec mismatch errors)
    /// </summary>
    public string? ProvidedSpec1 { get; set; }

}
