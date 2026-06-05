using QuoteFlow.PriceOffers.PriceOfferCustomers;
using QuoteFlow.PriceOffers.PriceOfferCustomers.ParameterObject;
using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;
using static QuoteFlow.Shared.Excels.ExcelImportContextKeys;

namespace QuoteFlow.PriceOffers.Excels.PriceOfferPPs.Converters;

public class PriceOfferPPCustomerExcelDtoConverter : ExcelDtoConverter<PriceOfferCustomerImportDto, PriceOfferCustomerCreateParams>
{
    public PriceOfferPPCustomerExcelDtoConverter(
        IObjectMapper objectMapper,
        IGuidGenerator guidGenerator) : base(objectMapper, guidGenerator)
    {
    }

    protected override IEnumerable<string> RequiredValidationContextKeys => [
        ExcelImportContextKeys.ParentEntityId,
        ExcelImportContextKeys.PriceOfferCustomer.CustomerIdLookupMap
    ];

    protected override async Task<PriceOfferCustomerCreateParams?> MapToCreateParamsAsync(PriceOfferCustomerImportDto importDto, ExcelImportContext context, CancellationToken cancellationToken)
    {
        // Get customer info from pre-resolved customer lookup maps
        Guid customerId;
        CustomerInfoLookup existingCustomerInfo;

        if (!string.IsNullOrWhiteSpace(importDto.CustomerTaxCode))
        {
            // First check if this is an existing customer (has full info in CustomerInfoLookupMap)
            var customerInfoLookupMap = context.GetData<Dictionary<string, CustomerInfoLookup>>(ExcelImportContextKeys.PriceOfferCustomer.CustomerInfoLookupMap);
            if (customerInfoLookupMap != null && customerInfoLookupMap.TryGetValue(importDto.CustomerTaxCode!, out existingCustomerInfo))
            {
                // Existing customer - use customer info from database, ignore user-entered values
                customerId = existingCustomerInfo.CustomerId;
            }
            else
            {
                // New customer - use customer ID from lookup map, keep user-entered CustomerName and CustomerType
                var customerIdLookupMap = context.GetData<Dictionary<string, Guid>>(ExcelImportContextKeys.PriceOfferCustomer.CustomerIdLookupMap);
                if (customerIdLookupMap != null && customerIdLookupMap.TryGetValue(importDto.CustomerTaxCode!, out customerId))
                {
                    existingCustomerInfo = new CustomerInfoLookup
                    {
                        CustomerId = customerId,
                        CustomerName = importDto.CustomerName,
                        CustomerType = importDto.CustomerType,
                        Address = importDto.CustomerAddress,
                        Country = importDto.CustomerNationality,
                        Industry = importDto.CustomerIndustry
                    };
                }
                else
                {
                    // Fallback - should not happen if batching is working correctly
                    throw new InvalidOperationException($"Customer with TaxCode '{importDto.CustomerTaxCode}' was not found in the pre-resolved customer lookup map.");
                }
            }
        }
        else
        {
            throw new UserFriendlyException("CustomerTaxCode is required to resolve CustomerId.");
        }

        context.SetData(ExcelImportContextKeys.PriceOfferCustomer.CustomerId, customerId);

        var createParams = ToCreateParams(importDto, context, existingCustomerInfo);
        return createParams;
    }

    private PriceOfferCustomerCreateParams ToCreateParams(PriceOfferCustomerImportDto importDto, ExcelImportContext context, CustomerInfoLookup customerInfo)
    {
        var priceOfferId = context.GetData<Guid>(ExcelImportContextKeys.ParentEntityId);
        var customerId = context.GetData<Guid>(ExcelImportContextKeys.PriceOfferCustomer.CustomerId);
        return new PriceOfferCustomerCreateParams(
            priceOfferId,
            importDto.SaleChannel!,
            customerId,
            importDto.CustomerTaxCode!,
            customerInfo.CustomerName,
            customerInfo.Address,
            customerInfo.Country,
            customerInfo.CustomerType,
            customerInfo.Industry
        );
    }
}
