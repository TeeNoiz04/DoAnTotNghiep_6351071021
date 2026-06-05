using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.PriceOffers.PriceOfferDetails.ParameterObjects;
using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

namespace QuoteFlow.PriceOffers.Excels.PriceOfferAPs.Converters;

public class PriceOfferAPDetailExcelDtoConverter : ExcelDtoConverter<PriceOfferDetailImportDto, PriceOfferDetailCreateParams>
{
    public PriceOfferAPDetailExcelDtoConverter(
        IObjectMapper objectMapper,
        IGuidGenerator guidGenerator) : base(objectMapper, guidGenerator)
    {
    }

    protected override IEnumerable<string> RequiredValidationContextKeys => [
        ExcelImportContextKeys.ParentEntityId,
        ExcelImportContextKeys.PriceOfferDetail.ImportGuid
    ];

    public override Task<ValidationResult> ValidateRowAsync(ExcelRowResult<PriceOfferDetailImportDto> rowResult, ExcelImportContext context, CancellationToken cancellationToken = default)
    {
        // add BUSINESS RULE validations here if there's any.
        return base.ValidateRowAsync(rowResult, context, cancellationToken);
    }

    protected override async Task<PriceOfferDetailCreateParams?> MapToCreateParamsAsync(PriceOfferDetailImportDto importDto, ExcelImportContext context, CancellationToken cancellationToken)
    {
        var createParams = ToCreateParams(importDto, context);
        return createParams;
    }

    private PriceOfferDetailCreateParams ToCreateParams(PriceOfferDetailImportDto importDto, ExcelImportContext context)
    {
        return new PriceOfferDetailCreateParams(
            importDto.RowNo,
            context.GetData<Guid>(ExcelImportContextKeys.ParentEntityId),
            importDto.GolfaCode!,
            importDto.ModelName!,
            importDto.Qty!.Value,
            importDto.StandardPrice!.Value,
            importDto.StandardAmount!.Value,
            importDto.MEVNOfferPrice!.Value,
            context.GetData<Guid>(ExcelImportContextKeys.PriceOfferDetail.ImportGuid),
            importDto.SpecialSpec1,
            importDto.SpecialSpec2,
            importDto.BuyerPrice,
            importDto.RequestedAmount,
            importDto.RequestedDiscountRatio,
            importDto.PriceToCustomer,
            importDto.CompetitorBrand,
            importDto.CompetitorModel,
            importDto.CompetitorPrice
        );
    }
}
