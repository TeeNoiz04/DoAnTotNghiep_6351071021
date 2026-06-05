using QuoteFlow.GICs;
using QuoteFlow.SaleOrders.Excel;
using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;

public class SaleOrderGICInternalUseChangeRowValidation : IExcelRowValidator<SaleOrderGICInternalUseChangeExcelDto>
{
    private const string DATE_TIME = "dd/MM/yyyy";

    public SaleOrderGICInternalUseChangeExcelDto ParseRow(IDictionary<string, object> rowData)
    {
        return new SaleOrderGICInternalUseChangeExcelDto
        {
            GICNo = ExcelParser.GetValue<string?>(rowData, "B")?.Trim(),                 // GIC No (*)
            MaterialCode = ExcelParser.GetValue<string?>(rowData, "I")?.Trim(),          // Material Code (*)
            ModelName = ExcelParser.GetValue<string?>(rowData, "K")?.Trim(),             // Model name
            SONo = ExcelParser.GetValue<string?>(rowData, "O")?.Trim(),                  // SO No (*)
            GICGivNo = ExcelParser.GetValue<string?>(rowData, "R")?.Trim(),              // GIV No
            GICGivDate = ExcelParser.GetValue<DateTime?>(rowData, "S", DATE_TIME),       // GIV Date
            GICReservationNo = ExcelParser.GetValue<string?>(rowData, "T")?.Trim(),      // Reservation No
            GICSAPLandingCost = ExcelParser.GetValue<decimal?>(rowData, "U"),            // SAP Landing Cost
            GICAmountSAPLandingCost = ExcelParser.GetValue<decimal?>(rowData, "V"),      // Amount in SAP Landing Cost
            GICPORNo = ExcelParser.GetValue<string?>(rowData, "W")?.Trim(),              // POR No (*)
            GICPRNo = ExcelParser.GetValue<string?>(rowData, "X")?.Trim(),               // PR No (*)
            GICLocation = ExcelParser.GetValue<string?>(rowData, "Y")?.Trim(),           // Location (*)
            GICSalesPIC = ExcelParser.GetValue<string?>(rowData, "Z")?.Trim(),           // Sale PIC (*)
            Note = ExcelParser.GetValue<string?>(rowData, "AA")?.Trim(),                 // Change Note
        };
    }

    public ValidationResult ValidateRow(IDictionary<string, object> rowData, int rowIndex)
    {
        var result = new ValidationResult();

        var gicNo = ExcelParser.GetValue<string?>(rowData, "B");
        var process = ExcelParser.GetValue<string?>(rowData, "D");
        var materialCode = ExcelParser.GetValue<string?>(rowData, "I");
        var modelName = ExcelParser.GetValue<string?>(rowData, "K");
        var soNo = ExcelParser.GetValue<string?>(rowData, "O");
        var givNo = ExcelParser.GetValue<string?>(rowData, "R");
        var reservationNo = ExcelParser.GetValue<string?>(rowData, "T");
        var porNo = ExcelParser.GetValue<string?>(rowData, "W");
        var prNo = ExcelParser.GetValue<string?>(rowData, "X");
        var location = ExcelParser.GetValue<string?>(rowData, "Y");
        var salesPIC = ExcelParser.GetValue<string?>(rowData, "Z");

        if (string.IsNullOrWhiteSpace(gicNo))
            result.Errors.Add($"GIC No (B) is required.");

        if (string.IsNullOrWhiteSpace(process))
            result.Errors.Add($"Process (D) is required.");

        if (string.IsNullOrWhiteSpace(materialCode))
            result.Errors.Add($"Material Code (I) is required.");

        if (string.IsNullOrWhiteSpace(modelName))
            result.Errors.Add($"Model (K) is required.");

        if (string.IsNullOrWhiteSpace(soNo))
            result.Errors.Add($"SO No (O) is required.");

        if (string.IsNullOrWhiteSpace(givNo))
            result.Errors.Add($"GIV No (R) is required.");

        if (string.IsNullOrWhiteSpace(porNo))
            result.Errors.Add($"POR No (W) is required.");

        //if (string.IsNullOrWhiteSpace(prNo))
        //    result.Errors.Add($"PR No (X) is required.");

        if (string.IsNullOrWhiteSpace(location))
            result.Errors.Add($"Location (Y) is required.");

        if (string.IsNullOrWhiteSpace(salesPIC))
            result.Errors.Add($"Sales PIC (Z) is required.");

        if (!string.IsNullOrWhiteSpace(process) && process == GICProcessCodes.ReservationNo)
        {
            if (string.IsNullOrWhiteSpace(reservationNo))
                result.Errors.Add($"Reservation No (T) is required when process = ReservationNo.");
        }
        var givDate = ExcelParser.GetValue<DateTime?>(rowData, "S");
        if (givDate is null)
        {
            result.Errors.Add($"GIV Date (S) is not a valid date. Please enter a valid date.");
        }
        ExcelParser.ValidateDecimal(rowData, "U", "SAP Landing Cost", rowIndex, result, true);
        ExcelParser.ValidateDecimal(rowData, "V", "Amount in SAP Landing Cost", rowIndex, result, true);

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
