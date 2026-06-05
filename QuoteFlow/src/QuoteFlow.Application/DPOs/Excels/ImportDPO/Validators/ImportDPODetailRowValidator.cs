using QuoteFlow.DPOs.DPODetails;
using QuoteFlow.PriceOffers;
using QuoteFlow.Shared.Excels;
using QuoteFlow.Shared.Helpers;
using QuoteFlow.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuoteFlow.DPOs.Excels.ImportDPO.Validators;

public class ImportDPODetailRowValidator : IExcelRowValidator<ImportDPODetailDto>
{
    public ImportDPODetailDto ParseRow(IDictionary<string, object> row)
    {
        var qty = ExcelParser.GetValue<int?>(row, "G");
        var unitPrice = ExcelParser.GetValue<decimal?>(row, "H");

        // Calculate amount instead of reading from Excel
        var amount = (qty ?? 0) * (unitPrice ?? 0);

        return new ImportDPODetailDto
        {
            RowNo = ExcelParser.GetValue<int?>(row, "A"),
            GolfaCode = ExcelParser.GetValue<string?>(row, "B"),
            Model = ExcelParser.GetValue<string?>(row, "C"),
            Spec1 = ExcelParser.GetValue<string?>(row, "D"),
            Spec2 = ExcelParser.GetValue<string?>(row, "E"),
            SPOCode = ExcelParser.GetValue<string?>(row, "F"),
            Qty = qty,
            UnitPrice = unitPrice,
            Amount = amount,
            RequestedETA = ExcelParser.GetValue<DateTime?>(row, "J"),
            CustomerName = ExcelParser.GetValue<string?>(row, "K"),
            CustomerTaxCode = ExcelParser.GetValue<string?>(row, "L"),
            CustomerType = ExcelParser.GetValue<string?>(row, "M"),
            Note = ExcelParser.GetValue<string?>(row, "N")
        };
    }

    public ValidationResult ValidateRow(IDictionary<string, object> rowData, int rowIndex)
    {
        var result = new ValidationResult();

        var rowNo = ExcelParser.GetValue<int?>(rowData, "A");
        var golfaCode = ExcelParser.GetValue<string?>(rowData, "B");
        var model = ExcelParser.GetValue<string?>(rowData, "C");
        var spoCode = ExcelParser.GetValue<string?>(rowData, "F");
        var qty = ExcelParser.GetValue<int?>(rowData, "G");
        var unitPrice = ExcelParser.GetValue<decimal?>(rowData, "H");
        var amountProvided = ExcelParser.GetValue<decimal?>(rowData, "I");
        var requestedETA = ExcelParser.GetValue<DateTime?>(rowData, "J");
        var requestedETAString = ExcelParser.GetValue<string?>(rowData, "J");
        var customerName = ExcelParser.GetValue<string?>(rowData, "K");
        var customerTaxCode = ExcelParser.GetValue<string?>(rowData, "L");
        var customerType = ExcelParser.GetValue<string?>(rowData, "M");

        // Required validations
        if (rowNo is null || rowNo.Value <= 0) result.AddError("Row number is required and must be greater than 0");
        if (string.IsNullOrWhiteSpace(golfaCode)) result.AddError("Material Code is required.");
        if (string.IsNullOrWhiteSpace(model)) result.AddError("Model is required.");
        if (qty is null || qty <= 0) result.AddError("Qty is required and must be greater than 0.");
        if (unitPrice is null || unitPrice < 0) result.AddError("Unit Price is required and must be greater than or equal to 0.");

        // Validate amount calculation if provided
        var (isValid, _, errorMessage) = ExcelValidationHelper.ValidateAmountCalculation(qty, unitPrice, amountProvided, "Amount");
        if (!isValid) result.AddError(errorMessage ?? "");

        // Conditional validations (required if SPO Code is empty)
        IEnumerable<string> spoCodePrefixRequiredCustomerInfo = [
            PriceOfferTypes.PriceOfferNB,
        ];
        var isCustomerInfoRequired = string.IsNullOrWhiteSpace(spoCode) ||
            (spoCodePrefixRequiredCustomerInfo.Any(prefix => spoCode != null && spoCode.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)));

        if (string.IsNullOrWhiteSpace(customerName) && isCustomerInfoRequired)
            result.AddError("Customer Name is required if SPO Code is empty or SPO-NB.");
        if (string.IsNullOrWhiteSpace(customerTaxCode) && isCustomerInfoRequired)
            result.AddError("Customer Tax Code is required if SPO Code is empty or SPO-NB.");
        if (string.IsNullOrWhiteSpace(customerType) && isCustomerInfoRequired)
            result.AddError("Customer Type is required if SPO Code is empty or SPO-NB.");

        // Date validation
        if (requestedETAString is not null &&
            ((requestedETA is not null && requestedETA == default) || requestedETA is null))
            result.AddError("Requested ETA must be a valid date.");

        if (!string.IsNullOrWhiteSpace(customerTaxCode) && isCustomerInfoRequired)
        {
            var taxCodeError = CodeHelper.ValidateTaxCode(customerTaxCode);
            if (taxCodeError != null)
            {
                result.AddError(taxCodeError);
            }
        }

        return result;
    }
}