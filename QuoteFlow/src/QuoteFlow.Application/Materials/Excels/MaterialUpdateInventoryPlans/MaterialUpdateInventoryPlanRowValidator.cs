using QuoteFlow.Shared.Excels;
using System.Collections.Generic;

namespace QuoteFlow.Materials.Excels.MaterialUpdateInventoryPlans;
public class MaterialUpdateInventoryPlanRowValidator : IExcelRowValidator<MaterialUpdateInventoryPlanImportDto>
{
    public ValidationResult ValidateRow(IDictionary<string, object> rowData, int rowIndex)
    {
        var result = new ValidationResult();

        var golfaCode = ExcelParser.GetValue<string?>(rowData, "A");
        var model = ExcelParser.GetValue<string?>(rowData, "B");
        var stockWarning = ExcelParser.GetValue<string?>(rowData, "D");

        if (string.IsNullOrWhiteSpace(golfaCode))
            result.AddError("Golfa Code is required.");
        if (string.IsNullOrWhiteSpace(model))
            result.AddError("Model is required.");
        if (!string.IsNullOrWhiteSpace(stockWarning) && stockWarning.ToUpper() != "NULL")
        {
            if (!int.TryParse(stockWarning, out var parsedStockWarning) || parsedStockWarning < 0)
            {
                result.AddError("Stock Warning must be a valid non-negative integer.");
            }
        }

        return result;
    }

    public MaterialUpdateInventoryPlanImportDto ParseRow(IDictionary<string, object> rowData)
    {
        string? GetString(string column)
        {
            var value = ExcelParser.GetValue<string?>(rowData, column)?.Trim();
            return value?.ToUpper() == "NULL" ? "-1" : value;
        }

        // helper cho int
        int? GetInt(string column)
        {
            var value = ExcelParser.GetValue<string?>(rowData, column)?.Trim();
            if (value?.ToUpper() == "NULL")
            {
                return -1;
            }
            return ExcelParser.GetValue<int?>(rowData, column);
        }

        return new MaterialUpdateInventoryPlanImportDto
        {
            GolfaCode = GetString("A") ?? string.Empty,
            Model = GetString("B") ?? string.Empty,
            InventoryCategory = GetString("C"),
            StockWarning = GetInt("D"),
        };
    }
}
