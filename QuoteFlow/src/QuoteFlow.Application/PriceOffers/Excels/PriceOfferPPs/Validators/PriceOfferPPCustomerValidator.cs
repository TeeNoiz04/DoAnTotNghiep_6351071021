using QuoteFlow.Customers;
using QuoteFlow.PriceOffers.PriceOfferCustomers;
using QuoteFlow.Shared.Excels;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static QuoteFlow.Shared.Excels.ExcelImportContextKeys;

namespace QuoteFlow.PriceOffers.Excels.PriceOfferPPs.Validators;

public class PriceOfferPPCustomerValidator : BaseExcelValidator<PriceOfferCustomerImportDto>
{
    private readonly ICustomerRepository _customerRepository;

    public PriceOfferPPCustomerValidator(
        ExcelValidationConfig config,
        IExcelRowValidator<PriceOfferCustomerImportDto> rowValidator,
        ILogger<BaseExcelValidator<PriceOfferCustomerImportDto>> logger,
        ICustomerRepository customerRepository
    ) : base(config, rowValidator, logger)
    {
        _customerRepository = customerRepository;
    }

    protected override async Task<ValidationResult> PreValidateAsync(Stream stream, ExcelImportContext? context)
    {
        // Skip validation for 'Correct information confirmation' (col I) and 'Checked by MEVN Sales PIC' (col J)
        return await base.PreValidateAsync(stream, context);
    }

    protected override async Task PostValidateAsync(ExcelValidationResult<PriceOfferCustomerImportDto> result)
    {
        // Perform batch customer lookup and populate customer information
        await EnrichCustomerInformationAsync(result);

        await base.PostValidateAsync(result);
    }

    private async Task EnrichCustomerInformationAsync(ExcelValidationResult<PriceOfferCustomerImportDto> result)
    {
        // Extract unique tax codes from all customer rows
        var taxCodes = result.ListData
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

        var customerInfoLookupMap = new Dictionary<string, CustomerInfoLookup>();

        // Create map for existing customers
        foreach (var customer in existingCustomers)
        {
            customerInfoLookupMap[customer.TaxCode] = new CustomerInfoLookup
            {
                CustomerId = customer.Id,
                CustomerName = customer.CustomerName,
                CustomerType = customer.CustomerType,
                Address = customer.Address,
                Country = customer.Country,
                Industry = customer.CustomerIndustry,
            };
        }

        // Update each row with the resolved customer information
        foreach (var row in result.ListData)
        {
            if (row.RowData == null || string.IsNullOrWhiteSpace(row.RowData.CustomerTaxCode))
            {
                continue;
            }

            // If this is an existing customer, populate with database values
            if (customerInfoLookupMap.TryGetValue(row.RowData.CustomerTaxCode, out var customerInfo))
            {
                row.RowData.CustomerName = customerInfo.CustomerName;
                row.RowData.CustomerType = customerInfo.CustomerType;
                row.RowData.CustomerAddress = customerInfo.Address;
                row.RowData.CustomerNationality = customerInfo.Country;
                row.RowData.CustomerIndustry = customerInfo.Industry;
            }
            // For new customers, the user-entered values from the Excel are already in RowData
            // No action needed - they'll be displayed as-is
        }
    }
}
