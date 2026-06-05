using QuoteFlow.Materials.MaterialImport.MaterialFactory;
using QuoteFlow.Materials.ParameterObjects;
using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;

namespace QuoteFlow.Materials.Excels.MaterialFactory;

public class MaterialFactoryExcelDtoConverter : ExcelDtoConverter<MaterialFactoryUpdateExcelDto, ExcelMaterialFactoryUpdateParams>
{

    public MaterialFactoryExcelDtoConverter(Volo.Abp.ObjectMapping.IObjectMapper objectMapper, IGuidGenerator guidGenerator) : base(objectMapper, guidGenerator)
    {
    }

    protected override IEnumerable<string> RequiredValidationContextKeys => [];
    //{
    //        ExcelImportContextKeys.ParentEntityId
    //    };
    public override Task<ValidationResult> ValidateRowAsync(
        ExcelRowResult<MaterialFactoryUpdateExcelDto> rowResult,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        return base.ValidateRowAsync(rowResult, context, cancellationToken);
    }

    protected override Task<ExcelMaterialFactoryUpdateParams?> MapToCreateParamsAsync(MaterialFactoryUpdateExcelDto importDto, ExcelImportContext context, CancellationToken cancellationToken)
    {
        var createParams = ToCreateParams(importDto, context);
        return Task.FromResult<ExcelMaterialFactoryUpdateParams?>(createParams);
    }

    private ExcelMaterialFactoryUpdateParams ToCreateParams(MaterialFactoryUpdateExcelDto importDto, ExcelImportContext context)
    {
        return new ExcelMaterialFactoryUpdateParams
        {
            Id = importDto.Id ?? Guid.Empty,
            GolfaCode = importDto.GolfaCode,
            Model = importDto.Model,
            ReferenceLeadTime = importDto.ReferenceLeadTime,
            CountryOfOrigin = importDto.CountryOfOrigin,
            Maxlot = importDto.Maxlot,
            ConcurrencyStamp = importDto.ConcurrencyStamp,
        };
    }
}