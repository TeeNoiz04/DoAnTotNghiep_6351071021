using QuoteFlow.Buyers;
using QuoteFlow.MaterialGroupBuyers;
using QuoteFlow.PriceOffers.PriceOfferCustomers;
using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.SalesAssignments;
using QuoteFlow.Shared.Excels;
using QuoteFlow.SystemCategories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.PriceOffers.Excels.PriceOfferPPs.Validators;

public class PriceOfferPPValidator : IExcelValidator<PriceOfferImportDto>
{
    private readonly IExcelValidator<PriceOfferDetailImportDto> _offerDetailValidator;
    private readonly IExcelValidator<PriceOfferCustomerImportDto> _offerCustomerValidator;
    private readonly ISystemCategoryRepository _systemCategoryRepository;
    private readonly IPriceOfferRepository _priceOfferRepository;
    private readonly PriceOfferManager _manager;
    private readonly IBuyerRepository _buyerRepository;
    private readonly ILogger<PriceOfferPPValidator> _logger;
    private readonly PriceOfferMaterialValidationService _materialValidationService;
    private readonly IMaterialGroupBuyerRepository _materialGroupBuyerRepository;
    private readonly ISalesAssignmentRepository _saleAssignmentRepository;
    public PriceOfferPPValidator(
        IExcelValidator<PriceOfferDetailImportDto> offerDetailValidator,
        IExcelValidator<PriceOfferCustomerImportDto> offerCustomerValidator,
        IServiceProvider serviceProvider)
    {
        _offerDetailValidator = offerDetailValidator;
        _offerCustomerValidator = offerCustomerValidator;
        _buyerRepository = serviceProvider.GetRequiredService<IBuyerRepository>();
        _priceOfferRepository = serviceProvider.GetRequiredService<IPriceOfferRepository>();
        _systemCategoryRepository = serviceProvider.GetRequiredService<ISystemCategoryRepository>();
        _materialGroupBuyerRepository = serviceProvider.GetRequiredService<IMaterialGroupBuyerRepository>();
        _manager = serviceProvider.GetRequiredService<PriceOfferManager>();
        _logger = serviceProvider.GetRequiredService<ILogger<PriceOfferPPValidator>>();
        _materialValidationService = serviceProvider.GetRequiredService<PriceOfferMaterialValidationService>();
        _saleAssignmentRepository = serviceProvider.GetRequiredService<ISalesAssignmentRepository>();
    }

    public async Task<ExcelValidationResult<PriceOfferImportDto>> ValidateAsync(Stream stream, string fileName, ExcelImportContext? context = null)
    {
        ValidateContextAsync(context);

        var buyerId = context?.GetData<Guid>(ExcelImportContextKeys.PriceOffer.BuyerId)
            ?? throw new ArgumentNullException(nameof(context), "Buyer Id must be provided in the context");
        var buyerTypeId = context?.GetData<Guid>(ExcelImportContextKeys.PriceOffer.BuyerTypeId)
            ?? throw new ArgumentNullException(nameof(context), "Buyer Type Id must be provided in the context");
        var currentUserName = context?.GetData<string>(ExcelImportContextKeys.PriceOffer.CurrentUserName)
            ?? throw new ArgumentNullException(nameof(context), "Can not find Username");
        var materialTypeForm = context?.GetData<string>(ExcelImportContextKeys.PriceOffer.MaterialType)
            ?? throw new ArgumentNullException(nameof(context), "Material type must be provided in the context");
        var locationId = context?.GetData<Guid?>(ExcelImportContextKeys.PriceOffer.LocationId)
            ?? throw new ArgumentNullException(nameof(context), "LocationId must be provided in the context");
        await ValidateSaleTeamAsync(buyerId, materialTypeForm, locationId, buyerTypeId, currentUserName);

        var priceOfferResult = new ExcelRowResult<PriceOfferImportDto>();

        stream.Seek(0, SeekOrigin.Begin);
        var offerDetailsResult = await _offerDetailValidator.ValidateAsync(stream, fileName);
        // Validate additional constraints for details
        //var materialTypes = await _systemCategoryRepository.GetListAsync(x => x.CategoryType == CategoryTypes.MaterialType && !x.IsDeactive);
        //if (offerDetailsResult.ListData.Any(x => !materialTypes.Any(mt => mt.Description == x.RowData.)))
        //{
        //    offerDetailsResult.Errors.Add("All details must have a valid Material Type from the system categories.");
        //}
        if (!offerDetailsResult.IsValid)
        {
            ExcelUtils.AddChildListErrors(priceOfferResult, offerDetailsResult, "[Price Offer Details]");
        }
        else if (offerDetailsResult.ListData.Count == 0)
        {
            priceOfferResult.Errors.Add("At least one detail must be specified in the Price Offer Details section.");
        }

        stream.Seek(0, SeekOrigin.Begin);
        var offerCustomersResult = await _offerCustomerValidator.ValidateAsync(stream, fileName);
        if (!offerCustomersResult.IsValid)
        {
            ExcelUtils.AddChildListErrors(priceOfferResult, offerCustomersResult, "[Price Offer Customers]");
        }
        else if (offerCustomersResult.ListData.Count == 0)
        {
            priceOfferResult.Errors.Add("At least one customer must be specified in the Price Offer Customers section.");
        }

        stream.Seek(0, SeekOrigin.Begin);
        var rows = MiniExcelHelper.ReadExcelRows(stream, PriceOfferConsts.ExcelImportSheetPP, PriceOfferConsts.ExcelOfferStartCell, PriceOfferConsts.ExcelOfferEndCell, false);

        // special info: C3 => C5
        stream.Seek(0, SeekOrigin.Begin);
        var specialDataRows = MiniExcelHelper.ReadExcelRows(stream, PriceOfferConsts.ExcelImportSheetPP, "C3", "C4", false);

        // these rows are a bit special since it's not a list of rows, but a multiple rows, vertically stacked, each row is a property of the PriceOfferImportDto
        // col L is data
        // col M is checked correct info
        // col N (merged) is SEC check => just check N11, cuz it's merged with N12, N13, etc.

        // logic: Pre-validate: check if all rows in M are filled, and N11 is filled
        // if not, add errors and do not go to next phase

        // 2. Validate: check if all rows in L are valid
        var preValidationResult = PreValidate(rows);
        if (!preValidationResult.IsValid)
        {
            priceOfferResult.Errors.AddRange(preValidationResult.Errors);
        }
        else
        {
            await ValidateRowsAsync(rows, specialDataRows, priceOfferResult, context!);
            priceOfferResult.RowData.FileName = fileName;
            priceOfferResult.RowData.Details = offerDetailsResult;
            priceOfferResult.RowData.Customers = offerCustomersResult;

            if (offerDetailsResult.ListData.Count > 0)
            {
                priceOfferResult.RowData.TotalMEVNOfferAmount = offerDetailsResult.ListData.Sum(x => (x.RowData.Qty ?? 0) * (x.RowData.MEVNOfferPrice ?? 0));
                priceOfferResult.RowData.TotalPriceToCustomer = offerDetailsResult.ListData.Sum(x => x.RowData.PriceToCustomer ?? 0);
                priceOfferResult.RowData.TotalRequestedAmount = offerDetailsResult.ListData.Sum(x => x.RowData.RequestedAmount ?? 0);
                priceOfferResult.RowData.TotalStandardAmount = offerDetailsResult.ListData.Sum(x => x.RowData.StandardAmount ?? 0);
                var rowData = priceOfferResult.RowData;
                priceOfferResult.RowData.DiscountRatio = (1 - rowData.TotalMEVNOfferAmount / rowData.TotalStandardAmount);

                var materialType = rowData.MaterialType ?? string.Empty;
                var totalMEVNOfferAmount = rowData.TotalMEVNOfferAmount ?? 0;
                if (!string.IsNullOrEmpty(materialType) && totalMEVNOfferAmount > 0)
                {
                    priceOfferResult.RowData.DiscountRatioConfigured = await _priceOfferRepository.GetDiscountRatioConfigured(
                        PriceOfferTypes.PriceOfferPP,
                        materialType,
                        totalMEVNOfferAmount
                    );
                }
            }
            var buyer = await _buyerRepository.FirstOrDefaultAsync(x => x.Id == buyerId);
            // Validate material combinations and material group buyers in a single optimized call
            await _materialValidationService.ValidateMaterialCombinationsAndGroupBuyersAsync(priceOfferResult.RowData.Details.ListData, priceOfferResult.RowData.MaterialType!, buyerId, buyer is null ? null : buyer.AppliedPrice);

            if (priceOfferResult.RowData.Details != null &&
                priceOfferResult.RowData.Details.ListData.Any(x => x.Errors.Any()))
            {
                ExcelUtils.AddChildListErrors(priceOfferResult, priceOfferResult.RowData.Details, "[Price Offer Details]");
            }
        }

        var overallResult = new ExcelValidationResult<PriceOfferImportDto>(singleRow: true, fileName);
        overallResult.ListData.Add(priceOfferResult);
        ExcelUtils.AddRowErrors(overallResult, 1, priceOfferResult.Errors);

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

    protected virtual void ValidateContextAsync(ExcelImportContext? context)
    {
        if (context == null)
        {
            _logger.LogError("ExcelImportContext is null during validation.");
            throw new ArgumentNullException(nameof(context), "Context for this validator cannot be null.");
        }
        if (string.IsNullOrEmpty(context.GetData<string>(ExcelImportContextKeys.PriceOffer.MaterialType)))
        {
            _logger.LogError("MaterialType is not set in the ExcelImportContext.");
            throw new ArgumentException("MaterialType is required in the context.", nameof(context));
        }
    }

    protected virtual ValidationResult PreValidate(IEnumerable<IDictionary<string, object>> rows)
    {
        var result = new ValidationResult();
        // Check if all rows in M (Correct information confirmation) are filled
        var hasBlankCorrectedInfo = rows.Any(r =>
        {
            if (r.TryGetValue("N", out object? correctInfo))
            {
                return correctInfo is null || correctInfo is string correctInfoString && string.IsNullOrWhiteSpace(correctInfoString);
            }
            return true;
        });

        // Check if N11 (SEC Check) is filled
        var hasBlankSecCheck = rows.FirstOrDefault()?.TryGetValue("O", out object? secCheck) != true ||
                               secCheck is null || secCheck is string secCheckString && string.IsNullOrWhiteSpace(secCheckString);
        if (hasBlankCorrectedInfo || hasBlankSecCheck)
        {
            result.Errors.Add("All rows must have 'Correct information confirmation' and 'SEC Check' filled out.");
        }

        return result;
    }

    protected virtual async Task ValidateRowsAsync(
        IEnumerable<IDictionary<string, object>> rows,
        IEnumerable<IDictionary<string, object>> specialRows,
        ExcelRowResult<PriceOfferImportDto> result,
        ExcelImportContext context
    )
    {
        var materialType = context.GetData<string>(ExcelImportContextKeys.PriceOffer.MaterialType) ?? string.Empty;
        var fileMaterialType = ExcelParser.GetValue<string?>(specialRows.ElementAt(0), "C");
        var projectType = ExcelParser.GetValue<string?>(specialRows.ElementAt(1), "C");
        //var EUIndustry = ExcelParser.GetValue<string?>(specialRows.ElementAt(2), "C");

        // Mapping rows L11 to L22
        //var projectName = ExcelParser.GetValue<string?>(rows.ElementAt(0), "L");  // L11
        var application = ExcelParser.GetValue<string?>(rows.ElementAt(1), "M");  // L12
        var country = ExcelParser.GetValue<string?>(rows.ElementAt(2), "M");  // L13
        var province = ExcelParser.GetValue<string?>(rows.ElementAt(3), "M");  // L14
        var detailedAddress = ExcelParser.GetValue<string?>(rows.ElementAt(4), "M");  // L15
        var competitorBrand = ExcelParser.GetValue<string?>(rows.ElementAt(5), "M");  // L16
        var priceGapWithCompetitor = ExcelParser.GetValue<string?>(rows.ElementAt(6), "M");  // L17
        var decisionRight = ExcelParser.GetValue<string?>(rows.ElementAt(7), "M");  // L18
        var poPlannedDate = ExcelParser.GetValue<DateTime?>(rows.ElementAt(8), "M");  // L19
        var deliveryDate = ExcelParser.GetValue<DateTime?>(rows.ElementAt(9), "M");  // L20
        var upcomingPotentialProject = ExcelParser.GetValue<string?>(rows.ElementAt(10), "M"); // L21
        var otherPJInformation = ExcelParser.GetValue<string?>(rows.ElementAt(11), "M"); // L22

        // Validate required fields
        if (string.IsNullOrWhiteSpace(materialType))
            result.Errors.Add("Material Type is required.");
        else if (string.IsNullOrWhiteSpace(fileMaterialType))
            result.Errors.Add("Material Type in the file is required.");
        else if (!string.Equals(materialType, fileMaterialType, StringComparison.OrdinalIgnoreCase))
            result.Errors.Add($"Material Type in the form '{materialType}' does not match the Material Type in the file '{fileMaterialType}'.");
        //if (string.IsNullOrWhiteSpace(projectName)) result.Errors.Add("Project Name is required.");
        if (string.IsNullOrWhiteSpace(application)) result.Errors.Add("Application is required.");
        if (string.IsNullOrWhiteSpace(country)) result.Errors.Add("Country is required.");
        if (string.IsNullOrWhiteSpace(province)) result.Errors.Add("Province is required.");
        if (string.IsNullOrWhiteSpace(detailedAddress)) result.Errors.Add("Detailed Address is required.");
        if (string.IsNullOrWhiteSpace(competitorBrand)) result.Errors.Add("Competitor Brand is required.");
        if (string.IsNullOrWhiteSpace(priceGapWithCompetitor)) result.Errors.Add("Price Gap with Competitor is required.");
        if (poPlannedDate is null || poPlannedDate == default) result.Errors.Add("PO Planned Date is required.");
        if (deliveryDate is null || deliveryDate == default) result.Errors.Add("Delivery Date is required.");

        Guid? projectTypeId = null;
        if (string.IsNullOrWhiteSpace(projectType)) result.Errors.Add("Project Type is required.");
        else if (!projectType.StartsWith(materialType, StringComparison.OrdinalIgnoreCase))
        {
            result.Errors.Add($"Project Type '{projectType}' does not belong to Material Type '{materialType}'.");
        }
        else
        {
            try
            {
                var projectTypeLower = projectType?.ToLower();
                var projectTypeCategory = await _systemCategoryRepository.GetAsync(x =>
                    x.CategoryType == CategoryTypes.ProjectType && string.Equals(x.Description, projectType)
                );

                projectTypeId = projectTypeCategory.Id;
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError(ex, "Project Type not found: {ProjectType}", projectType);
                result.Errors.Add($"Project Type '{projectType}' not found in the system categories.");
            }
        }

        string priceOfferCode = "";

        if (!string.IsNullOrWhiteSpace(materialType))
        {
            priceOfferCode = _manager.GetDraftCode(PriceOfferConsts.ProjectPrefix, materialType ?? string.Empty);
        }
        else
        {
            priceOfferCode = "N/A";
        }


        // Assigning to result.RowData
        result.RowData = new PriceOfferImportDto
        {
            PriceOfferCode = priceOfferCode,
            //ProjectName = projectName,
            Application = application,
            Country = country,
            Province = province,
            DetailedAddress = detailedAddress,
            CompetitorBrand = competitorBrand,
            PriceGapWithCompetitor = priceGapWithCompetitor,
            DecisionRight = decisionRight,
            POPlannedDate = poPlannedDate,
            DeliveryDate = deliveryDate,
            UpcomingPotentialProjects = upcomingPotentialProject,
            OtherPJInformation = otherPJInformation,
            MaterialType = materialType,
            ProjectTypeId = projectTypeId,
            ProjectTypeDescription = projectType,
        };
    }
}
