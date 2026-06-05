using QuoteFlow.MaterialGroupBuyers;
using QuoteFlow.Materials;
using QuoteFlow.Materials.MaterialGroups;
using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.Shared;
using QuoteFlow.Shared.Excels;
using QuoteFlow.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace QuoteFlow.PriceOffers.Excels;

public class PriceOfferMaterialValidationService : ITransientDependency
{
    private readonly IDuplicateDetectionService _duplicateDetectionService;
    private readonly IMaterialCombinationValidationService _materialValidationService;
    private readonly IMaterialGroupBuyerRepository _materialGroupBuyerRepository;
    private readonly IMaterialRepository _materialRepository;
    private readonly IMaterialGroupRepository _materialGroupRepository;
    public PriceOfferMaterialValidationService(IDuplicateDetectionService duplicateDetectionService, IMaterialCombinationValidationService materialValidationService, IMaterialGroupBuyerRepository materialGroupBuyerRepository, IMaterialRepository materialRepository, IMaterialGroupRepository materialGroupRepository)
    {
        _duplicateDetectionService = duplicateDetectionService;
        _materialValidationService = materialValidationService;
        _materialGroupBuyerRepository = materialGroupBuyerRepository;
        _materialRepository = materialRepository;
        _materialGroupRepository = materialGroupRepository;
    }

    /// <summary>
    /// Extended DTO for Price Offer validation that includes MEVNOfferPrice for duplicate detection
    /// </summary>
    private class PriceOfferCombinationDto : MaterialFullCombinationWithRowIndexDto
    {
        /// <summary>
        /// MEVN Offer Price - used for duplicate detection in Price Offer
        /// </summary>
        public decimal MEVNOfferPrice { get; set; }
    }

    public virtual async Task ValidateMaterialCombinationsAndGroupBuyersAsync(
        List<ExcelRowResult<PriceOfferDetailImportDto>> detailsResult,
        string materialType,
        Guid? buyerId,
        int? appliedPrice = null)
    {
        if (detailsResult == null || detailsResult.Count == 0)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(materialType))
        {
            return; // MaterialType validation is handled elsewhere
        }

        // Extract unique combinations and material codes from the detailsResult
        var fileCombinations = detailsResult
            .Where(x => x.RowData != null &&
                       !string.IsNullOrWhiteSpace(x.RowData.GolfaCode) &&
                       !string.IsNullOrWhiteSpace(x.RowData.ModelName))
            .Select(x => new PriceOfferCombinationDto
            {
                MaterialType = materialType,
                GolfaCode = x.RowData.GolfaCode!,
                ModelName = x.RowData.ModelName!,
                StandardPrice = x.RowData.StandardPrice ?? 0,
                MEVNOfferPrice = x.RowData.MEVNOfferPrice ?? 0,
                RowIndex = x.RowIndex,
                AppliedPrice = appliedPrice,
                Spec1 = x.RowData.SpecialSpec1
            })
            .ToList();

        var materialCodes = detailsResult
            .Where(x => x.RowData != null && !string.IsNullOrWhiteSpace(x.RowData.GolfaCode))
            .Select(x => x.RowData.GolfaCode!)
            .Distinct()
            .ToList();

        if (fileCombinations.Count == 0 && materialCodes.Count == 0)
        {
            return;
        }

        // Single database query to get material information for both validations
        var materialData = await _materialRepository.GetQueryableAsync();
        var validMaterialStatuses = new List<string> { MaterialStatuses.Active, MaterialStatuses.Discontinued, MaterialStatuses.Deactivated };
        var materials = materialData
            .Where(x => materialCodes.Contains(x.GolfaCode) && validMaterialStatuses.Contains(x.MaterialStatus))
            .Select(x => new MaterialGroupSupportInfo()
            {
                MaterialGroup = x.Material_Group,
                MaterialCode = x.GolfaCode,
                MaterialStatus = x.MaterialStatus
            })
            .Distinct()
            .ToList();

        var materialGroups = materials
            .Select(x => x.MaterialGroup)
            .Where(x => !string.IsNullOrEmpty(x))
            .Distinct()
            .ToList();

        // For NB type (no buyer), skip material group buyer validation
        var groupBuyers = buyerId.HasValue
            ? await _materialGroupBuyerRepository.GetListAsync(
                x => !string.IsNullOrEmpty(x.MaterialGroupCode) && materialGroups.Contains(x.MaterialGroupCode) && x.BuyerId == buyerId.Value)
            : new List<MaterialGroupBuyers.MaterialGroupBuyer>();

        var detailRowMap = detailsResult
            .Where(x => x.RowData != null)
            .ToDictionary(x => x.RowIndex, x => x);

        // Validate material combinations if we have combinations to validate
        if (fileCombinations.Count > 0)
        {
            // Field mapping for duplicate detection - uses MEVNOfferPrice
            var duplicateDetectionFieldMapping = new MaterialCombinationFieldMapping<PriceOfferCombinationDto>
            {
                GetMaterialType = x => x.MaterialType,
                GetCode = x => x.GolfaCode,
                GetModel = x => x.ModelName,
                GetStandardPrice = x => x.MEVNOfferPrice, // Use MEVNOfferPrice for duplicate detection
                GetAppliedPrice = x => x.AppliedPrice,
                GetSpec1 = x => x.Spec1
            };

            // Field mapping for material validation - uses StandardPrice
            var materialValidationFieldMapping = new MaterialCombinationFieldMapping<PriceOfferCombinationDto>
            {
                GetMaterialType = x => x.MaterialType,
                GetCode = x => x.GolfaCode,
                GetModel = x => x.ModelName,
                GetStandardPrice = x => x.StandardPrice, // Use StandardPrice for material validation
                GetAppliedPrice = x => x.AppliedPrice,
                GetSpec1 = x => x.Spec1
            };

            // Check for duplicates within the file based on MEVNOfferPrice
            var duplicateGroups = _duplicateDetectionService.FindMaterialCombinationDuplicates(
                fileCombinations,
                duplicateDetectionFieldMapping);

            foreach (var duplicateGroup in duplicateGroups)
            {
                var errorMessage = $"Duplicate material combination: {duplicateGroup.Key} appears multiple times in the details.";

                foreach (var duplicate in duplicateGroup.Items)
                {
                    AddErrorIfMissing(duplicate.RowIndex, errorMessage);
                }
            }

            // Validate against materials database with enhanced validation using StandardPrice
            var validationResult = await _materialValidationService.ValidateAsync(
                fileCombinations, materialValidationFieldMapping);

            // Process validation errors
            foreach (var error in validationResult.Errors)
            {
                var rowIndex = error.Item.RowIndex;
                var errorMessage = error.ErrorType switch
                {
                    MaterialValidationErrorType.CombinationNotFound =>
                        $"Material combination not found: MaterialType='{error.ProvidedMaterialType}', GolfaCode='{error.GolfaCode}', ModelName='{error.ModelName}'",
                    MaterialValidationErrorType.StandardPriceMismatch =>
                        $"Standard price mismatch: Expected={error.ExpectedStandardPrice:N0}, Provided={error.ProvidedStandardPrice:N0} for MaterialType='{error.ProvidedMaterialType}', GolfaCode='{error.GolfaCode}', ModelName='{error.ModelName}'",
                    MaterialValidationErrorType.SellingPriceMismatch =>
                $"Selling price mismatch: Expected={error.ExpectedStandardPrice:N0}, Provided={error.ProvidedStandardPrice:N0} for MaterialType='{error.ProvidedMaterialType}', GolfaCode='{error.GolfaCode}', ModelName='{error.ModelName}'",
                    MaterialValidationErrorType.MaterialDeactivated =>
                        $"Material is deactivated: MaterialType='{error.ProvidedMaterialType}', GolfaCode='{error.GolfaCode}', ModelName='{error.ModelName}'",
                    MaterialValidationErrorType.WrongMaterialType =>
                    $"Wrong material type: Expected MaterialType='{error.ExpectedMaterialType}', but got MaterialType='{error.ProvidedMaterialType}' for GolfaCode='{error.GolfaCode}', ModelName='{error.ModelName}'",
                    MaterialValidationErrorType.Spec1Mismatch =>
                        $"Specification 1 mismatch: Expected='{error.ExpectedSpec1}', Provided='{error.ProvidedSpec1}' for MaterialType='{error.ProvidedMaterialType}', GolfaCode='{error.GolfaCode}', ModelName='{error.ModelName}'",
                    _ => error.ErrorMessage
                };

                AddErrorIfMissing(rowIndex, errorMessage);
            }
        }

        // Validate material group buyers (skip for NB type - no buyer)
        if (buyerId.HasValue && buyerId.Value != Guid.Empty)
        {
            foreach (var detail in detailsResult)
            {
                if (detail.RowData == null || string.IsNullOrWhiteSpace(detail.RowData.GolfaCode))
                {
                    continue;
                }
                var materialCode = detail.RowData.GolfaCode!;
                var material = materials
                    .FirstOrDefault(x => x.MaterialCode.Equals(materialCode, StringComparison.OrdinalIgnoreCase));

                var materialGroup = material?.MaterialGroup;

                if (material is null || string.IsNullOrEmpty(materialGroup))
                {
                    detail.Errors.Add($"Material '{materialCode}' does not belong to any material group.");
                    continue;
                }
                else if (material.MaterialStatus == MaterialStatuses.Deactivated)
                {
                    // This error is already captured in the material combination validation
                    continue;
                }

                var groupBuyer = groupBuyers.FirstOrDefault(x => string.Equals(x.MaterialGroupCode, materialGroup, StringComparison.OrdinalIgnoreCase));
                if (groupBuyer == null)
                {
                    detail.Errors.Add($"Material group '{materialGroup}' is not supported for the selected buyer.");
                }
            }
        }



        void AddErrorIfMissing(int rowIndex, string message)
        {
            if (!detailRowMap.TryGetValue(rowIndex, out var detailRow))
                return;

            if (detailRow.Errors.Contains(message))
                return;

            detailRow.Errors.Add(message);
        }
    }

    public async Task ValidateMaterialGroupKeyAccountAsync(
    List<ExcelRowResult<PriceOfferDetailImportDto>> detailsResult)
    {
        var materialCodes = detailsResult
            .Where(x => x.RowData != null && !string.IsNullOrWhiteSpace(x.RowData.GolfaCode))
            .Select(x => x.RowData.GolfaCode!)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (materialCodes.Count == 0)
            return;

        // GolfaCode → MaterialGroup (single join query)
        var materialData = await _materialRepository.GetQueryableAsync();
        var groupData = await _materialGroupRepository.GetQueryableAsync();

        var codeToGroupAllowed = materialData
            .Where(x => materialCodes.Contains(x.GolfaCode))
            .Select(x => new { x.GolfaCode, x.Material_Group })
            .Distinct()
            .Join(
                groupData,
                m => m.Material_Group.ToLower(),
                g => g.Code.ToLower(),
                (m, g) => new
                {
                    m.GolfaCode,
                    m.Material_Group,
                    g.AllowKeyAccount
                })
            .ToDictionary(
                x => x.GolfaCode,
                x => new { x.Material_Group, x.AllowKeyAccount },
                StringComparer.OrdinalIgnoreCase);

        foreach (var detail in detailsResult)
        {
            if (detail.RowData == null || string.IsNullOrWhiteSpace(detail.RowData.GolfaCode))
                continue;

            var materialCode = detail.RowData.GolfaCode!;

            if (!codeToGroupAllowed.TryGetValue(materialCode, out var info))
                continue;

            if (!info.AllowKeyAccount)
            {
                detail.Errors.Add(
                    $"Material group '{info.Material_Group}' does not allow Key Account for material '{materialCode}'.");
            }
        }
    }


    private class MaterialGroupSupportInfo
    {
        public string? MaterialGroup { get; set; } = string.Empty;
        public string MaterialCode { get; set; } = string.Empty;
        public string MaterialStatus { get; set; } = string.Empty;
    }
}
