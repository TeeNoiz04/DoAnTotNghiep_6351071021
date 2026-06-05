using QuoteFlow.Materials.MaterialImport.MaterialSAP;
using QuoteFlow.Shared.Excels;
using System.Collections.Generic;

namespace QuoteFlow.Materials.Excels.MaterialSAP;

public class MaterialSAPRowValidatior : IExcelRowValidator<MaterialSAPUpdateExcelDto>
{

    public MaterialSAPRowValidatior()
    {
    }

    public MaterialSAPUpdateExcelDto ParseRow(IDictionary<string, object> rowData)
    {
        string? GetValue(string column)
        {
            var value = ExcelParser.GetValue<string?>(rowData, column)?.Trim();
            return value?.ToUpper() == "NULL" ? "-1" : value;
        }

        var sapCode = GetValue("C");
        var descriptionVN = GetValue("D");
        var productHierarchy = GetValue("E");

        var code = ExcelParser.GetValue<string?>(rowData, "A")?.Trim();
        var model = ExcelParser.GetValue<string?>(rowData, "B")?.Trim();
        decimal? vat = null;
        var vatCheck = ExcelParser.GetValue<string?>(rowData, "F");
        if (vatCheck?.ToUpper() != "KCT")
        {
            vat = ExcelParser.GetValue<decimal?>(rowData, "F");
        }
        else
        {
            vat = -1;
        }

        return new MaterialSAPUpdateExcelDto
        {
            GolfaCode = code,
            Model = model,
            SAPCode = sapCode,
            DescriptionVN = descriptionVN,
            ProductHiearchy = productHierarchy,
            VAT = vat
        };
    }
    private string? GetStringOrDefault(IDictionary<string, object> rowData, string column)
    {
        var value = ExcelParser.GetValue<string?>(rowData, column)?.Trim();
        if (!string.IsNullOrEmpty(value) && value.ToUpper() == "NULL")
        {
            return "-1";
        }
        return value;
    }

    public ValidationResult ValidateRow(IDictionary<string, object> rowData, int rowIndex)
    {
        var result = new ValidationResult();

        var golfaCode = ExcelParser.GetValue<string?>(rowData, "A")?.Trim();
        var model = ExcelParser.GetValue<string?>(rowData, "B")?.Trim();
        var sAPCode = ExcelParser.GetValue<string?>(rowData, "C")?.Trim();
        var descriptionVN = ExcelParser.GetValue<string?>(rowData, "D")?.Trim();
        var productHiearchuy = ExcelParser.GetValue<string?>(rowData, "E")?.Trim();
        var vat = ExcelParser.GetValue<string?>(rowData, "F");

        if (string.IsNullOrWhiteSpace(golfaCode))
        {
            result.Errors.Add($"Golfa Code is required.");
        }
        if (string.IsNullOrWhiteSpace(model))
        {
            result.Errors.Add($"Model is required.");
        }
        if (!string.IsNullOrWhiteSpace(vat) && vat?.ToUpper() != "KCT")
        {
            if (!decimal.TryParse(vat, out var parsedVat) || parsedVat < 0)
            {
                result.Errors.Add($"VAT must be a valid non-negative decimal number.");
            }
        }

        return result;
    }
}

