using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace QuoteFlow.Shared.Utils;

public static partial class NameHelper
{
    public static string ConvertClassNameToEntityName(string? className, CasingStyle casing = CasingStyle.Original)
    {
        if (string.IsNullOrWhiteSpace(className))
            return string.Empty;

        // Remove "Entity" suffix
        if (className.EndsWith("Entity", StringComparison.Ordinal))
            className = className[..^6];

        // Insert spaces before uppercase letters, handling acronyms better
        var spaced = PascalCaseWordSplitRegex().Replace(className, " ").Trim();

        // Apply casing
        return casing switch
        {
            CasingStyle.Lower => spaced.ToLower(),
            CasingStyle.Upper => spaced.ToUpper(),
            CasingStyle.Title => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(spaced.ToLower()),
            _ => spaced
        };
    }

    [GeneratedRegex(@"(?<!^)(?=[A-Z][a-z])|(?<=[a-z])(?=[A-Z])")]
    private static partial Regex PascalCaseWordSplitRegex();
}

public enum CasingStyle
{
    Original,
    Lower,
    Upper,
    Title
}