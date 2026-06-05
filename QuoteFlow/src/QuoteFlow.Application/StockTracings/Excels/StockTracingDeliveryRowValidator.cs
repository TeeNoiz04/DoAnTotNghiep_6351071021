using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;
namespace QuoteFlow.StockTracings.Excels;

public class StockTracingDeliveryRowValidator : IExcelRowValidator<StockTracingDeliveryImportDto>
{
    private const string DATE_TIME_FORMAT = "dd/MM/yyyy HH:mm:ss";
    private const string DATE_TIME = "dd/MM/yyyy";
    public StockTracingDeliveryImportDto ParseRow(IDictionary<string, object> rowData)
    {
        return new StockTracingDeliveryImportDto
        {
            RowNo = ExcelParser.GetValue<int?>(rowData, "A"),
            CheckListCode = ExcelParser.GetValue<string?>(rowData, "B")?.Trim(),
            DateEntered = ExcelParser.GetValue<DateTime?>(rowData, "C", DATE_TIME_FORMAT),
            Stock = ExcelParser.GetValue<string?>(rowData, "D")?.Trim(),
            BU = ExcelParser.GetValue<string?>(rowData, "E")?.Trim(),
            Customer = ExcelParser.GetValue<string?>(rowData, "F")?.Trim(),
            Category = ExcelParser.GetValue<string?>(rowData, "G")?.Trim(),
            GIV = ExcelParser.GetValue<string?>(rowData, "H")?.Trim(),
            Invoice = ExcelParser.GetValue<string?>(rowData, "I")?.Trim(),
            SKUCode = ExcelParser.GetValue<string?>(rowData, "J")?.Trim(),
            SKUName = ExcelParser.GetValue<string?>(rowData, "K")?.Trim(),
            Quality = ExcelParser.GetValue<string?>(rowData, "L")?.Trim(),
            Warranty = ExcelParser.GetValue<string?>(rowData, "M")?.Trim(),
            Unit = ExcelParser.GetValue<string?>(rowData, "N")?.Trim(),
            Series = ExcelParser.GetValue<string?>(rowData, "O")?.Trim(),
            GolfaCode = ExcelParser.GetValue<string?>(rowData, "P")?.Trim(),
            OriginCode = ExcelParser.GetValue<string?>(rowData, "Q")?.Trim(),
            ProductionDate = ExcelParser.GetValue<DateTime?>(rowData, "R", DATE_TIME),
            Location = ExcelParser.GetValue<string?>(rowData, "S")?.Trim(),
            Note = ExcelParser.GetValue<string?>(rowData, "T")?.Trim(),
        };
    }

    public ValidationResult ValidateRow(IDictionary<string, object> rowData, int rowIndex)
    {
        var result = new ValidationResult();

        var no = ExcelParser.GetValue<string?>(rowData, "A").Trim();
        //var checklistCode = ExcelParser.GetValue<string?>(rowData, "B")?.Trim();
        var dateEntered = ExcelParser.GetValue<DateTime?>(rowData, "C", DATE_TIME_FORMAT);
        //var stock = ExcelParser.GetValue<string?>(rowData, "D")?.Trim();
        //var bu = ExcelParser.GetValue<string?>(rowData, "E")?.Trim();
        //var customer = ExcelParser.GetValue<string?>(rowData, "F")?.Trim();
        //var category = ExcelParser.GetValue<string?>(rowData, "G")?.Trim();
        //var giv = ExcelParser.GetValue<string?>(rowData, "H")?.Trim();
        //var invoice = ExcelParser.GetValue<string?>(rowData, "I")?.Trim();
        //var skuCode = ExcelParser.GetValue<string?>(rowData, "J")?.Trim();
        //var skuName = ExcelParser.GetValue<string?>(rowData, "K")?.Trim();
        //var quality = ExcelParser.GetValue<string?>(rowData, "L")?.Trim();
        //var warranty = ExcelParser.GetValue<string?>(rowData, "M")?.Trim();
        //var unit = ExcelParser.GetValue<string?>(rowData, "N")?.Trim();
        var series = ExcelParser.GetValue<string?>(rowData, "O")?.Trim();
        var golfaCode = ExcelParser.GetValue<string?>(rowData, "P")?.Trim();
        //var originCode = ExcelParser.GetValue<string?>(rowData, "Q")?.Trim();
        var productionDate = ExcelParser.GetValue<DateTime?>(rowData, "R", DATE_TIME);
        //var location = ExcelParser.GetValue<string?>(rowData, "S")?.Trim();

        if (!int.TryParse(no, out int rowNo))
            result.Errors.Add($"RowNo is number.");
        if (!dateEntered.HasValue || dateEntered.GetValueOrDefault() == DateTime.MinValue)
            result.Errors.Add($"DateEntered is required or invalid.");
        if (string.IsNullOrWhiteSpace(golfaCode))
            result.Errors.Add($"Golfa is required.");
        if (string.IsNullOrWhiteSpace(series))
            result.Errors.Add($"Series is required.");

        if (productionDate.HasValue && productionDate.GetValueOrDefault() == DateTime.MinValue)
            result.Errors.Add($"ProductionDate is invalid.");

        return result;
    }

}
