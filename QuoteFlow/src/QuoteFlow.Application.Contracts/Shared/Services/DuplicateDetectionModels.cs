using System;
using System.Collections.Generic;

namespace QuoteFlow.Shared.Services;

/// <summary>
/// Represents a group of duplicate items with their common key.
/// </summary>
/// <typeparam name="TKey">Type of the key that identifies the duplicates</typeparam>
/// <typeparam name="T">Type of the duplicate items</typeparam>
public class DuplicateGroup<TKey, T>
{
    /// <summary>
    /// The key that is duplicated across multiple items
    /// </summary>
    public TKey Key { get; set; } = default(TKey)!;

    /// <summary>
    /// The items that share the same key
    /// </summary>
    public List<T> Items { get; set; } = new List<T>();
}

/// <summary>
/// Represents a material combination key for duplicate detection.
/// Provides proper equality comparison with case-insensitive string matching.
/// </summary>
public class MaterialCombinationKey : IEquatable<MaterialCombinationKey>
{
    public string? MaterialType { get; set; }
    public string? GolfaCode { get; set; }
    public string? Model { get; set; }
    public decimal StandardPrice { get; set; }

    public bool Equals(MaterialCombinationKey? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return string.Equals(MaterialType, other.MaterialType, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(GolfaCode, other.GolfaCode, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(Model, other.Model, StringComparison.OrdinalIgnoreCase) &&
               StandardPrice == other.StandardPrice;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as MaterialCombinationKey);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            MaterialType?.ToLowerInvariant(),
            GolfaCode?.ToLowerInvariant(),
            Model?.ToLowerInvariant(),
            StandardPrice);
    }

    public override string ToString()
    {
        return $"MaterialType='{MaterialType}', Code='{GolfaCode}', Model='{Model}', StandardPrice='{StandardPrice}'";
    }
}

/// <summary>
/// Key for two-field combinations.
/// </summary>
public class TwoFieldKey : IEquatable<TwoFieldKey>
{
    public string? Field1 { get; set; }
    public string? Field2 { get; set; }

    public bool Equals(TwoFieldKey? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return string.Equals(Field1?.Trim(), other.Field1?.Trim(), StringComparison.OrdinalIgnoreCase) &&
               string.Equals(Field2?.Trim(), other.Field2?.Trim(), StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj) => Equals(obj as TwoFieldKey);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Field1?.Trim().ToLowerInvariant());
        hash.Add(Field2?.Trim().ToLowerInvariant());
        return hash.ToHashCode();
    }

    public override string ToString()
    {
        return $"Field1='{Field1}', Field2='{Field2}'";
    }
}
