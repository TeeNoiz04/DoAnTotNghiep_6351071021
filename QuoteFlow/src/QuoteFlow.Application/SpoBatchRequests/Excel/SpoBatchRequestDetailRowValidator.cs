using QuoteFlow.Shared.Excels;
using QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails;
using System;
using System.Collections.Generic;

namespace QuoteFlow.SpoBatchRequests.Excel;

public class SpoBatchRequestDetailRowValidator : IExcelRowValidator<SpoBatchRequestDetailImportDto>
{


    public SpoBatchRequestDetailRowValidator()
    {

    }

    public ValidationResult ValidateRow(IDictionary<string, object> rowData, int rowIndex)
    {
        var result = new ValidationResult();

        var spoCode = ExcelParser.GetValue<string?>(rowData, "A")?.Trim();
        var golfaCode = ExcelParser.GetValue<string?>(rowData, "B")?.Trim() ?? string.Empty;
        var action = ExcelParser.GetValue<string?>(rowData, "D")?.Trim();
        var actionDate = ExcelParser.GetValue<DateTime?>(rowData, "C");
        var note = ExcelParser.GetValue<string?>(rowData, "E");

        if (string.IsNullOrWhiteSpace(action))
        {
            result.AddError("Action is required.");
        }
        else if (!action.Equals("Open", StringComparison.OrdinalIgnoreCase) &&
                 !action.Equals("Close", StringComparison.OrdinalIgnoreCase))
        {
            result.AddError($"Invalid Action: '{action}'. Only 'Open' or 'Close' are allowed.");
        }


        return result;
    }

    public SpoBatchRequestDetailImportDto ParseRow(IDictionary<string, object> rowData)
    {
        return new SpoBatchRequestDetailImportDto
        {
            SPOCode = ExcelParser.GetValue<string?>(rowData, "A")?.Trim(),
            GolfaCode = ExcelParser.GetValue<string?>(rowData, "B")?.Trim() ?? string.Empty,
            Action = ExcelParser.GetValue<string?>(rowData, "D")?.Trim(),
            ActionDate = ExcelParser.GetValue<DateTime?>(rowData, "C"),
            Note = ExcelParser.GetValue<string?>(rowData, "E"),
        };
    }
}
