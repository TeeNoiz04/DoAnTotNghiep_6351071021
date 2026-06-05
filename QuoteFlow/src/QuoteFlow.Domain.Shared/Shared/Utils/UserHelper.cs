using System;

namespace QuoteFlow.Shared.Utils;

public static class UserHelper
{
    public static string GetFullName(string? name, string? surName)
    {
        const string NA = "N/A";

        if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(surName))
        {
            return NA;
        }
        if (string.IsNullOrWhiteSpace(surName))
        {
            return name ?? NA;
        }
        if (string.IsNullOrWhiteSpace(name))
        {
            return surName ?? NA;
        }

        return $"{name}, {surName}";
    }

    public static bool CompareUsername(string username1, string username2)
    {
        if (string.IsNullOrWhiteSpace(username1) || string.IsNullOrWhiteSpace(username2))
        {
            return false;
        }

        return username1.Equals(username2, StringComparison.OrdinalIgnoreCase);
    }
}