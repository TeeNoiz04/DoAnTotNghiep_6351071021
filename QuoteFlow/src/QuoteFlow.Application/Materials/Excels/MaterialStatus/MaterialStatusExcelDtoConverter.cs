using QuoteFlow.Materials.MaterialImport.MaterialStatus;
using QuoteFlow.Materials.ParameterObjects;
using QuoteFlow.Shared.Excels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;

namespace QuoteFlow.Materials.Excels.MaterialStatus;

public class MaterialStatusExcelDtoConverter : ExcelDtoConverter<MaterialStatusUpdateExcelDto, ExcelMaterialStatusUpdateParams>
{

    public MaterialStatusExcelDtoConverter(Volo.Abp.ObjectMapping.IObjectMapper objectMapper, IGuidGenerator guidGenerator) : base(objectMapper, guidGenerator)
    {
    }

    protected override IEnumerable<string> RequiredValidationContextKeys => [];
    //{
    //        ExcelImportContextKeys.ParentEntityId
    //    };
    public override Task<ValidationResult> ValidateRowAsync(
        ExcelRowResult<MaterialStatusUpdateExcelDto> rowResult,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        return base.ValidateRowAsync(rowResult, context, cancellationToken);
    }

    protected override Task<ExcelMaterialStatusUpdateParams?> MapToCreateParamsAsync(MaterialStatusUpdateExcelDto importDto, ExcelImportContext context, CancellationToken cancellationToken)
    {
        var createParams = ToCreateParams(importDto, context);
        return Task.FromResult<ExcelMaterialStatusUpdateParams?>(createParams);
    }

    private ExcelMaterialStatusUpdateParams ToCreateParams(MaterialStatusUpdateExcelDto importDto, ExcelImportContext context)
    {
        return new ExcelMaterialStatusUpdateParams
        {
            Id = importDto.Id!.Value,
            GolfaCode = importDto.GolfaCode,
            Model = importDto.Model,
            AcceptanceDate = importDto.AcceptanceDate,
            ActiveDate = importDto.ActiveDate,
            Action = importDto.Action,
            Source = importDto.Source,
            Reason = importDto.Reason,
            FactoryRefDoc = importDto.FactoryRefDoc,
            ConcurrencyStamp = importDto.ConcurrencyStamp,
        };
    }
}
