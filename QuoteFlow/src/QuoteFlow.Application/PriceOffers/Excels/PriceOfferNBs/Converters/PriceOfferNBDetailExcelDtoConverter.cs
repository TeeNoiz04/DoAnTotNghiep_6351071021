using QuoteFlow.Materials;
using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.PriceOffers.PriceOfferDetails.ParameterObjects;
using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

namespace QuoteFlow.PriceOffers.Excels.PriceOfferNBs.Converters;

public class PriceOfferNBDetailExcelDtoConverter : ExcelDtoConverter<PriceOfferDetailImportDto, PriceOfferDetailCreateParams>
{
    private readonly IMaterialRepository _materialRepository;

    public PriceOfferNBDetailExcelDtoConverter(
        IObjectMapper objectMapper,
        IGuidGenerator guidGenerator,
        IMaterialRepository materialRepository
    ) : base(objectMapper, guidGenerator)
    {
        _materialRepository = materialRepository;
    }

    protected override IEnumerable<string> RequiredValidationContextKeys => [ExcelImportContextKeys.ParentEntityId];

    protected override async Task<PriceOfferDetailCreateParams?> MapToCreateParamsAsync(PriceOfferDetailImportDto importDto, ExcelImportContext context, CancellationToken cancellationToken)
    {
        var priceOfferId = context.GetData<Guid>(ExcelImportContextKeys.ParentEntityId);
        var importGuid = context.GetData<Guid>(ExcelImportContextKeys.PriceOfferDetail.ImportGuid);

        var materialCode = importDto.GolfaCode ?? string.Empty;
        var material = await _materialRepository.FirstOrDefaultAsync(x => x.GolfaCode == materialCode, cancellationToken: cancellationToken);

        if (material == null)
        {
            return null;
        }

        return new PriceOfferDetailCreateParams(
            importDto.RowNo,
            priceOfferId,
            importDto.GolfaCode!,
            importDto.ModelName!,
            importDto.Qty!.Value,
            importDto.StandardPrice!.Value,
            importDto.StandardAmount!.Value,
            importDto.MEVNOfferPrice!.Value,
            importGuid,
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
