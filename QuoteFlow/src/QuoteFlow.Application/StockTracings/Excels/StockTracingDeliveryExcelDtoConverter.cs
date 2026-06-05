using QuoteFlow.Shared.Excels;
using QuoteFlow.StockTracingDetails.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

namespace QuoteFlow.StockTracings.Excels;

public class StockTracingDeliveryExcelDtoConverter : ExcelDtoConverter<StockTracingDeliveryImportDto, StockTracingDetailCreateParams>
{
    public StockTracingDeliveryExcelDtoConverter(IObjectMapper objectMapper, IGuidGenerator guidGenerator) : base(objectMapper, guidGenerator)
    {
    }

    protected override IEnumerable<string> RequiredValidationContextKeys => new[]
{
        ExcelImportContextKeys.ParentEntityId
    };
    public override Task<ValidationResult> ValidateRowAsync(
        ExcelRowResult<StockTracingDeliveryImportDto> rowResult,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        // Add business rule validations here
        return base.ValidateRowAsync(rowResult, context, cancellationToken);
    }

    protected override Task<StockTracingDetailCreateParams?> MapToCreateParamsAsync(
        StockTracingDeliveryImportDto importDto,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        var createParams = ToCreateParams(importDto, context);
        return Task.FromResult<StockTracingDetailCreateParams?>(createParams);
    }

    private StockTracingDetailCreateParams ToCreateParams(StockTracingDeliveryImportDto importDto, ExcelImportContext context)
    {
        return new StockTracingDetailCreateParams
        {
            StockTracingId = context.GetData<Guid>(ExcelImportContextKeys.ParentEntityId),
            RowNo = importDto.RowNo,
            CheckListCode = importDto.CheckListCode,
            DateEntered = importDto.DateEntered,
            Stock = importDto.Stock,
            BU = importDto.BU,
            Customer = importDto.Customer,
            Category = importDto.Category,
            GIV = importDto.GIV,
            Invoice = importDto.Invoice,
            SKUCode = importDto.SKUCode,
            SKUName = importDto.SKUName,
            Quality = importDto.Quality,
            Warranty = importDto.Warranty,
            Unit = importDto.Unit,
            Series = importDto.Series,
            GolfaCode = importDto.GolfaCode,
            OriginCode = importDto.OriginCode,
            ProductionDate = importDto.ProductionDate,
            Location = importDto.Location,
            Note = importDto.Note
        };
    }
}