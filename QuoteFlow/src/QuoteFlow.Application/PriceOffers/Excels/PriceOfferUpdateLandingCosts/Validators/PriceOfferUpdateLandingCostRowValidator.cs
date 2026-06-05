using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.Shared.Excels;
using System.Collections.Generic;
using System.Globalization;

namespace QuoteFlow.PriceOffers.Excels.PriceOfferUpdateLandingCosts.Validators;

public class PriceOfferUpdateLandingCostRowValidator : IExcelRowValidator<PriceOfferUpdateLandingCostImportDto>
{
    public PriceOfferUpdateLandingCostRowValidator()
    {
    }

    public ValidationResult ValidateRow(IDictionary<string, object> rowData, int rowIndex)
    {
        var result = new ValidationResult();

        var golfaCode = ExcelParser.GetStringValue(rowData, "A");
        var modelName = ExcelParser.GetStringValue(rowData, "B");
        var saleOfferPrice = ExcelParser.GetStringValue(rowData, "C");
        var qty = ExcelParser.GetStringValue(rowData, "D");
        var landingCost = ExcelParser.GetStringValue(rowData, "E");
        var newSaleOfferPrice = ExcelParser.GetStringValue(rowData, "F");

        // Validate required fields (first 3 columns)
        if (string.IsNullOrWhiteSpace(golfaCode))
            result.AddError("Golfa Code is required.");
        if (string.IsNullOrWhiteSpace(modelName))
            result.AddError("Model Name is required.");
        if (string.IsNullOrWhiteSpace(saleOfferPrice))
            result.AddError("Sale Offer Price is required.");


        if (!decimal.TryParse(saleOfferPrice, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal saleOfferPriceDecimal) && !string.IsNullOrWhiteSpace(saleOfferPrice))
            result.AddError("Sale Offer Price must be a valid decimal number.");
        else if (!decimal.IsInteger(saleOfferPriceDecimal))
            result.AddError("Sale Offer Price must be a whole number.");

        // Check if all three update fields are empty
        var isQtyEmpty = string.IsNullOrWhiteSpace(qty);
        var isLandingCostEmpty = string.IsNullOrWhiteSpace(landingCost);
        var isNewSaleOfferPriceEmpty = string.IsNullOrWhiteSpace(newSaleOfferPrice);

        if (isQtyEmpty && isLandingCostEmpty && isNewSaleOfferPriceEmpty)
        {
            result.AddError("At least one of Qty, Landing Cost, or New Sale Offer Price must be provided.");
        }

        // Validate numeric formats for non-empty fields
        if (!isQtyEmpty && !int.TryParse(qty, out _))
            result.AddError("Qty must be a valid integer.");

        // Landing Cost
        if (!decimal.TryParse(landingCost, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal landingCostDecimal) && !isLandingCostEmpty)
            result.AddError("Landing Cost must be a valid decimal number.");
        else if (!decimal.IsInteger(landingCostDecimal))
            result.AddError("Landing Cost must be a whole number.");

        // New Sale Offer Price
        if (!decimal.TryParse(newSaleOfferPrice, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal newSaleOfferPriceDecimal) && !isNewSaleOfferPriceEmpty)
            result.AddError("New Sale Offer Price must be a valid decimal number.");
        else if (!decimal.IsInteger(newSaleOfferPriceDecimal))
            result.AddError("New Sale Offer Price must be a whole number.");
        return result;
    }

    public PriceOfferUpdateLandingCostImportDto ParseRow(IDictionary<string, object> rowData)
    {
        var golfaCode = ExcelParser.GetStringValue(rowData, "A");
        var modelName = ExcelParser.GetStringValue(rowData, "B");
        var saleOfferPriceStr = ExcelParser.GetStringValue(rowData, "C");
        var qtyStr = ExcelParser.GetStringValue(rowData, "D");
        var landingCostStr = ExcelParser.GetStringValue(rowData, "E");
        var newSaleOfferPriceStr = ExcelParser.GetStringValue(rowData, "F");

        decimal? saleOfferPrice = null;
        if (!string.IsNullOrWhiteSpace(saleOfferPriceStr) && decimal.TryParse(saleOfferPriceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var saleOfferPriceValue))
            saleOfferPrice = decimal.Round(saleOfferPriceValue);

        int? qty = null;
        if (!string.IsNullOrWhiteSpace(qtyStr) && int.TryParse(qtyStr, out var qtyValue))
            qty = qtyValue;

        decimal? landingCost = null;
        if (!string.IsNullOrWhiteSpace(landingCostStr) && decimal.TryParse(landingCostStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var landingCostValue))
            landingCost = decimal.Round(landingCostValue);

        decimal? newSaleOfferPrice = null;
        if (!string.IsNullOrWhiteSpace(newSaleOfferPriceStr) && decimal.TryParse(newSaleOfferPriceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var newSaleOfferPriceValue))
            newSaleOfferPrice = decimal.Round(newSaleOfferPriceValue);

        return new PriceOfferUpdateLandingCostImportDto
        {
            GolfaCode = golfaCode,
            ModelName = modelName,
            SaleOfferPrice = saleOfferPrice,
            Qty = qty,
            LandingCost = landingCost,
            NewSaleOfferPrice = newSaleOfferPrice
        };
    }
}
