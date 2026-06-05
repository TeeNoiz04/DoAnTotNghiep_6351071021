using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.Shared.Excels;
using QuoteFlow.Shared.Helpers;
using System;
using System.Collections.Generic;

namespace QuoteFlow.PriceOffers.Excels.PriceOfferDSs.Validators;

public class PriceOfferDSDetailRowValidator : IExcelRowValidator<PriceOfferDetailImportDto>
{
    // we can do dependency injection here if needed, e.g. for logging or other services
    public PriceOfferDSDetailRowValidator()
    {

    }

    public ValidationResult ValidateRow(IDictionary<string, object> rowData, int rowIndex)
    {
        var result = new ValidationResult();
        var rowNo = ExcelParser.GetValue<string>(rowData, "A");
        var golfaCode = ExcelParser.GetValue<string?>(rowData, "B");
        var modelName = ExcelParser.GetValue<string?>(rowData, "C");
        var qty = ExcelParser.GetValue<decimal?>(rowData, "F");
        var standardPrice = ExcelParser.GetValue<decimal?>(rowData, "G");
        var standardAmountProvided = ExcelParser.GetValue<decimal?>(rowData, "H");
        var buyerPrice = ExcelParser.GetValue<decimal?>(rowData, "I");
        var requestedAmountProvided = ExcelParser.GetValue<decimal?>(rowData, "J");
        var requestedDiscountRatioProvided = ExcelParser.GetValue<decimal?>(rowData, "K");
        var priceToCustomerString = ExcelParser.GetValue<string?>(rowData, "L");
        decimal? priceToCustomer = string.IsNullOrEmpty(priceToCustomerString) ? null : decimal.Parse(priceToCustomerString);
        var MEVNOfferPrice = ExcelParser.GetValue<decimal?>(rowData, "M");

        if (string.IsNullOrEmpty(rowNo)) result.AddError("Row No is required.");
        else if (int.TryParse(rowNo, out var rowNumber) == false || rowNumber <= 0) result.AddError("Row No must be a positive integer.");
        if (string.IsNullOrEmpty(golfaCode)) result.AddError("Golfa Code is required.");
        if (string.IsNullOrEmpty(modelName)) result.AddError("Model Name is required.");
        if (qty is null || qty <= 0) result.AddError("Qty must be greater than 0.");
        if (standardPrice is null || standardPrice <= 0) result.AddError("Standard Price must be greater than 0.");
        else if (standardPrice.HasValue && !decimal.IsInteger(standardPrice.Value)) result.AddError("Standard Price must be a whole number.");
        if (buyerPrice is null || buyerPrice < 0) result.AddError("Buyer Price must be greater than 0.");
        else if (buyerPrice.HasValue && !decimal.IsInteger(buyerPrice.Value)) result.AddError("Buyer Price must be a whole number.");
        if (priceToCustomer is not null && priceToCustomer < 0) result.AddError("Price to Customer must be greater than 0.");
        else if (priceToCustomer.HasValue && !decimal.IsInteger(priceToCustomer.Value)) result.AddError("Price to Customer must be a whole number.");
        if (MEVNOfferPrice is null || MEVNOfferPrice <= 0) result.AddError("MEVN Offer Price must be greater than 0.");
        else if (MEVNOfferPrice.HasValue && !decimal.IsInteger(MEVNOfferPrice.Value)) result.AddError("MEVN Offer Price must be a whole number.");

        // Validate Standard Amount if provided
        var (isValid, _, errorMessage) = ExcelValidationHelper.ValidateAmountCalculation(qty, standardPrice, standardAmountProvided, "Standard Amount");
        if (!isValid) result.AddError(errorMessage ?? "");

        // Validate Requested Amount if provided
        (isValid, _, errorMessage) = ExcelValidationHelper.ValidateAmountCalculation(qty, buyerPrice, requestedAmountProvided, "Requested Amount");
        if (!isValid) result.AddError(errorMessage ?? "");

        // Validate Requested Discount Ratio if provided
        if (standardPrice.HasValue && standardPrice.Value > 0 && buyerPrice.HasValue)
        {
            var calculatedDiscountRatio = ((standardPrice.Value - buyerPrice.Value) / standardPrice.Value);
            if (requestedDiscountRatioProvided.HasValue && Math.Abs(requestedDiscountRatioProvided.Value - calculatedDiscountRatio) > 0.01m)
            {
                result.AddError($"Requested Discount Ratio is incorrect. Expected: {calculatedDiscountRatio:N2}%, but got: {requestedDiscountRatioProvided.Value:N2}%");
            }
        }

        return result;
    }

    public PriceOfferDetailImportDto ParseRow(IDictionary<string, object> rowData)
    {
        var rowNo = ExcelParser.GetValue<int>(rowData, "A");
        var golfaCode = ExcelParser.GetValue<string?>(rowData, "B");
        var modelName = ExcelParser.GetValue<string?>(rowData, "C");
        var specialSpec1 = ExcelParser.GetValue<string?>(rowData, "D");
        var specialSpec2 = ExcelParser.GetValue<string?>(rowData, "E");
        var qty = ExcelParser.GetValue<decimal?>(rowData, "F");
        var standardPrice = ExcelParser.GetValue<decimal?>(rowData, "G");
        var buyerPrice = ExcelParser.GetValue<decimal?>(rowData, "I");
        var priceToCustomer = ExcelParser.GetValue<decimal?>(rowData, "L");
        var MEVNOfferPrice = ExcelParser.GetValue<decimal?>(rowData, "M");

        // Calculate amounts and discount ratio instead of reading from Excel
        standardPrice = decimal.Round(standardPrice ?? 0, 0);
        buyerPrice = decimal.Round(buyerPrice ?? 0, 0);
        priceToCustomer = decimal.Round(priceToCustomer ?? 0, 0);
        MEVNOfferPrice = decimal.Round(MEVNOfferPrice ?? 0, 0);

        var standardAmount = (qty ?? 0) * (standardPrice ?? 0);
        var requestedAmount = (qty ?? 0) * (buyerPrice ?? 0);
        var MEVNOfferAmount = (qty ?? 0) * (MEVNOfferPrice ?? 0);

        decimal? requestedDiscountRatio = null;
        if (standardPrice.HasValue && standardPrice.Value > 0 && buyerPrice.HasValue)
        {
            requestedDiscountRatio = standardPrice.Value == 0 ? 0 : ((standardPrice.Value - buyerPrice.Value) / standardPrice.Value);
        }

        return new PriceOfferDetailImportDto()
        {
            RowNo = rowNo,
            GolfaCode = golfaCode,
            ModelName = modelName,
            SpecialSpec1 = specialSpec1,
            SpecialSpec2 = specialSpec2,
            Qty = qty,
            StandardPrice = standardPrice,
            StandardAmount = standardAmount,
            BuyerPrice = buyerPrice,
            RequestedAmount = requestedAmount,
            RequestedDiscountRatio = requestedDiscountRatio,
            PriceToCustomer = priceToCustomer,
            MEVNOfferPrice = MEVNOfferPrice,
            MEVNOfferAmount = MEVNOfferAmount,
        };
    }

}
