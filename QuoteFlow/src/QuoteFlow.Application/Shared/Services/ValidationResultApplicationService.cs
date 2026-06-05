using QuoteFlow.Shared.Excels;
using System.Collections.Generic;
using System.Linq;

namespace QuoteFlow.Shared.Services;

/// <summary>
/// Service for applying validation results to Excel row results.
/// </summary>
public static class ValidationResultApplicationService
{
    /// <summary>
    /// Applies duplicate detection results to Excel row results.
    /// </summary>
    /// <typeparam name="T">Type of the Excel row data</typeparam>
    /// <param name="duplicateGroups">Groups of duplicate items found</param>
    /// <param name="rowResultMap">Dictionary mapping row indices to Excel row results</param>
    /// <param name="getRowIndex">Function to extract row index from duplicate item</param>
    /// <param name="createErrorMessage">Function to create error message from duplicate key</param>
    public static void ApplyDuplicateErrors<TKey, T>(
        IEnumerable<DuplicateGroup<TKey, T>> duplicateGroups,
        IDictionary<int, ExcelRowResult<T>> rowResultMap,
        System.Func<T, int> getRowIndex,
        System.Func<TKey, string> createErrorMessage)
    {
        foreach (var duplicateGroup in duplicateGroups)
        {
            var errorMessage = createErrorMessage(duplicateGroup.Key);

            foreach (var duplicate in duplicateGroup.Items)
            {
                var rowIndex = getRowIndex(duplicate);
                if (rowResultMap.TryGetValue(rowIndex, out var rowResult))
                {
                    if (!rowResult.Errors.Contains(errorMessage))
                    {
                        rowResult.Errors.Add(errorMessage);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Applies missing material combination results to Excel row results.
    /// </summary>
    /// <typeparam name="T">Type of the Excel row data</typeparam>
    /// <param name="missingCombinations">List of missing material combinations</param>
    /// <param name="originalCombinations">Original combinations with row indices</param>
    /// <param name="rowResultMap">Dictionary mapping row indices to Excel row results</param>
    /// <param name="matchCombination">Function to match missing combination with original</param>
    /// <param name="getRowIndex">Function to extract row index from original combination</param>
    /// <param name="createErrorMessage">Function to create error message from missing combination</param>
    public static void ApplyMissingCombinationErrors<TCombination, TOriginal>(
        IEnumerable<TCombination> missingCombinations,
        IEnumerable<TOriginal> originalCombinations,
        IDictionary<int, ExcelRowResult<TOriginal>> rowResultMap,
        System.Func<TCombination, TOriginal, bool> matchCombination,
        System.Func<TOriginal, int> getRowIndex,
        System.Func<TCombination, string> createErrorMessage)
    {
        foreach (var missing in missingCombinations)
        {
            var errorMessage = createErrorMessage(missing);

            // Find all rows with this missing combination and add error to each
            var affectedRows = originalCombinations
                .Where(original => matchCombination(missing, original))
                .ToList();

            foreach (var affectedRow in affectedRows)
            {
                var rowIndex = getRowIndex(affectedRow);
                if (rowResultMap.TryGetValue(rowIndex, out var rowResult))
                {
                    if (!rowResult.Errors.Contains(errorMessage))
                    {
                        rowResult.Errors.Add(errorMessage);
                    }
                }
            }
        }
    }
}
