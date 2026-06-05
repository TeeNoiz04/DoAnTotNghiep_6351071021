namespace QuoteFlow.StockManagements.Excels.StockInventory.Converters;
using QuoteFlow.MaterialStockUploadDetails;
using QuoteFlow.MaterialStockUploadDetails.ParameterObjects;
using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

public class StockInventoryExcelDtoConverter
    : ExcelDtoConverter<MaterialStockUploadDetailImportInventoryDto, MaterialStockUploadDetailCreateParams>
{
    public StockInventoryExcelDtoConverter(
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
        ExcelRowResult<MaterialStockUploadDetailImportInventoryDto> rowResult,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        // Nếu cần logic kiểm tra bổ sung, thêm tại đây.
        return base.ValidateRowAsync(rowResult, context, cancellationToken);
    }

    protected override Task<MaterialStockUploadDetailCreateParams?> MapToCreateParamsAsync(
        MaterialStockUploadDetailImportInventoryDto importDto,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        var createParams = ToCreateParams(importDto, context);
        return Task.FromResult<MaterialStockUploadDetailCreateParams?>(createParams);
    }

    private MaterialStockUploadDetailCreateParams ToCreateParams(
        MaterialStockUploadDetailImportInventoryDto importDto,
        ExcelImportContext context)
    {
        return new MaterialStockUploadDetailCreateParams
        {
            RequestId = context.GetData<Guid>(ExcelImportContextKeys.ParentEntityId),
            MaterialCode = importDto.MaterialCode,
            Model = importDto.Model,
            Storage = importDto.Storage,
            Qty = importDto.Qty,
            RefDoc = importDto.RefDoc,
            Remark = importDto.Remark,
            StorageSrc_Id = importDto.StorageSrc_Id,
        };
    }
}
