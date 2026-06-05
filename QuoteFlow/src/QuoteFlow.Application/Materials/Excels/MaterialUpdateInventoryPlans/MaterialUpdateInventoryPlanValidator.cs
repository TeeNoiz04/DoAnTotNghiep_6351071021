using QuoteFlow.Shared.Excels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Materials.Excels.MaterialUpdateInventoryPlans;

public class MaterialUpdateInventoryPlanValidator : BaseExcelValidator<MaterialUpdateInventoryPlanImportDto>
{
    protected readonly IServiceProvider _provider;
    protected readonly IMaterialRepository _materialRepository;

    public MaterialUpdateInventoryPlanValidator(ExcelValidationConfig config, IExcelRowValidator<MaterialUpdateInventoryPlanImportDto> rowValidator, ILogger<BaseExcelValidator<MaterialUpdateInventoryPlanImportDto>> logger, IServiceProvider provider) : base(config, rowValidator, logger)
    {
        _provider = provider;
        _materialRepository = _provider.GetRequiredService<IMaterialRepository>();
    }

    protected override async Task PostValidateAsync(ExcelValidationResult<MaterialUpdateInventoryPlanImportDto> result)
    {
        var materials = await _materialRepository.GetListWithDeactiveAsync(
            new(),
            x => new MaterialSupportInfo(x.Id)
            {
                ConcurrencyStamp = x.ConcurrencyStamp,
                GolfaCode = x.GolfaCode,
                Model = x.Model,
                MaterialStatus = x.MaterialStatus,
                StockWarning = x.StockWarning
            });
        var materialLookup = materials
                .GroupBy(x => new { x.GolfaCode, x.Model }) // 1. Find duplicates
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderByDescending(m => m.MaterialStatus == MaterialStatuses.Active || m.MaterialStatus == MaterialStatuses.Discontinued) // 2. Prioritize 'Active'
                          .First() // 3. Pick the best one
                );

        var duplicateKeys = result.ListData
            .GroupBy(x => new { x.RowData.GolfaCode, x.RowData.Model })
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToHashSet();
        foreach (var row in result.ListData)
        {
            var code = row.RowData.GolfaCode;
            var model = row.RowData.Model;
            if (duplicateKeys.Contains(new { GolfaCode = code, Model = model }))
            {
                row.Errors.Add($"Duplicate Material Code = {code} & Model Name = {model} found in Excel.");
            }

            if (materialLookup.TryGetValue(new { GolfaCode = code, Model = model }, out var material))
            {
                row.RowData.Id = material.Id;
                row.RowData.ConcurrencyStamp = material.ConcurrencyStamp;
                row.RowData.CurrentStockWarning = material.StockWarning;
            }
            else
            {
                row.Errors.Add($"Cannot find Golfa Code = {code} & Model = {model}");
            }

            if (row.HasErrors)
            {
                ExcelUtils.AddRowErrors(result, row.RowIndex, row.Errors);
            }
        }

    }

    private class MaterialSupportInfo : Entity<Guid>, IHasConcurrencyStamp
    {
        public string ConcurrencyStamp { get; set; } = null!;
        public string GolfaCode { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string MaterialStatus { get; set; } = null!;
        public int? StockWarning { get; set; }

        public MaterialSupportInfo(Guid id)
        {
            Id = id;
        }
    }
}