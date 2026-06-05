using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuoteFlow.Shared.Services;

/// <summary>
/// Service interface for validating material combinations against the materials database.
/// Supports flexible field mapping for different classes with different property names.
/// </summary>
public interface IMaterialCombinationValidationService
{
    /// <summary>
    /// Validates material combinations including existence, standard price, and material status.
    /// </summary>
    /// <typeparam name="T">Type of the input items</typeparam>
    /// <param name="items">Collection of items to validate</param>
    /// <param name="fieldMapping">Mapping configuration for extracting fields from items</param>
    /// <returns>Validation result with categorized errors</returns>
    Task<MaterialValidationResult<T>> ValidateAsync<T>(
        IEnumerable<T> items,
        MaterialCombinationFieldMapping<T> fieldMapping);
}
