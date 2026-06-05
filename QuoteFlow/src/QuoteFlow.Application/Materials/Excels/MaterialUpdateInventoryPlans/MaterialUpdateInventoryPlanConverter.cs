using QuoteFlow.Materials.ParameterObjects;
using QuoteFlow.Shared.Excels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;

namespace QuoteFlow.Materials.Excels.MaterialUpdateInventoryPlans;

internal class MaterialUpdateInventoryPlanConverter : ExcelDtoConverter<MaterialUpdateInventoryPlanImportDto, ExcelMaterialUpdateInventoryPlanUpdateParams>
{

    public MaterialUpdateInventoryPlanConverter(Volo.Abp.ObjectMapping.IObjectMapper objectMapper, IGuidGenerator guidGenerator) : base(objectMapper, guidGenerator)
    {
    }

    protected override IEnumerable<string> RequiredValidationContextKeys => [];
    //{
    //        ExcelImportContextKeys.ParentEntityId
    //    };
    public override Task<ValidationResult> ValidateRowAsync(
        ExcelRowResult<MaterialUpdateInventoryPlanImportDto> rowResult,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        return base.ValidateRowAsync(rowResult, context, cancellationToken);
    }

    protected override Task<ExcelMaterialUpdateInventoryPlanUpdateParams?> MapToCreateParamsAsync(MaterialUpdateInventoryPlanImportDto importDto, ExcelImportContext context, CancellationToken cancellationToken)
    {
        var createParams = ToCreateParams(importDto, context);
        return Task.FromResult<ExcelMaterialUpdateInventoryPlanUpdateParams?>(createParams);
    }

    private ExcelMaterialUpdateInventoryPlanUpdateParams ToCreateParams(MaterialUpdateInventoryPlanImportDto importDto, ExcelImportContext context)
    {
        return new ExcelMaterialUpdateInventoryPlanUpdateParams
        {
            Id = importDto.Id!.Value,
            GolfaCode = importDto.GolfaCode,
            Model = importDto.Model,
            StockWarning = importDto.StockWarning,
            InventoryCategory = importDto.InventoryCategory,
            ConcurrencyStamp = importDto.ConcurrencyStamp,
        };
    }
}
