using QuoteFlow.SaleOrders.Excel;
using QuoteFlow.SaleOrdersSapImports.ParameterObjects;
using QuoteFlow.Shared.Excels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

namespace QuoteFlow.SaleOrders.GICExcel.Warranty.Converter;

public class SaleOrderGICWrrantyConverter : ExcelDtoConverter<SaleOrderGICWarrantyExcelDto, SaleOrderSapImportCreateParams>
{
    public SaleOrderGICWrrantyConverter(IObjectMapper objectMapper, IGuidGenerator guidGenerator) : base(objectMapper, guidGenerator)
    {
    }

    protected override IEnumerable<string> RequiredValidationContextKeys => [];
    public override Task<ValidationResult> ValidateRowAsync(
        ExcelRowResult<SaleOrderGICWarrantyExcelDto> rowResult,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        // Add business rule validations here
        return base.ValidateRowAsync(rowResult, context, cancellationToken);
    }

    protected override Task<SaleOrderSapImportCreateParams?> MapToCreateParamsAsync(
        SaleOrderGICWarrantyExcelDto importDto,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        var createParams = ToCreateParams(importDto, context);
        return Task.FromResult<SaleOrderSapImportCreateParams?>(createParams);
    }

    private SaleOrderSapImportCreateParams ToCreateParams(SaleOrderGICWarrantyExcelDto importDto, ExcelImportContext context)
    {
        var result = new SaleOrderSapImportCreateParams
        {
            SONo = importDto.SONo,
            //DONo = importDto.SAPDONo,
            //DODate
            //DONote
            SOSAPNo = importDto.SAPSONo,
            Note = importDto.Note,
            InvoiceDate = importDto.InvoiceDate,
            InvoiceNo = importDto.InvoiceNo,
            GICLandingCost = importDto.GICSAPLandingCost,
            GICAmountLandingCost = importDto.GICAmountSAPLandingCost,
            GICPORNo = importDto.GICPORNo,
            GICPRNo = importDto.GICPRNo,
            GICGivNo = importDto.GICGivNo,
            GICGivDate = importDto.GICGivDate,
            SOType = importDto.SOType,
            MaterialCode = importDto.MaterialCode,
            ModelName = importDto.ModelName,
            GICNo = importDto.GICNo,
            BillingNo = importDto.BillingNo,
            DOSAPNo = importDto.DONo,

        };
        return result;

    }
}
