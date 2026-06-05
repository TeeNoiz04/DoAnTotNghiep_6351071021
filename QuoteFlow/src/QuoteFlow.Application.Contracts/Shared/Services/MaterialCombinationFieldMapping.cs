using System;

namespace QuoteFlow.Shared.Services;

/// <summary>
/// Configuration for mapping fields from input items to material combination fields.
/// </summary>
/// <typeparam name="T">Type of the input items</typeparam>
public class MaterialCombinationFieldMapping<T>
{
    /// <summary>
    /// Function to extract the material type from an item. Can be null if not needed.
    /// </summary>
    public Func<T, string?>? GetMaterialType { get; set; }

    /// <summary>
    /// Function to extract the code (GolfaCode, MaterialCode, etc.) from an item.
    /// </summary>
    public Func<T, string?> GetCode { get; set; } = null!;

    /// <summary>
    /// Function to extract the model from an item.
    /// </summary>
    public Func<T, string?> GetModel { get; set; } = null!;

    /// <summary>
    /// Function to extract the standard price from an item. Can be null if price validation is not needed.
    /// </summary>
    public Func<T, decimal?>? GetStandardPrice { get; set; }

    public Func<T, int?>? GetAppliedPrice { get; set; }

    public Func<T, string?>? GetSpec1 { get; set; }
}
