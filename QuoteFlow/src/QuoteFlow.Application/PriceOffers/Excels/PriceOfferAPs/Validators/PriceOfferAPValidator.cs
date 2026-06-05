using QuoteFlow.Buyers;
using QuoteFlow.Materials;
using QuoteFlow.PriceOffers.Excels.PriceOfferPPs.Validators;
using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.SalesAssignments;
using QuoteFlow.Shared.Excels;
using QuoteFlow.Shared.Models;
using QuoteFlow.SystemCategories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.PriceOffers.Excels.PriceOfferAPs.Validators;
public class PriceOfferAPValidator : IExcelValidator<PriceOfferImportDto>
{
    private readonly IExcelValidator<PriceOfferDetailImportDto> _offerDetailValidator;
    private readonly ILogger<PriceOfferPPValidator> _logger;
    private readonly PriceOfferMaterialValidationService _materialValidationService;
    private readonly IPriceOfferRepository _priceOfferRepository;
    private readonly ISystemCategoryRepository _systemCategoryRepository;
    private readonly IBuyerRepository _buyerRepository;
    private readonly IMaterialRepository _materialRepository;
    private readonly ISalesAssignmentRepository _saleAssignmentRepository;
    public PriceOfferAPValidator(
        IExcelValidator<PriceOfferDetailImportDto> offerDetailValidator,
        IServiceProvider serviceProvider)
    {
        _offerDetailValidator = offerDetailValidator;
        _buyerRepository = serviceProvider.GetRequiredService<IBuyerRepository>();
        _logger = serviceProvider.GetRequiredService<ILogger<PriceOfferPPValidator>>();
        _materialValidationService = serviceProvider.GetRequiredService<PriceOfferMaterialValidationService>();
        _priceOfferRepository = serviceProvider.GetRequiredService<IPriceOfferRepository>();
        _systemCategoryRepository = serviceProvider.GetRequiredService<ISystemCategoryRepository>();
        _materialRepository = serviceProvider.GetRequiredService<IMaterialRepository>();
        _saleAssignmentRepository = serviceProvider.GetRequiredService<ISalesAssignmentRepository>();
    }

    public async Task<ExcelValidationResult<PriceOfferImportDto>> ValidateAsync(Stream stream, string fileName, ExcelImportContext? context = null)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context), "ExcelImportContext cannot be null");
        }

        var buyerId = context?.GetData<Guid>(ExcelImportContextKeys.PriceOffer.BuyerId)
            ?? throw new ArgumentNullException(nameof(context), "Buyer Id must be provided in the context");
        var buyerTypeId = context?.GetData<Guid>(ExcelImportContextKeys.PriceOffer.BuyerTypeId)
            ?? throw new ArgumentNullException(nameof(context), "Buyer Type Id must be provided in the context");
        var currentUserName = context?.GetData<string>(ExcelImportContextKeys.PriceOffer.CurrentUserName)
            ?? throw new ArgumentNullException(nameof(context), "Can not find Username");
        var materialType = context?.GetData<string>(ExcelImportContextKeys.PriceOffer.MaterialType)
            ?? throw new ArgumentNullException(nameof(context), "Material type must be provided in the context");
        var locationId = context?.GetData<Guid?>(ExcelImportContextKeys.PriceOffer.LocationId)
            ?? throw new ArgumentNullException(nameof(context), "LocationId must be provided in the context");
        var keyAccountClassId = context?.GetData<Guid?>(ExcelImportContextKeys.PriceOffer.KeyAccountClassId)
            ?? throw new ArgumentNullException(nameof(context), "Key Account Class Id must be provided in the context");
        var keyAccountId = context?.GetData<Guid?>(ExcelImportContextKeys.PriceOffer.KeyAccountId)
            ?? throw new ArgumentNullException(nameof(context), "Key Account Id must be provided in the context");
        var getPriceAuto = context?.GetData<bool?>(ExcelImportContextKeys.PriceOffer.GetPriceAuto)
            ?? throw new ArgumentNullException(nameof(context), "Get Price Auto must be provided in the context");
        var priceOfferResult = new ExcelRowResult<PriceOfferImportDto>();

        // Validate Price Offer for a key account must be only 1 per fiscal year (from August this year to August next year)
        await ValidatePriceOfferUniquenessAsync(buyerId, keyAccountId, materialType, locationId, priceOfferResult);
        await ValidateSaleTeamAsync(buyerId, materialType, locationId, buyerTypeId, currentUserName);

        stream.Seek(0, SeekOrigin.Begin);
        var offerDetailsResult = await _offerDetailValidator.ValidateAsync(stream, fileName);
        // Check get price automatically and fill data here
        await FillStandardPriceIfNotProvidedAsync(offerDetailsResult.ListData);
        if (getPriceAuto)
        {
            await AutoFillOfferPriceAsync(offerDetailsResult.ListData, keyAccountId);
        }
        else
        {
            ValidateRequiredFields(offerDetailsResult.ListData);
        }


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
            if (rowData.TotalStandardAmount <= 0)
            {
                priceOfferResult.Errors.Add("Total Standard Amount must be greater than 0.");
            }
            else
            {
                priceOfferResult.RowData.DiscountRatio = (1 - rowData.TotalMEVNOfferAmount / rowData.TotalStandardAmount);
            }

            var totalMEVNOfferAmount = rowData.TotalMEVNOfferAmount ?? 0;
            if (keyAccountClassId != Guid.Empty)
            {
                var keyAccountClass = await _systemCategoryRepository.GetAsync(keyAccountClassId, false);

                if (!string.IsNullOrEmpty(materialType) && totalMEVNOfferAmount > 0)
                {
                    priceOfferResult.RowData.DiscountRatioConfigured = await _priceOfferRepository.GetDiscountRatioConfigured(
                        PriceOfferTypes.PriceOfferAP,
                        materialType,
                        totalMEVNOfferAmount,
                        keyAccountClass.Description
                    );
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(materialType) && totalMEVNOfferAmount > 0)
                {
                    priceOfferResult.RowData.DiscountRatioConfigured = await _priceOfferRepository.GetDiscountRatioConfigured(
                        PriceOfferTypes.PriceOfferAP,
                        materialType,
                        totalMEVNOfferAmount
                    );
                }
            }

            _logger.LogInformation("Validating material combinations and material group buyers...");

            var buyer = await _buyerRepository.FirstOrDefaultAsync(x => x.Id == buyerId);
            await _materialValidationService.ValidateMaterialCombinationsAndGroupBuyersAsync(priceOfferResult.RowData.Details.ListData, materialType, buyerId, buyer is null ? null : buyer.AppliedPrice);
            await _materialValidationService.ValidateMaterialGroupKeyAccountAsync(priceOfferResult.RowData.Details.ListData);
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

    private async Task ValidatePriceOfferUniquenessAsync(Guid buyerId, Guid? keyAccountId, string materialType, Guid locationId, ExcelRowResult<PriceOfferImportDto> result)
    {
        if (keyAccountId.HasValue)
        {
            var now = DateTime.Now;
            var fiscalYearStart = new DateTime(now.Month >= 4 ? now.Year : now.Year - 1, 4, 1);
            var fiscalYearEnd = fiscalYearStart.AddYears(1).AddDays(-1);
            var existingOffer = await _priceOfferRepository.FirstOrDefaultAsync(po =>
                po.MaterialType == materialType &&
                po.LocationId == locationId &&
                po.KeyAccountId == keyAccountId &&
                po.PriceOfferCode.StartsWith(PriceOfferTypes.PriceOfferAP) &&
                (
                    po.ApprovalStatus != QuoteFlowStatuses.Cancelled
                    && po.ApprovalStatus != QuoteFlowStatuses.Rejected
                    && po.ApprovalStatus != QuoteFlowStatuses.Closed
                ) &&
                po.CreationTime!.Value.Date >= fiscalYearStart &&
                po.CreationTime!.Value.Date <= fiscalYearEnd);

            if (existingOffer != null)
            {
                result.Errors.Add(
                    $"Current active: {existingOffer.PriceOfferCode} - {existingOffer.MaterialType} - {existingOffer.LocationDescription} - {existingOffer.BuyerCode}." +
                    $"Only one Active SPO.AP is allowed per Key Account within the current fiscal year. " +
                    "Please ensure the existing SPO is Cancelled before importing a new one.");
            }
        }
    }
    private async Task ValidateSaleTeamAsync(Guid buyerId, string materialType, Guid locationId, Guid buyerTypeId, string currentUserName)
    {
        var saleTeam = await _saleAssignmentRepository.GetListAsync(x =>
            x.BuyerId == buyerId
            && x.MaterialType == materialType
            && x.LocationId == locationId
            && x.BuyerTypeId == buyerTypeId
            && x.SaleUserName.ToUpper() == currentUserName.ToUpper());

        if (saleTeam == null)
        {
            throw new UserFriendlyException("You cannot perform this action because you are not part of this sales team.");
        }

    }

    private async Task AutoFillOfferPriceAsync(List<ExcelRowResult<PriceOfferDetailImportDto>> details, Guid keyAccountId)
    {
        var discount = await _priceOfferRepository.GetDiscountRatioAsync(keyAccountId);
        foreach (var detail in details)
        {
            if (!detail.RowData.StandardPrice.HasValue || detail.RowData.StandardPrice <= 0)
            {
                detail.Errors.Add("Standard Price must be greater than 0 to calculate automatic offer price.");
                continue;
            }
            detail.RowData.MEVNOfferPrice = Math.Ceiling(detail.RowData.StandardPrice.Value * (1 - discount));
            detail.RowData.MEVNOfferAmount = detail.RowData.MEVNOfferPrice * detail.RowData.Qty;
        }

    }

    private void ValidateRequiredFields(List<ExcelRowResult<PriceOfferDetailImportDto>> details)
    {
        foreach (var detail in details)
        {
            var rowErrors = new List<string>();
            //StandardAmount
            if (!detail.RowData.StandardAmount.HasValue || detail.RowData.StandardAmount <= 0)
            {
                rowErrors.Add("Standard Amount must be provided and greater than 0 when not using automatic pricing.");
            }
            if (!detail.RowData.MEVNOfferPrice.HasValue || detail.RowData.MEVNOfferPrice <= 0)
            {
                rowErrors.Add("MEVN Offer Price must be provided and greater than 0 when not using automatic pricing.");
            }
            if (!detail.RowData.BuyerPrice.HasValue || detail.RowData.BuyerPrice <= 0)
            {
                rowErrors.Add("Distributor requested price must be provided and greater than 0 when not using automatic pricing.");
            }

            detail.Errors.AddRange(rowErrors);
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
}
