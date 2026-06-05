using QuoteFlow.Customers;
using QuoteFlow.Customers.ParameterObjects;
using QuoteFlow.DPOs.DPODetails;
using QuoteFlow.DPOs.DPODetails.ParameterObjects;
using QuoteFlow.DPOs.ParameterObjects;
using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;
using static QuoteFlow.Shared.Excels.ExcelImportContextKeys;

namespace QuoteFlow.DPOs.Excels.ImportDPO.Converters;

public class ImportDPOExcelDtoConverter : ExcelDtoConverter<ImportDPODto, DPOCreateParams>
{
    private readonly IExcelImportFactory _excelImportFactory;
    private readonly ICustomerRepository _customerRepository;
    private readonly CustomerManager _customerManager;

    public ImportDPOExcelDtoConverter(
        IObjectMapper objectMapper,
        IGuidGenerator guidGenerator,
        IExcelImportFactory excelImportFactory,
        ICustomerRepository customerRepository,
        CustomerManager customerManager) : base(objectMapper, guidGenerator)
    {
        _excelImportFactory = excelImportFactory;
        _customerRepository = customerRepository;
        _customerManager = customerManager;
    }

    protected override IEnumerable<string> RequiredValidationContextKeys => [];

    protected override async Task<DPOCreateParams?> MapToCreateParamsAsync(
        ImportDPODto importDto,
        ExcelImportContext context,
        CancellationToken cancellationToken)
    {
        var details = new List<DPODetailCreateParams>();
        var newDPOId = _guidGenerator.Create();
        context.SetData(ExcelImportContextKeys.ParentEntityId, newDPOId);

        if (importDto.Details is not null && importDto.Details.IsValid)
        {
            // Batch process customers - resolve all customer lookups at once
            await ResolveBatchCustomersAsync(importDto.Details.ListData, context, cancellationToken);

            var detailConverter = _excelImportFactory.CreateCreateParamsConverter<ImportDPODetailDto, DPODetailCreateParams>(ExcelImporters.DPODetail);
            foreach (var detailImportDto in importDto.Details.ListData)
            {
                var detailCreateParams = await detailConverter.ConvertToCreateParamsAsync(detailImportDto, context, cancellationToken);
                if (detailCreateParams != null)
                {
                    details.Add(detailCreateParams);
                }
            }
        }

        var createParams = ToCreateParams(importDto, context, details);
        return createParams;
    }

    private async Task ResolveBatchCustomersAsync(IEnumerable<ExcelRowResult<ImportDPODetailDto>> detailRows, ExcelImportContext context, CancellationToken cancellationToken)
    {
        // Only process rows WITHOUT SPO codes - rows with SPO codes get customer info from validation phase
        var nonSPORows = detailRows.Where(row => string.IsNullOrWhiteSpace(row.RowData?.SPOCode));

        // Extract unique tax codes from non-SPO detail rows that have customer info
        var taxCodes = nonSPORows
            .Where(row => !string.IsNullOrWhiteSpace(row.RowData?.CustomerTaxCode))
            .Select(row => row.RowData!.CustomerTaxCode!)
            .Distinct()
            .ToList();

        if (!taxCodes.Any())
        {
            context.SetData(ExcelImportContextKeys.PriceOfferCustomer.CustomerIdLookupMap, new Dictionary<string, Guid>());
            context.SetData(PriceOfferCustomer.CustomerInfoLookupMap, new Dictionary<string, CustomerInfoLookup>());
            return;
        }

        // Batch lookup existing customers
        var existingCustomers = await _customerRepository.GetListAsync(
            x => taxCodes.Contains(x.TaxCode),
            includeDetails: false,
            cancellationToken: cancellationToken);

        var customerIdLookupMap = new Dictionary<string, Guid>();
        var customerInfoLookupMap = new Dictionary<string, CustomerInfoLookup>();

        // Create map for existing customers
        foreach (var customer in existingCustomers)
        {
            customerIdLookupMap[customer.TaxCode] = customer.Id;
            customerInfoLookupMap[customer.TaxCode] = new CustomerInfoLookup
            {
                CustomerId = customer.Id,
                CustomerName = customer.CustomerName,
                CustomerType = customer.CustomerType,
                Industry = customer.CustomerIndustry
            };
        }

        // Create new customers for missing tax codes
        var existingTaxCodes = existingCustomers.Select(c => c.TaxCode).ToHashSet();
        var missingTaxCodes = taxCodes.Where(tc => !existingTaxCodes.Contains(tc)).ToList();

        foreach (var taxCode in missingTaxCodes)
        {
            // Find the first non-SPO row with this tax code to get customer details
            var detailRow = nonSPORows.FirstOrDefault(row => row.RowData?.CustomerTaxCode == taxCode);
            if (detailRow?.RowData != null)
            {
                var customerCreateParams = new CustomerCreateParams
                {
                    TaxCode = detailRow.RowData.CustomerTaxCode!,
                    CustomerName = detailRow.RowData.CustomerName!,
                    Address = null, // DPO doesn't have customer address
                    CustomerType = detailRow.RowData.CustomerType // Using CustomerType as Country for now
                };

                var newCustomer = await _customerManager.CreateAsync(customerCreateParams);
                customerIdLookupMap[taxCode] = newCustomer.Id;
                // For new customers, no need to add to customerInfoLookupMap - we use user-entered values
            }
        }

        // Store the lookup maps in context for use by individual detail converters
        context.SetData(PriceOfferCustomer.CustomerIdLookupMap, customerIdLookupMap);
        context.SetData(PriceOfferCustomer.CustomerInfoLookupMap, customerInfoLookupMap);
    }

    private DPOCreateParams ToCreateParams(ImportDPODto importDto, ExcelImportContext context, ICollection<DPODetailCreateParams> details)
    {
        return new DPOCreateParams
        {
            Id = context.GetData<Guid>(ExcelImportContextKeys.ParentEntityId),
            DPONo = importDto.DPONo,
            MaterialType = importDto.MaterialType,
            Remark = importDto.Remark,
            FileName = importDto.FileName,
            BuyerId = importDto.BuyerId,
            BuyerTypeId = importDto.BuyerTypeId,
            OrderDate = importDto.ConfirmDate,
            Details = details,
            TotalAmount = details.Sum(d => d.Amount) ?? 0
        };
    }
}