using QuoteFlow.Materials.MaterialImport.MaterialStatus;
using QuoteFlow.Shared.Excels;
using QuoteFlow.Shared.Models;
using System;
using System.Collections.Generic;

namespace QuoteFlow.Materials.Excels.MaterialStatus;

public class MaterialStatusRowValidator : IExcelRowValidator<MaterialStatusUpdateExcelDto>
{
    private const string DATE_TIME = "dd/MM/yyyy";
    public MaterialStatusRowValidator(IMaterialRepository materialMaterialRepository = null)
    {
    }

    public MaterialStatusUpdateExcelDto ParseRow(IDictionary<string, object> rowData)
    {
        return new MaterialStatusUpdateExcelDto
        {
            GolfaCode = ExcelParser.GetValue<string?>(rowData, "A")?.Trim(),
            Model = ExcelParser.GetValue<string?>(rowData, "B")?.Trim(),
            AcceptanceDate = ExcelParser.GetValue<DateTime?>(rowData, "C"),
            ActiveDate = ExcelParser.GetValue<DateTime?>(rowData, "D"),
            Action = ExcelParser.GetValue<string?>(rowData, "E")?.Trim(),
            Source = ExcelParser.GetValue<string?>(rowData, "F")?.Trim(),
            Reason = ExcelParser.GetValue<string?>(rowData, "G")?.Trim(),
            FactoryRefDoc = ExcelParser.GetValue<string?>(rowData, "H")?.Trim(),
        };
    }


    public ValidationResult ValidateRow(IDictionary<string, object> rowData, int rowIndex)
    {
        var result = new ValidationResult();

        var golfaCode = ExcelParser.GetValue<string?>(rowData, "A")?.Trim();
        var model = ExcelParser.GetValue<string?>(rowData, "B")?.Trim();
        var acceptanceDate = ExcelParser.GetValue<DateTime?>(rowData, "C");
        var activeDate = ExcelParser.GetValue<DateTime?>(rowData, "D");
        var action = ExcelParser.GetValue<string?>(rowData, "E")?.Trim();
        var source = ExcelParser.GetValue<string?>(rowData, "F")?.Trim();
        var reason = ExcelParser.GetValue<string?>(rowData, "G")?.Trim();
        var actoryRefDoc = ExcelParser.GetValue<string?>(rowData, "H")?.Trim();

        if (string.IsNullOrWhiteSpace(golfaCode))
        {
            result.Errors.Add($"Marerial Code is required.");
        }
        if (string.IsNullOrWhiteSpace(model))
        {
            result.Errors.Add($"Model is required.");
        }
        if (string.IsNullOrWhiteSpace(source))
        {
            result.Errors.Add($"Source is required.");
        }
        if (string.IsNullOrWhiteSpace(reason))
        {
            result.Errors.Add($"Reason is required.");
        }
        if (activeDate is null || activeDate.Value == DateTime.MinValue)
        {
            result.Errors.Add($"Action Date is invalid or required.");
        }
        //if (activeDate.HasValue && activeDate.GetValueOrDefault() == DateTime.MinValue)
        //{
        //    result.Errors.Add($"Active date is invalid.");
        //}
        bool isInvalidAction = !string.Equals(action, HistoryActions.Material.Discontinue, StringComparison.OrdinalIgnoreCase)
            && !string.Equals(action, HistoryActions.Material.Active, StringComparison.OrdinalIgnoreCase)
            && !string.Equals(action, HistoryActions.Material.Deactive, StringComparison.OrdinalIgnoreCase);

        if (isInvalidAction)
        {
            result.Errors.Add($"Action must be: 'Active', 'Discontinue' or 'Deactive'.");
        }
        else if (
            string.Equals(action, HistoryActions.Material.Discontinue, StringComparison.OrdinalIgnoreCase)
            && acceptanceDate.GetValueOrDefault() == DateTime.MinValue)
        {
            result.Errors.Add($"Acceptance Date is required or invalid.");
        }



        return result;
    }
}