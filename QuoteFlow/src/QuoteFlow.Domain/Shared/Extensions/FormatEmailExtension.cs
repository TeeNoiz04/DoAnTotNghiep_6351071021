using System;
using System.Net.Mail;

namespace QuoteFlow.Shared.Extensions;

public static class FormatEmailExtension
{
    public static bool IsValidEmail(this string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        var trimmed = email.Trim();

        try
        {
            var addr = new MailAddress(trimmed);
            return addr.Address.Equals(trimmed, StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }
}
