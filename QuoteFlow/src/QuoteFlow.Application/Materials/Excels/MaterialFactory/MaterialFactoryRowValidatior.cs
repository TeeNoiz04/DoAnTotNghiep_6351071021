using QuoteFlow.Materials.MaterialImport.MaterialFactory;
using QuoteFlow.Shared.Excels;
using System.Collections.Generic;

namespace QuoteFlow.Materials.Excels.MaterialFactory;

public class MaterialFactoryRowValidatior : IExcelRowValidator<MaterialFactoryUpdateExcelDto>
{

    public MaterialFactoryRowValidatior(IMaterialRepository materialMaterialRepository = null)
    {
    }

    public MaterialFactoryUpdateExcelDto ParseRow(IDictionary<string, object> rowData)
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

        return new MaterialFactoryUpdateExcelDto
        {
            GolfaCode = ExcelParser.GetValue<string?>(rowData, "A")?.Trim(),
            Model = ExcelParser.GetValue<string?>(rowData, "B")?.Trim(),
            ReferenceLeadTime = GetInt("C"),
            CountryOfOrigin = GetString("D"),
            Maxlot = GetInt("E"),
        };
    }


    public ValidationResult ValidateRow(IDictionary<string, object> rowData, int rowIndex)
    {
        var result = new ValidationResult();

        var golfaCode = ExcelParser.GetValue<string?>(rowData, "A")?.Trim();
        var model = ExcelParser.GetValue<string?>(rowData, "B")?.Trim();
        var referenceLeadTimeStr = ExcelParser.GetValue<string?>(rowData, "C");
        var countryOfOriginStr = ExcelParser.GetValue<string?>(rowData, "D")?.Trim();
        var maxlotStr = ExcelParser.GetValue<string?>(rowData, "E");

        if (string.IsNullOrWhiteSpace(golfaCode))
        {
            result.Errors.Add($"Golfa Code is required.");
        }
        if (string.IsNullOrWhiteSpace(model))
        {
            result.Errors.Add($"Model is required.");
        }
        if (!string.IsNullOrWhiteSpace(referenceLeadTimeStr) && referenceLeadTimeStr.ToUpper() != "NULL")
        {
            if (!int.TryParse(referenceLeadTimeStr, out var parsedLeadTime) || parsedLeadTime < 0)
            {
                result.Errors.Add($"Reference LeadTime must be a valid non-negative number.");
            }
        }

        if (!string.IsNullOrWhiteSpace(maxlotStr) && maxlotStr.ToUpper() != "NULL")
        {
            if (!int.TryParse(maxlotStr, out var parsedMaxlot) || parsedMaxlot < 0)
            {
                result.Errors.Add($"Maxlot must be a valid non-negative number.");
            }
        }



        return result;
    }
}