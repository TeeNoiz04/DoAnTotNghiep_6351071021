using QuoteFlow.Shared.Excels;
using QuoteFlow.StockTracingDetails.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

namespace QuoteFlow.StockTracings.Excels;

public class StockTracingInventoryExcelDtoConverter : ExcelDtoConverter<StockTracingInventoryImportDto, StockTracingDetailCreateParams>
{
    public StockTracingInventoryExcelDtoConverter(IObjectMapper objectMapper, IGuidGenerator guidGenerator) : base(objectMapper, guidGenerator)
    {
    }

    protected override IEnumerable<string> RequiredValidationContextKeys => new[]
{
        ExcelImportContextKeys.ParentEntityId
    };
    public override Task<ValidationResult> ValidateRowAsync(
        ExcelRowResult<StockTracingInventoryImportDto> rowResult,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        // Add business rule validations here
        return base.ValidateRowAsync(rowResult, context, cancellationToken);
    }

    protected override Task<StockTracingDetailCreateParams?> MapToCreateParamsAsync(
        StockTracingInventoryImportDto importDto,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        var createParams = ToCreateParams(importDto, context);
        return Task.FromResult<StockTracingDetailCreateParams?>(createParams);
    }

    private StockTracingDetailCreateParams ToCreateParams(StockTracingInventoryImportDto importDto, ExcelImportContext context)
    {
        return new StockTracingDetailCreateParams
        {
            StockTracingId = context.GetData<Guid>(ExcelImportContextKeys.ParentEntityId),
            RowNo = importDto.RowNo,
            Stock = importDto.WareHouse,
            BU = importDto.BUs,
            Customer = importDto.Customer,
            Category = importDto.Category,
            SKUCode = importDto.SKUCode,
            SKUName = importDto.SKUName,
            Quality = importDto.Quality,
            Warranty = importDto.Warranty,
            Unit = importDto.Unit,
            Series = importDto.Series,
            OriginCode = importDto.OrginCode,
            ProductionDate = importDto.ProductionDate,
            GolfaCode = importDto.GolfaCode,
            Note = importDto.Note,
        };
    }
}