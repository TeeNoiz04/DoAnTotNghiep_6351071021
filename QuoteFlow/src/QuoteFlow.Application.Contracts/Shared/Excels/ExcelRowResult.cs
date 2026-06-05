using System.Collections.Generic;
namespace QuoteFlow.Shared.Excels;
public class ExcelRowResult<T>
{
    public T RowData { get; set; }
    public List<string> Errors { get; set; } = [];
    public List<string> Warnings { get; set; } = [];
    public int RowIndex { get; set; }
    public bool HasErrors => Errors.Count != 0;
    public bool HasWarnings => Warnings?.Count > 0;
    public ExcelRowResult(
        T data,
        List<string> errors,
        int rowIndex)
    {
        RowData = data;
        Errors = errors;
        RowIndex = rowIndex;
        Warnings = [];
    }
    public ExcelRowResult()
    {
        RowData = default!;
        Errors = [];
        Warnings = [];
        RowIndex = 0;
    }
}