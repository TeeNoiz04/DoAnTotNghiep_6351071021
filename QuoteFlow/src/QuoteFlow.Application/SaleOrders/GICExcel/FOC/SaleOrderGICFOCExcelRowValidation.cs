using QuoteFlow.SaleOrders.Excel;
using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;

namespace QuoteFlow.SaleOrders.GICExcel.FOC;

public class SaleOrderGICFOCExcelRowValidation : IExcelRowValidator<SaleOrderGICFOCExcelDto>
{
    //private const string DATE_TIME = "dd/MM/yyyy";


    public SaleOrderGICFOCExcelDto ParseRow(IDictionary<string, object> rowData)
    {
        return new SaleOrderGICFOCExcelDto
        {
            SONo = ExcelParser.GetValue<string?>(rowData, "R")?.Trim(),              // SO No (*)
            GICNo = ExcelParser.GetValue<string?>(rowData, "B")?.Trim(),              // GIC No (*)
            InvoiceNo = ExcelParser.GetValue<string?>(rowData, "AA")?.Trim(),         // Invoice No (**)
            InvoiceDate = ExcelParser.GetValue<DateTime?>(rowData, "AB"),             // Invoice Date (**)
            MaterialCode = ExcelParser.GetValue<string?>(rowData, "K")?.Trim(),       // Material Code (*)
            ModelName = ExcelParser.GetValue<string?>(rowData, "L")?.Trim(),          // Model name
            GICSAPLandingCost = ExcelParser.GetValue<decimal?>(rowData, "AC"),        // SAP Landing Cost (**)
            SOQty = ExcelParser.GetValue<decimal?>(rowData, "S"),                     // SO Qty
            GICAmountSAPLandingCost = ExcelParser.GetValue<decimal?>(rowData, "AD"),  // Amount in SAP Landing Cost
            GICPORNo = ExcelParser.GetValue<string?>(rowData, "AE")?.Trim(),          // POR No (*)
            GICPRNo = ExcelParser.GetValue<string?>(rowData, "AF")?.Trim(),           // PR No (**)
            SAPSONo = ExcelParser.GetValue<string?>(rowData, "W")?.Trim(),            // SAP SO No (**)
            GICGivNo = ExcelParser.GetValue<string?>(rowData, "Z")?.Trim(),           // GIV No (**)
            Note = ExcelParser.GetValue<string?>(rowData, "AG")?.Trim(),              // Change Note
            DONo = ExcelParser.GetValue<string?>(rowData, "X")?.Trim(),               // SAP DO No (**)
            BillingNo = ExcelParser.GetValue<string?>(rowData, "Y")?.Trim(),          // SAP Billing No (**)
        };

    }

    public ValidationResult ValidateRow(IDictionary<string, object> rowData, int rowIndex)
    {
        var result = new ValidationResult();

        var gicNo = ExcelParser.GetValue<string?>(rowData, "B");
        var materialCode = ExcelParser.GetValue<string?>(rowData, "K");
        var modelName = ExcelParser.GetValue<string?>(rowData, "L");
        var soNo = ExcelParser.GetValue<string?>(rowData, "R");
        //var sapSoNo = ExcelParser.GetValue<string?>(rowData, "W");
        //var sapDoNo = ExcelParser.GetValue<string?>(rowData, "X");
        //var sapBillingNo = ExcelParser.GetValue<string?>(rowData, "Y");
        //var givNo = ExcelParser.GetValue<string?>(rowData, "Z");
        //var invoiceNo = ExcelParser.GetValue<string?>(rowData, "AA");
        //var porNo = ExcelParser.GetValue<string?>(rowData, "AE");

        // Required fields
        if (string.IsNullOrWhiteSpace(gicNo))
            result.Errors.Add($"GIC No (B) is required.");

        if (string.IsNullOrWhiteSpace(materialCode))
            result.Errors.Add($"Material Code (K) is required.");

        if (string.IsNullOrWhiteSpace(modelName))
            result.Errors.Add($"Model Name (L) is required.");

        if (string.IsNullOrWhiteSpace(soNo))
            result.Errors.Add($"SO No (R) is required.");

        //if (string.IsNullOrWhiteSpace(sapSoNo))
        //    result.Errors.Add($"SAP SO No (W) is required.");

        //if (string.IsNullOrWhiteSpace(sapDoNo))
        //    result.Errors.Add($"SAP DO No (X) is required.");

        //if (string.IsNullOrWhiteSpace(sapBillingNo))
        //    result.Errors.Add($"SAP Billing No (Y) is required.");

        //if (string.IsNullOrWhiteSpace(givNo))
        //    result.Errors.Add($"GIV No (Z) is required.");

        //if (string.IsNullOrWhiteSpace(invoiceNo))
        //    result.Errors.Add($"Invoice No (AA) is required.");


        //if (string.IsNullOrWhiteSpace(porNo))
        //    result.Errors.Add($"POR No (AE) is required.");

        //ExcelParser.ValidateDecimal(rowData, "AC", "SAP Landing Cost", rowIndex, result, true);
        //ExcelParser.ValidateDecimal(rowData, "AD", "Amount in SAP Landing Cost", rowIndex, result, true);
        //ExcelParser.ValidateDate(rowData, "AB", "Invoice Date", rowIndex, result);

        //ExcelParser.ValidateAmountCalculation(
        //    rowData,
        //    "AC", "S", "AD",
        //    "SAP Landing Cost", "SO Qty", "Amount in SAP Landing Cost",
        //    rowIndex,
        //    result
        //);


        return result;
    }



}