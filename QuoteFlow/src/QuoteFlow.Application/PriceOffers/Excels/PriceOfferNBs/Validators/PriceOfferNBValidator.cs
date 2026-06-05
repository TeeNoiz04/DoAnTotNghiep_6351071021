using QuoteFlow.Materials;
using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.Shared.Excels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteFlow.PriceOffers.Excels.PriceOfferNBs.Validators;

public class PriceOfferNBValidator : IExcelValidator<PriceOfferImportDto>
{
    private readonly IExcelValidator<PriceOfferDetailImportDto> _offerDetailValidator;
    private readonly ILogger<PriceOfferNBValidator> _logger;
    private readonly PriceOfferMaterialValidationService _materialValidationService;
    private readonly IPriceOfferRepository _priceOfferRepository;
    private readonly IMaterialRepository _materialRepository;

    public PriceOfferNBValidator(
        IExcelValidator<PriceOfferDetailImportDto> offerDetailValidator,
        IServiceProvider serviceProvider)
    {
        _offerDetailValidator = offerDetailValidator;
        _logger = serviceProvider.GetRequiredService<ILogger<PriceOfferNBValidator>>();
        _materialValidationService = serviceProvider.GetRequiredService<PriceOfferMaterialValidationService>();
        _priceOfferRepository = serviceProvider.GetRequiredService<IPriceOfferRepository>();
        _materialRepository = serviceProvider.GetRequiredService<IMaterialRepository>();
    }

    public async Task<ExcelValidationResult<PriceOfferImportDto>> ValidateAsync(System.IO.Stream stream, string fileName, ExcelImportContext? context = null)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context), "ExcelImportContext cannot be null");
        }

        var materialType = context.GetData<string>(ExcelImportContextKeys.PriceOffer.MaterialType)
            ?? throw new ArgumentNullException(nameof(context), "Material type must be provided in the context");

        var priceOfferResult = new ExcelRowResult<PriceOfferImportDto>();

        stream.Seek(0, System.IO.SeekOrigin.Begin);
        var offerDetailsResult = await _offerDetailValidator.ValidateAsync(stream, fileName);

        // Fill standard price if not provided (same as AP)
        await FillStandardPriceIfNotProvidedAsync(offerDetailsResult.ListData);

        if (!offerDetailsResult.IsValid)
        {
            ExcelUtils.AddChildListErrors(priceOfferResult, offerDetailsResult, "[Price Offer Details]");
        }

        priceOfferResult.RowData = new();
        if (offerDetailsResult.ListData.Count > 0)
        {
            priceOfferResult.RowData.FileName = fileName;
            priceOfferResult.RowData.Details = offerDetailsResult;
            priceOfferResult.RowData.TotalMEVNOfferAmount = offerDetailsResult.ListData.Sum(x => (x.RowData.Qty ?? 0) * (x.RowData.MEVNOfferPrice ?? 0));
            priceOfferResult.RowData.TotalPriceToCustomer = offerDetailsResult.ListData.Sum(x => x.RowData.PriceToCustomer ?? 0);
            priceOfferResult.RowData.TotalRequestedAmount = offerDetailsResult.ListData.Sum(x => x.RowData.RequestedAmount ?? 0);
            priceOfferResult.RowData.TotalStandardAmount = offerDetailsResult.ListData.Sum(x => x.RowData.StandardAmount ?? 0);

            var rowData = priceOfferResult.RowData;
            priceOfferResult.RowData.DiscountRatio = (1 - rowData.TotalMEVNOfferAmount / rowData.TotalStandardAmount);

            var totalMEVNOfferAmount = rowData.TotalMEVNOfferAmount ?? 0;

            if (!string.IsNullOrEmpty(materialType) && totalMEVNOfferAmount > 0)
            {
                // Note: NB type doesn't have key account class, so we pass empty string
                priceOfferResult.RowData.DiscountRatioConfigured = await _priceOfferRepository.GetDiscountRatioConfigured(
                    PriceOfferTypes.PriceOfferNB,
                    materialType,
                    totalMEVNOfferAmount
                );
            }

            _logger.LogInformation("Validating material combinations without buyer (NB type)...");

            // NB type: no buyer validation - pass null for buyer ID and applied price
            await _materialValidationService.ValidateMaterialCombinationsAndGroupBuyersAsync(
                priceOfferResult.RowData.Details.ListData,
                materialType,
                null,  // No buyer for NB type
                null   // No applied price for NB type
            );

            if (priceOfferResult.RowData.Details != null &&
                priceOfferResult.RowData.Details.ListData.Any(x => x.Errors.Count != 0))
            {
                _logger.LogWarning("Detected errors in child detail rows. Adding error messages to result.");
                ExcelUtils.AddChildListErrors(priceOfferResult, priceOfferResult.RowData.Details, "[Price Offer Details]");
            }
        }
        else
        {
            _logger.LogWarning("No valid offer details found in the file: {FileName}", fileName);
            priceOfferResult.Errors.Add("No valid offer details found in the file.");
        }

        var overallResult = new ExcelValidationResult<PriceOfferImportDto>(singleRow: true, fileName);
        overallResult.ListData.Add(priceOfferResult);
        ExcelUtils.AddRowErrors(overallResult, 1, priceOfferResult.Errors);

        return overallResult;
    }

    private async Task FillStandardPriceIfNotProvidedAsync(System.Collections.Generic.List<ExcelRowResult<PriceOfferDetailImportDto>> details)
    {
        if (details == null || details.Count == 0)
        {
            return;
        }
        var materialCodes = details
            .Where(d => !string.IsNullOrEmpty(d.RowData.GolfaCode))
            .Select(d => d.RowData.GolfaCode!)
            .Distinct()
            .ToList();
        var materials = await _materialRepository.GetListAsync(m => materialCodes.Contains(m.GolfaCode));

        foreach (var detail in details)
        {
            if (!detail.RowData.StandardPrice.HasValue || detail.RowData.StandardPrice <= 0)
            {
                decimal? standardPrice = materials
                    .Where(m => string.Equals(m.GolfaCode, detail.RowData.GolfaCode, StringComparison.OrdinalIgnoreCase))
                    .Select(m => m.Standard_Price)
                    .FirstOrDefault();

                if (standardPrice.HasValue)
                {
                    detail.RowData.StandardPrice = standardPrice;
                    detail.RowData.StandardAmount = standardPrice * (detail.RowData.Qty ?? 0);
                }
                else
                {
                    detail.RowData.StandardPrice = 0;
                    detail.RowData.StandardAmount = 0;
                }
            }
            else
            {
                detail.RowData.StandardAmount = detail.RowData.StandardPrice * (detail.RowData.Qty ?? 0);
            }
        }
    }
}
