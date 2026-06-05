using System.Collections.Generic;

namespace QuoteFlow.Shared.Excels;
public interface IExcelMultiRowValidator<T> : IExcelRowValidator<T>
{
    List<T> ParseRows(IDictionary<string, object> rowData, int rowIndex);
}
