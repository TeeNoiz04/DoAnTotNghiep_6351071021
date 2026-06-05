namespace QuoteFlow.Shared.Services;

/// <summary>
/// Types of material validation errors
/// </summary>
public enum MaterialValidationErrorType
{
    /// <summary>
    /// Material combination does not exist in database
    /// </summary>
    CombinationNotFound = 1,

    /// <summary>
    /// Standard price does not match database value
    /// </summary>
    StandardPriceMismatch = 2,
    SellingPriceMismatch = 3,

    /// <summary>
    /// Material is discontinued (not active)
    /// </summary>
    MaterialDeactivated = 4,

    WrongMaterialType = 5,
    Spec1Mismatch = 6
}
