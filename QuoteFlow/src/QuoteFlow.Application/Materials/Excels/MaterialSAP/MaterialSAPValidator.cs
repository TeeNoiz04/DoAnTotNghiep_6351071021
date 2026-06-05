using QuoteFlow.Materials.MaterialImport.MaterialSAP;
using QuoteFlow.Shared.Excels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.Materials.Excels.MaterialSAP;

public class MaterialSAPValidator : BaseExcelValidator<MaterialSAPUpdateExcelDto>
{
    protected readonly IServiceProvider _provider;
    protected readonly IMaterialRepository _materialRepository;
    public MaterialSAPValidator(ExcelValidationConfig config, IExcelRowValidator<MaterialSAPUpdateExcelDto> rowValidator, ILogger<BaseExcelValidator<MaterialSAPUpdateExcelDto>> logger, IServiceProvider provider) : base(config, rowValidator, logger)
    {
        _provider = provider;

        _materialRepository = _provider.GetRequiredService<IMaterialRepository>();
    }
    protected override async Task PostValidateAsync(ExcelValidationResult<MaterialSAPUpdateExcelDto> result)
    {
        if (result.ListData.Any(x => x.HasErrors))
        {
            return;
        }
        var materials = await _materialRepository.GetListWithDeactiveAsync(
            new(),
            x => new MaterialSupportInfo(x.Id)
            {
                ConcurrencyStamp = x.ConcurrencyStamp,
                GolfaCode = x.GolfaCode,
                Model = x.Model,
                MaterialStatus = x.MaterialStatus
            });
        var duplicateKeys = result.ListData
            .GroupBy(x => new { x.RowData.GolfaCode, x.RowData.Model })
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToHashSet();
        foreach (var row in result.ListData)
        {
            var code = row.RowData.GolfaCode;
            var model = row.RowData.Model;
            var material = materials.FirstOrDefault(x => x.GolfaCode.Equals(code, StringComparison.OrdinalIgnoreCase) && x.Model.Equals(model, StringComparison.OrdinalIgnoreCase));
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

        public MaterialSupportInfo(Guid id)
        {
            Id = id;
        }
    }
}
