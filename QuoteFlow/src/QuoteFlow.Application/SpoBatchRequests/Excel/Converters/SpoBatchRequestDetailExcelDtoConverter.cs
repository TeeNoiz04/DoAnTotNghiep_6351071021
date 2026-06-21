using QuoteFlow.Shared.Excels;
using QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails;
using QuoteFlow.SpoBatchRequests.SpoBatchRequestDetails.ParameterObject;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

namespace QuoteFlow.SpoBatchRequests.Excel.Converters;

public class SpoBatchRequestDetailExcelDtoConverter
    : ExcelDtoConverter<SpoBatchRequestDetailImportDto, SpoBatchRequestDetailCreateParams>
{
    public SpoBatchRequestDetailExcelDtoConverter(
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
        ExcelRowResult<SpoBatchRequestDetailImportDto> rowResult,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        // Nếu cần logic kiểm tra bổ sung, thêm tại đây.
        return base.ValidateRowAsync(rowResult, context, cancellationToken);
    }

    protected override Task<SpoBatchRequestDetailCreateParams?> MapToCreateParamsAsync(
        SpoBatchRequestDetailImportDto importDto,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        var createParams = ToCreateParams(importDto, context);
        return Task.FromResult<SpoBatchRequestDetailCreateParams?>(createParams);
    }

    private SpoBatchRequestDetailCreateParams ToCreateParams(
        SpoBatchRequestDetailImportDto importDto,
        ExcelImportContext context)
    {
        return new SpoBatchRequestDetailCreateParams
        {
            RequestId = context.GetData<Guid>(ExcelImportContextKeys.ParentEntityId),
            SPOCode = importDto.SPOCode,
            GolfaCode = importDto.GolfaCode,
            Action = importDto.Action,
            ActionDate = importDto.ActionDate,
            Note = importDto.Note
        };
    }
}
