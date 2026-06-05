using System.Collections.Generic;

namespace QuoteFlow.Shared.Services;

/// <summary>
/// Service interface for detecting duplicates in collections with flexible field mapping.
/// </summary>
public interface IDuplicateDetectionService
{
    /// <summary>
    /// Finds duplicates in a collection based on a custom key selector.
    /// </summary>
    /// <typeparam name="T">Type of items in the collection</typeparam>
    /// <typeparam name="TKey">Type of the key used for duplicate detection</typeparam>
    /// <param name="items">Collection to check for duplicates</param>
    /// <param name="keySelector">Function to extract the key from each item</param>
    /// <returns>Collection of duplicate groups</returns>
    List<DuplicateGroup<TKey, T>> FindDuplicates<T, TKey>(
        IEnumerable<T> items,
        System.Func<T, TKey> keySelector)
        where TKey : notnull;

    /// <summary>
    /// Finds material combination duplicates using field mapping.
    /// </summary>
    /// <typeparam name="T">Type of items containing material combinations</typeparam>
    /// <param name="items">Collection to check for duplicates</param>
    /// <param name="fieldMapping">Mapping configuration for extracting material combination fields</param>
    /// <returns>Collection of duplicate groups with material combination keys</returns>
    List<DuplicateGroup<MaterialCombinationKey, T>> FindMaterialCombinationDuplicates<T>(
        IEnumerable<T> items,
        MaterialCombinationFieldMapping<T> fieldMapping);
}
