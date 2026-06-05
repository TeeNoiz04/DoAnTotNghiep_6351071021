using QuoteFlow.PurchaseOrdersSapImports.Excel;
using QuoteFlow.PurchaseOrdersSapImports.ParameterObject;
using QuoteFlow.Shared.Excels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

namespace QuoteFlow.PurchaseOrders.Excel.Conventer;
public class PurchaseOrderSapImportExcelConventer : ExcelDtoConverter<PurchaseOrdersSapImportsExcelDto, PurchaseOrderSapImportCreateParams>
{
    public PurchaseOrderSapImportExcelConventer(IObjectMapper objectMapper, IGuidGenerator guidGenerator) : base(objectMapper, guidGenerator)
    {
    }

    protected override IEnumerable<string> RequiredValidationContextKeys => [];
    public override Task<ValidationResult> ValidateRowAsync(
        ExcelRowResult<PurchaseOrdersSapImportsExcelDto> rowResult,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        // Add business rule validations here
        return base.ValidateRowAsync(rowResult, context, cancellationToken);
    }

    protected override Task<PurchaseOrderSapImportCreateParams?> MapToCreateParamsAsync(
        PurchaseOrdersSapImportsExcelDto importDto,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        var createParams = ToCreateParams(importDto, context);
        return Task.FromResult<PurchaseOrderSapImportCreateParams?>(createParams);
    }

    private PurchaseOrderSapImportCreateParams ToCreateParams(PurchaseOrdersSapImportsExcelDto importDto, ExcelImportContext context)
    {
        var result = new PurchaseOrderSapImportCreateParams
        {
            PONo = importDto.PONo,
            POSAPNo = importDto.SAPPONo,
            POSAPDate = importDto.SAPPODate,
        };
        return result;

    }
}
