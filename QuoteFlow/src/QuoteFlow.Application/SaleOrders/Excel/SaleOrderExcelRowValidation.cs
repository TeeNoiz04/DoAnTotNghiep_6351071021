using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;

namespace QuoteFlow.SaleOrders.Excel;
public class SaleOrderExcelRowValidation : IExcelRowValidator<SaleOrderExcelDto>
{
    private const string DATE_TIME = "dd/MM/yyyy";
    public SaleOrderExcelDto ParseRow(IDictionary<string, object> rowData)
    {
        return new SaleOrderExcelDto
        {
            SONo = ExcelParser.GetValue<string?>(rowData, "A")?.Trim(),
            SAPSONo = ExcelParser.GetValue<string?>(rowData, "B")?.Trim(),
            SAPDONo = ExcelParser.GetValue<string?>(rowData, "C")?.Trim(),
            SAPBillingNo = ExcelParser.GetValue<string?>(rowData, "D")?.Trim(),
            SAPINV = ExcelParser.GetValue<string?>(rowData, "E")?.Trim(),
            SAPINVDate = ExcelParser.GetValue<DateTime?>(rowData, "F", DATE_TIME),
        };
    }

    public ValidationResult ValidateRow(IDictionary<string, object> rowData, int rowIndex)
    {
        var result = new ValidationResult();
        var sAPINVDate = ExcelParser.GetValue<DateTime?>(rowData, "F", DATE_TIME);
        if (sAPINVDate.HasValue && sAPINVDate.GetValueOrDefault() == DateTime.MinValue)
            result.Errors.Add($"SAP INV Date is invalid.");

        return result;
    }
}
