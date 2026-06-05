using QuoteFlow.PurchaseOrdersSapImports.Excel;
using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;

namespace QuoteFlow.PurchaseOrders.Excel;
public class PurchaseOrderSapImportExcelRowValidation : IExcelRowValidator<PurchaseOrdersSapImportsExcelDto>
{
    private const string DATE_TIME = "dd/MM/yyyy";
    public PurchaseOrdersSapImportsExcelDto ParseRow(IDictionary<string, object> rowData)
    {
        return new PurchaseOrdersSapImportsExcelDto
        {

            PONo = ExcelParser.GetValue<string?>(rowData, "A")?.Trim(),
            SAPPODate = ExcelParser.GetValue<DateTime?>(rowData, "B", DATE_TIME),
            SAPPONo = ExcelParser.GetValue<string?>(rowData, "C")?.Trim()
        };
    }

    public ValidationResult ValidateRow(IDictionary<string, object> rowData, int rowIndex)
    {
        var result = new ValidationResult();


        var poNo = ExcelParser.GetValue<string?>(rowData, "A")?.Trim();
        var sapPODate = ExcelParser.GetValue<DateTime?>(rowData, "B", DATE_TIME);
        var sapPONo = ExcelParser.GetValue<string?>(rowData, "C")?.Trim();

        if (string.IsNullOrWhiteSpace(poNo))
            result.Errors.Add($"PO No is required.");
        if (sapPODate.GetValueOrDefault() == DateTime.MinValue)
            result.Errors.Add($"SAP PO Date is invalid.");
        if (string.IsNullOrWhiteSpace(sapPONo))
            result.Errors.Add($"SAP PO No is required.");

        return result;

    }
}
