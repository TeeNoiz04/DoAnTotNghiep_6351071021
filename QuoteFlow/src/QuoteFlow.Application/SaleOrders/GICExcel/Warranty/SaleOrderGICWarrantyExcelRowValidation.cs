using QuoteFlow.GICs;
using QuoteFlow.SaleOrders.Excel;
using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;

namespace QuoteFlow.SaleOrders.GICExcel.Warranty;

public class SaleOrderGICWarrantyRowValidation : IExcelRowValidator<SaleOrderGICWarrantyExcelDto>
{
    private const string DATE_TIME = "dd/MM/yyyy";

    public SaleOrderGICWarrantyExcelDto ParseRow(IDictionary<string, object> rowData)
    {
        return new SaleOrderGICWarrantyExcelDto
        {
            GICNo = ExcelParser.GetValue<string?>(rowData, "E")?.Trim(),                    // GIC No
            GICGivDate = ExcelParser.GetValue<DateTime?>(rowData, "AD", DATE_TIME),         // GIV Date
            GICGivNo = ExcelParser.GetValue<string?>(rowData, "AC")?.Trim(),                // GIV No
            GICPORNo = ExcelParser.GetValue<string?>(rowData, "AI")?.Trim(),                // POR No
            GICPRNo = ExcelParser.GetValue<string?>(rowData, "AJ")?.Trim(),                 // PR No
            SAPSONo = ExcelParser.GetValue<string?>(rowData, "Z")?.Trim(),                  // SAP SO No
            DONo = ExcelParser.GetValue<string?>(rowData, "AA")?.Trim(),                 // SAP DO No
            BillingNo = ExcelParser.GetValue<string?>(rowData, "AB")?.Trim(),            // SAP Billing No
            MaterialCode = ExcelParser.GetValue<string?>(rowData, "K")?.Trim(),             // Material Code
            ModelName = ExcelParser.GetValue<string?>(rowData, "L")?.Trim(),                // Model Name
            SONo = ExcelParser.GetValue<string?>(rowData, "U")?.Trim(),                     // SO No
            SOQty = ExcelParser.GetValue<decimal?>(rowData, "V"),
            InvoiceNo = ExcelParser.GetValue<string?>(rowData, "AE")?.Trim(),               // Invoice No
            InvoiceDate = ExcelParser.GetValue<DateTime?>(rowData, "AF", DATE_TIME),        // Invoice Date
            GICSAPLandingCost = ExcelParser.GetValue<decimal?>(rowData, "AG"),              // SAP Landing Cost
            GICAmountSAPLandingCost = ExcelParser.GetValue<decimal?>(rowData, "AH"),        // Amount in SAP Landing Cost
            Note = ExcelParser.GetValue<string?>(rowData, "AK")?.Trim(),                    // Change Note
        };
    }




    public ValidationResult ValidateRow(IDictionary<string, object> rowData, int rowIndex)
    {
        var result = new ValidationResult();

        var process = ExcelParser.GetValue<string?>(rowData, "B"); // Warranty Process
        var gicNo = ExcelParser.GetValue<string?>(rowData, "E");
        var materialCode = ExcelParser.GetValue<string?>(rowData, "K");
        var modelName = ExcelParser.GetValue<string?>(rowData, "L");
        var soNo = ExcelParser.GetValue<string?>(rowData, "U");

        var sapSoNo = ExcelParser.GetValue<string?>(rowData, "Z");
        var sapDoNo = ExcelParser.GetValue<string?>(rowData, "AA");
        var sapBillingNo = ExcelParser.GetValue<string?>(rowData, "AB");
        var givNo = ExcelParser.GetValue<string?>(rowData, "AC");
        var invoiceNo = ExcelParser.GetValue<string?>(rowData, "AE");
        var porNo = ExcelParser.GetValue<string?>(rowData, "AI");
        var prNo = ExcelParser.GetValue<string?>(rowData, "AJ");

        if (string.IsNullOrWhiteSpace(gicNo))
            result.Errors.Add($"GIC No (E) is required.");

        if (string.IsNullOrWhiteSpace(materialCode))
            result.Errors.Add($"Material Code (K) is required.");

        if (string.IsNullOrWhiteSpace(modelName))
            result.Errors.Add($"Model (L) is required.");

        if (string.IsNullOrWhiteSpace(soNo))
            result.Errors.Add($"SO No (U) is required.");

        //if (string.IsNullOrWhiteSpace(sapSoNo))
        //    result.Errors.Add($"SAP SO No (Z) is required.");

        //if (string.IsNullOrWhiteSpace(sapDoNo))
        //    result.Errors.Add($"SAP DO No (AA) is required.");

        //if (string.IsNullOrWhiteSpace(sapBillingNo))
        //    result.Errors.Add($"SAP Billing No (AB) is required.");

        if (string.IsNullOrWhiteSpace(process))
            result.Errors.Add($"Warranty Process (B) is required.");


        if (process == GICProcessCodes.PartReplacement)
        {
            if (string.IsNullOrWhiteSpace(givNo))
                result.Errors.Add($"GIV No (AC) is required.");

            //ExcelParser.ValidateDate(rowData, "AD", "GIV Date", rowIndex, result);
            var givDate = ExcelParser.GetValue<DateTime?>(rowData, "AD");
            if (givDate is null)
            {
                result.Errors.Add($"GIV Date (AD) is not a valid date. Please enter a valid date.");
            }
        }

        if (process == GICProcessCodes.ProductExchange)
        {
            if (string.IsNullOrWhiteSpace(invoiceNo))
                result.Errors.Add($"Invoice No (AE) is required.");

            //ExcelParser.ValidateDate(rowData, "AF", "Invoice Date", rowIndex, result);
            var invoiceDate = ExcelParser.GetValue<DateTime?>(rowData, "AF");
            if (invoiceDate is null)
            {
                result.Errors.Add($"Invoice Date (AF) is not a valid date. Please enter a valid date.");
            }
        }

        //ExcelParser.ValidateDecimal(rowData, "AG", "SAP Landing Cost", rowIndex, result, true);
        //ExcelParser.ValidateDecimal(rowData, "AH", "Amount in SAP Landing Cost", rowIndex, result, true);

        //if (string.IsNullOrWhiteSpace(porNo))
        //    result.Errors.Add($"POR No (AI) is required.");

        //if (string.IsNullOrWhiteSpace(prNo))
        //    result.Errors.Add($"PR No (AJ) is required.");

        // ExcelParser.ValidateAmountCalculation(
        //    rowData,
        //    "AG", "V", "AH",
        //    "SAP Landing Cost", "SO Qty", "Amount in SAP Landing Cost",
        //    rowIndex,
        //    result
        //);

        return result;
    }


}