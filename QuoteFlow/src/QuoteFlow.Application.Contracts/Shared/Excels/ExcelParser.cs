using System;
using System.Collections.Generic;
using System.Globalization;

namespace QuoteFlow.Shared.Excels;

public static class ExcelParser
{

    public static T? GetValue<T>(IDictionary<string, object> row, string columnName, string? format = null)
    {
        var type = typeof(T);
        var nullableType = Nullable.GetUnderlyingType(type);
        var isNullable = nullableType != null;
        var targetType = nullableType ?? type;

        // Handle null or missing values
        if (row == null || !row.TryGetValue(columnName, out object? value) || value == null)
        {
            return default;
        }

        try
        {
            return targetType switch
            {
                Type t when t == typeof(string) => (T)(object)GetStringValue(row, columnName)!,
                Type t when t == typeof(decimal) => isNullable ? (T?)(object?)ParseDecimalSafeCanEmpty(row, columnName, false) : (T)(object)ParseDecimalSafe(row, columnName, !isNullable),
                Type t when t == typeof(DateTime) => isNullable ? (T?)(object?)ParseDateSafeCanEmpty(row, columnName, false, format) : (T)(object)ParseDateSafe(row, columnName, !isNullable, format),
                Type t when t == typeof(Guid) => (T)(object)ParseGuidSafe(row, columnName, !isNullable),
                Type t when t == typeof(int) => isNullable ? (T?)(object?)ParseIntSafeCanEmpty(row, columnName, false) : (T)(object)ParseIntSafe(row, columnName, !isNullable),
                Type t when t == typeof(long) => isNullable ? (T?)(object?)ParseLongSafeCanEmpty(row, columnName, false) : (T)(object)ParseLongSafe(row, columnName, !isNullable),
                Type t when t == typeof(bool) => (T)(object)ParseBoolSafe(row, columnName, !isNullable),
                Type t when t == typeof(double) => (T)(object)ParseDoubleSafe(row, columnName, !isNullable),
                Type t when t == typeof(float) => (T)(object)ParseFloatSafe(row, columnName, !isNullable),
                _ when targetType.IsEnum => (T)ParseEnumSafe(row, columnName, targetType, !isNullable),
                _ => throw new NotSupportedException($"Type {type.Name} is not supported")
            };
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to convert value for column '{columnName}' to type {type.Name}", ex);
        }
    }

    public static string? GetStringValue(IDictionary<string, object> row, string columnName)
    {
        if (row == null || !row.ContainsKey(columnName))
        {
            return null;
        }
        var value = row[columnName]?.ToString()?.Trim();
        return value;
    }

    public static decimal ParseDecimalSafe(IDictionary<string, object> row, string columnName, bool throwOnNull)
    {
        if (!row.TryGetValue(columnName, out object? value) || value == null)
        {
            if (throwOnNull)
                throw new InvalidOperationException($"Cannot convert null value to decimal for column '{columnName}'");
            return 0m;
        }

        if (decimal.TryParse(value.ToString(), out decimal result))
        {
            result = decimal.Round(result, QuoteFlowSharedConsts.SystemDecimalPlaces);
            return result;
        }

        if (throwOnNull)
            throw new FormatException($"Cannot parse '{value}' as decimal for column '{columnName}'");
        return 0m;
    }
    public static decimal? ParseDecimalSafeCanEmpty(IDictionary<string, object> row, string columnName, bool throwOnNull)
    {
        if (!row.TryGetValue(columnName, out object? value) || value == null)
        {
            // If the value is not found or is null, return null as per the method's purpose.
            return null;
        }

        if (decimal.TryParse(value.ToString(), out decimal result))
        {
            // If parsing succeeds, round the result and return it.
            result = decimal.Round(result, QuoteFlowSharedConsts.SystemDecimalPlaces);
            return result;
        }

        if (throwOnNull)
        {
            throw new FormatException($"Cannot parse '{value}' as decimal for column '{columnName}'");
        }

        return null;
    }
    public static DateTime ParseDateSafe(IDictionary<string, object> row, string columnName, bool throwOnNull, string? format = null)
    {
        if (!row.TryGetValue(columnName, out object? value) || value == null)
        {
            if (throwOnNull)
                throw new InvalidOperationException($"Cannot convert null value to DateTime for column '{columnName}'");
            return default(DateTime);
        }

        var dateString = value.ToString();
        var effectiveFormat = format ?? "M/d/yyyy h:mm:ss tt";
        var culture = CultureInfo.InvariantCulture;

        if (DateTime.TryParseExact(dateString, effectiveFormat, culture, DateTimeStyles.None, out DateTime result))
            return result;

        effectiveFormat = "dd/MM/yyyy";
        if (DateTime.TryParseExact(dateString, effectiveFormat, culture, DateTimeStyles.None, out result))
            return result;

        if (double.TryParse(dateString, out double excelDateSerial))
        {
            return DateTime.FromOADate(excelDateSerial);
        }

        if (throwOnNull)
            throw new FormatException($"Cannot parse '{value}' as DateTime (expected format: {effectiveFormat}) for column '{columnName}'");

        return default;
    }
    public static DateTime? ParseDateSafeCanEmpty(IDictionary<string, object> row, string columnName, bool throwOnNull, string? format = null)
    {
        if (!row.TryGetValue(columnName, out object? value) || value == null)
        {
            return null;
        }

        var dateString = value.ToString();
        var effectiveFormat = format ?? "dd/MM/yyyy";
        var culture = CultureInfo.InvariantCulture;

        if (DateTime.TryParseExact(dateString, effectiveFormat, culture, DateTimeStyles.None, out DateTime result))
            return result;

        effectiveFormat = "M/d/yyyy h:mm:ss tt";
        if (DateTime.TryParseExact(dateString, effectiveFormat, culture, DateTimeStyles.None, out result))
            return result;


        effectiveFormat = "yyyy/MM/dd";
        if (DateTime.TryParseExact(dateString, effectiveFormat, culture, DateTimeStyles.None, out result))
            return result;

        if (double.TryParse(dateString, out double excelDateSerial))
        {
            return DateTime.FromOADate(excelDateSerial);
        }

        return null;
    }
    public static Guid InValidGuidSafe(IDictionary<string, object> row, string columnName, Guid guid, bool throwOnNull)
    {
        if (!row.TryGetValue(columnName, out object? value) || guid == Guid.Empty)
        {

            return default;

        }
        return guid;

    }
    public static Guid ParseGuidSafe(IDictionary<string, object> row, string columnName, bool throwOnNull)
    {
        if (!row.TryGetValue(columnName, out object? value) || value == null)
        {
            if (throwOnNull)
                throw new InvalidOperationException($"Cannot convert null value to Guid for column '{columnName}'");
            return default(Guid);
        }

        if (Guid.TryParse(value.ToString(), out Guid result))
            return result;

        if (throwOnNull)
            throw new FormatException($"Cannot parse '{value}' as Guid for column '{columnName}'");
        return default(Guid);
    }

    public static int ParseIntSafe(IDictionary<string, object> row, string columnName, bool throwOnNull)
    {
        if (!row.TryGetValue(columnName, out object? value) || value == null)
        {
            if (throwOnNull)
                throw new InvalidOperationException($"Cannot convert null value to int for column '{columnName}'");
            return 0;
        }

        if (int.TryParse(value.ToString(), out int result))
            return result;

        if (throwOnNull)
            throw new FormatException($"Cannot parse '{value}' as int for column '{columnName}'");
        return 0;
    }

    public static int? ParseIntSafeCanEmpty(IDictionary<string, object> row, string columnName, bool throwOnNull)
    {
        if (!row.TryGetValue(columnName, out object? value) || value == null)
        {
            return null;
        }

        if (int.TryParse(value.ToString(), out int result))
        {
            return result;
        }

        if (throwOnNull)
        {
            throw new FormatException($"Cannot parse '{value}' as int for column '{columnName}'");
        }

        return null;
    }

    public static long ParseLongSafe(IDictionary<string, object> row, string columnName, bool throwOnNull)
    {
        if (!row.TryGetValue(columnName, out object? value) || value == null)
        {
            if (throwOnNull)
                throw new InvalidOperationException($"Cannot convert null value to long for column '{columnName}'");
            return 0L;
        }

        if (long.TryParse(value.ToString(), out long result))
            return result;

        if (throwOnNull)
            throw new FormatException($"Cannot parse '{value}' as long for column '{columnName}'");
        return 0L;
    }

    public static long? ParseLongSafeCanEmpty(IDictionary<string, object> row, string columnName, bool throwOnNull)
    {
        if (!row.TryGetValue(columnName, out object? value) || value == null)
        {
            return null;
        }

        if (long.TryParse(value.ToString(), out long result))
        {
            return result;
        }

        if (throwOnNull)
        {
            throw new FormatException($"Cannot parse '{value}' as long for column '{columnName}'");
        }

        return null;
    }

    public static bool ParseBoolSafe(IDictionary<string, object> row, string columnName, bool throwOnNull)
    {
        if (!row.TryGetValue(columnName, out object? value) || value == null)
        {
            if (throwOnNull)
                throw new InvalidOperationException($"Cannot convert null value to bool for column '{columnName}'");
            return false;
        }

        if (bool.TryParse(value.ToString(), out bool result))
            return result;

        // Handle common boolean representations
        var stringValue = value.ToString()?.ToLowerInvariant();
        if (stringValue == "1" || stringValue == "yes" || stringValue == "y")
            return true;
        if (stringValue == "0" || stringValue == "no" || stringValue == "n")
            return false;

        if (throwOnNull)
            throw new FormatException($"Cannot parse '{value}' as bool for column '{columnName}'");
        return false;
    }

    public static double ParseDoubleSafe(IDictionary<string, object> row, string columnName, bool throwOnNull)
    {
        if (!row.TryGetValue(columnName, out object? value) || value == null)
        {
            if (throwOnNull)
                throw new InvalidOperationException($"Cannot convert null value to double for column '{columnName}'");
            return 0d;
        }

        if (double.TryParse(value.ToString(), out double result))
            return result;

        if (throwOnNull)
            throw new FormatException($"Cannot parse '{value}' as double for column '{columnName}'");
        return 0d;
    }

    public static float ParseFloatSafe(IDictionary<string, object> row, string columnName, bool throwOnNull)
    {
        if (!row.TryGetValue(columnName, out object? value) || value == null)
        {
            if (throwOnNull)
                throw new InvalidOperationException($"Cannot convert null value to float for column '{columnName}'");
            return 0f;
        }

        if (float.TryParse(value.ToString(), out float result))
            return result;

        if (throwOnNull)
            throw new FormatException($"Cannot parse '{value}' as float for column '{columnName}'");
        return 0f;
    }

    public static object ParseEnumSafe(IDictionary<string, object> row, string columnName, Type enumType, bool throwOnNull)
    {
        if (!row.TryGetValue(columnName, out object? value) || value == null)
        {
            if (throwOnNull)
                throw new InvalidOperationException($"Cannot convert null value to {enumType.Name} for column '{columnName}'");
            return Activator.CreateInstance(enumType)!;
        }

        var stringValue = value.ToString();
        if (Enum.TryParse(enumType, stringValue, true, out object? result))
            return result;

        // Try parsing as integer
        if (int.TryParse(stringValue, out int intValue) && Enum.IsDefined(enumType, intValue))
            return Enum.ToObject(enumType, intValue);

        if (throwOnNull)
            throw new FormatException($"Cannot parse '{value}' as {enumType.Name} for column '{columnName}'");
        return Activator.CreateInstance(enumType)!;
    }

    // Keep your original methods for nullable versions
    public static DateTime? ParseDate(IDictionary<string, object> row, string columnName)
    {
        if (row == null || !row.TryGetValue(columnName, out object? value) || value == null)
        {
            return null;
        }

        if (DateTime.TryParse(value.ToString(), out DateTime result))
        {
            return result;
        }
        return null;
    }
    public static void ValidateDecimal(IDictionary<string, object> rowData, string column, string fieldName, int rowIndex, ValidationResult result, bool wholeNumber)
    {
        var value = GetValue<string?>(rowData, column);

        if (string.IsNullOrWhiteSpace(value))
            result.Errors.Add($"Row {rowIndex}: {fieldName} ({column}) is required.");
        else if (wholeNumber && (!decimal.TryParse(value, out var d) || d != Math.Truncate(d)))
            result.Errors.Add($"Row {rowIndex}: {fieldName} ({column}) is not a valid whole. Please enter a valid whole number (e.g., 0,1,2,3,4,...).");
        else if (!decimal.TryParse(value, out _) && !wholeNumber)
            result.Errors.Add($"Row {rowIndex}: {fieldName} ({column}) is not a valid decimal. Please enter a valid decimal number (e.g., 123.45, 0, 10.5,...).");
    }

    public static void ValidateDate(IDictionary<string, object> rowData, string column, string fieldName, int rowIndex, ValidationResult result)
    {
        var value = GetValue<string?>(rowData, column);

        if (string.IsNullOrWhiteSpace(value))
        {
            result.Errors.Add($"Row {rowIndex}: {fieldName} ({column}) is required.");
            return;
        }
        value = value.Split(' ')[0].Trim();
        var formats = new[] { "dd/MM/yyyy", "d/M/yyyy", "dd-MM-yyyy", "d-M-yyyy", "MM/dd/yyyy" };

        if (!DateTime.TryParseExact(value, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            result.Errors.Add($"Row {rowIndex}: {fieldName} ({column}) is not a valid date. Please enter a valid date.");
        }
    }

    public static void ValidateAmountCalculation(
    IDictionary<string, object> rowData,
    string costColumn, string qtyColumn, string amountColumn,
    string costFieldName, string qtyFieldName, string amountFieldName,
    int rowIndex,
    ValidationResult result,
    double tolerance = 0.01)
    {
        var cost = GetValue<decimal?>(rowData, costColumn);
        var quantity = GetValue<decimal?>(rowData, qtyColumn);
        var amount = GetValue<decimal?>(rowData, amountColumn);

        if (cost == null || quantity == null || amount == null)
            return;

        var expected = cost.Value * quantity.Value;
        if (Math.Abs(expected - amount.Value) > (decimal)tolerance)
        {
            result.Errors.Add(
                $"Row {rowIndex}: {amountFieldName} ({amountColumn}) must equal {costFieldName} ({costColumn}) × {qtyFieldName} ({qtyColumn}). " +
                $"Expected: {expected.ToString("N0")}, but got: {amount.Value.ToString("N0")}."
            );
        }
    }


}
