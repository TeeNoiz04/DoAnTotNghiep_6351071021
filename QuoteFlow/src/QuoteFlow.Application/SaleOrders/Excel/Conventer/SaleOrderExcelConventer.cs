using QuoteFlow.SaleOrdersSapImports.ParameterObjects;
using QuoteFlow.Shared.Excels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

namespace QuoteFlow.SaleOrders.Excel.Conventer;
public class SaleOrderExcelConventer : ExcelDtoConverter<SaleOrderExcelDto, SaleOrderSapImportCreateParams>
{
    public SaleOrderExcelConventer(IObjectMapper objectMapper, IGuidGenerator guidGenerator) : base(objectMapper, guidGenerator)
    {
    }

    protected override IEnumerable<string> RequiredValidationContextKeys => [];
    public override Task<ValidationResult> ValidateRowAsync(
        ExcelRowResult<SaleOrderExcelDto> rowResult,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        // Add business rule validations here
        return base.ValidateRowAsync(rowResult, context, cancellationToken);
    }

    protected override Task<SaleOrderSapImportCreateParams?> MapToCreateParamsAsync(
        SaleOrderExcelDto importDto,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        var createParams = ToCreateParams(importDto, context);
        return Task.FromResult<SaleOrderSapImportCreateParams?>(createParams);
    }

    private SaleOrderSapImportCreateParams ToCreateParams(SaleOrderExcelDto importDto, ExcelImportContext context)
    {
        var result = new SaleOrderSapImportCreateParams
        {
            SONo = importDto.SONo,
            //DONo = importDto.SAPDONo,
            //DODate
            //DONote
            SOSAPNo = importDto.SAPSONo,
            DOSAPNo = importDto.SAPDONo,
            BillingNo = importDto.SAPBillingNo,
            InvoiceNo = importDto.SAPINV,
            InvoiceDate = importDto.SAPINVDate,

        };
        return result;

    }
}
