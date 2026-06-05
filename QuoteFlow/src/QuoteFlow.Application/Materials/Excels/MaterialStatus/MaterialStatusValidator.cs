using QuoteFlow.Materials.MaterialImport.MaterialStatus;
using QuoteFlow.Shared.Excels;
using QuoteFlow.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Materials.Excels.MaterialStatus;

public class MaterialStatusValidator : BaseExcelValidator<MaterialStatusUpdateExcelDto>
{
    protected readonly IServiceProvider _provider;
    protected readonly IMaterialRepository _materialRepository;
    public MaterialStatusValidator(ExcelValidationConfig config, IExcelRowValidator<MaterialStatusUpdateExcelDto> rowValidator, ILogger<BaseExcelValidator<MaterialStatusUpdateExcelDto>> logger, IServiceProvider provider) : base(config, rowValidator, logger)
    {
        _provider = provider;

        _materialRepository = _provider.GetRequiredService<IMaterialRepository>();
    }
    protected override async Task PostValidateAsync(ExcelValidationResult<MaterialStatusUpdateExcelDto> result)
    {
        var materials = await _materialRepository.GetListWithDeactiveAsync(
            new(),
            x => new MaterialSupportInfo(x.Id)
            {
                ConcurrencyStamp = x.ConcurrencyStamp,
                GolfaCode = x.GolfaCode,
                Model = x.Model,
                SupplierBUCode = x.SupplierBUCode,
                SupplierCode = x.SupplierCode,
                InputCurrency = x.InputCurrency,
                MaterialStatus = x.MaterialStatus
            });

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

            var lookupKey = $"{code?.ToUpperInvariant()}|{model?.ToUpperInvariant()}";
            materialLookup.TryGetValue(lookupKey, out var material);
            //var material = materials.FirstOrDefault(x =>
            //    x.GolfaCode.Equals(code, StringComparison.OrdinalIgnoreCase)
            //    && x.Model.Equals(model, StringComparison.OrdinalIgnoreCase)
            //);

            if (duplicateKeys.Contains(new { GolfaCode = code, Model = model }))
            {
                row.Errors.Add($"Duplicate Material Code = {code} & Model Name = {model} found in Excel.");
            }

            if (material != null)
            {
                if (row.RowData.Action.Equals(HistoryActions.Material.Active, StringComparison.OrdinalIgnoreCase))
                {
                    if (material.InputCurrency is null)
                        row.Errors.Add($"This material could not be activated because it is missing the mandatory Input Currency field.");

                    if (material.SupplierCode is null)
                        row.Errors.Add("This material could not be activated because it is missing the mandatory Supplier field.");

                    if (material.SupplierBUCode is null)
                        row.Errors.Add("This material could not be activated because it is missing the mandatory Supplier BU field.");
                }


                row.RowData.Id = material.Id;
                row.RowData.ConcurrencyStamp = material.ConcurrencyStamp;
            }
            else
            {
                row.Errors.Add($"Cannot find Material Code = {code} & Model = {model}");
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
        public string? SupplierBUCode { get; set; }
        public string? SupplierCode { get; set; }
        public string? InputCurrency { get; set; }
        public string MaterialStatus { get; set; } = null!;

        public MaterialSupportInfo(Guid id)
        {
            Id = id;
        }
    }
}

