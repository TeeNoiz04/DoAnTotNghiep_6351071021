using QuoteFlow.PriceOffers.ParameterObjects;
using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.PriceOffers.PriceOfferDetails.ParameterObjects;
using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

namespace QuoteFlow.PriceOffers.Excels.PriceOfferDSs.Converters;
public class PriceOfferDSExcelDtoConverter : ExcelDtoConverter<PriceOfferImportDto, PriceOfferCreateParams>
{
    private readonly PriceOfferManager _priceOfferManager;
    private readonly IExcelImportFactory _excelImportFactory;

    public PriceOfferDSExcelDtoConverter(
        IObjectMapper objectMapper,
        IGuidGenerator guidGenerator,
        PriceOfferManager priceOfferManager,
        IExcelImportFactory excelImportFactory) : base(objectMapper, guidGenerator)
    {
        _priceOfferManager = priceOfferManager;
        _excelImportFactory = excelImportFactory;
    }

    protected override IEnumerable<string> RequiredValidationContextKeys => [];

    protected override async Task<PriceOfferCreateParams?> MapToCreateParamsAsync(PriceOfferImportDto importDto, ExcelImportContext context, CancellationToken cancellationToken)
    {
        var details = new List<PriceOfferDetailCreateParams>();
        var newPriceOfferId = _guidGenerator.Create();
        context.SetData(ExcelImportContextKeys.ParentEntityId, newPriceOfferId);


        if (importDto.Details is not null && importDto.Details.IsValid)
        {
            var importGuid = Guid.NewGuid();
            var detailConverter = _excelImportFactory.CreateCreateParamsConverter<PriceOfferDetailImportDto, PriceOfferDetailCreateParams>(ExcelImporters.PriceOfferPPDetail);
            context.SetData(ExcelImportContextKeys.PriceOfferDetail.ImportGuid, importGuid);
            foreach (var detailImportDto in importDto.Details.ListData)
            {
                var detailCreateParams = await detailConverter.ConvertToCreateParamsAsync(detailImportDto, context, cancellationToken);
                if (detailCreateParams != null)
                {
                    details.Add(detailCreateParams);
                }
            }
        }

        var createParams = await ToCreateParams(importDto, context, details);
        return createParams;
    }

    private async Task<PriceOfferCreateParams> ToCreateParams(
        PriceOfferImportDto importDto,
        ExcelImportContext context,
        ICollection<PriceOfferDetailCreateParams> details)
    {
        var priceOfferCode = await _priceOfferManager.GenerateNewCodeAsync(PriceOfferConsts.BuyerPrefix, importDto.MaterialType!);

        return new PriceOfferCreateParams(
            priceOfferCode,
            importDto.BuyerId,
            importDto.BuyerTypeId,
            importDto.MaterialType!,
            importDto.LocationId,
            importDto.ProjectName,
            importDto.ProjectTypeId,
            importDto.EUIndustryId,
            importDto.Application,
            importDto.Country,
            importDto.Province,
            importDto.DetailedAddress,
            importDto.CompetitorBrand,
            importDto.PriceGapWithCompetitor,
            importDto.DecisionRight,
            importDto.POPlannedDate,
            importDto.DeliveryDate,
            importDto.UpcomingPotentialProjects,
            importDto.OtherPJInformation,
            importDto.FileName,
            importDto.TotalMEVNOfferAmount,
            importDto.AccountNo,
            [],
            details,
            importDto.Note,
            importDto.CloseDate,
            importDto.KeyAccountId,
            importDto.KeyAccountTypeId,
            importDto.KeyAccountClassId
        )
        {
            Id = context.GetData<Guid>(ExcelImportContextKeys.ParentEntityId),
        };
    }
}
