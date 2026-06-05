using QuoteFlow.Shared.Utils;
using System.Collections.Generic;
using System.Linq;
namespace QuoteFlow.Shared.Excels;
public static class ExcelUtils
{
    public static void AddRowErrors<T>(ExcelValidationResult<T> listResult, int rowIndex, List<string> rowErrors, string? prefix = null)
    {
        prefix = StringHelper.AddSpace(prefix);
        var formattedErrors = rowErrors.Select(err => $"{prefix}Row {rowIndex}: {err}");
        foreach (var error in formattedErrors)
        {
            if (!listResult.Errors.Contains(error))
            {
                listResult.Errors.Add(error);
            }
        }
    }

    public static void AddRowWarnings<T>(ExcelValidationResult<T> listResult, int rowIndex, List<string> rowWarnings, string? prefix = null)
    {
        prefix = StringHelper.AddSpace(prefix);
        var formattedWarnings = rowWarnings.Select(w => $"{prefix}Row {rowIndex}: {w}");
        foreach (var warning in formattedWarnings)
        {
            if (!listResult.Warnings.Contains(warning))
            {
                listResult.Warnings.Add(warning);
            }
        }
    }

    public static void AddChildListErrors<T, TChild>(ExcelRowResult<T> rowResult, ExcelValidationResult<TChild> childResult, string? prefix = null)
    {
        prefix = StringHelper.AddSpace(prefix);
        var childListData = childResult.ListData;
        foreach (var row in childListData)
        {
            var rowIndex = row.RowIndex;
            AddChildErrors(rowResult, rowIndex, row.Errors, prefix);
        }
        AddGeneralErrors(rowResult, childResult.Errors, prefix);
    }

    public static void AddGeneralErrors<T>(ExcelRowResult<T> rowResult, List<string> notRowErrors, string? prefix = null)
    {
        prefix = StringHelper.AddSpace(prefix);
        var formattedErrors = notRowErrors
            .Where(err => !err.StartsWith("Row "))
            .Select(err => $"{prefix}{err}");
        foreach (var error in formattedErrors)
        {
            if (!rowResult.Errors.Contains(error))
            {
                rowResult.Errors.Add(error);
            }
        }
    }

    public static void AddChildErrors<T>(ExcelRowResult<T> rowResult, int childRowIndex, List<string> childErrors, string? prefix = null)
    {
        prefix = StringHelper.AddSpace(prefix);
        var formattedErrors = childErrors.Select(err => $"{prefix}Child Row {childRowIndex}: {err}");
        foreach (var error in formattedErrors)
        {
            if (!rowResult.Errors.Contains(error))
            {
                rowResult.Errors.Add(error);
            }
        }
    }
}