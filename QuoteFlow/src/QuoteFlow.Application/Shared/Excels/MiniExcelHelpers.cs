using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Content;

namespace QuoteFlow.Shared.Excels;

public static class MiniExcelHelper
{
    public static IEnumerable<IDictionary<string, object>> ReadExcelRows(
        Stream stream,
        string mainSheetName,
        string topLeftDataHeaderCell = "A1",
        string bottomRightDataHeaderCell = "Z1000",
        bool dataRangeHasHeader = true
    )
    {
        stream.Seek(0, SeekOrigin.Begin); // Reset position before reading
        try
        {
            var result = MiniExcel.QueryRange(
                stream: stream,
                startCell: topLeftDataHeaderCell,
                endCell: bottomRightDataHeaderCell,
                useHeaderRow: dataRangeHasHeader,
                sheetName: mainSheetName)
            .Cast<IDictionary<string, object>>()
            .ToList();
            return result;
        }
        catch (NotSupportedException ex)
        {
            throw new UserFriendlyException($"Only .xlsx Excel files are supported. Please resave your file as .xlsx and try again. ");
        }
        catch (InvalidOperationException)
        {
            throw new UserFriendlyException($"Invalid sheet name. Please ensure your data is inside the sheet named '{mainSheetName}'.");
        }
    }

    public static IEnumerable<IDictionary<string, object>> ReadExcelRows(
        Stream stream,
        string topLeftDataHeaderCell = "A1",
        string bottomRightDataHeaderCell = "Z1000",
        bool dataRangeHasHeader = true
    )
    {
        stream.Seek(0, SeekOrigin.Begin); // Reset position before reading
        var result = MiniExcel.QueryRange(
            stream: stream,
            startCell: topLeftDataHeaderCell,
            endCell: bottomRightDataHeaderCell,
            useHeaderRow: dataRangeHasHeader)
        .Cast<IDictionary<string, object>>()
        .ToList();
        return result;
    }

    public static IEnumerable<IDictionary<string, object>> ReadExcelRows(
        Stream stream,
        string mainSheetName,
        string headerToFind,
        string startColumn = "A", // E.g. "A"
        string endColumn = "Z",
        bool dataRangeHasHeader = true
    )
    {
        stream.Seek(0, SeekOrigin.Begin);

        try
        {
            // Use streaming approach to avoid loading all data into memory at once
            var rows = MiniExcel.QueryRange(
                stream: stream,
                sheetName: mainSheetName,
                startCell: $"{startColumn}1",
                endCell: $"{endColumn}1",
                useHeaderRow: false);

            var rowList = new List<IDictionary<string, object>>();
            int currentRowIndex = 1; // Excel is 1-indexed
            int headerRowIndex = -1;
            bool headerFound = false;

            int startColNum = ColumnLetterToNumber(startColumn);
            int endColNum = ColumnLetterToNumber(endColumn);

            foreach (var row in rows)
            {
                if (row is IDictionary<string, object> rowDict)
                {
                    // If we haven't found the header yet, search for it
                    if (!headerFound)
                    {
                        foreach (var cell in rowDict.Values)
                        {
                            if (cell?.ToString()?.Trim().Equals(headerToFind, StringComparison.OrdinalIgnoreCase) == true)
                            {
                                headerRowIndex = dataRangeHasHeader ? currentRowIndex + 1 : currentRowIndex + 2;
                                headerFound = true;
                                break;
                            }
                        }
                    }

                    // If header is found and we're at or past the data start row, filter and collect
                    if (headerFound && currentRowIndex >= headerRowIndex - (dataRangeHasHeader ? 1 : 0))
                    {
                        var filteredRow = FilterRowColumns(rowDict, startColNum, endColNum);
                        if (filteredRow.Any())
                        {
                            rowList.Add(filteredRow);
                        }
                    }
                }
                currentRowIndex++;
            }

            if (!headerFound)
            {
                throw new UserFriendlyException($"Cannot locate the header '{headerToFind}' in sheet '{mainSheetName}'.");
            }

            return rowList;
        }
        catch (NotSupportedException ex)
        {
            throw new UserFriendlyException($"Only .xlsx Excel files are supported. Please resave your file as .xlsx and try again. ");
        }
        catch (InvalidOperationException)
        {
            throw new UserFriendlyException($"Invalid sheet name. Please ensure your data is inside the sheet named '{mainSheetName}'.");
        }
    }

    public static async Task<IEnumerable<IDictionary<string, object>>> ReadExcelRowsAsync(
        Stream stream,
        string mainSheetName,
        string headerToFind,
        string startColumn = "A",
        string endColumn = "Z",
        bool dataRangeHasHeader = true
    )
    {
        stream.Seek(0, SeekOrigin.Begin);

        try
        {
            // Use streaming approach to avoid loading all data into memory at once
            var rows = await MiniExcel.QueryAsync(
                stream: stream,
                sheetName: mainSheetName,
                useHeaderRow: false);

            var rowList = new List<IDictionary<string, object>>();
            int currentRowIndex = 1; // Excel is 1-indexed
            int headerRowIndex = -1;
            bool headerFound = false;

            int startColNum = ColumnLetterToNumber(startColumn);
            int endColNum = ColumnLetterToNumber(endColumn);

            foreach (var row in rows)
            {
                if (row is IDictionary<string, object> rowDict)
                {
                    // If we haven't found the header yet, search for it
                    if (!headerFound)
                    {
                        foreach (var cell in rowDict.Values)
                        {
                            if (cell?.ToString()?.Trim().Equals(headerToFind, StringComparison.OrdinalIgnoreCase) == true)
                            {
                                headerRowIndex = dataRangeHasHeader ? currentRowIndex + 1 : currentRowIndex + 2;
                                headerFound = true;
                                break;
                            }
                        }
                    }

                    // If header is found and we're at or past the data start row, filter and collect
                    if (headerFound && currentRowIndex >= headerRowIndex - (dataRangeHasHeader ? 1 : 0))
                    {
                        var filteredRow = FilterRowColumns(rowDict, startColNum, endColNum);
                        if (filteredRow.Any())
                        {
                            rowList.Add(filteredRow);
                        }
                    }
                }
                currentRowIndex++;
            }

            if (!headerFound)
            {
                throw new UserFriendlyException($"Cannot locate the header '{headerToFind}' in sheet '{mainSheetName}'.");
            }

            return rowList;
        }
        catch (NotSupportedException ex)
        {
            throw new UserFriendlyException($"Only .xlsx Excel files are supported. Please resave your file as .xlsx and try again. ");
        }
        catch (InvalidOperationException)
        {
            throw new UserFriendlyException($"Invalid sheet name. Please ensure your data is inside the sheet named '{mainSheetName}'.");
        }
    }

    private static IDictionary<string, object> FilterRowColumns(
        IDictionary<string, object> row,
        int startColNum,
        int endColNum)
    {
        var filteredRow = new Dictionary<string, object>();

        foreach (var kvp in row)
        {
            // Extract column letter from key (assuming format like "A", "B", "AA", etc.)
            if (TryExtractColumnNumber(kvp.Key, out int colNum))
            {
                if (colNum >= startColNum && colNum <= endColNum)
                {
                    var trimmedValue = kvp.Value is string str ? str.Trim() : kvp.Value;
                    filteredRow[kvp.Key] = trimmedValue;
                }
            }
        }

        return filteredRow;
    }

    private static bool TryExtractColumnNumber(string cellReference, out int columnNumber)
    {
        columnNumber = 0;

        // Extract only the letter part from cell reference (e.g., "A1" -> "A", "AA1" -> "AA")
        var columnPart = new string(cellReference.TakeWhile(char.IsLetter).ToArray());

        if (string.IsNullOrEmpty(columnPart))
            return false;

        columnNumber = ColumnLetterToNumber(columnPart);
        return true;
    }

    private static int ColumnLetterToNumber(string columnLetter)
    {
        int result = 0;
        for (int i = 0; i < columnLetter.Length; i++)
        {
            result = result * 26 + (columnLetter[i] - 'A' + 1);
        }
        return result;
    }

    public static async Task<Stream> GetSeekableStreamAsync(IRemoteStreamContent formFile)
    {
        var originalStream = formFile.GetStream();
        if (originalStream.CanSeek)
        {
            return originalStream; // No need to copy
        }

        var memoryStream = new MemoryStream();
        await originalStream.CopyToAsync(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream;
    }

    public static IEnumerable<IDictionary<string, object>> FilterBlankRows(
    IEnumerable<IDictionary<string, object>> rows,
    string startColumn,
    string stopColumn,
    string? stopColumnOverride = null)
    {
        ArgumentNullException.ThrowIfNull(rows, nameof(rows));
        if (string.IsNullOrWhiteSpace(startColumn))
            throw new ArgumentException("Start column cannot be null or empty", nameof(startColumn));
        if (string.IsNullOrWhiteSpace(stopColumn))
            throw new ArgumentException("Stop column cannot be null or empty", nameof(stopColumn));

        var rowsList = rows as IList<IDictionary<string, object>> ?? [.. rows];
        if (!rowsList.Any())
            return rowsList;

        var headers = rowsList.First().Keys.ToList();
        if (headers.Count == 0)
            return rowsList;

        int startIndex = headers.IndexOf(startColumn);
        int stopIndex = headers.IndexOf(stopColumn);

        if (startIndex == -1)
            throw new ArgumentException($"Start column '{startColumn}' not found in data. Available columns: {string.Join(", ", headers)}", nameof(startColumn));

        if (stopIndex == -1)
            throw new ArgumentException($"Stop column '{stopColumn}' not found in data. Available columns: {string.Join(", ", headers)}", nameof(stopColumn));

        if (startIndex > stopIndex)
            throw new ArgumentException($"Start column '{startColumn}' comes after stop column '{stopColumn}' in column order");

        // Use stopColumnOverride if provided, otherwise use the original stopColumn
        string columnToCheck = stopColumnOverride ?? stopColumn;

        // Validate stopColumnOverride exists in headers if provided
        if (!string.IsNullOrWhiteSpace(stopColumnOverride) && !headers.Contains(stopColumnOverride))
        {
            throw new ArgumentException($"Stop column override '{stopColumnOverride}' not found in data. Available columns: {string.Join(", ", headers)}", nameof(stopColumnOverride));
        }

        // For stopColumnOverride, we only check that specific column
        // For normal operation, we check the range from startColumn to stopColumn
        var columnsToCheck = string.IsNullOrWhiteSpace(stopColumnOverride)
            ? headers.Skip(startIndex).Take(stopIndex - startIndex + 1).ToList()
            : new List<string> { columnToCheck };

        var result = new List<IDictionary<string, object>>();

        foreach (var row in rowsList)
        {
            if (HasValueInRange(row, columnsToCheck))
            {
                result.Add(row);
            }
            else
            {
                break; // Stop immediately when a blank row is encountered
            }
        }

        return result;
    }

    private static bool HasValueInRange(IDictionary<string, object> row, IEnumerable<string> columnsToCheck)
    {
        return columnsToCheck.Any(column => row.TryGetValue(column, out var value) && !IsEmptyValue(value));
    }

    private static bool IsEmptyValue(object value)
    {
        return value == null ||
               (value is string str && string.IsNullOrWhiteSpace(str)) ||
               (value is DBNull);
    }
}

