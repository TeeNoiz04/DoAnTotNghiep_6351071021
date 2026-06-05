using QuoteFlow.Materials;
using QuoteFlow.Materials.ParameterObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace QuoteFlow.Shared.Services;

/// <summary>
/// Generic service for validating material combinations against the materials database.
/// Supports flexible field mapping for different classes with different property names.
/// </summary>
public class MaterialCombinationValidationService : IMaterialCombinationValidationService, ITransientDependency
{
    private readonly IMaterialRepository _materialRepository;
    private readonly ILogger<MaterialCombinationValidationService> _logger;

    public MaterialCombinationValidationService(
        IMaterialRepository materialRepository,
        ILogger<MaterialCombinationValidationService> logger)
    {
        _materialRepository = materialRepository;
        _logger = logger;
    }

    /// <summary>
    /// Validates material combinations including existence, standard price, and material status.
    /// </summary>
    /// <typeparam name="T">Type of the input items</typeparam>
    /// <param name="items">Collection of items to validate</param>
    /// <param name="fieldMapping">Mapping configuration for extracting fields from items</param>
    /// <returns>Validation result with categorized errors</returns>
    public async Task<MaterialValidationResult<T>> ValidateAsync<T>(
        IEnumerable<T> items,
        MaterialCombinationFieldMapping<T> fieldMapping)
    {
        var result = new MaterialValidationResult<T>();

        if (items == null || !items.Any())
        {
            return result;
        }

        var validItems = items
            .Where(item => !string.IsNullOrWhiteSpace(fieldMapping.GetCode(item)) &&
                          !string.IsNullOrWhiteSpace(fieldMapping.GetModel(item)))
            .ToList();

        if (!validItems.Any())
        {
            return result;
        }

        try
        {
            // Query active materials that match any of our combinations
            var filterParams = new MaterialFilterParams
            {
                SkipCount = 0,
                MaxResultCount = int.MaxValue,
                //MaterialStatus = MaterialStatuses.Active // Only get active materials
            };

            var existingMaterials = await _materialRepository.GetListWithDeactiveAsync(
                filterParams,
                m => new MaterialDbCombination
                {
                    MaterialType = m.MaterialType,
                    Code = m.GolfaCode,
                    Model = m.Model,
                    StandardPrice = m.Standard_Price,
                    SellingPrice1 = m.SellingPrice1,
                    MaterialStatus = m.MaterialStatus,
                    Spec1 = m.Spec1
                });

            var existingCombinationsDict = existingMaterials
                .GroupBy(m => CreateCombinationKey(m.MaterialType, m.Code, m.Model))
                .ToDictionary(g => g.Key, g => g.First());

            // Check each item for validation errors
            foreach (var item in validItems)
            {
                var materialType = fieldMapping.GetMaterialType?.Invoke(item);
                var golfaCode = fieldMapping.GetCode(item);
                var modelName = fieldMapping.GetModel(item);

                var itemKey = CreateCombinationKey(materialType, golfaCode, modelName);

                // Check if combination exists in active materials
                if (!existingCombinationsDict.TryGetValue(itemKey, out var existingMaterial))
                {
                    string otherMaterialType = (materialType == "FA") ? "LVS" : "FA";

                    // Only perform this check if the expected material type is either FA or LVS
                    // and the other material type is different from the expected one.
                    if (materialType == "FA" || materialType == "LVS")
                    {
                        var otherKey = CreateCombinationKey(otherMaterialType, golfaCode, modelName);

                        if (existingCombinationsDict.TryGetValue(otherKey, out var actualExistingMaterial))
                        {
                            // Material combination *found* under the other MaterialType (e.g., expected FA, found LVS).
                            result.Errors.Add(new MaterialValidationError<T>
                            {
                                Item = item,
                                ErrorType = MaterialValidationErrorType.WrongMaterialType, // Re-using this type or define a new one like 'WrongMaterialType'
                                ErrorMessage = $"Material '{golfaCode} - {modelName}' does not belong to the expected MaterialType '{materialType}'. Actual MaterialType: '{otherMaterialType}'.",
                                ProvidedMaterialType = materialType,
                                ExpectedMaterialType = otherMaterialType,
                                GolfaCode = golfaCode,
                                ModelName = modelName
                            });
                            continue;
                        }
                    }

                    // Material combination doesn't exist at all
                    result.Errors.Add(new MaterialValidationError<T>
                    {
                        Item = item,
                        ErrorType = MaterialValidationErrorType.CombinationNotFound,
                        ErrorMessage = $"Material combination not found: MaterialType='{materialType}', GolfaCode='{golfaCode}', ModelName='{modelName}'",
                        ProvidedMaterialType = materialType,
                        GolfaCode = golfaCode,
                        ModelName = modelName
                    });
                    continue;
                }

                if (existingMaterial.MaterialStatus == MaterialStatuses.Deactivated)
                {
                    // Material exists but is deactivated (error state)
                    result.Errors.Add(new MaterialValidationError<T>
                    {
                        Item = item,
                        ErrorType = MaterialValidationErrorType.MaterialDeactivated,
                        ErrorMessage = $"Material combination is deactivated: MaterialType='{materialType}', GolfaCode='{golfaCode}', ModelName='{modelName}'",
                        ProvidedMaterialType = materialType,
                        GolfaCode = golfaCode,
                        ModelName = modelName
                    });
                    continue;
                }

                // Validate standard price if provided
                var providedPrice = fieldMapping.GetStandardPrice?.Invoke(item);
                var appliedPrice = fieldMapping.GetAppliedPrice?.Invoke(item);
                if (providedPrice.HasValue)
                {
                    var expectedPrice = existingMaterial.StandardPrice;
                    var expectedPriceSource = "Standard Price";
                    var errorType = MaterialValidationErrorType.StandardPriceMismatch;

                    if (appliedPrice == 1)
                    {
                        expectedPrice = existingMaterial.SellingPrice1 ?? existingMaterial.StandardPrice;
                        expectedPriceSource = existingMaterial.SellingPrice1.HasValue ? "Selling Price" : "Standard Price (fb)";
                        errorType = MaterialValidationErrorType.SellingPriceMismatch;
                    }

                    if (expectedPrice.HasValue && Math.Abs(providedPrice.Value - expectedPrice.Value) > 0.01m) // Allow small rounding differences
                    {
                        result.Errors.Add(new MaterialValidationError<T>
                        {
                            Item = item,
                            ErrorType = errorType,
                            ErrorMessage =
                                $"Price mismatch for MaterialType='{materialType}', GolfaCode='{golfaCode}', ModelName='{modelName}': " +
                                $"Expected {expectedPriceSource}={expectedPrice:F2}, Provided={providedPrice:F2}",
                            ProvidedMaterialType = materialType,
                            GolfaCode = golfaCode,
                            ModelName = modelName,
                            ExpectedStandardPrice = expectedPrice,
                            ProvidedStandardPrice = providedPrice
                        });
                    }
                }

                //var spec1 = fieldMapping.GetSpec1?.Invoke(item);
                //if (fieldMapping.GetSpec1 is not null)
                //{
                //    if (!string.Equals(existingMaterial.Spec1?.Trim(), spec1?.Trim(), StringComparison.OrdinalIgnoreCase))
                //    {
                //        if (string.IsNullOrWhiteSpace(existingMaterial?.Spec1) && string.IsNullOrWhiteSpace(spec1))
                //        {
                //            continue; // Both are null/empty, consider as match
                //        }

                //        result.Errors.Add(new MaterialValidationError<T>
                //        {
                //            Item = item,
                //            ErrorType = MaterialValidationErrorType.Spec1Mismatch,
                //            ErrorMessage =
                //                $"Spec 1 mismatch for MaterialType='{materialType}', GolfaCode='{golfaCode}', ModelName='{modelName}': " +
                //                $"Expected Spec 1='{existingMaterial.Spec1}', Provided Spec 1='{spec1}'",
                //            ProvidedMaterialType = materialType,
                //            GolfaCode = golfaCode,
                //            ModelName = modelName,
                //            ExpectedSpec1 = existingMaterial.Spec1,
                //            ProvidedSpec1 = spec1
                //        });
                //    }
                //}
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating material combinations against database");
            throw new InvalidOperationException("Error occurred while validating material combinations against database.", ex);
        }
    }

    /// <summary>
    /// Creates a normalized key for material combination comparison.
    /// </summary>
    private static string CreateCombinationKey(string? materialType, string? code, string? model)
    {
        var normalizedMaterialType = materialType?.Trim().ToLowerInvariant() ?? string.Empty;
        var normalizedCode = code?.Trim().ToLowerInvariant() ?? string.Empty;
        var normalizedModel = model?.Trim().ToLowerInvariant() ?? string.Empty;

        return $"{normalizedMaterialType}|{normalizedCode}|{normalizedModel}";
    }

    /// <summary>
    /// Internal class for database material combinations.
    /// </summary>
    internal class MaterialDbCombination
    {
        public string? MaterialType { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public decimal? StandardPrice { get; set; }
        public decimal? SellingPrice1 { get; set; }
        public string? MaterialStatus { get; set; }
        public string? Spec1 { get; set; }
    }

    /// <summary>
    /// Internal class for simple combinations.
    /// </summary>
    internal class SimpleCombination
    {
        public string? MaterialType { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
    }
}
