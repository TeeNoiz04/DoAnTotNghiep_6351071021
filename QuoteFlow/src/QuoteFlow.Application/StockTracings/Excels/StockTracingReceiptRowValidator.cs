using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;

namespace QuoteFlow.StockTracings.Excels;

public class StockTracingReceiptRowValidator : IExcelRowValidator<StockTracingReceiptImportDto>
{
    private const string DATE_TIME_FORMAT = "dd/MM/yyyy HH:mm:ss";
    private const string DATE_FORMAT = "dd/MM/yyyy";

    public StockTracingReceiptImportDto ParseRow(IDictionary<string, object> rowData)
    {
        return new StockTracingReceiptImportDto
        {
            RowNo = ExcelParser.GetValue<int?>(rowData, "A"),
            PackingListCode = ExcelParser.GetValue<string?>(rowData, "B")?.Trim(),
            DateEntered = ExcelParser.GetValue<DateTime?>(rowData, "C", DATE_TIME_FORMAT),
            Stock = ExcelParser.GetValue<string?>(rowData, "D")?.Trim(),
            BUs = ExcelParser.GetValue<string?>(rowData, "E")?.Trim(),
            Customer = ExcelParser.GetValue<string?>(rowData, "F")?.Trim(),
            Category = ExcelParser.GetValue<string?>(rowData, "G")?.Trim(),
            SKUCode = ExcelParser.GetValue<string?>(rowData, "H")?.Trim(),
            SKUName = ExcelParser.GetValue<string?>(rowData, "I")?.Trim(),
            QuaLity = ExcelParser.GetValue<string?>(rowData, "J")?.Trim(),
            Warranty = ExcelParser.GetValue<string?>(rowData, "K")?.Trim(),
            Unit = ExcelParser.GetValue<string?>(rowData, "L")?.Trim(),
            Series = ExcelParser.GetValue<string?>(rowData, "M")?.Trim(),
            OrginCode = ExcelParser.GetValue<string?>(rowData, "N")?.Trim(),
            ProductionDate = ExcelParser.GetValue<DateTime?>(rowData, "O", DATE_FORMAT),
            Location = ExcelParser.GetValue<string?>(rowData, "P")?.Trim(),
            GolfaCode = ExcelParser.GetValue<string?>(rowData, "Q")?.Trim(),
            Note = ExcelParser.GetValue<string?>(rowData, "R")?.Trim(),
        };
    }

    public ValidationResult ValidateRow(IDictionary<string, object> rowData, int rowIndex)
    {
        var result = new ValidationResult();

        var rowNo = ExcelParser.GetValue<string?>(rowData, "A");
        //var packingListCode = ExcelParser.GetValue<string?>(rowData, "B")?.Trim();
        var dateEntered = ExcelParser.GetValue<DateTime?>(rowData, "C", DATE_TIME_FORMAT);
        //var stock = ExcelParser.GetValue<string?>(rowData, "D")?.Trim();
        //var bus = ExcelParser.GetValue<string?>(rowData, "E")?.Trim();
        //var customer = ExcelParser.GetValue<string?>(rowData, "F")?.Trim();
        //var category = ExcelParser.GetValue<string?>(rowData, "G")?.Trim();
        //var skuCode = ExcelParser.GetValue<string?>(rowData, "H")?.Trim();
        //var skuName = ExcelParser.GetValue<string?>(rowData, "I")?.Trim();
        //var quality = ExcelParser.GetValue<string?>(rowData, "J")?.Trim();
        //var warranty = ExcelParser.GetValue<string?>(rowData, "K")?.Trim();
        //var unit = ExcelParser.GetValue<string?>(rowData, "L")?.Trim();
        var series = ExcelParser.GetValue<string?>(rowData, "M")?.Trim();
        //var originCode = ExcelParser.GetValue<string?>(rowData, "N")?.Trim();
        var productionDate = ExcelParser.GetValue<DateTime?>(rowData, "O", DATE_FORMAT);
        //var location = ExcelParser.GetValue<string?>(rowData, "P")?.Trim();
        var golfaCode = ExcelParser.GetValue<string?>(rowData, "Q")?.Trim();

        // Required field validations
        if (!int.TryParse(rowNo, out int no))
            result.Errors.Add($"RowNo is number.");
        if (string.IsNullOrWhiteSpace(series))
            result.Errors.Add($"Series is required.");
        if (!dateEntered.HasValue || dateEntered.Value == DateTime.MinValue)
            result.Errors.Add($"DateEntered is required or invalid.");
        if (productionDate.HasValue && productionDate.Value == DateTime.MinValue)
            result.Errors.Add($"ProductionDate is invalid.");
        if (string.IsNullOrWhiteSpace(golfaCode))
            result.Errors.Add($"Golfa Code is required.");

        return result;
    }
}
