using QuoteFlow.PriceOffers.ParameterObjects;
using QuoteFlow.PriceOffers.PriceOfferReportDetails;
using QuoteFlow.PriceOffers.PriceOfferReportDetails.ParameterObjects;
using QuoteFlow.PriceOffers.PriceOfferReportGenerals;
using QuoteFlow.PriceOffers.PriceOfferReportGenerals.ParameterObjects;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace QuoteFlow.PriceOffers;

public interface IPriceOfferRepository : IRepository<PriceOffer, Guid>
{
    Task<List<PriceOffer>> GetListAsync(
        PriceOfferFilterParams filterParams,
        string? currentUsername = null,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        CancellationToken cancellationToken = default
    );
    Task<List<PriceOfferReportDetail>> GetListReportDetailAsync(PriceOfferReportDetailFilterParams filterParams);
    Task<List<PriceOfferReportGeneral>> GetListReportGeneralAsync(PriceOfferReportGeneralFilterParams filterParams);

    Task<long> GetCountAsync(
        PriceOfferFilterParams filterParams,
        string? currentUsername = null,
        CancellationToken cancellationToken = default);

    Task<PriceOffer> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PriceOffer> GetWithDetailsNoTrackingAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PriceOffer> GetWithDetailsAsync(
        Guid id,
        params Expression<Func<PriceOffer, object?>>[] includes);

    Task<string?> GetLatestCodeAsync(
        string prefix,
        string productType,
        CancellationToken cancellationToken = default);

    Task GenerateApprovalRouteAsync(Guid priceOfferId, string priceOfferCode, string? note = null, CancellationToken cancellationToken = default);

    Task<bool> HasLevel5ApprovalAsync(Guid priceOfferId, string priceOfferCode, CancellationToken cancellationToken = default);

    Task<PriceOffer> UpdateCalculatedFieldsAsync(Guid priceOfferId, bool applySpecialInputPrice = false, CancellationToken cancellationToken = default);

    Task<List<PriceOfferApprovalRoute>> GetListApprovalRoutesAsync(
        Guid? priceOfferId
    );
    Task<long> GetCountMyApprovalsAsync(PriceOfferFilterParams filterParams, string currentUsername);
    Task<List<PriceOffer>> GetListMyApprovalsAsync(PriceOfferFilterParams filterParams, string currentUsername, string? sorting, int maxResultCount, int skipCount);
    Task<List<PriceOfferMessage>> GetListMessagesAsync(Guid priceOfferId, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default);
    Task<long> GetCountMessagesAsync(Guid priceOfferId, CancellationToken cancellationToken = default);
    Task<decimal?> GetDiscountRatioConfigured(
        string priceOfferType,
        string materialType,
        decimal totalMEVNOfferAmount,
        string? keyAccountClassDescription = null);

    Task<decimal> GetDiscountRatioAsync(Guid? keyAccountId);
}