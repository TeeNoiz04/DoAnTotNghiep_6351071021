using QuoteFlow.Buyers;
using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.SalesAssignments;
using QuoteFlow.Shared.Excels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.PriceOffers.Excels.PriceOfferDSs.Validators;
public class PriceOfferDSValidator : IExcelValidator<PriceOfferImportDto>
{
    private readonly IExcelValidator<PriceOfferDetailImportDto> _offerDetailValidator;
    private readonly ILogger<PriceOfferDSValidator> _logger;
    private readonly PriceOfferMaterialValidationService _materialValidationService;
    private readonly IPriceOfferRepository _priceOfferRepository;
    private readonly IBuyerRepository _buyerRepository;
    private readonly ISalesAssignmentRepository _saleAssignmentRepository;
    public PriceOfferDSValidator(
        IExcelValidator<PriceOfferDetailImportDto> offerDetailValidator,
        IServiceProvider serviceProvider)
    {
        _offerDetailValidator = offerDetailValidator;
        _buyerRepository = serviceProvider.GetRequiredService<IBuyerRepository>();
        _logger = serviceProvider.GetRequiredService<ILogger<PriceOfferDSValidator>>();
        _priceOfferRepository = serviceProvider.GetRequiredService<IPriceOfferRepository>();
        _materialValidationService = serviceProvider.GetRequiredService<PriceOfferMaterialValidationService>();
        _saleAssignmentRepository = serviceProvider.GetRequiredService<ISalesAssignmentRepository>();
    }

    public async Task<ExcelValidationResult<PriceOfferImportDto>> ValidateAsync(Stream stream, string fileName, ExcelImportContext? context = null)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context), "ExcelImportContext cannot be null");
        }
        //var buyerId = context?.GetData<Guid>(ExcelImportContextKeys.PriceOffer.BuyerId)
        //?? throw new ArgumentNullException(nameof(context), "Buyer Id must be provided in the context");
        //var materialType = context?.GetData<string>(ExcelImportContextKeys.PriceOffer.MaterialType)
        //    ?? throw new ArgumentNullException(nameof(context), "Material type must be provided in the context");
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


        _logger.LogInformation("Starting validation for file: {FileName}", fileName);

        await ValidateSaleTeamAsync(buyerId, materialType, locationId, buyerTypeId, currentUserName);

        var priceOfferResult = new ExcelRowResult<PriceOfferImportDto>();

        stream.Seek(0, SeekOrigin.Begin);
        _logger.LogInformation("Validating offer details...");
        var offerDetailsResult = await _offerDetailValidator.ValidateAsync(stream, fileName);

        if (!offerDetailsResult.IsValid)
        {
            _logger.LogWarning("Offer details validation failed. Adding child list errors.");
            ExcelUtils.AddChildListErrors(priceOfferResult, offerDetailsResult, "[Price Offer Details]");
        }
        else if (offerDetailsResult.ListData.Count == 0)
        {
            _logger.LogWarning("No valid offer details found in the file: {FileName}", fileName);
            priceOfferResult.Errors.Add("No valid offer details found in the file.");
        }

        stream.Seek(0, SeekOrigin.Begin);
        _logger.LogInformation("Reading Excel rows for main price offer data...");
        var rows = MiniExcelHelper.ReadExcelRows(stream, PriceOfferConsts.ExcelImportSheetDS, PriceOfferConsts.ExcelMaterialCodeStartCell, PriceOfferConsts.ExcelMEVNOfferPriceEndCell, false);

        stream.Seek(0, SeekOrigin.Begin);

        _logger.LogInformation("Validating main rows...");
        await ValidateRowsAsync(rows, priceOfferResult);

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
                priceOfferResult.RowData.DiscountRatioConfigured = await _priceOfferRepository.GetDiscountRatioConfigured(
                    PriceOfferTypes.PriceOfferDS,
                    materialType,
                    totalMEVNOfferAmount
                );
            }
        }

        _logger.LogInformation("Validating material combinations and material group buyers...");

        var buyer = await _buyerRepository.FirstOrDefaultAsync(x => x.Id == buyerId);

        await _materialValidationService.ValidateMaterialCombinationsAndGroupBuyersAsync(priceOfferResult.RowData.Details.ListData, materialType, buyerId, buyer is null ? null : buyer.AppliedPrice);

        if (priceOfferResult.RowData.Details != null &&
            priceOfferResult.RowData.Details.ListData.Any(x => x.Errors.Any()))
        {
            _logger.LogWarning("Detected errors in child detail rows. Adding error messages to result.");
            ExcelUtils.AddChildListErrors(priceOfferResult, priceOfferResult.RowData.Details, "[Price Offer Details]");
        }

        var overallResult = new ExcelValidationResult<PriceOfferImportDto>(singleRow: true, fileName);
        overallResult.ListData.Add(priceOfferResult);
        ExcelUtils.AddRowErrors(overallResult, 1, priceOfferResult.Errors);

        if (!overallResult.IsValid)
        {
            _logger.LogWarning("Validation failed for file: {FileName}", fileName);
        }
        else
        {
            _logger.LogInformation("Validation succeeded for file: {FileName}", fileName);
        }

        return overallResult;
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

    protected virtual async Task ValidateRowsAsync(
        IEnumerable<IDictionary<string, object>> rows,
        ExcelRowResult<PriceOfferImportDto> result
    )
    {
        _logger.LogDebug("Begin parsing rows to build PriceOfferImportDto...");
        result.RowData = new PriceOfferImportDto();
        // Additional row validations and mappings can be logged here if implemented
        _logger.LogDebug("Finished parsing rows.");
    }
}
