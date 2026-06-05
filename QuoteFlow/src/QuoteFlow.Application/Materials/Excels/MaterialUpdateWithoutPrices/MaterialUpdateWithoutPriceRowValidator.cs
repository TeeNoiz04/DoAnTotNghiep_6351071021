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
        var registrationDate = ExcelParser.GetValue<DateTime?>(rowData, "C"); //C -reset
        var validFrom = ExcelParser.GetValue<DateTime?>(rowData, "D"); // D
        var validTo = ExcelParser.GetValue<DateTime?>(rowData, "E"); // E
        var referenceLeadTime = ExcelParser.GetValue<string?>(rowData, "U");            // U
        var warrantyTime = ExcelParser.GetValue<string?>(rowData, "V");
        var maxLot = ExcelParser.GetValue<string?>(rowData, "AB");               // AB
        var stockWarning = ExcelParser.GetValue<string?>(rowData, "AC"); //AC
        var stockQty = ExcelParser.GetValue<string?>(rowData, "AD");    //AD



        if (string.IsNullOrWhiteSpace(materialCode))
            result.AddError("Material Code (A) is required.");

        //if (string.IsNullOrWhiteSpace(modelName))
        //    result.AddError("Model Name (B) is required.");
        var registrationDateStr = ExcelParser.GetValue<string?>(rowData, "C");
        if (registrationDateStr?.ToUpper() != "NULL")
        {
            if (registrationDate.HasValue && registrationDate.Value == DateTime.MinValue)
                result.AddError("Registration Date is invalid.");
        }


        //if (!validFrom.HasValue || validFrom.Value == DateTime.MinValue)
        //    result.AddError("Valid From is required or invalid.");

        //if (!validTo.HasValue || validTo.Value == DateTime.MinValue)
        //    result.AddError("Valid To is required or invalid.");

        if (validFrom.HasValue && validTo.HasValue && validFrom > validTo)
            result.AddError("Price Valid From must be earlier than or equal to Price Valid To.");
        if (referenceLeadTime?.ToUpper() != "NULL")
        {
            ValidateIntergerField(referenceLeadTime, "Reference Lead Time (U)", false);
        }
        ValidateIntergerField(warrantyTime, "Warranty Time (V)", false); // dont allow "NULL"
        if (maxLot?.ToUpper() != "NULL")
        {
            ValidateIntergerField(maxLot, "Max Lot (AB)", false);
        }
        if (stockWarning?.ToUpper() != "NULL")
        {
            ValidateIntergerField(stockWarning, "Stock Warning (AC)", false);
        }
        if (stockQty?.ToUpper() != "NULL")
        {
            ValidateIntergerField(stockQty, "Stock Qty (AD)", false);
        }


        void ValidateIntergerField(string? value, string fieldName, bool required)
        {
            if (!int.TryParse(value, out var parsedValue) && !string.IsNullOrWhiteSpace(value))
            {
                result.AddError($"{fieldName} must be a valid decimal number.");
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
            else
            {
                return ExcelParser.GetValue<DateTime?>(rowData, column);
            }

            //return null; // hoặc throw exception nếu muốn chặt chẽ hơn
        }

        return new MaterialUpdateWithoutPriceImportDto
        {
            MaterialCode = ExcelParser.GetValue<string?>(rowData, "A")?.Trim(),        // A* - Not allow "NULL"
            ModelName = ExcelParser.GetValue<string?>(rowData, "B")?.Trim(),           // B* - Not allow "NULL"

            RegistrationDate = GetDate("C"),                                            // C

            ValidFrom = ExcelParser.GetValue<DateTime?>(rowData, "D"),     // D* - Not allow "NULL"
            ValidTo = ExcelParser.GetValue<DateTime?>(rowData, "E"),       // E* - Not allow "NULL"

            Spec1 = GetString("F"),                                                   // F
            Spec2 = GetString("G"),                                                   // G
            Spec3 = GetString("H"),                                                   // H
            Spec4 = GetString("I"),                                                   // I

            DescriptionEN = ExcelParser.GetValue<string?>(rowData, "J")?.Trim(),      // J* - Not allow "NULL"

            DescriptionVN = GetString("K"),                                           // K

            Supplier = ExcelParser.GetValue<string?>(rowData, "L")?.Trim(),           // L* - Not allow "NULL"
            SupplierBU = ExcelParser.GetValue<string?>(rowData, "M")?.Trim(),         // M* - Not allow "NULL"
            Factory = ExcelParser.GetValue<string?>(rowData, "N")?.Trim(),            // N* - Not allow "NULL"
            MaterialType = ExcelParser.GetValue<string?>(rowData, "O")?.Trim(),       // O* - Not allow "NULL"
            Unit = ExcelParser.GetValue<string?>(rowData, "P")?.Trim(),               // P* - Not allow "NULL"
            MaterialGroup = ExcelParser.GetValue<string?>(rowData, "Q")?.Trim(),      // Q* - Not allow "NULL"

            SAPGroup = GetString("R"),                                                // R
            ProductHierarchy_Description = GetString("S"),                            // S
            CountryOfOrigin = GetString("T"),                                         // T
            ReferenceLeadTime = GetInt("U"),                                          // U

            WarrantyTime = ExcelParser.GetValue<int?>(rowData, "V"),                  // V* - Not allow "NULL"

            InventoryCategory = GetString("W"),                                       // W
            CargoNote = GetString("X"),                                               // X
            Weight = GetString("Y"),                                                  // Y
            Size = GetString("Z"),                                                    // Z
            QRCode = GetString("AA"),                                                 // AA
            MaxLot = GetInt("AB"),                                                    // AB
            StockWarning = GetInt("AC"),                                              // AC
            StockQty = GetInt("AD"),                                                  // AD
            HSCode = GetString("AE")                                                  // AE
        };
    }

}
