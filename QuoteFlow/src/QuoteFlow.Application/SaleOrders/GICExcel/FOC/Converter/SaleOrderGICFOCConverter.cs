using QuoteFlow.SaleOrders.Excel;
using QuoteFlow.SaleOrdersSapImports.ParameterObjects;
using QuoteFlow.Shared.Excels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

namespace QuoteFlow.SaleOrders.GICExcel.FOC.Converter;

public class SaleOrderGICFOCConverter : ExcelDtoConverter<SaleOrderGICFOCExcelDto, SaleOrderSapImportCreateParams>
{
    public SaleOrderGICFOCConverter(IObjectMapper objectMapper, IGuidGenerator guidGenerator) : base(objectMapper, guidGenerator)
    {
    }

    protected override IEnumerable<string> RequiredValidationContextKeys => [];
    public override Task<ValidationResult> ValidateRowAsync(
        ExcelRowResult<SaleOrderGICFOCExcelDto> rowResult,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        // Add business rule validations here
        return base.ValidateRowAsync(rowResult, context, cancellationToken);
    }

    protected override Task<SaleOrderSapImportCreateParams?> MapToCreateParamsAsync(
        SaleOrderGICFOCExcelDto importDto,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        var createParams = ToCreateParams(importDto, context);
        return Task.FromResult<SaleOrderSapImportCreateParams?>(createParams);
    }

    private SaleOrderSapImportCreateParams ToCreateParams(SaleOrderGICFOCExcelDto importDto, ExcelImportContext context)
    {
        var result = new SaleOrderSapImportCreateParams
        {
            SONo = importDto.SONo,
            //DONo = importDto.SAPDONo,
            //DODate
            //DONote

            Note = importDto.Note,

            GICLandingCost = importDto.GICSAPLandingCost,
            GICAmountLandingCost = importDto.GICAmountSAPLandingCost,
            GICPORNo = importDto.GICPORNo,
            GICPRNo = importDto.GICPRNo,
            GICGivNo = importDto.GICGivNo,
            SOSAPNo = importDto.SAPSONo,
            InvoiceDate = importDto.InvoiceDate,
            InvoiceNo = importDto.InvoiceNo,
            SOType = importDto.SOType,
            MaterialCode = importDto.MaterialCode,
            ModelName = importDto.ModelName,
            GICNo = importDto.GICNo,
            DOSAPNo = importDto.DONo,
            BillingNo = importDto.BillingNo


        };
        return result;

    }
}
