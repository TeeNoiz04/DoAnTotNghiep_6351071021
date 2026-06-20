using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;

namespace QuoteFlow.Materials.Excels.MaterialUpdateWithoutPrices;
public class MaterialUpdateWithoutPriceRowValidator : IExcelRowValidator<MaterialUpdateWithoutPriceImportDto>
{
    private const string DATE_TIME = "dd/MM/yyyy";

    public MaterialUpdateWithoutPriceRowValidator()
    {
    }

    public ValidationResult ValidateRow(IDictionary<string, object> rowData, int rowIndex)
    {
        var result = new ValidationResult();

        var materialCode = ExcelParser.GetValue<string?>(rowData, "A")?.Trim(); // A*
        var modelName = ExcelParser.GetValue<string?>(rowData, "B")?.Trim(); // B
        var registrationDate = ExcelParser.GetValue<DateTime?>(rowData, "C"); //C
        var validFrom = ExcelParser.GetValue<DateTime?>(rowData, "D"); // D
        var validTo = ExcelParser.GetValue<DateTime?>(rowData, "E"); // E
        var referenceLeadTime = ExcelParser.GetValue<string?>(rowData, "S"); // S
        var warrantyTime = ExcelParser.GetValue<string?>(rowData, "T");      // T
        var stockWarning = ExcelParser.GetValue<string?>(rowData, "X");      // X
        var stockQty = ExcelParser.GetValue<string?>(rowData, "Y");          // Y

        if (string.IsNullOrWhiteSpace(materialCode))
            result.AddError("Material Code (A) is required.");

        var registrationDateStr = ExcelParser.GetValue<string?>(rowData, "C");
        if (registrationDateStr?.ToUpper() != "NULL")
        {
            if (registrationDate.HasValue && registrationDate.Value == DateTime.MinValue)
                result.AddError("Registration Date is invalid.");
        }

        if (validFrom.HasValue && validTo.HasValue && validFrom > validTo)
            result.AddError("Price Valid From must be earlier than or equal to Price Valid To.");

        if (referenceLeadTime?.ToUpper() != "NULL")
        {
            ValidateIntergerField(referenceLeadTime, "Reference Lead Time (S)", false);
        }
        ValidateIntergerField(warrantyTime, "Warranty Time (T)", false);
        if (stockWarning?.ToUpper() != "NULL")
        {
            ValidateIntergerField(stockWarning, "Stock Warning (X)", false);
        }
        if (stockQty?.ToUpper() != "NULL")
        {
            ValidateIntergerField(stockQty, "Stock Qty (Y)", false);
        }

        void ValidateIntergerField(string? value, string fieldName, bool required)
        {
            if (!int.TryParse(value, out var parsedValue) && !string.IsNullOrWhiteSpace(value))
            {
                result.AddError($"{fieldName} must be a valid integer.");
            }
            else if (parsedValue < 0)
            {
                result.AddError($"{fieldName} must be greater than or equal to 0.");
            }
        }

        return result;
    }

    public MaterialUpdateWithoutPriceImportDto ParseRow(IDictionary<string, object> rowData)
    {
        string? GetString(string column)
        {
            var value = ExcelParser.GetValue<string?>(rowData, column)?.Trim();
            return value?.ToUpper() == "NULL" ? "-1" : value;
        }

        int? GetInt(string column)
        {
            var value = ExcelParser.GetValue<string?>(rowData, column)?.Trim();
            if (value?.ToUpper() == "NULL")
            {
                return -1;
            }
            return ExcelParser.GetValue<int?>(rowData, column);
        }

        DateTime? GetDate(string column)
        {
            var value = ExcelParser.GetValue<string?>(rowData, column)?.Trim();
            if (!string.IsNullOrWhiteSpace(value) && value.ToUpper() == "NULL")
            {
                return DateTime.MinValue;
            }
            return ExcelParser.GetValue<DateTime?>(rowData, column);
        }

        return new MaterialUpdateWithoutPriceImportDto
        {
            MaterialCode = ExcelParser.GetValue<string?>(rowData, "A")?.Trim(),  // A*
            ModelName = ExcelParser.GetValue<string?>(rowData, "B")?.Trim(),     // B
            RegistrationDate = GetDate("C"),                                      // C
            ValidFrom = ExcelParser.GetValue<DateTime?>(rowData, "D"),           // D
            ValidTo = ExcelParser.GetValue<DateTime?>(rowData, "E"),             // E
            Spec1 = GetString("F"),                                               // F
            Spec2 = GetString("G"),                                               // G
            Spec3 = GetString("H"),                                               // H
            Spec4 = GetString("I"),                                               // I
            DescriptionEN = ExcelParser.GetValue<string?>(rowData, "J")?.Trim(), // J
            DescriptionVN = GetString("K"),                                       // K
            Supplier = ExcelParser.GetValue<string?>(rowData, "L")?.Trim(),      // L
            SupplierBU = ExcelParser.GetValue<string?>(rowData, "M")?.Trim(),    // M
            Factory = ExcelParser.GetValue<string?>(rowData, "N")?.Trim(),       // N
            MaterialType = ExcelParser.GetValue<string?>(rowData, "O")?.Trim(),  // O
            Unit = ExcelParser.GetValue<string?>(rowData, "P")?.Trim(),          // P
            MaterialGroup = ExcelParser.GetValue<string?>(rowData, "Q")?.Trim(), // Q
            CountryOfOrigin = GetString("R"),                                     // R  (was T)
            ReferenceLeadTime = GetInt("S"),                                      // S  (was U)
            WarrantyTime = ExcelParser.GetValue<int?>(rowData, "T"),             // T  (was V)
            Weight = GetString("U"),                                              // U  (was Y)
            Size = GetString("V"),                                                // V  (was Z)
            QRCode = GetString("W"),                                              // W  (was AA)
            StockWarning = GetInt("X"),                                           // X  (was AC)
            StockQty = GetInt("Y"),                                               // Y  (was AD)
        };
    }
}
