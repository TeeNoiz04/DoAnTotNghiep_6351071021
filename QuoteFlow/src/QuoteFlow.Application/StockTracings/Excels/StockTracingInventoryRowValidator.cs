using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;

namespace QuoteFlow.StockTracings.Excels;

public class StockTracingInventoryRowValidator : IExcelRowValidator<StockTracingInventoryImportDto>
{
    private const string DATE_TIME_FORMAT = "dd/MM/yyyy";

    public StockTracingInventoryImportDto ParseRow(IDictionary<string, object> rowData)
    {
        return new StockTracingInventoryImportDto
        {
            RowNo = ExcelParser.GetValue<int?>(rowData, "A"),
            WareHouse = ExcelParser.GetValue<string?>(rowData, "B")?.Trim(),
            BUs = ExcelParser.GetValue<string?>(rowData, "C")?.Trim(),
            Customer = ExcelParser.GetValue<string?>(rowData, "D")?.Trim(),
            Category = ExcelParser.GetValue<string?>(rowData, "E")?.Trim(),
            SKUCode = ExcelParser.GetValue<string?>(rowData, "F")?.Trim(),
            SKUName = ExcelParser.GetValue<string?>(rowData, "G")?.Trim(),
            Quality = ExcelParser.GetValue<string?>(rowData, "H")?.Trim(),
            Warranty = ExcelParser.GetValue<string?>(rowData, "I")?.Trim(),
            Unit = ExcelParser.GetValue<string?>(rowData, "J")?.Trim(),
            Series = ExcelParser.GetValue<string?>(rowData, "K")?.Trim(),
            OrginCode = ExcelParser.GetValue<string?>(rowData, "L")?.Trim(),
            ProductionDate = ExcelParser.GetValue<DateTime?>(rowData, "M", DATE_TIME_FORMAT),
            GolfaCode = ExcelParser.GetValue<string?>(rowData, "N")?.Trim(),
            Note = ExcelParser.GetValue<string?>(rowData, "O")?.Trim(),
        };
    }

    public ValidationResult ValidateRow(IDictionary<string, object> rowData, int rowIndex)
    {
        var result = new ValidationResult();

        var rowNo = ExcelParser.GetValue<string?>(rowData, "A");
        //var wareHouse = ExcelParser.GetValue<string?>(rowData, "B")?.Trim();
        //var BUs = ExcelParser.GetValue<string?>(rowData, "C")?.Trim();
        //var customer = ExcelParser.GetValue<string?>(rowData, "D")?.Trim();
        //var category = ExcelParser.GetValue<string?>(rowData, "E")?.Trim();
        //var skuCode = ExcelParser.GetValue<string?>(rowData, "F")?.Trim();
        //var skuName = ExcelParser.GetValue<string?>(rowData, "G")?.Trim();
        //var quality = ExcelParser.GetValue<string?>(rowData, "H")?.Trim();
        //var warranty = ExcelParser.GetValue<string?>(rowData, "I")?.Trim();
        //var unit = ExcelParser.GetValue<string?>(rowData, "J")?.Trim();
        var series = ExcelParser.GetValue<string?>(rowData, "K")?.Trim();
        //var originCode = ExcelParser.GetValue<string?>(rowData, "L")?.Trim();
        var productionDate = ExcelParser.GetValue<DateTime?>(rowData, "M", DATE_TIME_FORMAT); ;
        var golfaCode = ExcelParser.GetValue<string?>(rowData, "N")?.Trim();
        // Required field validations

        if (!int.TryParse(rowNo, out int no))
            result.Errors.Add($"RowNo is number.");
        if (string.IsNullOrWhiteSpace(series))
            result.Errors.Add($"Series is required.");

        // Validate production date if present
        if (productionDate.HasValue && productionDate.Value == DateTime.MinValue)
            result.Errors.Add($"ProductionDate is invalid.");

        if (string.IsNullOrWhiteSpace(golfaCode))
            result.Errors.Add($"Golfa Code is required.");

        return result;
    }
}
