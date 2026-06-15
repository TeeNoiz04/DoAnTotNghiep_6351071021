using QuoteFlow.Shared.Excels;
using System.Collections.Generic;

namespace QuoteFlow.SupplierBUs.Excels.Validations;
public class SupplierBURowValidator : IExcelRowValidator<SupplierBUImportDto>
{
    public SupplierBUImportDto ParseRow(IDictionary<string, object> rowData)
    {
        return new SupplierBUImportDto
        {
            No = ExcelParser.GetValue<int?>(rowData, "A"),
            SupplierBU = ExcelParser.GetValue<string?>(rowData, "B")?.Trim(),
            SupplierBURemarks = ExcelParser.GetValue<string?>(rowData, "C")?.Trim(),
            OrderMethod = ExcelParser.GetValue<string?>(rowData, "D")?.Trim(),
            POTemplate = ExcelParser.GetValue<string?>(rowData, "E")?.Trim(),
            Contact = ExcelParser.GetValue<string?>(rowData, "F")?.Trim(),
            Email = ExcelParser.GetValue<string?>(rowData, "G")?.Trim(),
            IncoTerm = ExcelParser.GetValue<string?>(rowData, "H")?.Trim(),
            PaymentTermCode = ExcelParser.GetValue<string?>(rowData, "I")?.Trim(),
            PaymentDescription = ExcelParser.GetValue<string?>(rowData, "J")?.Trim(),
            Currency = ExcelParser.GetValue<string?>(rowData, "K")?.Trim(),
            MaterialType = ExcelParser.GetValue<string?>(rowData, "L")?.Trim(),
            SAPCode = ExcelParser.GetValue<string?>(rowData, "M")?.Trim(),
            //SupplierID = ExcelParser.GetValue<string?>(rowData, "C")?.Trim(),
            SupplierCode = ExcelParser.GetValue<string?>(rowData, "N")?.Trim(),
            SupplierAddress = ExcelParser.GetValue<string?>(rowData, "O")?.Trim(),
            FASCMVendorCode = ExcelParser.GetValue<string?>(rowData, "P")?.Trim(),
            FASCMBuyerCode = ExcelParser.GetValue<string?>(rowData, "Q")?.Trim(),
            FASCMConsigneeCode = ExcelParser.GetValue<string?>(rowData, "R")?.Trim(),
            FASCMSectionCode = ExcelParser.GetValue<string?>(rowData, "S")?.Trim(),
            FASCMPaymentTerm = ExcelParser.GetValue<string?>(rowData, "T")?.Trim(),
            FASCMFreightMethod = ExcelParser.GetValue<string?>(rowData, "U")?.Trim(),
            FASCMDeliveryTerms = ExcelParser.GetValue<string?>(rowData, "V")?.Trim(),
            FASCMPlaceOfDeliveryTerms = ExcelParser.GetValue<string?>(rowData, "W")?.Trim(),
            FASCMShippingMarkCode = ExcelParser.GetValue<string?>(rowData, "X")?.Trim(),

        };
    }

    public ValidationResult ValidateRow(IDictionary<string, object> rowData, int rowIndex)
    {
        var result = new ValidationResult();

        var supplierBU = ExcelParser.GetValue<string?>(rowData, "B")?.Trim();
        var currency = ExcelParser.GetValue<string?>(rowData, "K")?.Trim();
        var materialType = ExcelParser.GetValue<string?>(rowData, "L")?.Trim();
        var supplierCode = ExcelParser.GetValue<string?>(rowData, "M")?.Trim();
        var supplierShortenName = ExcelParser.GetValue<string?>(rowData, "N")?.Trim();
        if (string.IsNullOrWhiteSpace(supplierCode))
            result.Errors.Add($"Supplier Code (SAP Code) is required.");
        if (string.IsNullOrWhiteSpace(supplierShortenName))
            result.Errors.Add($"Supplier Shorten Name is required.");
        if (string.IsNullOrWhiteSpace(supplierBU))
            result.Errors.Add($"Supplier BU is required.");
        if (string.IsNullOrWhiteSpace(currency))
            result.Errors.Add($"Currency is required.");
        if (string.IsNullOrWhiteSpace(materialType))
            result.Errors.Add($"Material Type is required.");
        else if (!string.Equals(materialType, "FA") && !string.Equals(materialType, "LVS"))
        {
            result.Errors.Add($"Material Type must be: FA or LVS.");
        }
        return result;
    }
}
