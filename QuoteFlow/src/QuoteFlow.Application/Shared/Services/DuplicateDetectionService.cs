using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.DependencyInjection;

namespace QuoteFlow.Shared.Services;

/// <summary>
/// Service for detecting duplicate combinations in collections.
/// </summary>
public class DuplicateDetectionService : IDuplicateDetectionService, ITransientDependency
{
    /// <summary>
    /// Finds duplicate material combinations in a collection.
    /// </summary>
    /// <typeparam name="T">Type of the item containing combination data</typeparam>
    /// <param name="items">Collection of items to check for duplicates</param>
    /// <param name="keySelector">Function to extract the combination key from each item</param>
    /// <returns>Grouped duplicates with their keys and items</returns>
    public List<DuplicateGroup<TKey, T>> FindDuplicates<T, TKey>(
        IEnumerable<T> items,
        Func<T, TKey> keySelector)
        where TKey : notnull
    {
        return items
            .GroupBy(keySelector)
            .Where(g => g.Count() > 1)
            .Select(g => new DuplicateGroup<TKey, T>
            {
                Key = g.Key,
                Items = g.ToList()
            })
            .ToList();
    }

    /// <summary>
    /// Finds duplicate material combinations using flexible field mapping.
    /// </summary>
    /// <typeparam name="T">Type of the item containing combination data</typeparam>
    /// <param name="items">Collection of items to check for duplicates</param>
    /// <param name="fieldMapping">Mapping configuration for extracting fields from items</param>
    /// <returns>Grouped duplicates with their keys and items</returns>
    public List<DuplicateGroup<MaterialCombinationKey, T>> FindMaterialCombinationDuplicates<T>(
        IEnumerable<T> items,
        MaterialCombinationFieldMapping<T> fieldMapping)
    {
        return FindDuplicates(items, item => new MaterialCombinationKey
        {
            MaterialType = fieldMapping.GetMaterialType?.Invoke(item),
            GolfaCode = fieldMapping.GetCode(item),
            Model = fieldMapping.GetModel(item),
            StandardPrice = fieldMapping.GetStandardPrice?.Invoke(item) ?? 0m
        });
    }

    /// <summary>
    /// Finds duplicate combinations based on two fields (e.g., GolfaCode + ModelName).
    /// </summary>
    /// <typeparam name="T">Type of the item containing combination data</typeparam>
    /// <param name="items">Collection of items to check for duplicates</param>
    /// <param name="field1Selector">Function to extract first field</param>
    /// <param name="field2Selector">Function to extract second field</param>
    /// <returns>Grouped duplicates with their two-field keys and items</returns>
    public List<DuplicateGroup<TwoFieldKey, T>> FindTwoFieldDuplicates<T>(
        IEnumerable<T> items,
        Func<T, string?> field1Selector,
        Func<T, string?> field2Selector)
    {
        return FindDuplicates(items, item => new TwoFieldKey
        {
            Field1 = field1Selector(item),
            Field2 = field2Selector(item)
        });
    }
}
