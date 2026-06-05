using QuoteFlow.Customers;
using QuoteFlow.Customers.ParameterObjects;
using QuoteFlow.PriceOffers.ParameterObjects;
using QuoteFlow.PriceOffers.PriceOfferCustomers;
using QuoteFlow.PriceOffers.PriceOfferCustomers.ParameterObject;
using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.PriceOffers.PriceOfferDetails.ParameterObjects;
using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;
using static QuoteFlow.Shared.Excels.ExcelImportContextKeys;

namespace QuoteFlow.PriceOffers.Excels.PriceOfferPPs.Converters;

public class PriceOfferPPExcelDtoConverter : ExcelDtoConverter<PriceOfferImportDto, PriceOfferCreateParams>
{
    private readonly PriceOfferManager _priceOfferManager;
    private readonly IExcelImportFactory _excelImportFactory;
    private readonly ICustomerRepository _customerRepository;
    private readonly CustomerManager _customerManager;

    public PriceOfferPPExcelDtoConverter(
        IObjectMapper objectMapper,
        IGuidGenerator guidGenerator,
        PriceOfferManager priceOfferManager,
        IExcelImportFactory excelImportFactory,
        ICustomerRepository customerRepository,
        CustomerManager customerManager) : base(objectMapper, guidGenerator)
    {
        _priceOfferManager = priceOfferManager;
        _excelImportFactory = excelImportFactory;
        _customerRepository = customerRepository;
        _customerManager = customerManager;
    }

    protected override IEnumerable<string> RequiredValidationContextKeys => [];

    protected override async Task<PriceOfferCreateParams?> MapToCreateParamsAsync(PriceOfferImportDto importDto, ExcelImportContext context, CancellationToken cancellationToken)
    {
        var customers = new List<PriceOfferCustomerCreateParams>();
        var details = new List<PriceOfferDetailCreateParams>();
        var newPriceOfferId = _guidGenerator.Create();
        context.SetData(ExcelImportContextKeys.ParentEntityId, newPriceOfferId);

        // Batch process customers - resolve all customer lookups at once
        if (importDto.Customers is not null && importDto.Customers.IsValid)
        {
            await ResolveBatchCustomersAsync(importDto.Customers.ListData, context, cancellationToken);

            var customerConverter = _excelImportFactory.CreateCreateParamsConverter<PriceOfferCustomerImportDto, PriceOfferCustomerCreateParams>(ExcelImporters.PriceOfferPPCustomer);
            foreach (var customerImportDto in importDto.Customers.ListData)
            {
                var customerCreateParams = await customerConverter.ConvertToCreateParamsAsync(customerImportDto, context, cancellationToken);
                if (customerCreateParams != null)
                {
                    customers.Add(customerCreateParams);
                }
            }
        }

        if (importDto.Details is not null && importDto.Details.IsValid)
        {
            var importGuid = Guid.NewGuid();
            var detailConverter = _excelImportFactory.CreateCreateParamsConverter<PriceOfferDetailImportDto, PriceOfferDetailCreateParams>(ExcelImporters.PriceOfferPPDetail);
            context.SetData(ExcelImportContextKeys.PriceOfferDetail.ImportGuid, importGuid);
            foreach (var detailImportDto in importDto.Details.ListData)
            {
                var detailCreateParams = await detailConverter.ConvertToCreateParamsAsync(detailImportDto, context, cancellationToken);
                if (detailCreateParams != null)
                {
                    details.Add(detailCreateParams);
                }
            }
        }

        var createParams = await ToCreateParams(importDto, context, customers, details);
        return createParams;
    }

    private async Task ResolveBatchCustomersAsync(IEnumerable<ExcelRowResult<PriceOfferCustomerImportDto>> customerRows, ExcelImportContext context, CancellationToken cancellationToken)
    {
        // Extract unique tax codes from all customer rows
        var taxCodes = customerRows
            .Where(row => !string.IsNullOrWhiteSpace(row.RowData?.CustomerTaxCode))
            .Select(row => row.RowData!.CustomerTaxCode!)
            .Distinct()
            .ToList();

        if (!taxCodes.Any())
        {
            context.SetData(ExcelImportContextKeys.PriceOfferCustomer.CustomerIdLookupMap, new Dictionary<string, Guid>());
            context.SetData(ExcelImportContextKeys.PriceOfferCustomer.CustomerInfoLookupMap, new Dictionary<string, CustomerInfoLookup>());
            return;
        }

        // Batch lookup existing customers
        var existingCustomers = await _customerRepository.GetListAsync(
            x => taxCodes.Contains(x.TaxCode),
            includeDetails: false,
            cancellationToken: cancellationToken);

        var customerLookupMap = new Dictionary<string, Guid>();
        var customerInfoLookupMap = new Dictionary<string, CustomerInfoLookup>();

        // Create map for existing customers
        foreach (var customer in existingCustomers)
        {
            customerLookupMap[customer.TaxCode] = customer.Id;
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

        // Create new customers for missing tax codes
        var existingTaxCodes = existingCustomers.Select(c => c.TaxCode).ToHashSet();
        var missingTaxCodes = taxCodes.Where(tc => !existingTaxCodes.Contains(tc)).ToList();

        foreach (var taxCode in missingTaxCodes)
        {
            // Find the first row with this tax code to get customer details
            var customerRow = customerRows.FirstOrDefault(row => row.RowData?.CustomerTaxCode == taxCode);
            if (customerRow?.RowData != null)
            {
                var customerCreateParams = new CustomerCreateParams
                {
                    TaxCode = customerRow.RowData.CustomerTaxCode!,
                    CustomerName = customerRow.RowData.CustomerName!,
                    Address = customerRow.RowData.CustomerAddress,
                    Country = customerRow.RowData.CustomerNationality,
                    CustomerType = customerRow.RowData.CustomerType,
                    CustomerIndustry = customerRow.RowData.CustomerIndustry,
                };

                var newCustomer = await _customerManager.CreateAsync(customerCreateParams);
                customerLookupMap[taxCode] = newCustomer.Id;
                // For new customers, no need to add to customerInfoLookupMap - we use user-entered values
            }
        }

        // Store the lookup maps in context for use by individual customer converters
        context.SetData(ExcelImportContextKeys.PriceOfferCustomer.CustomerIdLookupMap, customerLookupMap);
        context.SetData(ExcelImportContextKeys.PriceOfferCustomer.CustomerInfoLookupMap, customerInfoLookupMap);
    }

    private async Task<PriceOfferCreateParams> ToCreateParams(
        PriceOfferImportDto importDto,
        ExcelImportContext context,
        ICollection<PriceOfferCustomerCreateParams> customers,
        ICollection<PriceOfferDetailCreateParams> details)
    {
        var priceOfferCode = await _priceOfferManager.GenerateNewCodeAsync(PriceOfferConsts.ProjectPrefix, importDto.MaterialType!);

        return new PriceOfferCreateParams(
            priceOfferCode,
            importDto.BuyerId,
            importDto.BuyerTypeId,
            importDto.MaterialType!,
            importDto.LocationId,
            importDto.ProjectName,
            importDto.ProjectTypeId,
            importDto.EUIndustryId,
            importDto.Application,
            importDto.Country,
            importDto.Province,
            importDto.DetailedAddress,
            importDto.CompetitorBrand,
            importDto.PriceGapWithCompetitor,
            importDto.DecisionRight,
            importDto.POPlannedDate,
            importDto.DeliveryDate,
            importDto.UpcomingPotentialProjects,
            importDto.OtherPJInformation,
            importDto.FileName,
            importDto.TotalMEVNOfferAmount,
            importDto.AccountNo,
            customers,
            details,
            importDto.Note,
            importDto.CloseDate
        )
        {
            Id = context.GetData<Guid>(ExcelImportContextKeys.ParentEntityId),
        };
    }
}
