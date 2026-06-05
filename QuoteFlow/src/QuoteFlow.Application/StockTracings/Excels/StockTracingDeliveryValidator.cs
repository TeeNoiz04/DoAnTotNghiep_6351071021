using QuoteFlow.Materials;
using QuoteFlow.Shared.Excels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.StockTracings.Excels;

public class StockTracingDeliveryValidator : BaseExcelValidator<StockTracingDeliveryImportDto>
{
    //private readonly IStockTracingDetailRepository _stockTracingDetailRepository;
    protected readonly IMaterialRepository _materialRepository;
    public StockTracingDeliveryValidator(ExcelValidationConfig config, IExcelRowValidator<StockTracingDeliveryImportDto> rowValidator, ILogger<BaseExcelValidator<StockTracingDeliveryImportDto>> logger, IServiceProvider provider) : base(config, rowValidator, logger)
    {
        //_stockTracingDetailRepository = serviceProvider.GetRequiredService<IStockTracingDetailRepository>();
        _materialRepository = provider.GetRequiredService<IMaterialRepository>();
    }

    protected override async Task PostValidateAsync(ExcelValidationResult<StockTracingDeliveryImportDto> result, ExcelImportContext? context = null)
    {
        var fromDate = context?.GetData<DateTime?>(ExcelImportContextKeys.StockTracing.FromDate)
           ?? throw new ArgumentNullException(nameof(context), "From Date is required");
        var toDate = context?.GetData<DateTime?>(ExcelImportContextKeys.StockTracing.ToDate)
           ?? throw new ArgumentNullException(nameof(context), "To Date is required");
        await base.PostValidateAsync(result);
        var materialCodes = result.ListData
            .Select(x => x.RowData.GolfaCode)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct()
            .ToList();

        var materialData = await _materialRepository.GetQueryableAsync();
        var materials = materialData
            .Where(x => materialCodes.Contains(x.GolfaCode))
            .Select(
                x => new MaterialSupportInfo(x.Id)
                {
                    ConcurrencyStamp = x.ConcurrencyStamp,
                    GolfaCode = x.GolfaCode,
                    Model = x.Model
                })
            .ToList();

        // Build dictionary for O(1) lookup instead of O(n) Exists
        // Key: GolfaCode (uppercase), Value: MaterialSupportInfo
        var materialLookup = materials
            .GroupBy(x => x.GolfaCode?.ToUpperInvariant() ?? "")
            .ToDictionary(g => g.Key, g => g.First());

        var listData = result.ListData;

        // ---- Check duplicated combination of Series + GolfaCode + DateEntered ----
        var duplicatedCombos = listData
            .Where(x =>
                !string.IsNullOrWhiteSpace(x.RowData.Series) &&
                !string.IsNullOrWhiteSpace(x.RowData.GolfaCode) &&
                x.RowData.DateEntered.HasValue)
            .GroupBy(x => new
            {
                Series = x.RowData.Series!.Trim(),
                GolfaCode = x.RowData.GolfaCode!.Trim(),
                DateEntered = x.RowData.DateEntered!.Value.Date
            })
            .Where(g => g.Count() > 1);

        foreach (var group in duplicatedCombos)
        {
            var key = group.Key;
            string displayDate = key.DateEntered.ToString("yyyy-MM-dd");

            var baseRow = group.First();
            var baseRowNo = baseRow.RowData.RowNo ?? listData.IndexOf(baseRow) + 1;

            foreach (var row in group.Skip(1))
            {
                var rowNo = row.RowData.RowNo ?? listData.IndexOf(row) + 1;
                row.Errors.Add(
                    $"Combination of Series='{key.Series}', GolfaCode='{key.GolfaCode}', DateEntered='{displayDate}' is duplicated with line {baseRowNo} (current line {rowNo})."
                );
                ExcelUtils.AddRowErrors(result, row.RowIndex, row.Errors);
            }
        }
        foreach (var row in result.ListData)
        {
            // O(1) dictionary lookup instead of O(n) Exists
            var materialCode = row.RowData.GolfaCode;
            var codeKey = materialCode?.ToUpperInvariant() ?? "";
            if (row.RowData.DateEntered == null)
            {
                row.Errors.Add($"Date Entered is required.");
                ExcelUtils.AddRowErrors(result, row.RowIndex, row.Errors);
            }
            else
            {
                var entered = row.RowData.DateEntered.Value.Date;
                var from = fromDate.Date;
                var to = toDate.Date;

                if (entered < from || entered > to)
                {
                    row.Errors.Add(
                        $"Date Entered '{row.RowData.DateEntered:dd/MM/yyyy}' " +
                        $"is outside the valid range ({from:dd/MM/yyyy} - {to:dd/MM/yyyy})."
                    );
                    ExcelUtils.AddRowErrors(result, row.RowIndex, row.Errors);
                }
            }



            if (!materialLookup.ContainsKey(codeKey))
            {
                row.Errors.Add($"Material Code '{materialCode}' was not found in the system.");
                ExcelUtils.AddRowErrors(result, row.RowIndex, row.Errors);
                continue;
            }
        }


    }
    private class MaterialSupportInfo : Entity<Guid>
    {
        public string ConcurrencyStamp { get; set; } = null!;
        public string GolfaCode { get; set; } = null!;
        public string Model { get; set; } = null!;

        public MaterialSupportInfo(Guid id)
        {
            Id = id;
        }
    }

}