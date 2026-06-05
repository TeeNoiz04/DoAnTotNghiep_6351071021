using System;

namespace QuoteFlow.Shared.Extensions;

public static class DateTimeExtensions
{
    public const string StandardFormat = "dd/MM/yyyy h:mm tt";

    public static string ToStandardString(this DateTime dateTime)
    {
        return dateTime.ToString(StandardFormat);
    }

    // name the method, it's convert date to: ddMMyyyy_HHmmss
    public static string ToFileNameString(this DateTime dateTime)
    {
        return dateTime.ToString("ddMMyyyy_HHmmss");
    }

    public static string ToDateOrDateTimeString(this DateTime dateTime)
    {
        return dateTime.TimeOfDay == TimeSpan.Zero
            ? dateTime.ToString("dd/MM/yyyy")
            : dateTime.ToString("dd/MM/yyyy HH:mm");
    }
}