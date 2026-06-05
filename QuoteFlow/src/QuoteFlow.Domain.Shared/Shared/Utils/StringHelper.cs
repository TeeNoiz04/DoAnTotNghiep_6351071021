namespace QuoteFlow.Shared.Utils;

public static class StringHelper
{
    public static string AddSpace(string? str)
    {
        if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str)) return str ?? string.Empty;
        return str + " ";
    }
}
