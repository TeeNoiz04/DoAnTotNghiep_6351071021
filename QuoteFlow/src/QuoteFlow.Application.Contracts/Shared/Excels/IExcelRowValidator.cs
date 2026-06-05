using System.Collections.Generic;

namespace QuoteFlow.Shared.Excels;

public interface IExcelRowValidator<T>
{
    ValidationResult ValidateRow(IDictionary<string, object> rowData, int rowIndex);
    T ParseRow(IDictionary<string, object> rowData);
    List<T> ParseRows(IDictionary<string, object> rowData, int rowIndex)
       => new List<T> { ParseRow(rowData) };
}

