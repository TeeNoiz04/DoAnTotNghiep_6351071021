using QuoteFlow.SaleOrders.Excel;
using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;

namespace QuoteFlow.SaleOrders.GICExcel.InternalUse;

public class SaleOrderGICInternalUseRowValidation : IExcelRowValidator<SaleOrderGICInternalUseExcelDto>
{
    public SaleOrderGICInternalUseExcelDto ParseRow(IDictionary<string, object> rowData)
    {
        return new SaleOrderGICInternalUseExcelDto
        {
            SONo = ExcelParser.GetValue<string?>(rowData, "O")?.Trim(),

            MaterialCode = ExcelParser.GetValue<string?>(rowData, "I")?.Trim(),
            ModelName = ExcelParser.GetValue<string?>(rowData, "K")?.Trim(),
            GICNo = ExcelParser.GetValue<string?>(rowData, "B")?.Trim(),
            GICSAPLandingCost = ExcelParser.GetValue<decimal?>(rowData, "U"),
            GICAmountSAPLandingCost = ExcelParser.GetValue<decimal?>(rowData, "V"),
            GICPORNo = ExcelParser.GetValue<string?>(rowData, "W")?.Trim(),
            GICPRNo = ExcelParser.GetValue<string?>(rowData, "X")?.Trim(),
            GICGivNo = ExcelParser.GetValue<string?>(rowData, "R")?.Trim(),
            GICGivDate = ExcelParser.GetValue<DateTime?>(rowData, "S"),
            GICSalesPIC = ExcelParser.GetValue<string?>(rowData, "Z")?.Trim(),
            GICLocation = ExcelParser.GetValue<string?>(rowData, "Y")?.Trim(),
            GICReservationNo = ExcelParser.GetValue<string?>(rowData, "T")?.Trim(),
            GICAssetClass = ExcelParser.GetValue<string?>(rowData, "AA")?.Trim(),
            GICMainAssetCode = ExcelParser.GetValue<string?>(rowData, "AB")?.Trim(),
            GICSubAssetCode = ExcelParser.GetValue<string?>(rowData, "AC")?.Trim(),
            GICAssetName = ExcelParser.GetValue<string?>(rowData, "AD")?.Trim(),
            GICProcess = ExcelParser.GetValue<string?>(rowData, "D")?.Trim()

        };
    }


    public ValidationResult ValidateRow(IDictionary<string, object> rowData, int rowIndex)
    {
        var result = new ValidationResult();

        var soNo = ExcelParser.GetValue<string?>(rowData, "O");
        var gicNo = ExcelParser.GetValue<string?>(rowData, "B");
        var materialCode = ExcelParser.GetValue<string?>(rowData, "I");
        var modelName = ExcelParser.GetValue<string?>(rowData, "K");
        var gicPORNo = ExcelParser.GetValue<string?>(rowData, "W");
        //var gicPRNo = ExcelParser.GetValue<string?>(rowData, "X");
        //var gicGivNo = ExcelParser.GetValue<string?>(rowData, "R");
        //var gicSalesPIC = ExcelParser.GetValue<string?>(rowData, "Z");
        //var gicLocation = ExcelParser.GetValue<string?>(rowData, "Y");
        //var gicReservationNo = ExcelParser.GetValue<string?>(rowData, "T");
        //var gicAssetClass = ExcelParser.GetValue<string?>(rowData, "AA");
        //var gicMainAssetCode = ExcelParser.GetValue<string?>(rowData, "AB");
        //var gicSubAssetCode = ExcelParser.GetValue<string?>(rowData, "AC");
        //var gicAssetName = ExcelParser.GetValue<string?>(rowData, "AD");
        var process = ExcelParser.GetValue<string?>(rowData, "D");

        // Required string fields
        if (string.IsNullOrWhiteSpace(gicNo))
            result.Errors.Add($"GIC No (B) is required.");

        if (string.IsNullOrWhiteSpace(materialCode))
            result.Errors.Add($"Material Code (I) is required.");

        if (string.IsNullOrWhiteSpace(modelName))
            result.Errors.Add($"Model (K) is required.");

        if (string.IsNullOrWhiteSpace(soNo))
            result.Errors.Add($"SO No (O) is required.");

        if (string.IsNullOrWhiteSpace(process))
            result.Errors.Add($"GIC Process (D) is required.");

        //if (string.IsNullOrWhiteSpace(gicGivNo))
        //    result.Errors.Add($"GIV No (R) is required.");

        //if (!string.IsNullOrWhiteSpace(process) && process == GICProcessCodes.ReservationNo)
        //{
        //    if (string.IsNullOrWhiteSpace(gicReservationNo))
        //        result.Errors.Add($"Reservation No (T) is required.");
        //}

        if (string.IsNullOrWhiteSpace(gicPORNo))
            result.Errors.Add($"POR No (W) is required.");

        //if (string.IsNullOrWhiteSpace(gicLocation))
        //    result.Errors.Add($"Location (Y) is required.");

        //if (string.IsNullOrWhiteSpace(gicSalesPIC))
        //    result.Errors.Add($"Sales PIC (Z) is required.");

        //if (!string.IsNullOrWhiteSpace(process) &&
        //    (process == GICProcessCodes.Asset || process == GICProcessCodes.Tool))
        //{
        //    if (string.IsNullOrWhiteSpace(gicAssetClass))
        //        result.Errors.Add($"Asset Class (AA) is required.");

        //    if (string.IsNullOrWhiteSpace(gicMainAssetCode))
        //        result.Errors.Add($"Main Asset Code (AB) is required.");

        //    if (string.IsNullOrWhiteSpace(gicSubAssetCode))
        //        result.Errors.Add($"Sub Asset Code (AC) is required.");

        //    if (string.IsNullOrWhiteSpace(gicAssetName))
        //        result.Errors.Add($"Asset Name (AD) is required.");
        //}

        // Special type validations
        var givDate = ExcelParser.GetValue<DateTime?>(rowData, "S");
        if (givDate is null)
        {
            result.Errors.Add($"GIV Date (S) is not a valid date. Please enter a valid date.");
        }
        //ExcelParser.ValidateDate(rowData, "S", "GIV Date", rowIndex, result);
        ExcelParser.ValidateDecimal(rowData, "U", "SAP GIC Landing Cost", rowIndex, result, true);
        ExcelParser.ValidateDecimal(rowData, "V", "Amount in SAP GIC Landing Cost", rowIndex, result, true);

        ExcelParser.ValidateAmountCalculation(
            rowData,
            "U", "P", "V",
            "SAP Landing Cost", "SO Qty", "Amount in SAP Landing Cost",
            rowIndex,
            result
        );

        return result;
    }



}
