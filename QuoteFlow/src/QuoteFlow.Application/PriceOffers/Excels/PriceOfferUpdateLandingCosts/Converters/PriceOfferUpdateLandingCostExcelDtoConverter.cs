using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.PriceOffers.PriceOfferDetails.ParameterObjects;
using QuoteFlow.Shared.Excels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

namespace QuoteFlow.PriceOffers.Excels.PriceOfferUpdateLandingCosts.Converters;

public class PriceOfferUpdateLandingCostExcelDtoConverter : ExcelDtoConverter<PriceOfferUpdateLandingCostImportDto, PriceOfferDetailUpdateLandingCostParams>
{
    public PriceOfferUpdateLandingCostExcelDtoConverter(
        IObjectMapper objectMapper,
        IGuidGenerator guidGenerator) : base(objectMapper, guidGenerator)
    {
    }

    protected override IEnumerable<string> RequiredValidationContextKeys => [];

    public override Task<ValidationResult> ValidateRowAsync(ExcelRowResult<PriceOfferUpdateLandingCostImportDto> rowResult, ExcelImportContext context, CancellationToken cancellationToken = default)
    {
        // Additional business validations can be added here if needed
        return base.ValidateRowAsync(rowResult, context, cancellationToken);
    }
    protected override Task<PriceOfferDetailUpdateLandingCostParams?> MapToCreateParamsAsync(PriceOfferUpdateLandingCostImportDto importDto, ExcelImportContext context, CancellationToken cancellationToken)
    {
        if (!importDto.PriceOfferDetailId.HasValue)
            return Task.FromResult<PriceOfferDetailUpdateLandingCostParams?>(null);

        var result = new PriceOfferDetailUpdateLandingCostParams
        {
            Id = importDto.PriceOfferDetailId.Value,
            Qty = importDto.Qty,
            LandingCost = importDto.LandingCost,
            MEVNOfferPrice = importDto.NewSaleOfferPrice
        };

        return Task.FromResult<PriceOfferDetailUpdateLandingCostParams?>(result);
    }
}
