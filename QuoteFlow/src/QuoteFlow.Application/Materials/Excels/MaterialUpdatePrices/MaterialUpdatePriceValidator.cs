using QuoteFlow.Materials.MaterialGroups;
using QuoteFlow.Shared.Excels;
using QuoteFlow.SystemCategories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Materials.Excels.MaterialUpdatePrices;

public class MaterialUpdatePriceValidator : BaseExcelValidator<MaterialUpdatePriceImportDto>
{
    protected readonly IServiceProvider _provider;
    protected readonly ISystemCategoryRepository _systemCategoryRepository;
    protected readonly IMaterialRepository _materialRepository;
    protected readonly IMaterialGroupRepository _materialGroupRepository;
    public MaterialUpdatePriceValidator(ExcelValidationConfig config, IExcelRowValidator<MaterialUpdatePriceImportDto> rowValidator, ILogger<BaseExcelValidator<MaterialUpdatePriceImportDto>> logger, IServiceProvider provider) : base(config, rowValidator, logger)
    {
        _provider = provider;
        _systemCategoryRepository = _provider.GetRequiredService<ISystemCategoryRepository>();
        _materialRepository = _provider.GetRequiredService<IMaterialRepository>();
        _materialGroupRepository = _provider.GetRequiredService<IMaterialGroupRepository>();
    }

    protected override async Task PostValidateAsync(ExcelValidationResult<MaterialUpdatePriceImportDto> result)
    {
        // Fetch data from database
        var materials = await _materialRepository.GetListWithDeactiveAsync(
            new(),
            x => new MaterialSupportInfo(x.Id)
            {
                ConcurrencyStamp = x.ConcurrencyStamp,
                GolfaCode = x.GolfaCode,
                Model = x.Model,
                MaterialType = x.MaterialType,
                MaterialGroup = x.Material_Group,
                Currency = x.InputCurrency,
                MaterialStatus = x.MaterialStatus
            });
        var systemCategories = await _systemCategoryRepository.GetListAsync(
            new() { CategoryType = CategoryTypes.Currency },
            x => new SystemCategorySupportInfo(x.Id)
            {
                Code = x.Code,
                Description = x.Description,
                Value = x.Value
            });
        var materialGroups = await _materialGroupRepository.GetListAsync(
            new(),
            x => new MaterialGroupsSupportInfo(x.Id)
            {
                Code = x.Code,
                MaterialType = x.MaterialType
            });

        // Build dictionaries for O(1) lookup instead of O(n) FirstOrDefault
        var materialDict = new Dictionary<string, MaterialSupportInfo>(StringComparer.OrdinalIgnoreCase);
        foreach (var mat in materials)
        {
            var key = $"{mat.GolfaCode}|{mat.Model}";
            if (!materialDict.ContainsKey(key))
            {
                materialDict[key] = mat;
            }
        }

        var categoryDict = systemCategories
            .ToDictionary(x => x.Code, x => x, StringComparer.OrdinalIgnoreCase);

        var materialGroupDict = materialGroups
            .ToDictionary(x => x.Code, x => x, StringComparer.OrdinalIgnoreCase);

        // Check for duplicates in Excel
        var duplicateKeys = result.ListData
            .GroupBy(x => new { x.RowData.MaterialCode, x.RowData.ModelName })
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToHashSet();

        foreach (var row in result.ListData)
        {
            var code = row.RowData.MaterialCode;
            var model = row.RowData.ModelName;
            var codeCurrency = row.RowData.InputCurrency;
            var codeMaterialGroup = row.RowData.MaterialGroup;
            var materialType = row.RowData.MaterialType;

            // Dictionary lookup instead of FirstOrDefault - O(1) performance
            var materialKey = $"{code}|{model}";
            if (!materialDict.TryGetValue(materialKey, out var material))
            {
                row.Errors.Add($"Cannot find Golfa Code = {code} & Model = {model}");
                ExcelUtils.AddRowErrors(result, row.RowIndex, row.Errors);
                continue;
            }

            // Normalize material code & model with database values
            row.RowData.MaterialCode = material.GolfaCode;
            row.RowData.ModelName = material.Model;
            row.RowData.Id = material.Id;
            row.RowData.ConcurrencyStamp = material.ConcurrencyStamp;

            // Check for duplicates
            if (duplicateKeys.Contains(new { MaterialCode = code, ModelName = model }))
            {
                row.Errors.Add($"Duplicate Material Code = {code} & Model Name = {model} found in Excel.");
            }

            // Category currency validation - Dictionary lookup
            SystemCategorySupportInfo? catergory = null;
            if (codeCurrency is not null)
            {
                if (!categoryDict.TryGetValue(codeCurrency, out catergory))
                {
                    row.Errors.Add($"Cannot find Code Currency = {codeCurrency}");
                }
            }

            // Material Group validation - Dictionary lookup
            MaterialGroupsSupportInfo? materialGroup = null;
            if (!string.IsNullOrWhiteSpace(codeMaterialGroup))
            {
                if (!materialGroupDict.TryGetValue(codeMaterialGroup, out materialGroup))
                {
                    row.Errors.Add($"Cannot find Code Material = {codeMaterialGroup}");
                }
            }

            // Material Type validation
            if (!string.IsNullOrWhiteSpace(materialType))
            {
                // Material type must match material type of supplier BU
                if (!materialType.Equals(material.MaterialType))
                {
                    row.Errors.Add($"Material Type of Supplier BU existed = {material.MaterialType} does not match Material Type = {materialType}");
                }
                // If updating material group, validate material type matches
                else if (!string.IsNullOrWhiteSpace(codeMaterialGroup) && materialGroup is not null)
                {
                    if (!materialGroup.MaterialType.Equals(materialType))
                    {
                        row.Errors.Add($"Material Type of Material Group = {materialGroup.MaterialType} does not match Material Type = {materialType}");
                    }
                }
            }
            else
            {
                // If updating material group without material type, validate group's material type matches existing
                if (!string.IsNullOrWhiteSpace(codeMaterialGroup) && materialGroup is not null)
                {
                    if (!materialGroup.MaterialType.Equals(material.MaterialType))
                    {
                        row.Errors.Add($"Material Type of Material Group = {materialGroup.MaterialType} does not match Material Type existed= {material.MaterialType}");
                    }
                }
            }

            // Currency validation
            if (!string.IsNullOrWhiteSpace(codeCurrency))
            {
                if (!codeCurrency.Equals(material.Currency))
                {
                    row.Errors.Add($"Currency of Supplier BU existed = {material.Currency} does not match Currency = {codeCurrency}");
                }
            }

            // Propagate row errors to result
            if (row.HasErrors)
            {
                ExcelUtils.AddRowErrors(result, row.RowIndex, row.Errors);
            }
        }
    }

    private class SystemCategorySupportInfo : Entity<Guid>
    {
        public string Code { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal? Value { get; set; } = null;

        public SystemCategorySupportInfo(Guid id)
        {
            Id = id;
        }
    }
    private class MaterialSupportInfo : Entity<Guid>
    {
        public string ConcurrencyStamp { get; set; } = null!;
        public string GolfaCode { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string? MaterialType { get; set; }
        public string? MaterialGroup { get; set; }
        public string? Currency { get; set; }
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
