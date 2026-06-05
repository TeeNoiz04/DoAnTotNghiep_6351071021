using QuoteFlow.SaleOrders.Excel;
using QuoteFlow.SaleOrdersSapImports.ParameterObjects;
using QuoteFlow.Shared.Excels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

namespace QuoteFlow.SaleOrders.GICExcel.InternalUse_Change.Converter;

public class SaleOrderGICInternalUseChangeConverter : ExcelDtoConverter<SaleOrderGICInternalUseChangeExcelDto, SaleOrderSapImportCreateParams>
{
    public SaleOrderGICInternalUseChangeConverter(IObjectMapper objectMapper, IGuidGenerator guidGenerator) : base(objectMapper, guidGenerator)
    {
    }

    protected override IEnumerable<string> RequiredValidationContextKeys => [];
    public override Task<ValidationResult> ValidateRowAsync(
        ExcelRowResult<SaleOrderGICInternalUseChangeExcelDto> rowResult,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        // Add business rule validations here
        return base.ValidateRowAsync(rowResult, context, cancellationToken);
    }

    protected override Task<SaleOrderSapImportCreateParams?> MapToCreateParamsAsync(
        SaleOrderGICInternalUseChangeExcelDto importDto,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        var createParams = ToCreateParams(importDto, context);
        return Task.FromResult<SaleOrderSapImportCreateParams?>(createParams);
    }

    private SaleOrderSapImportCreateParams ToCreateParams(SaleOrderGICInternalUseChangeExcelDto importDto, ExcelImportContext context)
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
            GICGivDate = importDto.GICGivDate,
            GICSalesPIC = importDto.GICSalesPIC,
            GICLocation = importDto.GICLocation,
            GICReservationNo = importDto.GICReservationNo,

            SOType = importDto.SOType,
            MaterialCode = importDto.MaterialCode,
            ModelName = importDto.ModelName,
            GICNo = importDto.GICNo,


        };
        return result;

    }
}
