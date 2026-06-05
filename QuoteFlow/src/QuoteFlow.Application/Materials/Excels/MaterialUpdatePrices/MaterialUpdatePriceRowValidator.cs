using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;

namespace QuoteFlow.Materials.Excels.MaterialUpdatePrices;
public class MaterialUpdatePriceRowValidator : IExcelRowValidator<MaterialUpdatePriceImportDto>
{
    private const string DATE_TIME = "dd/MM/yyyy";
    public MaterialUpdatePriceRowValidator()
    {
    }

    public MaterialUpdatePriceImportDto ParseRow(IDictionary<string, object> rowData)
    {
        string? GetString(string column)
        {
            var value = ExcelParser.GetValue<string?>(rowData, column)?.Trim();
            return value?.ToUpper() == "NULL" ? "-1" : value;
        }

        decimal? GetDecimal(string column)
        {
            var value = ExcelParser.GetValue<string?>(rowData, column)?.Trim();
            if (value?.ToUpper() == "NULL")
            {
                return -1;
            }
            return ExcelParser.GetValue<decimal?>(rowData, column);
        }

        // special xử lý ImportDuty
        decimal? duty = null;
        var dutyCheck = ExcelParser.GetValue<string?>(rowData, "L");
        if (dutyCheck?.ToUpper() != "KCT")
        {
            duty = ExcelParser.GetValue<decimal?>(rowData, "L");
        }
        else
        {
            duty = -1;
        }

        return new MaterialUpdatePriceImportDto
        {
            MaterialCode = ExcelParser.GetValue<string?>(rowData, "A")?.Trim() ?? string.Empty,
            ModelName = ExcelParser.GetValue<string?>(rowData, "B")?.Trim() ?? string.Empty,

            Spec1 = GetString("C"),                       // Allow "NULL"

            MaterialType = ExcelParser.GetValue<string?>(rowData, "D")?.Trim(),
            MaterialGroup = ExcelParser.GetValue<string?>(rowData, "E")?.Trim(),
            PriceValidFrom = ExcelParser.GetValue<DateTime?>(rowData, "F"),
            PriceValidTo = ExcelParser.GetValue<DateTime?>(rowData, "G"),
            InputPrice = ExcelParser.GetValue<decimal?>(rowData, "H"),
            InputCurrency = ExcelParser.GetValue<string?>(rowData, "I")?.Trim(),
            Incoterms = ExcelParser.GetValue<string?>(rowData, "J")?.Trim(),
            EPA = ExcelParser.GetValue<bool?>(rowData, "K"),
            ImportDuty = duty,
            ExchangeRate = ExcelParser.GetValue<decimal?>(rowData, "M"),
            LandedCost = ExcelParser.GetValue<decimal?>(rowData, "N"),
            MaxSaleOfferPrice = ExcelParser.GetValue<decimal?>(rowData, "O"),
            MaxManagerOfferPrice = ExcelParser.GetValue<decimal?>(rowData, "P"),
            StandardPrice = ExcelParser.GetValue<decimal?>(rowData, "Q"),

            SellingPrice1 = GetDecimal("R"),              // Allow "NULL"
            SellingPrice2 = GetDecimal("S"),              // Allow "NULL"
            SellingPrice3 = GetDecimal("T"),              // Allow "NULL"
            SellingPrice4 = GetDecimal("U"),              // Allow "NULL"
            SellingPrice5 = GetDecimal("V"),              // Allow "NULL"
        };
    }


    public ValidationResult ValidateRow(IDictionary<string, object> rowData, int rowIndex)
    {
        var result = new ValidationResult();

        var materialCode = ExcelParser.GetValue<string?>(rowData, "A")?.Trim();
        var modelName = ExcelParser.GetValue<string?>(rowData, "B")?.Trim();
        var priceValidFrom = ExcelParser.GetValue<DateTime?>(rowData, "F");
        var priceValidTo = ExcelParser.GetValue<DateTime?>(rowData, "G");
        var inputPrice = ExcelParser.GetValue<string?>(rowData, "H"); // decimal
        var inputCurrency = ExcelParser.GetValue<string?>(rowData, "I")?.Trim();
        var incoterms = ExcelParser.GetValue<string?>(rowData, "J")?.Trim();
        var importDuty = ExcelParser.GetValue<string?>(rowData, "L"); // decimal
        var exchangeRate = ExcelParser.GetValue<string?>(rowData, "M"); // decimal
        var landedCost = ExcelParser.GetValue<string?>(rowData, "N"); // decimal
        var maxSaleOfferPrice = ExcelParser.GetValue<string?>(rowData, "O"); // decimal
        var maxManagerOfferPrice = ExcelParser.GetValue<string?>(rowData, "P"); // decimal
        var standardPrice = ExcelParser.GetValue<string?>(rowData, "Q"); // decimal
        var sellingPrice1 = ExcelParser.GetValue<string?>(rowData, "R"); // decimal
        var sellingPrice2 = ExcelParser.GetValue<string?>(rowData, "S"); // decimal
        var sellingPrice3 = ExcelParser.GetValue<string?>(rowData, "T"); // decimal
        var sellingPrice4 = ExcelParser.GetValue<string?>(rowData, "U"); // decimal
        var sellingPrice5 = ExcelParser.GetValue<string?>(rowData, "V"); // decimal

        // Required field validations
        if (string.IsNullOrWhiteSpace(materialCode))
            result.AddError("Material Code is required.");
        if (string.IsNullOrWhiteSpace(modelName))
            result.AddError("Model Name is required.");

        // Date validations
        if (priceValidFrom.HasValue && priceValidFrom.Value == DateTime.MinValue)
            result.AddError("Price Valid From is required or invalid.");

        if (priceValidTo.HasValue && priceValidTo.Value == DateTime.MinValue)
            result.AddError("Price Valid To is required or invalid.");

        if (priceValidFrom.HasValue && priceValidTo.HasValue && priceValidFrom > priceValidTo)
            result.AddError("Price Valid From must be earlier than or equal to Price Valid To.");

        // Decimal validations (required)
        ValidateDecimalField(inputPrice, "Input Price", required: false);
        if (importDuty?.ToUpper() != "KCT")
        {
            ValidateDecimalField(importDuty, "Import Duty", required: false);
        }
        ValidateDecimalField(exchangeRate, "Exchange Rate", required: false);
        ValidateDecimalField(landedCost, "Landed Cost", required: false);
        ValidateDecimalField(maxSaleOfferPrice, "Max Sale Offer Price", required: false);
        ValidateDecimalField(maxManagerOfferPrice, "Max Manager Offer Price", required: false);
        ValidateDecimalField(standardPrice, "Standard Price", required: false);

        // Decimal validations (optional)
        if (sellingPrice1?.ToUpper() != "NULL")
        {
            ValidateDecimalField(sellingPrice1, "Selling Price 1", required: false);
        }

        if (sellingPrice2?.ToUpper() != "NULL")
        {
            ValidateDecimalField(sellingPrice2, "Selling Price 2", required: false);
        }

        if (sellingPrice3?.ToUpper() != "NULL")
        {
            ValidateDecimalField(sellingPrice3, "Selling Price 3", required: false);
        }

        if (sellingPrice4?.ToUpper() != "NULL")
        {
            ValidateDecimalField(sellingPrice4, "Selling Price 4", required: false);
        }

        if (sellingPrice5?.ToUpper() != "NULL")
        {
            ValidateDecimalField(sellingPrice5, "Selling Price 5", required: false);
        }


        return result;

        // Local method for decimal validation
        void ValidateDecimalField(string? value, string fieldName, bool required)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (required)
                    result.AddError($"{fieldName} is required.");
            }
            else if (!decimal.TryParse(value, out var parsedValue))
            {
                result.AddError($"{fieldName} must be a valid decimal number.");
            }
            else if (parsedValue < 0)
            {
                result.AddError($"{fieldName} must be greater than or equal to 0.");
            }
        }
    }


}
