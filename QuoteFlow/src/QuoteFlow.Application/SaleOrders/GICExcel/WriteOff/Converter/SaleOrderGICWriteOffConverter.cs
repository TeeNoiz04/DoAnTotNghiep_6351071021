using QuoteFlow.SaleOrders.Excel;
using QuoteFlow.SaleOrdersSapImports.ParameterObjects;
using QuoteFlow.Shared.Excels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

namespace QuoteFlow.SaleOrders.GICExcel.WriteOff.Converter;

public class SaleOrderGICWriteOffConverter : ExcelDtoConverter<SaleOrderGICWriteOffExcelDto, SaleOrderSapImportCreateParams>
{
    public SaleOrderGICWriteOffConverter(IObjectMapper objectMapper, IGuidGenerator guidGenerator)
        : base(objectMapper, guidGenerator)
    {
    }

    protected override IEnumerable<string> RequiredValidationContextKeys => [];

    public override Task<ValidationResult> ValidateRowAsync(
        ExcelRowResult<SaleOrderGICWriteOffExcelDto> rowResult,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        // Add business rule validations here if needed
        return base.ValidateRowAsync(rowResult, context, cancellationToken);
    }

    protected override Task<SaleOrderSapImportCreateParams?> MapToCreateParamsAsync(
        SaleOrderGICWriteOffExcelDto importDto,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        var createParams = ToCreateParams(importDto, context);
        return Task.FromResult<SaleOrderSapImportCreateParams?>(createParams);
    }

    private SaleOrderSapImportCreateParams ToCreateParams(SaleOrderGICWriteOffExcelDto importDto, ExcelImportContext context)
    {
        var result = new SaleOrderSapImportCreateParams
        {
            SOType = importDto.SOType,
            GICNo = importDto.GICWONo,

            MaterialCode = importDto.MaterialCode,
            ModelName = importDto.ModelName,

            SONo = importDto.SONo,

            Note = importDto.Note,
            GICLandingCost = importDto.SAPLandingCost,
            GICAmountLandingCost = importDto.AmountInSAPLandingCost,
            Disposed = importDto.Disposed,

            GICGivNo = importDto.GIVNo,
            GICGivDate = importDto.GIVDate
        };

        return result;
    }
}
