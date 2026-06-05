using QuoteFlow.DPOs.DPODetails;
using QuoteFlow.DPOs.DPODetails.ParameterObjects;
using QuoteFlow.Shared.Excels;
using QuoteFlow.SpecialInputPrices;
using QuoteFlow.SpecialInputPrices.SpecialInputPriceDetails;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;
using static QuoteFlow.Shared.Excels.ExcelImportContextKeys;

namespace QuoteFlow.DPOs.Excels.ImportDPO.Converters;

public class ImportDPODetailExcelDtoConverter : ExcelDtoConverter<ImportDPODetailDto, DPODetailCreateParams>
{
    public ISpecialInputPriceDetailRepository _specialInputPriceDetailRepository;
    public ISpecialInputPriceRepository _specialInputPriceRepository;
    public ImportDPODetailExcelDtoConverter(
        IObjectMapper objectMapper,
        IGuidGenerator guidGenerator,
        ISpecialInputPriceDetailRepository specialInputPriceDetailRepository,
        ISpecialInputPriceRepository specialInputPriceRepository) : base(objectMapper, guidGenerator)
    {
        _specialInputPriceDetailRepository = specialInputPriceDetailRepository;
        _specialInputPriceRepository = specialInputPriceRepository;
    }

    protected override IEnumerable<string> RequiredValidationContextKeys => [
        ExcelImportContextKeys.ParentEntityId,
        ExcelImportContextKeys.DPO.SPOCodeAccountNoMap
    ];

    protected override async Task<DPODetailCreateParams?> MapToCreateParamsAsync(
        ImportDPODetailDto importDto,
        ExcelImportContext context,
        CancellationToken cancellationToken)
    {
        var createParams = await ToCreateParams(importDto, context);
        return createParams;
    }

    private async Task<DPODetailCreateParams> ToCreateParams(ImportDPODetailDto importDto, ExcelImportContext context)
    {
        var spoCodeAccountNoMap = context.GetData<Dictionary<string, string?>>(ExcelImportContextKeys.DPO.SPOCodeAccountNoMap);
        string? accountNo = null;
        if (importDto.SPOCode is not null && spoCodeAccountNoMap!.TryGetValue(importDto.SPOCode, out accountNo))
        {
            if (string.IsNullOrWhiteSpace(accountNo))
            {
                accountNo = null; // Ensure accountNo is null if empty
            }
            else
            {
                var specialInputPrice = await _specialInputPriceRepository.FirstOrDefaultAsync(x => x.AccountNo == accountNo);
                var exitsGolfaCodeOfAccountNo = await _specialInputPriceDetailRepository.FirstOrDefaultAsync(x => x.MaterialCode == importDto.GolfaCode && x.SpecialInputPriceId == specialInputPrice.Id);

                if (exitsGolfaCodeOfAccountNo == null)
                {
                    accountNo = null;
                }
            }
        }

        // Get customer info from pre-resolved customer lookup maps
        Guid? customerId = importDto.CustomerId;
        string? customerName = importDto.CustomerName;
        string? customerType = importDto.CustomerType;
        string? customerIndustry = importDto.CustomerIndustry;

        // If row has SPO code, use customer info already populated by validation phase
        if (!string.IsNullOrWhiteSpace(importDto.SPOCode))
        {
        }
        // Otherwise, use existing customer lookup logic for non-SPO rows
        else if (!string.IsNullOrWhiteSpace(importDto.CustomerTaxCode))
        {
            // First check if this is an existing customer (has full info in CustomerInfoLookupMap)
            var customerInfoLookupMap = context.GetData<Dictionary<string, CustomerInfoLookup>>(ExcelImportContextKeys.PriceOfferCustomer.CustomerInfoLookupMap);
            if (customerInfoLookupMap != null && customerInfoLookupMap.TryGetValue(importDto.CustomerTaxCode!, out var existingCustomerInfo))
            {
                // Existing customer - use customer info from database, ignore user-entered values
                customerId = existingCustomerInfo.CustomerId;
                customerName = existingCustomerInfo.CustomerName;
                customerType = existingCustomerInfo.CustomerType;
                customerIndustry = existingCustomerInfo.Industry;
            }
            else
            {
                // New customer - use customer ID from lookup map, keep user-entered CustomerName, CustomerType, and CustomerIndustry
                var customerLookupMap = context.GetData<Dictionary<string, Guid>>(ExcelImportContextKeys.PriceOfferCustomer.CustomerIdLookupMap);
                if (customerLookupMap != null && customerLookupMap.TryGetValue(importDto.CustomerTaxCode!, out var resolvedCustomerId))
                {
                    customerId = resolvedCustomerId;
                }
            }
        }

        return new DPODetailCreateParams
        {
            RowNo = importDto.RowNo,
            GolfaCode = importDto.GolfaCode!,
            Model = importDto.Model,
            Spec1 = importDto.Spec1,
            Spec2 = importDto.Spec2,
            SPOId = importDto.SPOId,
            SPOCode = importDto.SPOCode,
            Qty = importDto.Qty,
            NeedDelivery = importDto.Qty,
            UnitPrice = importDto.UnitPrice,
            Amount = importDto.Amount,
            RequestedETA = importDto.RequestedETA,
            CustomerId = customerId,
            CustomerName = customerName,
            CustomerType = customerType,
            CustomerTaxCode = importDto.CustomerTaxCode,
            CustomerIndustry = customerIndustry,
            Note = importDto.Note,
            DPOId = context.GetData<Guid>(ExcelImportContextKeys.ParentEntityId),
            AccountNo = accountNo
        };
    }
}