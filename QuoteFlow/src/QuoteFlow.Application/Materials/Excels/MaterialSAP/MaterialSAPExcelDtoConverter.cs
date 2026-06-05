using QuoteFlow.Materials.MaterialImport.MaterialSAP;
using QuoteFlow.Materials.ParameterObjects;
using QuoteFlow.Shared.Excels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;

namespace QuoteFlow.Materials.Excels.MaterialSAP;

public class MaterialSAPExcelDtoConverter : ExcelDtoConverter<MaterialSAPUpdateExcelDto, ExcelMaterialUpdateParams>
{

    public MaterialSAPExcelDtoConverter(Volo.Abp.ObjectMapping.IObjectMapper objectMapper, IGuidGenerator guidGenerator) : base(objectMapper, guidGenerator)
    {
    }

    protected override IEnumerable<string> RequiredValidationContextKeys => [];
    //{
    //        ExcelImportContextKeys.ParentEntityId
    //    };
    public override Task<ValidationResult> ValidateRowAsync(
        ExcelRowResult<MaterialSAPUpdateExcelDto> rowResult,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        return base.ValidateRowAsync(rowResult, context, cancellationToken);
    }

    protected override Task<ExcelMaterialUpdateParams?> MapToCreateParamsAsync(MaterialSAPUpdateExcelDto importDto, ExcelImportContext context, CancellationToken cancellationToken)
    {
        var createParams = ToCreateParams(importDto, context);
        return Task.FromResult<ExcelMaterialUpdateParams?>(createParams);
    }

    private ExcelMaterialUpdateParams ToCreateParams(MaterialSAPUpdateExcelDto importDto, ExcelImportContext context)
    {
        return new ExcelMaterialUpdateParams
        {
            Id = importDto.Id!.Value,
            GolfaCode = importDto.GolfaCode,
            Model = importDto.Model,
            SAPCode = importDto.SAPCode,
            DescriptionVN = importDto.DescriptionVN,
            ProductHiearchy = importDto.ProductHiearchy,
            VAT = importDto.VAT,
            ConcurrencyStamp = importDto.ConcurrencyStamp,
        };
    }
}
