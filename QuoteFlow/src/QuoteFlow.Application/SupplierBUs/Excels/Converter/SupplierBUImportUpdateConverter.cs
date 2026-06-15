using QuoteFlow.Shared.Excels;
using QuoteFlow.SupplierBUs.ParameterObjects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

namespace QuoteFlow.SupplierBUs.Excels.Converter;
public class SupplierBUImportUpdateConverter : ExcelDtoConverter<SupplierBUImportDto, SupplierBUImportUpdateParams>
{
    public SupplierBUImportUpdateConverter(IObjectMapper objectMapper, IGuidGenerator guidGenerator) : base(objectMapper, guidGenerator)
    {
    }

    protected override IEnumerable<string> RequiredValidationContextKeys => [];

    protected override async Task<SupplierBUImportUpdateParams> MapToCreateParamsAsync(
         SupplierBUImportDto importDto,
         ExcelImportContext context,
         CancellationToken cancellationToken = default)
    {
        var createParams = ToCreateParams(importDto, context);
        return createParams;
    }

    private SupplierBUImportUpdateParams ToCreateParams(SupplierBUImportDto importDto, ExcelImportContext context)
    {
        var result = new SupplierBUImportUpdateParams
        {
            Id = importDto.IdUpdate!.Value,
            SupplierBUCode = importDto.SupplierBU ?? string.Empty,
            SupplierBURemarks = importDto.SupplierBURemarks,
            OrderMethod = importDto.OrderMethod,
            POTemplate = importDto.POTemplate,
            Contact = importDto.Contact,
            Email = importDto.Email,
            INCOTerm = importDto.IncoTerm,
            PaymentTermCode = importDto.PaymentTermCode,
            PaymentDescription = importDto.PaymentDescription,
            Currency = importDto.Currency,
            MaterialType = importDto.MaterialType,
            SupplierId = importDto.SupplierID,
            SupplierCode = importDto.SupplierCode,
            SupplierShortName = importDto.SupplierCode,
            SupplierAddress = importDto.SupplierAddress,
            SortOrder = 0, // Assuming No is int from Excel
            FASCMVendorCode = importDto.FASCMVendorCode,
            FASCMBuyerCode = importDto.FASCMBuyerCode,
            FASCMConsigneeCode = importDto.FASCMConsigneeCode,
            FASCMSectionCode = importDto.FASCMSectionCode,
            FASCMPaymentTerm = importDto.FASCMPaymentTerm,
            FASCMFreightMethod = importDto.FASCMFreightMethod,
            FASCMDeliveryTerms = importDto.FASCMDeliveryTerms,
            FASCMPlaceOfDeliveryTerms = importDto.FASCMPlaceOfDeliveryTerms,
            FASCMShippingMarkCode = importDto.FASCMShippingMarkCode,
            ConcurrencyStamp = importDto.ConcurrencyStamp,
        };
        return result;
    }
}
