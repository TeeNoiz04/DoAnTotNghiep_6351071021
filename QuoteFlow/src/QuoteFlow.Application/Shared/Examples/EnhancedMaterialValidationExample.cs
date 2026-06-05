using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.Shared.Excels;
using QuoteFlow.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteFlow.Shared.Examples;

/// <summary>
/// Example usage of the enhanced MaterialCombinationValidationService
/// </summary>
public class EnhancedMaterialValidationExample
{
    private readonly IMaterialCombinationValidationService _materialValidationService;

    public EnhancedMaterialValidationExample(IMaterialCombinationValidationService materialValidationService)
    {
        _materialValidationService = materialValidationService;
    }

    /// <summary>
    /// Example 1: Basic validation for PriceOffer details with standard price validation
    /// </summary>
    public async Task<MaterialValidationResult<PriceOfferDetailImportDto>> ValidatePriceOfferDetailsAsync(
        List<PriceOfferDetailImportDto> details,
        string materialType)
    {
        var fieldMapping = new MaterialCombinationFieldMapping<PriceOfferDetailImportDto>
        {
            GetMaterialType = x => materialType, // Fixed material type for all items
            GetCode = x => x.GolfaCode,
            GetModel = x => x.ModelName,
            GetStandardPrice = x => x.StandardPrice // Include standard price validation
        };

        var result = await _materialValidationService.ValidateAsync(details, fieldMapping);

        // Process different types of errors
        if (result.HasErrors)
        {
            // Handle combination not found errors
            foreach (var error in result.CombinationNotFoundErrors)
            {
                // Log or handle missing material combinations
                Console.WriteLine($"Missing combination: {error.GolfaCode} - {error.ModelName}");
            }

            // Handle standard price mismatch errors
            foreach (var error in result.StandardPriceMismatchErrors)
            {
                Console.WriteLine($"Price mismatch for {error.GolfaCode}: Expected={error.ExpectedStandardPrice:F2}, Got={error.ProvidedStandardPrice:F2}");
            }

            // Handle discontinued material errors
            foreach (var error in result.MaterialDiscontinuedErrors)
            {
                Console.WriteLine($"Discontinued material: {error.GolfaCode} - {error.ModelName}");
            }
        }

        return result;
    }

    /// <summary>
    /// Example 2: Excel row validation with enhanced error reporting
    /// </summary>
    public async Task ValidateExcelRowsWithEnhancedErrorsAsync(
        List<ExcelRowResult<PriceOfferDetailImportDto>> excelRows,
        string materialType)
    {
        // Extract data for validation
        var validRows = excelRows
            .Where(row => row.RowData != null)
            .Select(row => new MaterialValidationRowWrapper
            {
                RowIndex = row.RowIndex,
                ExcelRow = row,
                GolfaCode = row.RowData!.GolfaCode,
                ModelName = row.RowData!.ModelName,
                StandardPrice = row.RowData!.StandardPrice
            })
            .ToList();

        var fieldMapping = new MaterialCombinationFieldMapping<MaterialValidationRowWrapper>
        {
            GetMaterialType = x => materialType,
            GetCode = x => x.GolfaCode,
            GetModel = x => x.ModelName,
            GetStandardPrice = x => x.StandardPrice
        };

        var validationResult = await _materialValidationService.ValidateAsync(validRows, fieldMapping);

        // Apply validation errors back to Excel rows
        foreach (var error in validationResult.Errors)
        {
            var excelRow = error.Item.ExcelRow;
            var errorMessage = error.ErrorType switch
            {
                MaterialValidationErrorType.CombinationNotFound =>
                    $"Material not found: {error.GolfaCode} - {error.ModelName}",
                MaterialValidationErrorType.StandardPriceMismatch =>
                    $"Price mismatch: Expected {error.ExpectedStandardPrice:F2}, provided {error.ProvidedStandardPrice:F2}",
                MaterialValidationErrorType.MaterialDeactivated =>
                    $"Material discontinued: {error.GolfaCode} - {error.ModelName}",
                _ => error.ErrorMessage
            };

            excelRow.Errors.Add(errorMessage);
        }
    }

    /// <summary>
    /// Example 3: Validation without standard price (existing functionality)
    /// </summary>
    public async Task<MaterialValidationResult<SimpleItem>> ValidateWithoutPriceAsync(
        List<SimpleItem> items,
        string? materialType = null)
    {
        var fieldMapping = new MaterialCombinationFieldMapping<SimpleItem>
        {
            GetMaterialType = x => materialType,
            GetCode = x => x.Code,
            GetModel = x => x.Model
            // GetStandardPrice not set - no price validation
        };

        return await _materialValidationService.ValidateAsync(items, fieldMapping);
    }

    /// <summary>
    /// Example wrapper class for Excel row validation
    /// </summary>
    public class MaterialValidationRowWrapper
    {
        public int RowIndex { get; set; }
        public ExcelRowResult<PriceOfferDetailImportDto> ExcelRow { get; set; } = null!;
        public string? GolfaCode { get; set; }
        public string? ModelName { get; set; }
        public decimal? StandardPrice { get; set; }
    }

    /// <summary>
    /// Simple item class for demonstration
    /// </summary>
    public class SimpleItem
    {
        public string Code { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
    }
}
