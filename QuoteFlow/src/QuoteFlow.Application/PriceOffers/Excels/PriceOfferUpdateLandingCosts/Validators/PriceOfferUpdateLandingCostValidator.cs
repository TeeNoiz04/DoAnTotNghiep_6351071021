using QuoteFlow.Materials;
using QuoteFlow.Materials.ParameterObjects;
using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.PriceOffers.PriceOfferDetails.ParameterObjects;
using QuoteFlow.Shared.Excels;
using QuoteFlow.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteFlow.PriceOffers.Excels.PriceOfferUpdateLandingCosts.Validators;

public class PriceOfferUpdateLandingCostValidator : BaseExcelValidator<PriceOfferUpdateLandingCostImportDto>
{
    private readonly IMaterialRepository _materialRepository;
    private readonly IPriceOfferDetailRepository _priceOfferDetailRepository;

    public PriceOfferUpdateLandingCostValidator(
        ExcelValidationConfig config,
        IExcelRowValidator<PriceOfferUpdateLandingCostImportDto> rowValidator,
        ILogger<BaseExcelValidator<PriceOfferUpdateLandingCostImportDto>> logger,
        IServiceProvider serviceProvider)
        : base(config, rowValidator, logger)
    {
        _materialRepository = serviceProvider.GetRequiredService<IMaterialRepository>();
        _priceOfferDetailRepository = serviceProvider.GetRequiredService<IPriceOfferDetailRepository>();
    }

    protected override async Task<ValidationResult> PreValidateAsync(Stream stream, ExcelImportContext? context)
    {
        ValidateContext(context);
        return await base.PreValidateAsync(stream, context);
    }

    protected override async Task PostValidateAsync(ExcelValidationResult<PriceOfferUpdateLandingCostImportDto> result, ExcelImportContext context)
    {
        var processedCombinations = new HashSet<string>();

        foreach (var row in result.ListData)
        {
            await ValidateBusinessRulesAsync(row, processedCombinations, context);

            // Add row-level errors to the main result
            if (row.Errors.Any())
            {
                ExcelUtils.AddRowErrors(result, row.RowIndex, row.Errors);
            }
        }

        await base.PostValidateAsync(result, context);
    }

    private void ValidateContext(ExcelImportContext? context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context), "ExcelImportContext cannot be null.");
        }

        var parentId = context.GetData<Guid>(ExcelImportContextKeys.ParentEntityId);
        if (parentId == Guid.Empty)
        {
            throw new ArgumentException("ParentEntityId must be provided in the ExcelImportContext.", nameof(context));
        }
    }

    private async Task ValidateBusinessRulesAsync(
        ExcelRowResult<PriceOfferUpdateLandingCostImportDto> rowResult,
        HashSet<string> processedCombinations,
        ExcelImportContext context)
    {
        var data = rowResult.RowData!;

        // Check for duplicate combinations in the file
        var combinationKey = $"{data.GolfaCode}|{data.ModelName}";

        if (!processedCombinations.Add(combinationKey))
        {
            rowResult.Errors.Add($"Duplicate combination of GolfaCode '{data.GolfaCode}' and ModelName '{data.ModelName}' found in the file.");
        }

        // Validate that the material exists
        var materialFilterParams = new MaterialFilterParams
        {
            SkipCount = 0,
            MaxResultCount = int.MaxValue
        };

        var materials = await _materialRepository.GetListAsync(
            materialFilterParams,
            m => new { m.Id, m.GolfaCode, m.Model });

        var material = materials.FirstOrDefault(m =>
            m.GolfaCode == data.GolfaCode && m.Model == data.ModelName); if (material == null)
        {
            rowResult.Errors.Add($"Material with GolfaCode '{data.GolfaCode}' and ModelName '{data.ModelName}' does not exist.");
            return;
        }

        // Store the material ID for later use
        data.MaterialId = material.Id;// Find the price offer detail that matches the material

        var priceOfferId = context.GetData<Guid>(ExcelImportContextKeys.ParentEntityId);
        var priceOfferDetailFilterParams = new PriceOfferDetailFilterParams
        {
            GolfaCode = data.GolfaCode,
            ModelName = data.ModelName,
            PriceOfferId = priceOfferId,
            Status = QuoteFlowStatuses.InProgress
        };
        var inProgressDetails = await _priceOfferDetailRepository.GetListAsync(priceOfferDetailFilterParams);

        if (inProgressDetails.Count == 0)
        {
            rowResult.Errors.Add(
                $"No 'In Progress' detail found for GolfaCode '{data.GolfaCode}' " +
                $"and ModelName '{data.ModelName}' in this SPO.");
            return;
        }

        if (inProgressDetails.Count > 1)
        {
            rowResult.Errors.Add(
                $"Multiple 'In Progress' details found for GolfaCode '{data.GolfaCode}' " +
                $"and ModelName '{data.ModelName}'. Only one is expected – please review the SPO data.");
            return;
        }

        var priceOfferDetail = inProgressDetails.Single();

        // Validate that the sale offer price matches the current detail's MEVNOfferPrice
        if (data.SaleOfferPrice.HasValue && priceOfferDetail.MEVNOfferPrice != data.SaleOfferPrice.Value)
        {
            rowResult.Errors.Add($"Sale Offer Price {data.SaleOfferPrice:N0} does not match the current MEVN Offer Price {priceOfferDetail.MEVNOfferPrice:N0} for this detail.");
        }

        // Store the price offer detail ID for later use in conversion
        rowResult.RowData.PriceOfferDetailId = priceOfferDetail.Id;
        rowResult.RowData.PriceOfferId = priceOfferDetail.PriceOfferId;
    }
}
