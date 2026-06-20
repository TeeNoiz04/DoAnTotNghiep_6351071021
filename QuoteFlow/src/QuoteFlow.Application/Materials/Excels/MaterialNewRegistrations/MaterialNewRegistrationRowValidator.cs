using QuoteFlow.Shared.Excels;
using QuoteFlow.SystemCategories;
using System;
using System.Collections.Generic;

namespace QuoteFlow.Materials.Excels.MaterialNewRegistrations;
public class MaterialNewRegistrationRowValidator : IExcelRowValidator<MaterialNewRegistrationImportDto>
{
    protected readonly ISystemCategoryRepository _systemCategoryRepository;
    private const string DATE_TIME = "dd/MM/yyyy";

    public MaterialNewRegistrationRowValidator(ISystemCategoryRepository systemCategoryRepository)
    {
        _systemCategoryRepository = systemCategoryRepository;
    }

    public ValidationResult ValidateRow(IDictionary<string, object> rowData, int rowIndex)
    {
        var result = new ValidationResult();

        var registrationDate = ExcelParser.GetValue<DateTime?>(rowData, "A");
        var validFrom = ExcelParser.GetValue<DateTime?>(rowData, "B");
        var validTo = ExcelParser.GetValue<DateTime?>(rowData, "C");
        var materialCode = ExcelParser.GetValue<string?>(rowData, "D")?.Trim();
        var modelName = ExcelParser.GetValue<string?>(rowData, "E")?.Trim();

        var descriptionEN = ExcelParser.GetValue<string?>(rowData, "J")?.Trim();
        var materialType = ExcelParser.GetValue<string?>(rowData, "L")?.Trim();
        var unit = ExcelParser.GetValue<string?>(rowData, "M")?.Trim();
        var materialClass = ExcelParser.GetValue<string?>(rowData, "N")?.Trim();
        var materialSECClassification = ExcelParser.GetValue<string?>(rowData, "O")?.Trim();
        var materialGroup = ExcelParser.GetValue<string?>(rowData, "P")?.Trim();
        var warrantyTimeStr = ExcelParser.GetValue<string?>(rowData, "T");

        var vatStr = ExcelParser.GetValue<string?>(rowData, "Y")?.Trim();
        var supplier = ExcelParser.GetValue<string?>(rowData, "Z")?.Trim();
        var supplierBU = ExcelParser.GetValue<string?>(rowData, "AA")?.Trim();
        var factory = ExcelParser.GetValue<string?>(rowData, "AB")?.Trim();
        var inputPriceStr = ExcelParser.GetValue<string?>(rowData, "AC");
        var inputCurrency = ExcelParser.GetValue<string?>(rowData, "AD")?.Trim();
        var incoterms = ExcelParser.GetValue<string?>(rowData, "AE")?.Trim();
        var epa = ExcelParser.GetValue<bool?>(rowData, "AF");
        var importDutyStr = ExcelParser.GetValue<string?>(rowData, "AG");
        var exchangeRateStr = ExcelParser.GetValue<string?>(rowData, "AH");
        var landedCostStr = ExcelParser.GetValue<string?>(rowData, "AI");
        var maxSaleOfferPriceStr = ExcelParser.GetValue<string?>(rowData, "AJ");
        var maxManagerOfferPriceStr = ExcelParser.GetValue<string?>(rowData, "AK");
        var standardPriceStr = ExcelParser.GetValue<string?>(rowData, "AL");

        var sellingPrice1Str = ExcelParser.GetValue<string?>(rowData, "AM");
        var sellingPrice2Str = ExcelParser.GetValue<string?>(rowData, "AN");
        var sellingPrice3Str = ExcelParser.GetValue<string?>(rowData, "AO");
        var sellingPrice4Str = ExcelParser.GetValue<string?>(rowData, "AP");
        var sellingPrice5Str = ExcelParser.GetValue<string?>(rowData, "AQ");

        if (!string.IsNullOrWhiteSpace(ExcelParser.GetValue<string?>(rowData, "A")) && registrationDate is null)
            result.AddError("Registration Date (A) must be DateTime.");

        if (string.IsNullOrWhiteSpace(materialCode))
            result.AddError($"Material Code (D) is required.");

        if (string.IsNullOrWhiteSpace(modelName))
            result.AddError($"Model Name (E) is required.");

        if (!validFrom.HasValue || validFrom.Value == DateTime.MinValue)
            result.AddError($"Valid From (B) is required or invalid.");

        if (!validTo.HasValue || validTo.Value == DateTime.MinValue)
            result.AddError($"Valid To (C) is required or invalid.");

        if (validFrom.HasValue && validTo.HasValue && validFrom > validTo)
            result.AddError($"Valid From (B) must be earlier than or equal to Valid To (C).");

        if (string.IsNullOrWhiteSpace(descriptionEN))
            result.AddError($"Description EN (J) is required.");

        if (string.IsNullOrWhiteSpace(materialType))
            result.AddError($"Material Type (L) is required.");

        if (string.IsNullOrWhiteSpace(unit))
            result.AddError($"Unit (M) is required.");

        if (string.IsNullOrWhiteSpace(materialClass))
            result.AddError($"Material Class (N) is required.");

        if (string.IsNullOrWhiteSpace(materialSECClassification))
            result.AddError($"Material SEC Classification (O) is required.");

        if (string.IsNullOrWhiteSpace(materialGroup))
            result.AddError($"Material Group (P) is required.");

        if (string.IsNullOrWhiteSpace(supplier))
            result.AddError($"Supplier (Z) is required.");

        if (!string.IsNullOrWhiteSpace(vatStr))
        {
            if (vatStr?.ToUpper() != "KCT")
                ValidateDecimalField(vatStr, "VAT (Y)", required: true, rowIndex);
        }

        if (string.IsNullOrWhiteSpace(supplierBU))
            result.AddError($"Supplier BU (AA) is required.");

        if (string.IsNullOrWhiteSpace(factory))
            result.AddError($"Factory (AB) is required.");

        if (string.IsNullOrWhiteSpace(inputCurrency))
            result.AddError($"Input Currency (AD) is required.");

        if (string.IsNullOrWhiteSpace(incoterms))
            result.AddError($"Incoterms (AE) is required.");

        if (string.IsNullOrWhiteSpace(warrantyTimeStr))
            result.AddError($"Warranty Time (T) is required.");
        else if (!int.TryParse(warrantyTimeStr, out _))
            result.AddError($"Warranty Time (T) must be a valid integer.");

        var epaRaw = ExcelParser.GetValue<string?>(rowData, "AF")?.Trim();
        if (string.IsNullOrWhiteSpace(epaRaw))
            result.AddError($"EPA (AF) is required.");
        else if (epa is null)
            result.AddError($"EPA (AF) must be a valid boolean (true/false).");

        ValidateDecimalField(inputPriceStr, "Input Price (AC)", required: true, rowIndex);

        if (importDutyStr != "KCT")
            ValidateDecimalField(importDutyStr, "Import Duty (AG)", required: true, rowIndex);

        ValidateDecimalField(exchangeRateStr, "Exchange Rate (AH)", required: true, rowIndex);
        ValidateDecimalField(landedCostStr, "Landed Cost (AI)", required: true, rowIndex);
        ValidateDecimalField(maxSaleOfferPriceStr, "Max Sale Offer Price (AJ)", required: true, rowIndex);
        ValidateDecimalField(maxManagerOfferPriceStr, "Max Manager Offer Price (AK)", required: true, rowIndex);
        ValidateDecimalField(standardPriceStr, "Standard Price (AL)", required: true, rowIndex);

        ValidateDecimalField(sellingPrice1Str, "Selling Price 1 (AM)", required: false, rowIndex);
        ValidateDecimalField(sellingPrice2Str, "Selling Price 2 (AN)", required: false, rowIndex);
        ValidateDecimalField(sellingPrice3Str, "Selling Price 3 (AO)", required: false, rowIndex);
        ValidateDecimalField(sellingPrice4Str, "Selling Price 4 (AP)", required: false, rowIndex);
        ValidateDecimalField(sellingPrice5Str, "Selling Price 5 (AQ)", required: false, rowIndex);

        return result;

        void ValidateDecimalField(string? value, string fieldName, bool required, int rowIndex)
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

    public MaterialNewRegistrationImportDto ParseRow(IDictionary<string, object> rowData)
    {
        decimal? vat = null;
        var vatCheck = ExcelParser.GetValue<string?>(rowData, "Y");
        if (vatCheck?.ToUpper() != "KCT")
            vat = ExcelParser.GetValue<decimal?>(rowData, "Y");

        decimal? duty = null;
        var dutyCheck = ExcelParser.GetValue<string?>(rowData, "AG");
        if (dutyCheck?.ToUpper() != "KCT")
            duty = ExcelParser.GetValue<decimal?>(rowData, "AG");

        return new MaterialNewRegistrationImportDto
        {
            MaterialCode = ExcelParser.GetValue<string?>(rowData, "D")?.Trim(),
            ModelName = ExcelParser.GetValue<string?>(rowData, "E")?.Trim(),
            RegistrationDate = ExcelParser.GetValue<DateTime?>(rowData, "A"),
            ValidFrom = ExcelParser.GetValue<DateTime?>(rowData, "B"),
            ValidTo = ExcelParser.GetValue<DateTime?>(rowData, "C"),
            Spec1 = ExcelParser.GetValue<string?>(rowData, "F")?.Trim(),
            Spec2 = ExcelParser.GetValue<string?>(rowData, "G")?.Trim(),
            Spec3 = ExcelParser.GetValue<string?>(rowData, "H")?.Trim(),
            Spec4 = ExcelParser.GetValue<string?>(rowData, "I")?.Trim(),
            DescriptionEN = ExcelParser.GetValue<string?>(rowData, "J")?.Trim(),
            DescriptionVN = ExcelParser.GetValue<string?>(rowData, "K")?.Trim(),
            MaterialType = ExcelParser.GetValue<string?>(rowData, "L")?.Trim(),
            Unit = ExcelParser.GetValue<string?>(rowData, "M")?.Trim(),
            MaterialClass = ExcelParser.GetValue<string?>(rowData, "N")?.Trim(),
            MaterialSECClassification = ExcelParser.GetValue<string?>(rowData, "O")?.Trim(),
            MaterialGroup = ExcelParser.GetValue<string?>(rowData, "P")?.Trim(),
            ProductHierarchy = ExcelParser.GetValue<string?>(rowData, "Q")?.Trim(),
            CountryOfOrigin = ExcelParser.GetValue<string?>(rowData, "R")?.Trim(),
            ReferenceLeadTime = ExcelParser.GetValue<int?>(rowData, "S"),
            WarrantyTime = ExcelParser.GetValue<int>(rowData, "T"),
            Weight = ExcelParser.GetValue<string?>(rowData, "U")?.Trim(),
            Size = ExcelParser.GetValue<string?>(rowData, "V")?.Trim(),
            QRCode = ExcelParser.GetValue<string?>(rowData, "W")?.Trim(),
            StockWarning = ExcelParser.GetValue<int?>(rowData, "X"),
            VAT = vat,
            Supplier = ExcelParser.GetValue<string?>(rowData, "Z")?.Trim(),
            SupplierBU = ExcelParser.GetValue<string?>(rowData, "AA")?.Trim(),
            Factory = ExcelParser.GetValue<string?>(rowData, "AB")?.Trim(),
            InputPrice = ExcelParser.GetValue<decimal?>(rowData, "AC") ?? 0,
            InputCurrency = ExcelParser.GetValue<string?>(rowData, "AD")?.Trim(),
            Incoterms = ExcelParser.GetValue<string?>(rowData, "AE")?.Trim(),
            EPA = ExcelParser.GetValue<bool>(rowData, "AF"),
            ImportDuty = duty,
            ExchangeRate = ExcelParser.GetValue<decimal?>(rowData, "AH") ?? 0,
            LandedCost = ExcelParser.GetValue<decimal?>(rowData, "AI") ?? 0,
            MaxSaleOfferPrice = ExcelParser.GetValue<decimal?>(rowData, "AJ") ?? 0,
            MaxManagerOfferPrice = ExcelParser.GetValue<decimal?>(rowData, "AK") ?? 0,
            StandardPrice = ExcelParser.GetValue<decimal?>(rowData, "AL") ?? 0,
            SellingPrice1 = ExcelParser.GetValue<decimal?>(rowData, "AM"),
            SellingPrice2 = ExcelParser.GetValue<decimal?>(rowData, "AN"),
            SellingPrice3 = ExcelParser.GetValue<decimal?>(rowData, "AO"),
            SellingPrice4 = ExcelParser.GetValue<decimal?>(rowData, "AP"),
            SellingPrice5 = ExcelParser.GetValue<decimal?>(rowData, "AQ"),
        };
    }
}
