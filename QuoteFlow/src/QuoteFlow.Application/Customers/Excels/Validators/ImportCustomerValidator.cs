using QuoteFlow.Shared.Excels;
using QuoteFlow.Shared.Helpers;
using QuoteFlow.Shared.Utils;
using QuoteFlow.SystemCategories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteFlow.Customers.Excels.Validators;
public class ImportCustomerValidator : BaseExcelValidator<CustomerImportDto>
{
    protected readonly IServiceProvider _provider;
    protected readonly ICustomerRepository _customerRepository;
    protected readonly ISystemCategoryRepository _systemCategoryRepository;

    public ImportCustomerValidator(
        ExcelValidationConfig config,
        IExcelRowValidator<CustomerImportDto> rowValidator,
        ILogger<BaseExcelValidator<CustomerImportDto>> logger,
        IServiceProvider provider)
        : base(config, rowValidator, logger)
    {
        _provider = provider;
        // Inject the Customer repository using the provider
        _customerRepository = _provider.GetRequiredService<ICustomerRepository>();
        _systemCategoryRepository = _provider.GetRequiredService<ISystemCategoryRepository>();
    }


    protected override async Task PostValidateAsync(ExcelValidationResult<CustomerImportDto> result, ExcelImportContext? context = null)
    {
        var customers = await _customerRepository.GetListAsync();

        // Fetch all required system categories in a single query for performance
        var categoryTypes = new[] {
            CategoryTypes.Nationality,
            CategoryTypes.CustomerType,
            CategoryTypes.EUIndustry
        };

        var systemCategories = await _systemCategoryRepository.GetListAsync(
            x => categoryTypes.Contains(x.CategoryType) && !x.IsDeactive
        );

        // Group categories by type for easy lookup
        var nationalityCategories = systemCategories.Where(x => x.CategoryType == CategoryTypes.Nationality).ToList();
        var customerTypeCategories = systemCategories.Where(x => x.CategoryType == CategoryTypes.CustomerType).ToList();
        var industryCategories = systemCategories.Where(x => x.CategoryType == CategoryTypes.EUIndustry).ToList();

        ValidateDuplicatedRows(result);

        foreach (var customerRow in result.ListData)
        {
            var importTaxCode = customerRow.RowData.TaxCode;
            var country = customerRow.RowData.Country;
            var customerType = customerRow.RowData.CustomerType;
            var customerIndustry = customerRow.RowData.CustomerIndustry;

            // Validation A: Country Master Data
            if (!string.IsNullOrWhiteSpace(country))
            {
                var matchedCountry = nationalityCategories.FirstOrDefault(c =>
                    c.Description.Equals(country, StringComparison.OrdinalIgnoreCase)
                );

                if (matchedCountry == null)
                {
                    customerRow.Errors.Add($"Country '{country}' is not valid. Please select from the available countries.");
                }
                else
                {
                    // Normalize the case to match master data
                    customerRow.RowData.Country = matchedCountry.Description;
                }
            }

            // Validation B: TaxCode Format Validation
            if (!string.IsNullOrWhiteSpace(importTaxCode))
            {
                var taxCodeError = CodeHelper.ValidateTaxCode(importTaxCode);
                if (taxCodeError != null)
                {
                    customerRow.Errors.Add(taxCodeError);
                }
            }

            // Validation C: CustomerType Master Data
            if (!string.IsNullOrWhiteSpace(customerType))
            {
                var matchedCustomerType = customerTypeCategories.FirstOrDefault(ct =>
                    ct.Description.Equals(customerType, StringComparison.OrdinalIgnoreCase)
                );

                if (matchedCustomerType == null)
                {
                    customerRow.Errors.Add($"Customer Type '{customerType}' is not valid. Please select from the available customer types.");
                }
                else
                {
                    // Normalize the case to match master data
                    customerRow.RowData.CustomerType = matchedCustomerType.Description;
                }
            }

            // Validation D: CustomerIndustry Master Data
            if (!string.IsNullOrWhiteSpace(customerIndustry))
            {
                var matchedIndustry = industryCategories.FirstOrDefault(ind =>
                    ind.Description.Equals(customerIndustry, StringComparison.OrdinalIgnoreCase)
                );

                if (matchedIndustry == null)
                {
                    customerRow.Errors.Add($"Customer Industry '{customerIndustry}' is not valid. Please select from the available industries.");
                }
                else
                {
                    // Normalize the case to match master data
                    customerRow.RowData.CustomerIndustry = matchedIndustry.Description;
                }
            }

            // Check if customer already exists for update/insert logic
            if (!string.IsNullOrWhiteSpace(importTaxCode))
            {
                var existCustomer = customers.FirstOrDefault(x => x.TaxCode == importTaxCode);

                if (existCustomer is not null)
                {
                    customerRow.RowData.IsUpdate = true;
                    customerRow.RowData.IdUpdate = existCustomer.Id;
                    customerRow.RowData.ConcurrencyStamp = existCustomer.ConcurrencyStamp;
                }
                else
                {
                    customerRow.RowData.IsUpdate = false;
                    customerRow.RowData.IdUpdate = null;
                }
            }

            // CRITICAL: Propagate row errors to main result
            if (customerRow.HasErrors)
            {
                ExcelUtils.AddRowErrors(result, customerRow.RowIndex, customerRow.Errors, "Customer");
            }
        }
    }

    private void ValidateDuplicatedRows(ExcelValidationResult<CustomerImportDto> result)
    {
        var taxCodeGroups = result.ListData
            .Where(r => !string.IsNullOrWhiteSpace(r.RowData.TaxCode))
            .GroupBy(r => r.RowData.TaxCode);

        foreach (var group in taxCodeGroups)
        {
            if (group.Count() > 1)
            {
                var duplicatedRowIndexes = group.Select(r => r.RowIndex).ToList();
                foreach (var rowIndex in duplicatedRowIndexes)
                {
                    var errorMessage = $"Duplicate TaxCode '{group.Key}' found in rows: {string.Join(", ", duplicatedRowIndexes)}.";
                    var rowResult = result.ListData.First(r => r.RowIndex == rowIndex);
                    rowResult.Errors.Add(errorMessage);

                    ExcelUtils.AddRowErrors(result, rowIndex, [errorMessage], "Customer");
                }
            }
        }
    }
}