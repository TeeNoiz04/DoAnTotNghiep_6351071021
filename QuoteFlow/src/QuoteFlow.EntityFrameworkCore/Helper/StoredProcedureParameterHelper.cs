using System.Collections.Generic;
using System.Linq;

namespace QuoteFlow.Helper;

public static class StoredProcedureParameterHelper
{
    /// <summary>
    /// Converts a list of items into a string parameter for a stored procedure, 
    /// handling null/empty/invalid values based on the required contract.
    /// </summary>
    /// <typeparam name="T">The type of items in the list (e.g., string, int, Guid).</typeparam>
    /// <param name="list">The input list of items.</param>
    /// <returns>
    /// - null: If the list is null or contains no valid items (treat as NO restriction).
    /// - "":   If the list contains only empty/default/invalid items (treat as TOTAL restriction/access denied).
    /// - "item1,item2": A comma-separated string of valid items.
    /// </returns>
    public static string? ListToStoredProcString<T>(IEnumerable<T> list)
    {
        if (list == null || !list.Any())
        {
            return null;
        }

        // Use the static Default property to get the comparer instance.
        var comparer = EqualityComparer<T>.Default;
        var defaultItemValue = default(T); // Gets the default value for the type T (e.g., 0 for int, null for string, Guid.Empty for Guid)

        // 1. Filter out invalid/empty items (e.g., null, string.Empty, Guid.Empty)
        var validItems = list
            .Where(item =>
            {
                // Filter out null references immediately
                if (item == null) return false;

                // For strings, check if it's empty/whitespace
                if (item is string s)
                {
                    return !string.IsNullOrWhiteSpace(s);
                }

                // For all other types (structs like Guid/int, or classes):
                // Check if the item is equal to the default value for its type (e.g., Guid.Empty, 0).
                // Use the comparer instance for reliable comparison of generic types.
                return !comparer.Equals(item, defaultItemValue);
            })
            .Select(item => item?.ToString())
            .ToList();

        // 2. Apply the contract rules based on the filtered list size
        if (validItems.Count == 0)
        {
            // [string.Empty], [Guid.Empty], or list containing only invalid items => "" in DB (TOTAL restriction)
            return string.Empty;
        }

        // 3. [item1, item2] => "item1,item2" in DB (SPECIFIC restriction)
        return string.Join(",", validItems);
    }
}
