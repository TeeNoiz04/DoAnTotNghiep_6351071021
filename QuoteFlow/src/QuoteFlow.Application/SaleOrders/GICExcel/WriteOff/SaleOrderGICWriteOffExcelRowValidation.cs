using QuoteFlow.SaleOrders.Excel;
using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;

namespace QuoteFlow.SaleOrders.GICExcel.WriteOff;

public class SaleOrderGICWriteOffExcelRowValidation : IExcelRowValidator<SaleOrderGICWriteOffExcelDto>
{
    private const string DATE_TIME = "dd/MM/yyyy";

    public SaleOrderGICWriteOffExcelDto ParseRow(IDictionary<string, object> rowData)
    {
        return new SaleOrderGICWriteOffExcelDto
        {
            GICWONo = ExcelParser.GetValue<string?>(rowData, "B")?.Trim(),                   // GIC-WO No
            GICWODate = ExcelParser.GetValue<DateTime?>(rowData, "C", DATE_TIME),            // GIC-WO Date
            CostCenter = ExcelParser.GetValue<string?>(rowData, "D")?.Trim(),                // Cost Center
            MaterialType = ExcelParser.GetValue<string?>(rowData, "E")?.Trim(),              // Material Type
            No = ExcelParser.GetValue<string?>(rowData, "F")?.Trim(),                        // No
            MaterialCode = ExcelParser.GetValue<string?>(rowData, "G")?.Trim(),              // Material Code
            ModelName = ExcelParser.GetValue<string?>(rowData, "H")?.Trim(),                 // Model Name
            SAPCode = ExcelParser.GetValue<string?>(rowData, "I")?.Trim(),                   // SAP Code
            Spec1 = ExcelParser.GetValue<string?>(rowData, "J")?.Trim(),                     // Spec1
                                                                                             // Qty
            SONo = ExcelParser.GetValue<string?>(rowData, "L")?.Trim(),                      // SO No
            SOQty = ExcelParser.GetValue<decimal?>(rowData, "M"),                            // SO Qty

            SOType = ExcelParser.GetValue<string?>(rowData, "O")?.Trim(),                    // SOType
            GIVNo = ExcelParser.GetValue<string?>(rowData, "P")?.Trim(),                     // GIV No
            GIVDate = ExcelParser.GetValue<DateTime?>(rowData, "Q", DATE_TIME),              // GIV Date
            SAPLandingCost = ExcelParser.GetValue<decimal?>(rowData, "R"),                   // SAP Landing Cost
            AmountInSAPLandingCost = ExcelParser.GetValue<decimal?>(rowData, "S"),           // Amount in SAP Landing Cost
            Note = ExcelParser.GetValue<string?>(rowData, "U")?.Trim(),
            Disposed = ExcelParser.GetValue<string?>(rowData, "T")?.Trim(),
        };
    }


    public ValidationResult ValidateRow(IDictionary<string, object> rowData, int rowIndex)
    {
        var result = new ValidationResult();

        var gicWONo = ExcelParser.GetValue<string?>(rowData, "B");
        var materialCode = ExcelParser.GetValue<string?>(rowData, "G");
        var modelName = ExcelParser.GetValue<string?>(rowData, "H");
        var soNo = ExcelParser.GetValue<string?>(rowData, "L");
        var givNo = ExcelParser.GetValue<string?>(rowData, "P");

        if (string.IsNullOrWhiteSpace(gicWONo))
            result.Errors.Add($"GIC-WO No (B) is required.");

        if (string.IsNullOrWhiteSpace(materialCode))
            result.Errors.Add($"Material Code (G) is required.");

        if (string.IsNullOrWhiteSpace(modelName))
            result.Errors.Add($"Model Name (H) is required.");

        if (string.IsNullOrWhiteSpace(soNo))
            result.Errors.Add($"SO No (L) is required.");

        //if (string.IsNullOrWhiteSpace(givNo))
        //    result.Errors.Add($"GIV No (P) is required.");

        //ExcelParser.ValidateDate(rowData, "Q", "GIV Date", rowIndex, result);
        //ExcelParser.ValidateDecimal(rowData, "R", "SAP Landing Cost", rowIndex, result, true);
        //ExcelParser.ValidateDecimal(rowData, "S", "Amount in SAP Landing Cost", rowIndex, result, true);

        // ExcelParser.ValidateAmountCalculation(
        //    rowData,
        //    "R", "M", "S",
        //    "SAP Landing Cost", "SO Qty", "Amount in SAP Landing Cost",
        //    rowIndex,
        //    result
        //);

        return result;
    }


}
