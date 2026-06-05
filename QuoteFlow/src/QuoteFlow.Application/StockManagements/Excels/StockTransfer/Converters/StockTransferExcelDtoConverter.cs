using QuoteFlow.MaterialStockUploadDetails;
using QuoteFlow.MaterialStockUploadDetails.ParameterObjects;
using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;


namespace QuoteFlow.StockManagements.Excels.StockTransfer.Converters;

public class StockTransferExcelDtoConverter
    : ExcelDtoConverter<MaterialStockUploadDetailImportTransferDto, MaterialStockUploadDetailCreateParams>
{
    public StockTransferExcelDtoConverter(
        IObjectMapper objectMapper,
        IGuidGenerator guidGenerator)
        : base(objectMapper, guidGenerator)
    {
    }

    protected override IEnumerable<string> RequiredValidationContextKeys => new[]
    {
        ExcelImportContextKeys.ParentEntityId
    };

    public override Task<ValidationResult> ValidateRowAsync(
        ExcelRowResult<MaterialStockUploadDetailImportTransferDto> rowResult,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        return base.ValidateRowAsync(rowResult, context, cancellationToken);
    }

    protected override Task<MaterialStockUploadDetailCreateParams?> MapToCreateParamsAsync(
        MaterialStockUploadDetailImportTransferDto importDto,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        var createParams = ToCreateParams(importDto, context);
        return Task.FromResult<MaterialStockUploadDetailCreateParams?>(createParams);
    }

    private MaterialStockUploadDetailCreateParams ToCreateParams(
        MaterialStockUploadDetailImportTransferDto importDto,
        ExcelImportContext context)
    {
        return new MaterialStockUploadDetailCreateParams
        {
            RequestId = context.GetData<Guid>(ExcelImportContextKeys.ParentEntityId),

            MaterialCode = importDto.MaterialCode,
            Model = importDto.Model,
            Storage = importDto.Storage,
            StorageDestination = importDto.StorageDestination,
            Qty = importDto.Qty,
            Remark = importDto.Remark,
            StorageSrc_Id = importDto.StorageSrc_Id,
            StorageDesc_Id = importDto.StorageDesc_Id,
        };
    }
}

