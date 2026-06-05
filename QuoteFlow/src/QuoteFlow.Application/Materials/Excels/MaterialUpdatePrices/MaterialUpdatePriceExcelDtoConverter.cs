using QuoteFlow.Materials.ParameterObjects;
using QuoteFlow.Shared.Excels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

namespace QuoteFlow.Materials.Excels.MaterialUpdatePrices;
internal class MaterialUpdatePriceExcelDtoConverter
    : ExcelDtoConverter<MaterialUpdatePriceImportDto, ExcelMaterialUpdatePriceParams>
{
    public MaterialUpdatePriceExcelDtoConverter(
        IObjectMapper objectMapper,
        IGuidGenerator guidGenerator)
        : base(objectMapper, guidGenerator)
    {
    }

    protected override IEnumerable<string> RequiredValidationContextKeys => [];

    public override Task<ValidationResult> ValidateRowAsync(
        ExcelRowResult<MaterialUpdatePriceImportDto> rowResult,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        // Add custom validation logic here if needed
        return base.ValidateRowAsync(rowResult, context, cancellationToken);
    }

    protected override Task<ExcelMaterialUpdatePriceParams?> MapToCreateParamsAsync(
        MaterialUpdatePriceImportDto importDto,
        ExcelImportContext context,
        CancellationToken cancellationToken = default)
    {
        var createParams = ToCreateParams(importDto, context);
        return Task.FromResult<ExcelMaterialUpdatePriceParams?>(createParams);
    }

    private ExcelMaterialUpdatePriceParams ToCreateParams(MaterialUpdatePriceImportDto importDto, ExcelImportContext context)
    {
        return new ExcelMaterialUpdatePriceParams
        {
            Id = importDto.Id,
            GolfaCode = importDto.MaterialCode,
            Model = importDto.ModelName,
            ValidFrom = importDto.PriceValidFrom,
            ValidTo = importDto.PriceValidTo,
            Spec1 = importDto.Spec1,
            MaterialType = importDto.MaterialType,
            MaterialGroup = importDto.MaterialGroup,
            //Material_GroupId = importDto.MaterialGroupId,
            Input_Price = importDto.InputPrice,
            InputCurrency = importDto.InputCurrency,
            INCOTERMS = importDto.Incoterms,
            EPA = importDto.EPA,
            ImportDuty = importDto.ImportDuty,
            AppliedExchangeRate = importDto.ExchangeRate,
            LandedCost = importDto.LandedCost,
            MaxSalesOfferPrice = importDto.MaxSaleOfferPrice,
            MaxMangerOfferPrice = importDto.MaxManagerOfferPrice,
            Standard_Price = importDto.StandardPrice,
            SellingPrice1 = importDto.SellingPrice1,
            SellingPrice2 = importDto.SellingPrice2,
            SellingPrice3 = importDto.SellingPrice3,
            SellingPrice4 = importDto.SellingPrice4,
            SellingPrice5 = importDto.SellingPrice5,
            ConcurrencyStamp = importDto.ConcurrencyStamp
        };
    }
}
