using QuoteFlow.Buyers;
using QuoteFlow.Customers;
using QuoteFlow.DPOs.DPODetails;
using QuoteFlow.KeyAccounts;
using QuoteFlow.Materials;
using QuoteFlow.PriceOffers;
using QuoteFlow.PriceOffers.PriceOfferCustomers;
using QuoteFlow.Shared;
using QuoteFlow.Shared.Excels;
using QuoteFlow.Shared.Services;
using QuoteFlow.SystemConfigurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Linq;

namespace QuoteFlow.DPOs.Excels.ImportDPO.Validators;


public class ImportDPOValidator : IExcelValidator<ImportDPODto>
{
    private readonly IExcelValidator<ImportDPODetailDto> _detailValidator;
    private readonly IMaterialRepository _materialRepository;
    private readonly IDPORepository _dpoRepository;
    private readonly IDPODetailRepository _dpoDetailRepository;
    private readonly IPriceOfferCustomerRepository _priceOfferCustomerRepository;
    private readonly IPriceOfferRepository _priceOfferRepository;
    private readonly ISystemConfigurationRepository _systemConfigurationRepository;
    private readonly ILogger<ImportDPOValidator> _logger;
    private readonly IAsyncQueryableExecuter _asyncQueryableExecuter;
    private readonly ICustomerRepository _customerRepository;
    private readonly IKeyAccountRepository _keyAccountRepository;
    private readonly IDuplicateDetectionService _duplicateDetectionService;
    private readonly IBuyerRepository _buyerRepository;

    public ImportDPOValidator(
        IExcelValidator<ImportDPODetailDto> detailValidator,
        IServiceProvider serviceProvider)
    {
        _detailValidator = detailValidator;
        _materialRepository = serviceProvider.GetRequiredService<IMaterialRepository>();
        _priceOfferCustomerRepository = serviceProvider.GetRequiredService<IPriceOfferCustomerRepository>();
        _logger = serviceProvider.GetRequiredService<ILogger<ImportDPOValidator>>();
        _dpoRepository = serviceProvider.GetRequiredService<IDPORepository>();
        _dpoDetailRepository = serviceProvider.GetRequiredService<IDPODetailRepository>();
        _priceOfferRepository = serviceProvider.GetRequiredService<IPriceOfferRepository>();
        _systemConfigurationRepository = serviceProvider.GetRequiredService<ISystemConfigurationRepository>();
        _asyncQueryableExecuter = serviceProvider.GetRequiredService<IAsyncQueryableExecuter>();
        _customerRepository = serviceProvider.GetRequiredService<ICustomerRepository>();
        _keyAccountRepository = serviceProvider.GetRequiredService<IKeyAccountRepository>();
        _duplicateDetectionService = serviceProvider.GetRequiredService<IDuplicateDetectionService>();
        _buyerRepository = serviceProvider.GetRequiredService<IBuyerRepository>();
    }

    public async Task<ExcelValidationResult<ImportDPODto>> ValidateAsync(Stream stream, string fileName, ExcelImportContext? context = null)
    {
        ValidateContext(context);

        var dpoResult = new ExcelRowResult<ImportDPODto>();
        // Read DPO header information (MaterialType at C2, DPONo at C3, Remark at C4)
        stream.Seek(0, SeekOrigin.Begin);
        var headerRows = MiniExcelHelper.ReadExcelRows(stream, DPOConsts.ExcelImportSheetName, "C2", "C4", false);

        if (headerRows.Any())
        {
            // ignore the material type from the excel file
            //var materialType = ExcelParser.GetValue<string?>(headerRows.ElementAtOrDefault(0), "C");
            var dpoNo = ExcelParser.GetValue<string?>(headerRows.ElementAtOrDefault(1), "C");
            var remark = ExcelParser.GetValue<string?>(headerRows.ElementAtOrDefault(2), "C");

            if (string.IsNullOrWhiteSpace(dpoNo))
            {
                dpoResult.Errors.Add("DPONo is required.");
            }

            // Check if DPO No already exists
            var dpoExist = await _dpoRepository.AnyAsync(dpo => dpo.DPONo == dpoNo);
            if (dpoExist)
            {
                dpoResult.Errors.Add($"DPONo '{dpoNo}' already exists in the system.");
            }

            // Create ImportDPODto
            dpoResult.RowData = new ImportDPODto
            {
                DPONo = dpoNo,
                Remark = remark,
                FileName = fileName,
            };
        }
        else
        {
            dpoResult.Errors.Add("Unable to read DPO header information.");
        }

        // Validate DPO details
        stream.Seek(0, SeekOrigin.Begin);
        var detailsResult = await _detailValidator.ValidateAsync(stream, fileName, context);
        dpoResult.RowData.Details = detailsResult;

        // Detect Duplicate DPO Details within the same import
        ValidateGolfaDuplication(detailsResult);

        if (!detailsResult.IsValid)
        {
            ExcelUtils.AddChildListErrors(dpoResult, detailsResult, "[DPO Details]");
        }


        // Validate DPO-level SPO business rules (only if details are valid)
        if (detailsResult.IsValid && detailsResult.ListData.Any())
        {
            await ValidateDPOLevelSPOBusinessRulesAsync(dpoResult, detailsResult, context);
        }

        var overallResult = new ExcelValidationResult<ImportDPODto>(singleRow: true, fileName);
        overallResult.ListData.Add(dpoResult);
        ExcelUtils.AddRowErrors(overallResult, 1, dpoResult.Errors);

        return overallResult;
    }

    private void ValidateGolfaDuplication(ExcelValidationResult<ImportDPODetailDto> detailsResult)
    {
        var fileCombinations = detailsResult.ListData
                    .Where(x => x.RowData != null &&
                               !string.IsNullOrWhiteSpace(x.RowData.GolfaCode) &&
                               !string.IsNullOrWhiteSpace(x.RowData.Model))
                    .Select(x => new MaterialFullCombinationWithRowIndexDto
                    {
                        GolfaCode = x.RowData.GolfaCode!,
                        ModelName = x.RowData.Model!,
                        RowIndex = x.RowIndex,
                    })
                    .ToList();

        var fieldMapping = new MaterialCombinationFieldMapping<MaterialFullCombinationWithRowIndexDto>
        {
            GetCode = x => x.GolfaCode,
            GetModel = x => x.ModelName,
        };
        var duplicateGroups = _duplicateDetectionService.FindMaterialCombinationDuplicates(
            fileCombinations,
            fieldMapping);

        foreach (var duplicateGroup in duplicateGroups)
        {
            var errorMessage = $"Duplicate material combination: Golfa Code = {duplicateGroup.Key.GolfaCode}, Model = {duplicateGroup.Key.Model} appears multiple times in the details.";

            // loop thru each duplicated item that in the same key and add err msg
            foreach (var duplicate in duplicateGroup.Items)
            {
                var detailRow = detailsResult.ListData.FirstOrDefault(x => x.RowIndex == duplicate.RowIndex);
                if (detailRow != null)
                {
                    detailRow.Errors.Add(errorMessage);
                    ExcelUtils.AddRowErrors(listResult: detailsResult, rowIndex: detailRow.RowIndex + 1, rowErrors: detailRow.Errors);
                }
            }
        }
    }

    private void ValidateContext(ExcelImportContext? context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context), "Excel import context cannot be null.");
        }
    }

    private async Task ValidateDPOLevelSPOBusinessRulesAsync(
        ExcelRowResult<ImportDPODto> dpoResult,
        ExcelValidationResult<ImportDPODetailDto> detailsResult,
        ExcelImportContext context)
    {
        var spoBasedRows = detailsResult.ListData.Where(x => !string.IsNullOrWhiteSpace(x.RowData.SPOCode)).ToList();

        if (!spoBasedRows.Any())
        {
            return; // No SPO-based rows to validate
        }

        // Get unique SPO codes from this DPO
        var uniqueSPOCodes = spoBasedRows.Select(x => x.RowData.SPOCode!).Distinct().ToList();

        // Get existing SPO price offers
        var existingSPOCodes = await _priceOfferRepository.GetListAsync(
            x => uniqueSPOCodes.Contains(x.PriceOfferCode)
        );
        var existingSPOOffers = existingSPOCodes.ToDictionary(x => x.PriceOfferCode, x => x);

        var dpoDetailUsedSPOCodes = await _dpoDetailRepository.GetListAsync(
            x => uniqueSPOCodes.Contains(x.SPOCode!)
        );

        // Check if all SPO codes are AP (Key Account) - if so, bypass all validations
        var allSPOsAreAP = uniqueSPOCodes.All(spoCode =>
            existingSPOOffers.TryGetValue(spoCode, out var po) &&
            po.GetPriceOfferType() == PriceOfferTypes.PriceOfferAP);

        // Validate SPO code uniqueness for PP and DS types
        var isSingleSPO = CheckSPOCodeUniqueness(dpoResult, uniqueSPOCodes, existingSPOOffers);
        PriceOfferCustomerProjection? customerInDPO = null;

        // Current rule: single SPO code per DPO import, so we'll get the last SPO customer only if there is a single SPO code.
        // NOTE: If later we allow multiple SPO codes, this logic will need to be adjusted.
        if (isSingleSPO)
        {
            var priceOffer = existingSPOOffers.Values.First();
            var offerCustomerData = await _priceOfferCustomerRepository.GetQueryableAsync();

            if (priceOffer.IsProjectPriceOffer())
            {
                customerInDPO = await _asyncQueryableExecuter.FirstOrDefaultAsync(
                    offerCustomerData
                        .Where(x => x.IsDeleted == false && x.PriceOfferId == priceOffer.Id)
                        .OrderByDescending(x => x.SaleChannelNumber)
                        .Select(x => new PriceOfferCustomerProjection
                        {
                            PriceOfferId = x.PriceOfferId,
                            CustomerId = x.CustomerId,
                            CustomerName = x.CustomerName ?? "",
                            CustomerTaxCode = x.CustomerTaxCode ?? "",
                            CustomerType = x.CustomerType ?? "",
                            CustomerIndustry = x.CustomerIndustry ?? ""
                        })
                );
            }
            else if (priceOffer.IsKeyAccountPriceOffer())
            {
                var keyAccount = await _keyAccountRepository.FirstOrDefaultAsync(x => x.Id == priceOffer.KeyAccountId);
                if (keyAccount == null)
                {
                    dpoResult.Errors.Add($"The SPO '{priceOffer.PriceOfferCode}' is linked to a Key Account that does not exist.");
                    return;
                }

                var keyAccountCustomer = await _customerRepository.FirstOrDefaultAsync(x => x.TaxCode == keyAccount.CustomerTaxCode);
                if (keyAccountCustomer == null)
                {
                    dpoResult.Errors.Add($"The Key Account '{keyAccount.KeyAccountName}' linked to SPO '{priceOffer.PriceOfferCode}' has a Customer Tax Code '{keyAccount.CustomerTaxCode}' that does not exist in the system.");
                    return;
                }
                customerInDPO = new PriceOfferCustomerProjection
                {
                    PriceOfferId = priceOffer.Id,
                    CustomerId = keyAccountCustomer.Id,
                    CustomerName = keyAccount.CustomerName ?? "",
                    CustomerTaxCode = keyAccount.CustomerTaxCode ?? "",
                    CustomerType = keyAccountCustomer.CustomerType ?? "",
                    CustomerIndustry = keyAccount.Industry ?? ""
                };

            }
            else if (priceOffer.IsBuyerStockPriceOffer())
            {
                // DS type: CustomerId = Buyer.Id, CustomerIndustry from Customer table (can be null)
                Guid? customerId = priceOffer.BuyerId;
                string? customerIndustry = null;

                // Try to get CustomerIndustry from Customer table by matching Buyer.TaxCode
                if (!string.IsNullOrWhiteSpace(priceOffer.Buyer?.TaxCode))
                {
                    var customer = await _customerRepository.FirstOrDefaultAsync(x => x.TaxCode == priceOffer.Buyer.TaxCode);
                    if (customer != null)
                    {
                        customerIndustry = customer.CustomerIndustry;
                    }
                }

                customerInDPO = new PriceOfferCustomerProjection
                {
                    PriceOfferId = priceOffer.Id,
                    CustomerId = customerId,
                    CustomerName = priceOffer.Buyer?.FullName ?? "",
                    CustomerTaxCode = priceOffer.Buyer?.TaxCode ?? "",
                    CustomerType = "Distributor",
                    CustomerIndustry = customerIndustry ?? ""
                };
            }
        }

        // Group rows by SPOCode for individual SPO validation
        var spoGroups = spoBasedRows.GroupBy(x => x.RowData.SPOCode!).ToList();

        foreach (var spoGroup in spoGroups)
        {
            var spoCode = spoGroup.Key;
            var rowsForSPO = spoGroup.ToList();

            // Add customer information to each row for this SPO
            foreach (var row in rowsForSPO)
            {
                row.RowData.SPOId = existingSPOOffers.TryGetValue(spoCode, out var po) ? po.Id : (Guid?)null;
                row.RowData.CustomerId = customerInDPO?.CustomerId;
                row.RowData.CustomerName = customerInDPO?.CustomerName ?? string.Empty;
                row.RowData.CustomerTaxCode = customerInDPO?.CustomerTaxCode ?? string.Empty;
                row.RowData.CustomerType = customerInDPO?.CustomerType ?? string.Empty;
                row.RowData.CustomerIndustry = customerInDPO?.CustomerIndustry ?? string.Empty;
            }

            if (allSPOsAreAP)
            {
                continue;
            }

            var detailsForSPO = dpoDetailUsedSPOCodes
                .Where(x => x.SPOCode == spoCode)
                .ToList();

            // Get the SPO price offer
            if (!existingSPOOffers.TryGetValue(spoCode, out var priceOffer))
            {
                continue; // SPO existence already validated at detail level
            }

            var priceOfferType = priceOffer.GetPriceOfferType();
            var materialType = priceOffer.MaterialType;

            // Validate buyer matching between DPO and SPO (applies to all price offer types)
            await ValidateBuyerMatchingAsync(context, dpoResult, priceOffer);

            // Calculate total amount for this specific SPO in the current DPO
            // NOTE: Now we only consider amounts for DPO details that use this specific SPO code
            var totalUsedAmountForThisSPO = detailsForSPO.Sum(x => x.Amount ?? 0);
            var totalDPOAmountForThisSPO = rowsForSPO.Sum(x => x.RowData.Amount ?? 0);

            // Get the total amount of the SPO
            var totalSPOAmount = priceOffer.TotalMEVNOfferAmount ?? 0;

            // Validate based on SPO type and material type
            if (priceOfferType == PriceOfferTypes.PriceOfferPP) // PP type
            {
                if (materialType == "FA") // PP.FA
                {
                    await ValidatePPFABusinessRulesAsync(dpoResult, spoCode, totalDPOAmountForThisSPO, totalUsedAmountForThisSPO, totalSPOAmount);
                }
                else if (materialType == "LVS") // PP.LVS
                {
                    await ValidatePPLVSBusinessRulesAsync(dpoResult, spoCode, totalDPOAmountForThisSPO, totalUsedAmountForThisSPO, totalSPOAmount);
                }
            }
            else if (priceOfferType == PriceOfferTypes.PriceOfferDS) // DS type
            {
                ValidateDSBusinessRules(dpoResult, spoCode, totalDPOAmountForThisSPO, totalSPOAmount);
            }
        }
    }

    private async Task ValidatePPFABusinessRulesAsync(
        ExcelRowResult<ImportDPODto> dpoResult,
        string spoCode,
        decimal totalDPOAmountForSPO,
        decimal totalUsedAmountForSPO,
        decimal totalSPOAmount)
    {
        // PP.FA Rules:
        // Fisrt DPO: TotalAmountDPO >= (FA_DPO.PP.FIRST config value) * TotalAmountSPO.PP.FA
        // From Second DPO: new total dpo amount + used amount >= (FA_DPO.PP.SECOND config value) * TotalAmountSPO.PP.FA

        var firstDPOPercentage = await GetConfigurationValueAsync(SystemConfigurationKeys.DpoImportPPFAFirst, 75m);
        var secondDPOPercentage = await GetConfigurationValueAsync(SystemConfigurationKeys.DpoImportPPFASecond, 90m);

        var minimumFirstDPOAmount = totalSPOAmount * firstDPOPercentage / 100;
        var minimumSecondDPOAmount = totalSPOAmount * secondDPOPercentage / 100;

        if (totalUsedAmountForSPO == 0 && totalDPOAmountForSPO >= minimumFirstDPOAmount)
        {
            // Valid as first DPO
            return;
        }
        if (totalUsedAmountForSPO > 0 && totalDPOAmountForSPO + totalUsedAmountForSPO >= minimumSecondDPOAmount)
        {
            // Valid as second+ DPO
            return;
        }
        else
        {
            // Invalid amount - this is a DPO-level error, not detail-level
            dpoResult.Errors.Add(
                $"Invalid DPO amount for SPO '{spoCode}'. " +
                (totalUsedAmountForSPO == 0
                    ? $"As this is the first DPO, it must be at least {firstDPOPercentage:N0}% of the SPO total amount ({minimumFirstDPOAmount:N0} / {totalSPOAmount:N0}). " +
                      $"Current DPO amount: {totalDPOAmountForSPO:N0}."
                    : $"As this is the second or later DPO, the sum of current and previous DPO amounts for this SPO must be at least {secondDPOPercentage:N0}% of the SPO total amount ({minimumSecondDPOAmount:N0} / {totalSPOAmount:N0}). " +
                      $"Current DPO amount: {totalDPOAmountForSPO:N0}, previous amount: {totalUsedAmountForSPO:N0}."
                )
            );
        }
    }

    private async Task ValidatePPLVSBusinessRulesAsync(
        ExcelRowResult<ImportDPODto> dpoResult,
        string spoCode,
        decimal totalDPOAmountForSPO,
        decimal totalUsedAmountForSPO,
        decimal totalSPOAmount)
    {
        // PP.LVS Rules:
        // First DPO: TotalAmountDPO >= (DPO.Import.PP_LVS_First config value) * TotalAmountSPO.PP.LVS
        // From second DPO: no constraint

        var firstDPOPercentage = await GetConfigurationValueAsync(SystemConfigurationKeys.DpoImportPPLVSFirst, 50m);
        var minimumFirstDPOAmount = totalSPOAmount * firstDPOPercentage / 100;

        if (totalUsedAmountForSPO == 0 && totalDPOAmountForSPO >= minimumFirstDPOAmount)
        {
            // Valid as first DPO
            return;
        }
        else if (totalUsedAmountForSPO == 0)
        {
            dpoResult.Errors.Add(
                $"SPO '{spoCode}' (PP.LVS): The first DPO must be at least {firstDPOPercentage:N0}% of the total SPO amount " +
                $"({minimumFirstDPOAmount:N0} / {totalSPOAmount:N0}). Current DPO amount: {totalDPOAmountForSPO:N0}. " +
                $"This rule applies only to the first DPO."
            );
        }
    }

    private void ValidateDSBusinessRules(ExcelRowResult<ImportDPODto> dpoResult, string spoCode, decimal totalDPOAmountForSPO, decimal totalSPOAmount)
    {
        // DS Rules:
        // Bắt buộc order 100% TotalAmountSPO.DS

        if (totalDPOAmountForSPO != totalSPOAmount)
        {
            dpoResult.Errors.Add($"SPO '{spoCode}' (DS) requires ordering exactly 100% of total SPO amount. Expected: {totalSPOAmount:N0}, Current DPO amount for this SPO: {totalDPOAmountForSPO:N0}");
        }
    }

    private async Task ValidateBuyerMatchingAsync(ExcelImportContext context, ExcelRowResult<ImportDPODto> dpoResult, PriceOffer priceOffer)
    {
        // NB type SPOs don't have a buyer, so skip buyer validation
        if (!priceOffer.BuyerId.HasValue)
        {
            return;
        }

        // Get DPO buyer from context (passed during import)
        var dpoBuyerId = context.GetData<Guid>(ExcelImportContextKeys.DPO.BuyerId);

        // Check if SPO buyer matches DPO buyer
        if (priceOffer.BuyerId != dpoBuyerId)
        {
            var errorMsg = $"DPO buyer does not match SPO buyer. SPO '{priceOffer.PriceOfferCode}' belongs to a different buyer.";
            foreach (var detail in dpoResult.RowData.Details.ListData)
            {
                if (detail.RowData.SPOCode == priceOffer.PriceOfferCode)
                {
                    detail.Errors.Add(errorMsg);
                }
            }

            dpoResult.Errors.Add(errorMsg);
        }
    }

    private async Task<decimal> GetConfigurationValueAsync(string configKey, decimal defaultValue)
    {
        try
        {
            var configuration = await _systemConfigurationRepository.GetAsync(x => x.CfgKey == configKey);
            if (configuration != null && decimal.TryParse(configuration.CfgValue, out var value))
            {
                return value;
            }
            return defaultValue;
        }
        catch
        {
            return defaultValue;
        }
    }

    private bool CheckSPOCodeUniqueness(
        ExcelRowResult<ImportDPODto> dpoResult,
        List<string> uniqueSPOCodes,
        Dictionary<string, PriceOffer> existingSPOOffers)
    {
        // Filter only PP and DS type SPO codes for validation
        var ppOrDsSPOCodes = uniqueSPOCodes
            .Where(spoCode => existingSPOOffers.TryGetValue(spoCode, out var po) &&
                            (po.GetPriceOfferType() == PriceOfferTypes.PriceOfferPP ||
                             po.GetPriceOfferType() == PriceOfferTypes.PriceOfferDS))
            .ToList();

        if (ppOrDsSPOCodes.Count <= 1)
        {
            return true; // Only one or no PP/DS SPO codes, so no validation needed
        }

        // Multiple PP/DS SPO codes found - this violates the uniqueness rule
        var spoDetails = ppOrDsSPOCodes.Select(spoCode =>
        {
            var po = existingSPOOffers[spoCode];
            return $"{spoCode} ({po.GetPriceOfferType()})";
        }).ToList();

        dpoResult.Errors.Add(
            $"For SPO types PP and DS, only one SPO code can be used across all DPO Details in a single DPO import. " +
            $"Found multiple PP/DS SPO codes: {string.Join(", ", spoDetails)}. " +
            $"Please split the import into separate DPO imports, each containing only one PP/DS SPO code."
        );
        return false;
    }

    private class MaterialTypeProjection
    {
        public string GolfaCode { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string? MaterialType { get; set; }
    }

    private class PriceOfferCustomerProjection
    {
        public Guid PriceOfferId { get; set; }
        public Guid? CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerTaxCode { get; set; } = string.Empty;
        public string CustomerType { get; set; } = string.Empty;
        public string CustomerIndustry { get; set; } = string.Empty;
    }
}