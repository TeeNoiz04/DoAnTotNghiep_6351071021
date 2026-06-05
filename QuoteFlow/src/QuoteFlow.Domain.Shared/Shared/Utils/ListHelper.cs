using System;
using System.Collections.Generic;
using System.Linq;

namespace QuoteFlow.Shared.Utils;

public static class ListHelper
{
    public static List<string> RemoveDuplicates(List<string> sourceList, List<string> checkList)
    {
        return [.. sourceList
            .Where(item => !string.IsNullOrWhiteSpace(item))
            .Except(checkList, StringComparer.OrdinalIgnoreCase)];
    }
}
