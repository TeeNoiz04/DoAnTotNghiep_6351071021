using QuoteFlow.Buyers;
using QuoteFlow.Materials;
using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.Shared.Excels;
using QuoteFlow.Shared.Models;
using QuoteFlow.SystemConfigurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.PriceOffers.Excels.PriceOfferAddMoreItems.Validators;

public class PriceOfferAddMoreItemValidator : BaseExcelValidator<PriceOfferDetailImportDto>
{
    private readonly PriceOfferMaterialValidationService _materialValidationService;
    private readonly IPriceOfferDetailRepository _priceOfferDetailRepository;
    private readonly ISystemConfigurationRepository _systemConfigurationRepository;
    private readonly IPriceOfferRepository _priceOfferRepository;
    private readonly IBuyerRepository _buyerRepository;
    private readonly IMaterialRepository _materialRepository;

    public PriceOfferAddMoreItemValidator(
        ExcelValidationConfig config,
        IExcelRowValidator<PriceOfferDetailImportDto> rowValidator,
        ILogger<BaseExcelValidator<PriceOfferDetailImportDto>> logger,
        IServiceProvider serviceProvider)
    : base(config, rowValidator, logger)
    {
        _materialValidationService = serviceProvider.GetRequiredService<PriceOfferMaterialValidationService>();
        _priceOfferDetailRepository = serviceProvider.GetRequiredService<IPriceOfferDetailRepository>();
        _systemConfigurationRepository = serviceProvider.GetRequiredService<ISystemConfigurationRepository>();
        _priceOfferRepository = serviceProvider.GetRequiredService<IPriceOfferRepository>();
        _buyerRepository = serviceProvider.GetRequiredService<IBuyerRepository>();
        _materialRepository = serviceProvider.GetRequiredService<IMaterialRepository>();
    }

    public override async Task<ExcelValidationResult<PriceOfferDetailImportDto>> ValidateAsync(Stream stream, string fileName, ExcelImportContext? context = null)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context), "ExcelImportContext cannot be null");
        }

        var materialType = context?.GetData<string>(ExcelImportContextKeys.PriceOffer.MaterialType)
            ?? throw new ArgumentNullException(nameof(context), "Material type must be provided in the context");
        var buyerId = context.GetData<Guid>(ExcelImportContextKeys.DPO.BuyerId);

        var validationResult = await base.ValidateAsync(stream, fileName, context);

        var priceOfferId = context.GetData<Guid>(ExcelImportContextKeys.ParentEntityId);
        var priceOfferDetails = await _priceOfferDetailRepository.GetListAsync(new() { PriceOfferId = priceOfferId });

        var priceOffer = await _priceOfferRepository.GetAsync(priceOfferId);
        var getPriceAuto = context.GetData<bool?>(ExcelImportContextKeys.PriceOffer.GetPriceAuto) ?? false;
        if (priceOffer.IsKeyAccountPriceOffer())
        {
            if (!priceOffer.KeyAccountId.HasValue)
            {
                throw new UserFriendlyException(
                    "This price offer is of type 'AP' but has no key account assigned. " +
                    "Please verify and contact the admin to update the record."
                );
            }

            await FillStandardPriceIfNotProvidedAsync(validationResult.ListData);
            if (getPriceAuto)
            {
                await AutoFillOfferPriceAsync(validationResult.ListData, priceOffer.KeyAccountId.Value);
            }
            else
            {
                ValidateRequiredFields(validationResult.ListData, true);
            }
        }
        else
        {
            ValidateRequiredFields(validationResult.ListData, false);
        }

        // Validate material combinations after all data is loaded
        var buyer = await _buyerRepository.FirstOrDefaultAsync(x => x.Id == buyerId);
        var appliedPrice = buyer is null ? null : buyer.AppliedPrice;
        await _materialValidationService.ValidateMaterialCombinationsAndGroupBuyersAsync(validationResult.ListData, materialType, buyerId, appliedPrice);

        ValidateDuplicateWithClosedItems(validationResult.ListData, priceOfferDetails);

        // Normalize GolfaCode to empty string if null for dictionary key consistency
        var detailsLookup = priceOfferDetails
            .Where(d => d.Status == QuoteFlowStatuses.Approved)
            .Distinct()
            .ToDictionary(
            d => ((d.GolfaCode ?? "").Trim(), d.MEVNOfferPrice)
        );

        foreach (var detail in validationResult.ListData)
        {
            var row = detail.RowData;
            if (row == null) continue;

            var key = ((row.GolfaCode ?? "").Trim(), row.MEVNOfferPrice ?? 0M);

            if (detailsLookup.TryGetValue(key, out var existingDetail))
            {
                var dpoUsed = existingDetail.DpoUsed ?? 0;

                var qtyAvailableForReduction = existingDetail.Qty - dpoUsed;
                var requestQty = row.Qty ?? 0;

                if (requestQty < 0 && qtyAvailableForReduction + requestQty < 0)
                {
                    detail.Errors.Add(
                        $"Cannot reduce quantity for {row.GolfaCode} at {row.MEVNOfferPrice:N0}. " +
                        $"The current quantity is {existingDetail.Qty}, {dpoUsed} items are already used by dpo. " +
                        $"You can only reduce by up to {qtyAvailableForReduction}."
                    );
                }
            }
            else
            {
                // if not found in details but also have negative quantity, add error
                if (row.Qty < 0)
                {
                    detail.Errors.Add(
                        $"Cannot reduce quantity for GolfaCode {row.GolfaCode} at price {row.MEVNOfferPrice:N0} since it does not exist in the current price offer details."
                    );
                }
            }
        }

        foreach (var row in validationResult.ListData)
        {
            ExcelUtils.AddRowErrors(validationResult, row.RowIndex, row.Errors, "Price Offer Details");
        }
        return validationResult;
    }

    protected override async Task PostValidateAsync(ExcelValidationResult<PriceOfferDetailImportDto> result, ExcelImportContext context)
    {
        await base.PostValidateAsync(result);

        if (result.ListData.Count == 0)
        {
            result.Errors.Add("No valid rows found in the Excel file.");
            return;
        }
    }

    private async Task FillStandardPriceIfNotProvidedAsync(List<ExcelRowResult<PriceOfferDetailImportDto>> details)
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

    private async Task AutoFillOfferPriceAsync(List<ExcelRowResult<PriceOfferDetailImportDto>> details, Guid keyAccountId)
    {
        var discount = await _priceOfferRepository.GetDiscountRatioAsync(keyAccountId);
        foreach (var detail in details)
        {
            detail.RowData.MEVNOfferPrice = Math.Ceiling(detail.RowData.StandardPrice!.Value * (1 - discount));
            detail.RowData.MEVNOfferAmount = detail.RowData.MEVNOfferPrice * detail.RowData.Qty;
        }

    }

    private void ValidateRequiredFields(List<ExcelRowResult<PriceOfferDetailImportDto>> details, bool isPriceOfferAP)
    {
        foreach (var detail in details)
        {
            var rowErrors = new List<string>();
            var standardPrice = detail.RowData.StandardPrice;
            var buyerPrice = detail.RowData.BuyerPrice;
            var priceToCustomer = detail.RowData.PriceToCustomer;
            var MEVNOfferPrice = detail.RowData.MEVNOfferPrice;
            var qty = detail.RowData.Qty;

            if (!isPriceOfferAP)
            {
                if (standardPrice is null || standardPrice < 0) rowErrors.Add("Standard Price must be greater than 0.");
                else if (standardPrice.HasValue && !decimal.IsInteger(standardPrice.Value)) rowErrors.Add("Standard Price must be a whole number.");
                if (priceToCustomer is not null && priceToCustomer < 0) rowErrors.Add("Price to Customer must be greater than 0.");
                else if (priceToCustomer.HasValue && !decimal.IsInteger(priceToCustomer.Value)) rowErrors.Add("Price to Customer must be a whole number.");
            }

            if (buyerPrice is null || buyerPrice < 0) rowErrors.Add("Buyer Price must be greater than 0.");
            else if (buyerPrice.HasValue && !decimal.IsInteger(buyerPrice.Value)) rowErrors.Add("Buyer Price must be a whole number.");
            if (MEVNOfferPrice is null || MEVNOfferPrice <= 0) rowErrors.Add("MEVN Offer Price is required and must be greater than 0.");
            else if (MEVNOfferPrice.HasValue && !decimal.IsInteger(MEVNOfferPrice.Value)) rowErrors.Add("MEVN Offer Price must be a whole number.");

            detail.Errors.AddRange(rowErrors);
        }
    }
    private void ValidateDuplicateWithClosedItems(
        List<ExcelRowResult<PriceOfferDetailImportDto>> importDetails,
        List<PriceOfferDetail> existingDetails)
    {
        var closedItems = existingDetails
            .Where(d => d.Status == QuoteFlowStatuses.Closed)
            .Select(d => new
            {
                GolfaCode = (d.GolfaCode ?? "").Trim(),
                ModelName = (d.ModelName ?? "").Trim(),
                MEVNOfferPrice = d.MEVNOfferPrice
            })
            .ToList();

        if (!closedItems.Any())
        {
            return;
        }

        foreach (var detail in importDetails)
        {
            var row = detail.RowData;
            if (row == null) continue;

            var importGolfaCode = (row.GolfaCode ?? "").Trim();
            var importModelName = (row.ModelName ?? "").Trim();
            var importMEVNOfferPrice = row.MEVNOfferPrice ?? 0;

            var isDuplicate = closedItems.Any(closedItem =>
                string.Equals(closedItem.GolfaCode, importGolfaCode, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(closedItem.ModelName, importModelName, StringComparison.OrdinalIgnoreCase) &&
                closedItem.MEVNOfferPrice == importMEVNOfferPrice);

            if (isDuplicate)
            {
                detail.Errors.Add(
                    $"Material Code '{importGolfaCode}', " +
                    $"Model Name '{importModelName}', and " +
                    $"MEVN Offer Price '{importMEVNOfferPrice:N0}' already exists with Closed status."
                );
            }
        }
    }
}
