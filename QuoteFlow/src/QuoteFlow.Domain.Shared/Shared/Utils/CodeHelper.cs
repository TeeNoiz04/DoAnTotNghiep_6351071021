using System;
using System.Text.RegularExpressions;
using Volo.Abp;

namespace QuoteFlow.Shared.Utils;

public static class CodeHelper
{
    /// <summary>
    /// Extracts the numeric suffix from a code string that is formatted as underscore-separated parts.
    /// </summary>
    /// <remarks>The method expects the input string to be in a specific format, where the last
    /// underscore-separated part is either a numeric value or a special case. Leading zeros in numeric suffixes are
    /// stripped. If the suffix consists entirely of 'x' characters (case-insensitive), the method returns -1.</remarks>
    /// <param name="code">The code string to process. Must be non-null, non-empty, and contain at least two underscore-separated parts.</param>
    /// <returns>The numeric suffix as an integer, with leading zeros removed. Returns -1 if the suffix consists entirely of 'x'
    /// characters.</returns>
    /// <exception cref="AbpException">Thrown if <paramref name="code"/> is null, empty, or does not contain at least two underscore-separated parts.
    /// Thrown if the suffix is empty or invalid (not numeric and not composed of 'x' characters).</exception>
    public static int ExtractNumericSuffix(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new AbpException("Code is null or empty.");
        }

        var parts = code.Split('_');
        if (parts.Length < 2)
        {
            throw new AbpException($"Invalid code format: '{code}'");
        }

        var lastPart = parts[^1]; // Get the last underscore-separated part

        if (string.IsNullOrWhiteSpace(lastPart))
        {
            throw new AbpException($"Code suffix is empty: '{code}'");
        }

        // Handle version suffix, e.g., 018.01 or 0181.21
        var numericCandidate = lastPart.Split('.')[0]; // Take only the part before the first dot

        if (Regex.IsMatch(numericCandidate, @"^x+$", RegexOptions.IgnoreCase))
        {
            return -1;
        }

        if (Regex.IsMatch(numericCandidate, @"^\d+$"))
        {
            return int.Parse(numericCandidate.TrimStart('0').Length > 0 ? numericCandidate.TrimStart('0') : "0");
        }

        throw new AbpException($"Invalid numeric suffix: '{lastPart}' in code: '{code}'");
    }

    public static int GetFiscalYear(DateTime date)
    {

        return (date.Month >= 4) ? date.Year : date.Year - 1;
    }

    /// <summary>
    /// Validates customer tax code format.
    /// Accepts either:
    /// 1. Vietnamese format: 10 alphanumeric characters, optionally followed by "-" and 3 alphanumeric characters for branches
    /// 2. Internal format: "INT-" prefix followed by 6 characters
    /// </summary>
    /// <param name="taxCode">The tax code to validate</param>
    /// <returns>Null if valid, error message string if invalid</returns>
    public static string? ValidateTaxCode(string? taxCode)
    {
        if (string.IsNullOrWhiteSpace(taxCode))
        {
            return null;
        }

        taxCode = taxCode.Trim();

        if (Regex.IsMatch(taxCode, @"^[A-Za-z]+-"))
        {
            // If it matches the pattern but doesn't start with "INT-", it's invalid
            if (!taxCode.StartsWith("INT-", StringComparison.OrdinalIgnoreCase))
            {
                return "Internal Tax Code format is invalid. It must be in the format 'INT-XXXXXX...' where XXXXXX... are alphanumeric characters.";
            }

            // Check if the part after "INT-" is alphanumeric
            var internalPattern = @"^INT-[A-Za-z0-9]+$";
            if (!Regex.IsMatch(taxCode, internalPattern, RegexOptions.IgnoreCase))
            {
                return "Internal Tax Code format is invalid. It must be in the format 'INT-XXXXXX...' where XXXXXX... are alphanumeric characters.";
            }

            return null; // Valid internal tax code
        }

        // Vietnamese tax code format: 10 alphanumeric characters, optionally followed by "-" and 3 alphanumeric characters
        var pattern = @"^[A-Za-z0-9]{10}(-[A-Za-z0-9]{3})?$";
        if (!Regex.IsMatch(taxCode, pattern))
        {
            // Provide more specific error messages
            if (taxCode.Contains('-'))
            {
                var parts = taxCode.Split('-');

                if (parts.Length > 2)
                    return "Tax Code has too many '-' characters.";

                var prefix = parts[0];
                var suffix = parts[1];

                if (prefix.Length != 10)
                    return "Tax Code prefix (before '-') must be exactly 10 characters.";

                if (suffix.Length != 3)
                    return "Tax Code suffix (after '-') must be exactly 3 alphanumeric characters.";

                if (!Regex.IsMatch(suffix, @"^[A-Za-z0-9]{3}$"))
                    return "Tax Code suffix must be alphanumeric.";

                if (!Regex.IsMatch(prefix, @"^[A-Za-z0-9]{10}$"))
                    return "Tax Code prefix must be alphanumeric.";
            }
            else
            {
                return "Tax Code format is invalid. Must be either: " +
                    "\n   (1) 'INT-' followed by alphanumeric characters for special customers" +
                    "\n   Or (2) 10 alphanumeric characters (optionally followed by '-' and 3 characters for branches) for Vietnamese format.";
            }
        }

        // Valid tax code
        return null;
    }
}