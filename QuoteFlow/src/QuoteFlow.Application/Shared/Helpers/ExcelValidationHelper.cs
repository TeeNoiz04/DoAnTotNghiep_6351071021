using System;
using System.Text.RegularExpressions;

namespace QuoteFlow.Shared.Helpers;

public static class ExcelValidationHelper
{
    /// <summary>
    /// Validates that amount equals quantity multiplied by unit price (within tolerance).
    /// </summary>
    /// <param name="qty">Quantity value</param>
    /// <param name="unitPrice">Unit price value</param>
    /// <param name="amount">Amount value provided by user</param>
    /// <param name="fieldName">Name of the field being validated (for error messages)</param>
    /// <param name="tolerance">Acceptable difference for decimal comparison (default: 0.01)</param>
    /// <returns>Tuple containing (isValid, expectedAmount, errorMessage)</returns>
    public static (bool IsValid, decimal? ExpectedAmount, string? ErrorMessage) ValidateAmountCalculation(
        decimal? qty,
        decimal? unitPrice,
        decimal? amount,
        string fieldName,
        decimal tolerance = 0.01m)
    {
        // If amount is not provided, no validation needed
        if (!amount.HasValue)
        {
            return (true, null, null);
        }

        // If qty or unitPrice is missing, we can't validate
        if (!qty.HasValue || !unitPrice.HasValue)
        {
            return (true, null, null);
        }

        var expectedAmount = qty.Value * unitPrice.Value;
        var difference = Math.Abs(amount.Value - expectedAmount);

        if (difference > tolerance)
        {
            var errorMessage = $"{fieldName} is incorrect. Expected: {expectedAmount:N0} (Qty: {qty.Value} × Price: {unitPrice.Value:N0}), but got: {amount.Value:N0}";
            return (false, expectedAmount, errorMessage);
        }

        return (true, expectedAmount, null);
    }
}
