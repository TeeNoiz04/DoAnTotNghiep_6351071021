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
        // Required fields
        var validFrom = ExcelParser.GetValue<DateTime?>(rowData, "B");
        var validTo = ExcelParser.GetValue<DateTime?>(rowData, "C");
        var materialCode = ExcelParser.GetValue<string?>(rowData, "D")?.Trim();
        var modelName = ExcelParser.GetValue<string?>(rowData, "E")?.Trim();

        var descriptionEN = ExcelParser.GetValue<string?>(rowData, "K")?.Trim();
        var materialType = ExcelParser.GetValue<string?>(rowData, "M")?.Trim();
        var unit = ExcelParser.GetValue<string?>(rowData, "N")?.Trim();
        var materialClass = ExcelParser.GetValue<string?>(rowData, "O")?.Trim();
        var materialSECClassification = ExcelParser.GetValue<string?>(rowData, "P")?.Trim();
        var materialGroup = ExcelParser.GetValue<string?>(rowData, "Q")?.Trim();
        var warrantyTimeStr = ExcelParser.GetValue<string?>(rowData, "W");

        var vatStr = ExcelParser.GetValue<string?>(rowData, "AE")?.Trim();
        var supplier = ExcelParser.GetValue<string?>(rowData, "AG")?.Trim();
        var supplierBU = ExcelParser.GetValue<string?>(rowData, "AH")?.Trim();
        var factory = ExcelParser.GetValue<string?>(rowData, "AI")?.Trim();
        var inputPriceStr = ExcelParser.GetValue<string?>(rowData, "AJ");
        var inputCurrency = ExcelParser.GetValue<string?>(rowData, "AK")?.Trim();
        var incoterms = ExcelParser.GetValue<string?>(rowData, "AL")?.Trim();
        var epa = ExcelParser.GetValue<bool?>(rowData, "AM");
        var importDutyStr = ExcelParser.GetValue<string?>(rowData, "AN");
        var exchangeRateStr = ExcelParser.GetValue<string?>(rowData, "AO");
        var landedCostStr = ExcelParser.GetValue<string?>(rowData, "AP");
        var maxSaleOfferPriceStr = ExcelParser.GetValue<string?>(rowData, "AQ");
        var maxManagerOfferPriceStr = ExcelParser.GetValue<string?>(rowData, "AR");
        var standardPriceStr = ExcelParser.GetValue<string?>(rowData, "AS");

        // Optional fields
        var sellingPrice1Str = ExcelParser.GetValue<string?>(rowData, "AT");
        var sellingPrice2Str = ExcelParser.GetValue<string?>(rowData, "AU");
        var sellingPrice3Str = ExcelParser.GetValue<string?>(rowData, "AV");
        var sellingPrice4Str = ExcelParser.GetValue<string?>(rowData, "AW");
        var sellingPrice5Str = ExcelParser.GetValue<string?>(rowData, "AX");

        if (!string.IsNullOrWhiteSpace(ExcelParser.GetValue<string?>(rowData, "A")) && registrationDate is null)
        {
            result.AddError("Registration Date (A) must be DateTime.");
        }


        // --- Required field validations ---
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
            result.AddError($"Description EN (K) is required.");

        if (string.IsNullOrWhiteSpace(materialType))
            result.AddError($"Material Type (M) is required.");

        if (string.IsNullOrWhiteSpace(unit))
            result.AddError($"Unit (N) is required.");

        if (string.IsNullOrWhiteSpace(materialClass))
            result.AddError($"Material Class (O) is required.");

        if (string.IsNullOrWhiteSpace(materialSECClassification))
            result.AddError($"Material SEC Classification (P) is required.");

        if (string.IsNullOrWhiteSpace(materialGroup))
            result.AddError($"Material Group (Q) is required.");

        if (string.IsNullOrWhiteSpace(supplier))
            result.AddError($"Supplier (AG) is required.");

        if (!string.IsNullOrWhiteSpace(vatStr)) //check !null
        {
            if (vatStr?.ToUpper() != "KCT") //check != "KCT"
            {
                ValidateDecimalField(vatStr, "VAT (AT)", required: true, rowIndex);
            }
        }

        if (string.IsNullOrWhiteSpace(supplierBU))
            result.AddError($"Supplier BU (AH) is required.");

        if (string.IsNullOrWhiteSpace(factory))
            result.AddError($"Factory (AI) is required.");

        if (string.IsNullOrWhiteSpace(inputCurrency))
            result.AddError($"Input Currency (AK) is required.");

        if (string.IsNullOrWhiteSpace(incoterms))
            result.AddError($"Incoterms (AL) is required.");

        // --- Integer validation ---
        if (string.IsNullOrWhiteSpace(warrantyTimeStr))
        {
            result.AddError($"Warranty Time (W) is required.");
        }
        else if (!int.TryParse(warrantyTimeStr, out _))
        {
            result.AddError($"Warranty Time (W) must be a valid integer.");
        }

        // --- Boolean validation (EPA) ---
        var epaRaw = ExcelParser.GetValue<string?>(rowData, "AM")?.Trim();
        if (string.IsNullOrWhiteSpace(epaRaw))
        {
            result.AddError($"EPA (AM) is required.");
        }
        else if (epa is null)
        {
            result.AddError($"EPA (AM) must be a valid boolean (true/false).");
        }

        // --- Decimal validations (required) ---
        ValidateDecimalField(inputPriceStr, "Input Price (AJ)", required: true, rowIndex);
        //ValidateDecimalField(importDutyStr, "Import Duty (AN)", required: , rowIndextrue);

        if (importDutyStr != "KCT") //check != "KCT"
        {
            ValidateDecimalField(importDutyStr, "Import Duty (AN)", required: true, rowIndex);
        }

        ValidateDecimalField(exchangeRateStr, "Exchange Rate (AO)", required: true, rowIndex);
        ValidateDecimalField(landedCostStr, "Landed Cost (AP)", required: true, rowIndex);
        ValidateDecimalField(maxSaleOfferPriceStr, "Max Sale Offer Price (AQ)", required: true, rowIndex);
        ValidateDecimalField(maxManagerOfferPriceStr, "Max Manager Offer Price (AR)", required: true, rowIndex);
        ValidateDecimalField(standardPriceStr, "Standard Price (AS)", required: true, rowIndex);

        // --- Decimal validations (optional) ---
        ValidateDecimalField(sellingPrice1Str, "Selling Price 1 (AT)", required: false, rowIndex);
        ValidateDecimalField(sellingPrice2Str, "Selling Price 2 (AU)", required: false, rowIndex);
        ValidateDecimalField(sellingPrice3Str, "Selling Price 3 (AV)", required: false, rowIndex);
        ValidateDecimalField(sellingPrice4Str, "Selling Price 4 (AW)", required: false, rowIndex);
        ValidateDecimalField(sellingPrice5Str, "Selling Price 5 (AX)", required: false, rowIndex);

        return result;

        // Local method for decimal validation
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




    //public Guid CategoryAsync(string code, string type)
    //{
    //    var systemCategories = _systemCategoryRepository.GetListAsync(x => x.CategoryType == type && x.Code == code);
    //    if (systemCategories.Result.Count == 0)
    //    {
    //        return Guid.Empty;
    //    }
    //    return systemCategories.Result[0].Id;
    //}

    public MaterialNewRegistrationImportDto ParseRow(IDictionary<string, object> rowData)
    {
        decimal? vat = null;
        var vatCheck = ExcelParser.GetValue<string?>(rowData, "AE");
        if (vatCheck?.ToUpper() != "KCT")
        {
            vat = ExcelParser.GetValue<decimal?>(rowData, "AE");
        }
        decimal? duty = null;
        var dutyCheck = ExcelParser.GetValue<string?>(rowData, "AN");
        if (dutyCheck?.ToUpper() != "KCT")
        {
            duty = ExcelParser.GetValue<decimal?>(rowData, "AN");
        }
        return new MaterialNewRegistrationImportDto
        {
            MaterialCode = ExcelParser.GetValue<string?>(rowData, "D")?.Trim(),
            ModelName = ExcelParser.GetValue<string?>(rowData, "E")?.Trim(),
            RegistrationDate = ExcelParser.GetValue<DateTime?>(rowData, "A"),
            ValidFrom = ExcelParser.GetValue<DateTime?>(rowData, "B"),
            ValidTo = ExcelParser.GetValue<DateTime?>(rowData, "C"),
            SAPCode = ExcelParser.GetValue<string?>(rowData, "F")?.Trim(),
            Spec1 = ExcelParser.GetValue<string?>(rowData, "G")?.Trim(),
            Spec2 = ExcelParser.GetValue<string?>(rowData, "H")?.Trim(),
            Spec3 = ExcelParser.GetValue<string?>(rowData, "I")?.Trim(),
            Spec4 = ExcelParser.GetValue<string?>(rowData, "J")?.Trim(),
            DescriptionEN = ExcelParser.GetValue<string?>(rowData, "K")?.Trim(),
            DescriptionVN = ExcelParser.GetValue<string?>(rowData, "L")?.Trim(),
            MaterialType = ExcelParser.GetValue<string?>(rowData, "M")?.Trim(),
            Unit = ExcelParser.GetValue<string?>(rowData, "N")?.Trim(),
            MaterialClass = ExcelParser.GetValue<string?>(rowData, "O")?.Trim(),
            MaterialSECClassification = ExcelParser.GetValue<string?>(rowData, "P")?.Trim(),
            MaterialGroup = ExcelParser.GetValue<string?>(rowData, "Q")?.Trim(),
            SAPGroup = ExcelParser.GetValue<string?>(rowData, "R")?.Trim(),
            ProductHierarchy = ExcelParser.GetValue<string?>(rowData, "S")?.Trim(),
            ProductHierarchy_Description = ExcelParser.GetValue<string?>(rowData, "T")?.Trim(),
            CountryOfOrigin = ExcelParser.GetValue<string?>(rowData, "U")?.Trim(),
            ReferenceLeadTime = ExcelParser.GetValue<int?>(rowData, "V"),
            WarrantyTime = ExcelParser.GetValue<int>(rowData, "W"),
            InventoryCategory = ExcelParser.GetValue<string?>(rowData, "X")?.Trim(),

            // --- 4 column mới ---
            CargoNote = ExcelParser.GetValue<string?>(rowData, "Y")?.Trim(),
            Weight = ExcelParser.GetValue<string?>(rowData, "Z")?.Trim(),
            Size = ExcelParser.GetValue<string?>(rowData, "AA")?.Trim(),
            QRCode = ExcelParser.GetValue<string?>(rowData, "AB")?.Trim(),

            // --- dịch mapping từ đây ---
            MaxLot = ExcelParser.GetValue<int?>(rowData, "AC"),
            StockWarning = ExcelParser.GetValue<int?>(rowData, "AD"),
            VAT = vat,
            HSCode = ExcelParser.GetValue<string?>(rowData, "AF")?.Trim(),
            Supplier = ExcelParser.GetValue<string?>(rowData, "AG")?.Trim(),
            SupplierBU = ExcelParser.GetValue<string?>(rowData, "AH")?.Trim(),
            Factory = ExcelParser.GetValue<string?>(rowData, "AI")?.Trim(),
            InputPrice = ExcelParser.GetValue<decimal?>(rowData, "AJ") ?? 0,
            InputCurrency = ExcelParser.GetValue<string?>(rowData, "AK")?.Trim(),
            Incoterms = ExcelParser.GetValue<string?>(rowData, "AL")?.Trim(),
            EPA = ExcelParser.GetValue<bool>(rowData, "AM"),
            ImportDuty = duty,
            ExchangeRate = ExcelParser.GetValue<decimal?>(rowData, "AO") ?? 0,
            LandedCost = ExcelParser.GetValue<decimal?>(rowData, "AP") ?? 0,
            MaxSaleOfferPrice = ExcelParser.GetValue<decimal?>(rowData, "AQ") ?? 0,
            MaxManagerOfferPrice = ExcelParser.GetValue<decimal?>(rowData, "AR") ?? 0,
            StandardPrice = ExcelParser.GetValue<decimal?>(rowData, "AS") ?? 0,
            SellingPrice1 = ExcelParser.GetValue<decimal?>(rowData, "AT"),
            SellingPrice2 = ExcelParser.GetValue<decimal?>(rowData, "AU"),
            SellingPrice3 = ExcelParser.GetValue<decimal?>(rowData, "AV"),
            SellingPrice4 = ExcelParser.GetValue<decimal?>(rowData, "AW"),
            SellingPrice5 = ExcelParser.GetValue<decimal?>(rowData, "AX"),
        };



    }
}

