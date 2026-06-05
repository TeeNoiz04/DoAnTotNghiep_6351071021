using QuoteFlow.Materials.MaterialImport.MaterialFactory;
using QuoteFlow.Shared.Excels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Materials.Excels.MaterialFactory;

public class MaterialFactoryValidator : BaseExcelValidator<MaterialFactoryUpdateExcelDto>
{
    protected readonly IServiceProvider _provider;
    protected readonly IMaterialRepository _materialRepository;
    public MaterialFactoryValidator(ExcelValidationConfig config, IExcelRowValidator<MaterialFactoryUpdateExcelDto> rowValidator, ILogger<BaseExcelValidator<MaterialFactoryUpdateExcelDto>> logger, IServiceProvider provider) : base(config, rowValidator, logger)
    {
        _provider = provider;

        _materialRepository = _provider.GetRequiredService<IMaterialRepository>();
    }
    protected override async Task PostValidateAsync(ExcelValidationResult<MaterialFactoryUpdateExcelDto> result)
    {
        var materials = await _materialRepository.GetListWithDeactiveAsync(
            new(),
            x => new MaterialSupportInfo(x.Id)
            {
                ConcurrencyStamp = x.ConcurrencyStamp,
                GolfaCode = x.GolfaCode,
                Model = x.Model,
                MaterialStatus = x.MaterialStatus
            });

        // Build dictionary for O(1) lookup instead of O(n) FirstOrDefault
        // Key: "GOLFACODE|MODEL" (uppercase), Value: MaterialSupportInfo
        var materialLookup = materials
            .GroupBy(x => $"{x.GolfaCode?.ToUpperInvariant()}|{x.Model?.ToUpperInvariant()}")
            .ToDictionary(g => g.Key, g => g.First());

        var duplicateKeys = result.ListData
            .GroupBy(x => new { x.RowData.GolfaCode, x.RowData.Model })
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToHashSet();

        foreach (var row in result.ListData)
        {
            var code = row.RowData.GolfaCode;
            var model = row.RowData.Model;

            // O(1) dictionary lookup instead of O(n) FirstOrDefault
            var lookupKey = $"{code?.ToUpperInvariant()}|{model?.ToUpperInvariant()}";
            materialLookup.TryGetValue(lookupKey, out var material);

            if (duplicateKeys.Contains(new { GolfaCode = code, Model = model }))
            {
                row.Errors.Add($"Duplicate Material Code = {code} & Model Name = {model} found in Excel.");
            }

            if (material != null)
            {
                row.RowData.Id = material.Id;
                row.RowData.ConcurrencyStamp = material.ConcurrencyStamp;
            }
            else
            {
                row.Warnings.Add($"Cannot find Golfa Code = {code} & Model = {model}");
            }
            if (row.HasErrors)
            {
                ExcelUtils.AddRowErrors(result, row.RowIndex, row.Errors);
            }
            if (row.HasWarnings)
            {
                ExcelUtils.AddRowWarnings(result, row.RowIndex, row.Warnings);
            }
        }
    }
    private class MaterialSupportInfo : Entity<Guid>, IHasConcurrencyStamp
    {
        public string ConcurrencyStamp { get; set; } = null!;
        public string GolfaCode { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string MaterialStatus { get; set; } = null!;

        public MaterialSupportInfo(Guid id)
        {
            Id = id;
        }
    }
}
