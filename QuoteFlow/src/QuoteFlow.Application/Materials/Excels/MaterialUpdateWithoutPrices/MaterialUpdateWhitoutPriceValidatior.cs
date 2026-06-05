using QuoteFlow.Materials.MaterialGroups;
using QuoteFlow.Shared.Excels;
using QuoteFlow.SupplierBUs;
using QuoteFlow.SystemCategories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Materials.Excels.MaterialUpdateWithoutPrices;

public class MaterialUpdateWhitoutPriceValidatior : BaseExcelValidator<MaterialUpdateWithoutPriceImportDto>
{
    protected readonly IServiceProvider _provider;
    protected readonly ISystemCategoryRepository _systemCategoryRepository;
    protected readonly IMaterialRepository _materialRepository;
    protected readonly IMaterialGroupRepository _materialGroupRepository;
    protected readonly ISupplierBURepository _supplierBURepository;
    public MaterialUpdateWhitoutPriceValidatior(ExcelValidationConfig config, IExcelRowValidator<MaterialUpdateWithoutPriceImportDto> rowValidator, ILogger<BaseExcelValidator<MaterialUpdateWithoutPriceImportDto>> logger, IServiceProvider provider) : base(config, rowValidator, logger)
    {
        _provider = provider;
        _systemCategoryRepository = _provider.GetRequiredService<ISystemCategoryRepository>();
        _materialRepository = _provider.GetRequiredService<IMaterialRepository>();
        _materialGroupRepository = _provider.GetRequiredService<IMaterialGroupRepository>();
        _supplierBURepository = _provider.GetRequiredService<ISupplierBURepository>();
    }
    protected override async Task PostValidateAsync(ExcelValidationResult<MaterialUpdateWithoutPriceImportDto> result)
    {
        // 1. Fetch Data
        var materials = await _materialRepository.GetListWithDeactiveAsync(
            new(),
            x => new MaterialSupportInfo(x.Id)
            {
                ConcurrencyStamp = x.ConcurrencyStamp,
                GolfaCode = x.GolfaCode,
                MaterialType = x.MaterialType,
                Supplier = x.SupplierCode,
                SupplierBU = x.SupplierBUCode,
                MaterialGroup = x.Material_Group,
                MaterialStatus = x.MaterialStatus
            });

        var materialGroups = await _materialGroupRepository.GetListAsync(
            new(),
            x => new MaterialGroupsSupportInfo(x.Id)
            {
                Code = x.Code,
                MaterialType = x.MaterialType
            });

        var supplierBUs = await _supplierBURepository.GetListAsync();

        // A. Material Lookup (Includes logic for Active/InProgress prioritization if needed)
        var materialLookup = materials
            .GroupBy(x => $"{x.GolfaCode?.ToUpperInvariant()}")
            .ToDictionary(g => g.Key, g => g.First());

        // B. Material Group Lookup
        var materialGroupLookup = materialGroups
            .Where(x => !string.IsNullOrEmpty(x.Code))
            .GroupBy(x => x.Code.ToUpperInvariant())
            .ToDictionary(g => g.Key, g => g.First());

        // C. Supplier BU Lookup (Composite Key: Supplier|BU)
        var supplierBULookup = supplierBUs
            .Where(x => !string.IsNullOrEmpty(x.SupplierCode) && !string.IsNullOrEmpty(x.SupplierBUCode))
            .GroupBy(x => $"{x.SupplierCode.ToUpperInvariant()}|{x.SupplierBUCode.ToUpperInvariant()}")
            .ToDictionary(g => g.Key, g => g.First());


        var duplicateKeys = result.ListData
           .GroupBy(x => new { x.RowData.MaterialCode })
           .Where(g => g.Count() > 1)
           .Select(g => g.Key)
           .ToHashSet();

        foreach (var row in result.ListData)
        {
            var code = row.RowData.MaterialCode;
            var model = row.RowData.ModelName;
            var codeMaterialGroup = row.RowData.MaterialGroup;
            var supplier = row.RowData.Supplier;
            var supplierBU = row.RowData.SupplierBU;
            var materialType = row.RowData.MaterialType;

            var lookupKey = $"{code?.ToUpperInvariant()}";

            // FAST LOOKUP: Material
            materialLookup.TryGetValue(lookupKey, out var material);

            if (material is null)
            {
                row.Errors.Add($"Cannot find Material with Material Code = {code}.");
                ExcelUtils.AddRowErrors(result, row.RowIndex, row.Errors);
                continue;
            }

            var materialTypeCheck = materialType ?? material.MaterialType;
            if (codeMaterialGroup == null && material.MaterialGroup == null)
            {
                row.Errors.Add($"Material Group is required when Material Code = {code} has no Material Group.");
                ExcelUtils.AddRowErrors(result, row.RowIndex, row.Errors);
                continue;
            }
            else
            {
                materialTypeCheck = materialType ?? material.MaterialType;
            }

            var materialGroupCheck = codeMaterialGroup ?? material.MaterialGroup;
            var supplierCheck = supplier ?? material.Supplier;
            var supplierBUCheck = supplierBU ?? material.SupplierBU;

            if (duplicateKeys.Contains(new { MaterialCode = code }))
            {
                row.Errors.Add($"Duplicate Material Code = {code} found in Excel.");
            }

            // Logic Block
            if (material is null)
            {
                row.Errors.Add($"Material Code = {code} does not exist.");
            }
            else
            {
                // FAST LOOKUP: Material Group
                MaterialGroupsSupportInfo MTFromMaterialGroup = null;
                if (!string.IsNullOrEmpty(materialGroupCheck))
                {
                    materialGroupLookup.TryGetValue(materialGroupCheck.ToUpperInvariant(), out MTFromMaterialGroup);
                }

                var supplierKey = $"{supplierCheck?.ToUpperInvariant()}|{supplierBUCheck?.ToUpperInvariant()}";
                supplierBULookup.TryGetValue(supplierKey, out var MTFromSupplierBU);


                // Validation Logic
                if (MTFromMaterialGroup is null)
                {
                    row.Errors.Add($"Material Group {materialGroupCheck} not found.");
                }
                if (MTFromSupplierBU is null)
                {
                    row.Errors.Add($"Supplier = {supplierCheck} and Supplier BU = {supplierBU} not found.");
                }

                if ((MTFromMaterialGroup is not null) && (MTFromSupplierBU is not null))
                {
                    if (!string.Equals(materialTypeCheck, MTFromMaterialGroup.MaterialType, StringComparison.OrdinalIgnoreCase))
                    {
                        row.Errors.Add($"Material Type = {materialTypeCheck} does not match the Material Type of Material Group = {MTFromMaterialGroup.MaterialType}.");
                    }
                    if (!string.Equals(materialTypeCheck, MTFromSupplierBU.MaterialType, StringComparison.OrdinalIgnoreCase))
                    {
                        row.Errors.Add($"Material Type = {materialTypeCheck} does not match the Material Type of Supplier BU = {MTFromSupplierBU.MaterialType}.");
                    }
                }
            }

            if (row.HasErrors)
            {
                ExcelUtils.AddRowErrors(result, row.RowIndex, row.Errors);
            }
            else
            {
                row.RowData.Id = material.Id;
                row.RowData.ConcurrencyStamp = material.ConcurrencyStamp;
            }
        }
    }

    private class MaterialSupportInfo : Entity<Guid>
    {
        public string ConcurrencyStamp { get; set; } = null!;
        public string GolfaCode { get; set; } = null!;
        //public string Model { get; set; } = null!;
        public string? MaterialType { get; set; }
        public string? Supplier { get; set; }
        public string? SupplierBU { get; set; }
        public string? MaterialGroup { get; set; }
        public string? MaterialStatus { get; set; }
        public MaterialSupportInfo(Guid id)
        {
            Id = id;
        }
    }
    private class MaterialGroupsSupportInfo : Entity<Guid>
    {
        public string Code { get; set; } = null!;
        public string? MaterialType { get; set; }

        public MaterialGroupsSupportInfo(Guid id)
        {
            Id = id;
        }
    }
}
