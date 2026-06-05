using QuoteFlow.Buyers;
using QuoteFlow.Customers;
using QuoteFlow.DPOs.DPODetails;
using QuoteFlow.MaterialGroupBuyers;
using QuoteFlow.Materials;
using QuoteFlow.PriceOffers;
using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.Shared.Excels;
using QuoteFlow.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.DPOs.Excels.ImportDPO.Validators;
public class ImportDPODetailValidator : BaseExcelValidator<ImportDPODetailDto>
{
    private readonly IPriceOfferRepository _priceOfferRepository;
    private readonly IPriceOfferDetailRepository _priceOfferDetailRepository;
    private readonly IMaterialRepository _materialRepository;
    private readonly IBuyerRepository _buyerRepository;
    private readonly IMaterialGroupBuyerRepository _materialGroupBuyerRepository;
    private readonly ICustomerRepository _customerRepository;

    public ImportDPODetailValidator(
        ImportDPODetailValidationConfig config,
        IExcelRowValidator<ImportDPODetailDto> rowValidator,
        IPriceOfferRepository priceOfferRepository,
        ILogger<BaseExcelValidator<ImportDPODetailDto>> logger,
        IServiceProvider provider
    ) : base(config, rowValidator, logger)
    {
        _priceOfferRepository = priceOfferRepository;
        _priceOfferDetailRepository = provider.GetRequiredService<IPriceOfferDetailRepository>();
        _materialRepository = provider.GetRequiredService<IMaterialRepository>();
        _buyerRepository = provider.GetRequiredService<IBuyerRepository>();
        _materialGroupBuyerRepository = provider.GetRequiredService<IMaterialGroupBuyerRepository>();
        _customerRepository = provider.GetRequiredService<ICustomerRepository>();
    }

    protected override async Task PostValidateAsync(ExcelValidationResult<ImportDPODetailDto> result, ExcelImportContext context)
    {
        // Get all SPO codes from the import data
        var spoCodes = result.ListData
            .Where(x => !string.IsNullOrWhiteSpace(x.RowData.SPOCode))
            .Select(x => x.RowData.SPOCode!)
            .Distinct()
            .ToList();

        // Get existing SPO price offers and their details
        Dictionary<string, PriceOffer> existingSPOOffers = new();
        Dictionary<string, List<PriceOfferDetail>> spoOfferDetails = new();

        if (spoCodes.Count != 0)
        {
            // Check if SPO codes exist in PriceOffers
            var existingSPOCodes = await _priceOfferRepository.GetListAsync(
                x => spoCodes.Contains(x.PriceOfferCode)
            );

            existingSPOOffers = existingSPOCodes.ToDictionary(x => x.PriceOfferCode, x => x);

            // Get all price offer details for existing SPO codes
            var priceOfferIds = existingSPOCodes.Select(x => x.Id).ToList();
            var allPriceOfferDetails = await _priceOfferDetailRepository.GetListAsync(
                pod => priceOfferIds.Contains(pod.PriceOfferId));

            foreach (var priceOffer in existingSPOCodes)
            {
                spoOfferDetails[priceOffer.PriceOfferCode] = allPriceOfferDetails
                    .Where(pod => pod.PriceOfferId == priceOffer.Id)
                    .ToList();
            }
        }

        // Get unique material combinations for standard price validation
        var materialLookups = result.ListData
            .Where(x => !string.IsNullOrWhiteSpace(x.RowData.GolfaCode) && !string.IsNullOrWhiteSpace(x.RowData.Model))
            .Select(x => new { GolfaCode = x.RowData.GolfaCode!, Model = x.RowData.Model! })
            .Distinct()
            .ToList();

        Dictionary<string, MaterialProjection> materialsByKey = new();

        //var materials = await _materialRepository.GetListWithDeactiveAsync(new(),
        //    x => new MaterialProjection
        //    {
        //        GolfaCode = x.GolfaCode,
        //        Model = x.Model,
        //        MaterialType = x.MaterialType,
        //        StandardPrice = x.Standard_Price,
        //        SellingPrice1 = x.SellingPrice1,
        //        MaterialStatus = x.MaterialStatus
        //    });
        var materials = await _materialRepository.GetListAsync(new(),
            x => new MaterialProjection
            {
                GolfaCode = x.GolfaCode,
                Model = x.Model,
                MaterialType = x.MaterialType,
                StandardPrice = x.Standard_Price,
                SellingPrice1 = x.SellingPrice1,
                //MaterialStatus = x.MaterialStatus
            });

        foreach (var lookup in materialLookups)
        {
            var material = materials.FirstOrDefault(m =>
                m.GolfaCode.Equals(lookup.GolfaCode, StringComparison.OrdinalIgnoreCase)
                && m.Model.Equals(lookup.Model, StringComparison.OrdinalIgnoreCase));
            if (material != null)
            {
                var key = $"{lookup.GolfaCode}_{lookup.Model}";
                materialsByKey[key] = material;
            }
        }

        var buyerId = context.GetData<Guid>(ExcelImportContextKeys.DPO.BuyerId);
        var expectedMaterialType = context.GetData<string>(ExcelImportContextKeys.DPO.MaterialType);
        // Validate each row for unit price and quantity (individual detail validation)
        foreach (var row in result.ListData)
        {
            var hasSPOCode = !string.IsNullOrWhiteSpace(row.RowData.SPOCode);
            if (hasSPOCode)
            {
                ValidateSPOBasedRow(row, existingSPOOffers, spoOfferDetails);
            }

            if (!string.IsNullOrWhiteSpace(row.RowData.GolfaCode) && !string.IsNullOrWhiteSpace(row.RowData.Model))
            {
                await ValidateMaterialAsync(row, materialsByKey, buyerId, expectedMaterialType, hasSPOCode);
            }

            // CRITICAL: Add row-level errors to the main result
            if (row.Errors.Count != 0)
            {
                ExcelUtils.AddRowErrors(result, row.RowIndex, row.Errors);
            }
        }

        await ValidateMaterialGroupBuyersAsync(result.ListData, buyerId);

        // Enrich customer information for non-SPO rows
        await EnrichCustomerInformationAsync(result);

        await base.PostValidateAsync(result, context);
    }

    private async Task EnrichCustomerInformationAsync(ExcelValidationResult<ImportDPODetailDto> result)
    {
        // Only process non-SPO rows - SPO rows get customer info from the parent validator
        var nonSPORows = result.ListData
            .Where(row => string.IsNullOrWhiteSpace(row.RowData?.SPOCode))
            .ToList();

        if (!nonSPORows.Any())
        {
            return;
        }

        // Extract unique tax codes from non-SPO detail rows
        var taxCodes = nonSPORows
            .Where(row => !string.IsNullOrWhiteSpace(row.RowData?.CustomerTaxCode))
            .Select(row => row.RowData!.CustomerTaxCode!)
            .Distinct()
            .ToList();

        if (!taxCodes.Any())
        {
            return;
        }

        // Batch lookup existing customers
        var existingCustomers = await _customerRepository.GetListAsync(
            x => taxCodes.Contains(x.TaxCode),
            includeDetails: false);

        var customerInfoLookupMap = new Dictionary<string, ExcelImportContextKeys.CustomerInfoLookup>();

        // Create map for existing customers
        foreach (var customer in existingCustomers)
        {
            customerInfoLookupMap[customer.TaxCode] = new ExcelImportContextKeys.CustomerInfoLookup
            {
                CustomerId = customer.Id,
                CustomerName = customer.CustomerName,
                CustomerType = customer.CustomerType,
                Industry = customer.CustomerIndustry
            };
        }

        // Update each non-SPO row with the resolved customer information
        foreach (var row in nonSPORows)
        {
            if (row.RowData == null || string.IsNullOrWhiteSpace(row.RowData.CustomerTaxCode))
            {
                continue;
            }

            // If this is an existing customer, populate with database values
            if (customerInfoLookupMap.TryGetValue(row.RowData.CustomerTaxCode, out var customerInfo))
            {
                row.RowData.CustomerId = customerInfo.CustomerId;
                row.RowData.CustomerName = customerInfo.CustomerName;
                row.RowData.CustomerType = customerInfo.CustomerType;
                row.RowData.CustomerIndustry = customerInfo.Industry;
            }
            // For new customers, the user-entered values from the Excel are already in RowData
            // No action needed - they'll be displayed as-is
        }
    }

    private void ValidateSPOBasedRow(
        ExcelRowResult<ImportDPODetailDto> row,
        Dictionary<string, PriceOffer> existingSPOOffers,
        Dictionary<string, List<PriceOfferDetail>> spoOfferDetails)
    {
        var spoCode = row.RowData.SPOCode!;
        var golfaCode = row.RowData.GolfaCode;
        var model = row.RowData.Model;
        var unitPrice = row.RowData.UnitPrice;
        var qty = row.RowData.Qty;

        // Check if SPO code exists
        if (!existingSPOOffers.ContainsKey(spoCode))
        {
            row.Errors.Add($"SPO Code '{spoCode}' does not exist in the system.");
            return;
        }

        var priceOffer = existingSPOOffers[spoCode];

        if (priceOffer.IsLost())
        {
            row.Errors.Add($"Price offer with code '{spoCode}' was lost and cannot be used for new DPO details.");
            return;
        }

        // Validate SPO approval status based on type
        var spoType = priceOffer.GetPriceOfferType();

        // PP must be APPROVED and confirmed as WIN/PRE_ORDER
        if (spoType == PriceOfferTypes.PriceOfferPP)
        {
            if (!priceOffer.IsApproved())
            {
                row.Errors.Add($"SPO Code '{spoCode}' of type {spoType} must have Approval Status = APPROVED. Current status: {priceOffer.ApprovalStatus}");
                return;
            }
            if (!priceOffer.IsWon() && !priceOffer.IsPreOrder())
            {
                row.Errors.Add($"SPO Code '{spoCode}' of type {spoType} must have project status WON or PRE_ORDER.");
                return;
            }
        }

        // DS must be APPROVED
        if (spoType == PriceOfferTypes.PriceOfferDS)
        {
            if (!priceOffer.IsApproved())
            {
                row.Errors.Add($"SPO Code '{spoCode}' of type {spoType} must have Approval Status = APPROVED. Current status: {priceOffer.ApprovalStatus}");
                return;
            }
        }

        // AP must be APPROVED or IN_PROGRESS
        if (spoType == PriceOfferTypes.PriceOfferAP)
        {
            if (!priceOffer.IsApproved() && !priceOffer.IsInProgress())
            {
                row.Errors.Add($"SPO Code '{spoCode}' of type AP must have Approval Status = APPROVED or IN_PROGRESS. Current status: {priceOffer.ApprovalStatus}");
                return;
            }
        }

        // Get matching price offer details based on GolfaCode and Model
        if (!spoOfferDetails.TryGetValue(spoCode, out List<PriceOfferDetail>? value))
        {
            row.Errors.Add($"No price offer details found for SPO Code '{spoCode}'.");
            return;
        }

        var matchingDetails = value.Where(d =>
            d.GolfaCode.Equals(golfaCode, StringComparison.OrdinalIgnoreCase)
            && d.ModelName.Equals(model, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (matchingDetails.Count == 0)
        {
            row.Errors.Add($"No matching price offer details found for SPO Code '{spoCode}', Material Code '{golfaCode}', and Model '{model}'.");
            return;
        }

        // Find the specific detail that matches the unit price (Rule 1: distinct by GolfaCode + Model + UnitPrice)
        var matchingApprovedPriceDetail = matchingDetails.FirstOrDefault(d => d.MEVNOfferPrice == unitPrice && d.IsApproved());
        var matchingPriceDetail = matchingDetails.FirstOrDefault(d => d.MEVNOfferPrice == unitPrice);
        if (matchingApprovedPriceDetail == null && matchingPriceDetail == null)
        {
            var availablePrices = matchingDetails
                .Where(d => d.IsApproved())
                .Select(d => $"{d.MEVNOfferPrice:N0}")
                .Distinct()
                .ToList();
            row.Errors.Add($"Unit price {unitPrice:N0} does not match any available MEVN Offer Price. Available prices: [{string.Join(", ", availablePrices)}]");
            return;
        }
        else if (matchingApprovedPriceDetail == null && matchingPriceDetail != null)
        {
            row.Errors.Add($"The price offer item with Material Code '{golfaCode}', Model '{model}', and Unit price {unitPrice:N0} is not approved and cannot be used.");
            return;
        }

        // Check if sufficient quantity is available for this specific price detail (Rule 2: Qty must be enough)
        var availableQty = matchingApprovedPriceDetail.Qty - (matchingApprovedPriceDetail.DpoUsed ?? 0);
        if (qty > availableQty)
        {
            row.Errors.Add($"Requested quantity {qty:N0} exceeds available quantity {availableQty:N0} for unit price {unitPrice:N0}.");
            return;
        }
    }

    private async Task ValidateMaterialAsync(
        ExcelRowResult<ImportDPODetailDto> row,
        Dictionary<string, MaterialProjection> materialsByKey,
        Guid buyerId,
        string? expectedMaterialType,
        bool hasSPOCode)
    {
        var golfaCode = row.RowData.GolfaCode!;
        var model = row.RowData.Model!;
        var unitPrice = row.RowData.UnitPrice;
        int appliedPrice = 0;

        try
        {
            var buyer = await _buyerRepository.GetAsync(buyerId);
            if (buyer != null && buyer.AppliedPrice.HasValue)
            {
                appliedPrice = buyer.AppliedPrice.Value;
            }
        }
        catch (EntityNotFoundException)
        {
            throw new UserFriendlyException("This is not a valid buyer. Please select a valid buyer.");
        }

        var materialKey = $"{golfaCode}_{model}";

        if (!materialsByKey.TryGetValue(materialKey, out MaterialProjection? material))
        {
            row.Errors.Add($"No material found for Material Code '{golfaCode}' and Model '{model}'.");
            return;
        }
        else if (!material.MaterialType?.Equals(expectedMaterialType) ?? false)
        {
            var actualMaterialType = material.MaterialType;
            row.Errors.Add($"Material '{golfaCode} - {model}' does not belong to the expected MaterialType '{expectedMaterialType}'. Actual MaterialType: '{actualMaterialType}'.");
        }
        else if (material.MaterialStatus == MaterialStatuses.Deactivated)
        {
            row.Errors.Add($"Material is deactivated: Material Code '{golfaCode}' and Model '{model}'.");
        }
        //check status

        //if (!hasSPOCode)
        //{
        //    var price = material.StandardPrice;
        //    var priceText = "Standard Price";
        //    switch (appliedPrice)
        //    {
        //        case 0:
        //            // No applied price, use standard price
        //            break;
        //        case 1:
        //            price = material.SellingPrice1 ?? price;
        //            priceText = "Selling Price";
        //            break;
        //        default:
        //            row.Errors.Add($"Invalid applied price option: {appliedPrice}");
        //            return;
        //    }
        //    if (unitPrice != price)
        //    {
        //        row.Errors.Add($"Unit price {unitPrice:N0} does not match the {priceText} {price:N0} for material '{golfaCode} - {model}'.");
        //    }
        //}
    }

    public virtual async Task ValidateMaterialGroupBuyersAsync(List<ExcelRowResult<ImportDPODetailDto>> detailsResult, Guid buyerId)
    {
        if (detailsResult == null || detailsResult.Count == 0)
        {
            return;
        }

        var materialCodes = detailsResult
            .Where(x => x.RowData != null && !string.IsNullOrWhiteSpace(x.RowData.GolfaCode))
            .Select(x => x.RowData.GolfaCode!)
            .Distinct()
            .ToList();

        if (materialCodes.Count == 0)
        {
            return;
        }

        var materialData = await _materialRepository.GetQueryableAsync();
        var materials = materialData
            .Where(x => materialCodes.Contains(x.GolfaCode)
            && x.MaterialStatus != MaterialStatuses.Deactivated
            )
            .Select(x => new MaterialWithGroupInfo()
            {
                MaterialGroup = x.Material_Group,
                MaterialCode = x.GolfaCode
            })
            .Distinct()
            .ToList();

        if (materials.Count == 0)
        {
            return;
        }

        var materialGroups = materials
            .Select(x => x.MaterialGroup)
            .Where(x => !string.IsNullOrEmpty(x))
            .Distinct()
            .ToList();
        var groupBuyers = await _materialGroupBuyerRepository.GetListAsync(
            x => !string.IsNullOrEmpty(x.MaterialGroupCode) && materialGroups.Contains(x.MaterialGroupCode) && x.BuyerId == buyerId);

        foreach (var detail in detailsResult)
        {
            if (detail.RowData == null || string.IsNullOrWhiteSpace(detail.RowData.GolfaCode))
            {
                continue;
            }
            var materialCode = detail.RowData.GolfaCode!;
            var materialGroup = materials.FirstOrDefault(x => x.MaterialCode.Equals(materialCode, StringComparison.OrdinalIgnoreCase))?.MaterialGroup;
            if (string.IsNullOrEmpty(materialGroup))
            {
                detail.Errors.Add($"Material '{materialCode}' does not belong to any material group.");
                continue;
            }
            var groupBuyer = groupBuyers.FirstOrDefault(x => string.Equals(x.MaterialGroupCode, materialGroup, StringComparison.OrdinalIgnoreCase));
            if (groupBuyer == null)
            {
                detail.Errors.Add($"Material group '{materialGroup}' is not supported for the selected buyer.");
            }
        }
    }

    private class MaterialProjection
    {
        public string GolfaCode { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string? MaterialType { get; set; }
        public decimal StandardPrice { get; set; } = 0m;
        public decimal? SellingPrice1 { get; set; }
        public string? MaterialStatus { get; set; }
    }

    private class MaterialWithGroupInfo
    {
        public string? MaterialGroup { get; set; }
        public string MaterialCode { get; set; } = null!;
    }
}