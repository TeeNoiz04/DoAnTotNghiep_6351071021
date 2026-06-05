using QuoteFlow.Customers;
using QuoteFlow.PriceOffers.ParameterObjects;
using QuoteFlow.PriceOffers.PriceOfferCustomers.ParameterObject;
using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.PriceOffers.PriceOfferDetails.ParameterObjects;
using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

namespace QuoteFlow.PriceOffers.Excels.PriceOfferNBs.Converters;

public class PriceOfferNBExcelDtoConverter : ExcelDtoConverter<PriceOfferImportDto, PriceOfferCreateParams>
{
    private readonly PriceOfferManager _priceOfferManager;
    private readonly IExcelImportFactory _excelImportFactory;
    private readonly ICustomerRepository _customerRepository;
    private readonly CustomerManager _customerManager;

    public PriceOfferNBExcelDtoConverter(
        IObjectMapper objectMapper,
        IGuidGenerator guidGenerator,
        PriceOfferManager priceOfferManager,
        IExcelImportFactory excelImportFactory,
        ICustomerRepository customerRepository,
        CustomerManager customerManager) : base(objectMapper, guidGenerator)
    {
        _priceOfferManager = priceOfferManager;
        _excelImportFactory = excelImportFactory;
        _customerRepository = customerRepository;
        _customerManager = customerManager;
    }

    protected override IEnumerable<string> RequiredValidationContextKeys => [];

    protected override async Task<PriceOfferCreateParams?> MapToCreateParamsAsync(PriceOfferImportDto importDto, ExcelImportContext context, CancellationToken cancellationToken)
    {
        var customers = new List<PriceOfferCustomerCreateParams>();
        var details = new List<PriceOfferDetailCreateParams>();
        var newPriceOfferId = _guidGenerator.Create();
        context.SetData(ExcelImportContextKeys.ParentEntityId, newPriceOfferId);

        if (importDto.Details is not null && importDto.Details.IsValid)
        {
            var importGuid = Guid.NewGuid();
            var detailConverter = _excelImportFactory.CreateCreateParamsConverter<PriceOfferDetailImportDto, PriceOfferDetailCreateParams>(ExcelImporters.PriceOfferNBDetail);
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

        var createParams = await ToCreateParams(importDto, context, customers, details);
        return createParams;
    }

    private async Task<PriceOfferCreateParams> ToCreateParams(
        PriceOfferImportDto importDto,
        ExcelImportContext context,
        ICollection<PriceOfferCustomerCreateParams> customers,
        ICollection<PriceOfferDetailCreateParams> details)
    {
        var priceOfferCode = await _priceOfferManager.GenerateNewCodeAsync(PriceOfferConsts.NoBuyerPrefix, importDto.MaterialType!);

        return new PriceOfferCreateParams(
            priceOfferCode,
            null,  // No buyer for NB type
            null,  // No buyer type for NB type
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
            customers,
            details,
            importDto.Note,
            importDto.CloseDate
        )
        {
            Id = context.GetData<Guid>(ExcelImportContextKeys.ParentEntityId),
        };
    }
}
