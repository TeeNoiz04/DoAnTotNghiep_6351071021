using QuoteFlow.ApprovalRoutes;
using QuoteFlow.Messages;
using QuoteFlow.PriceOffers.PriceOfferCustomers;
using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.PriceOffers.PriceOfferMessages;
using QuoteFlow.PriceOffers.PriceOfferReportDetails;
using QuoteFlow.PriceOffers.PriceOfferReportGenerals;
using QuoteFlow.Shared;
using QuoteFlow.Shared.Excels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace QuoteFlow.PriceOffers;

public interface IPriceOffersAppService : IApplicationService
{
    // Report
    Task<PagedResultDto<PriceOfferReportDetailDto>> GetListReportDetailAsync(GetPriceOfferReportDetailsInput input);

    Task<IRemoteStreamContent> GetListGeneralAsExcelFileAsync(GetPriceOfferReportGeneralsInput input);

    Task<PagedResultDto<PriceOfferReportGeneralDto>> GetListReportGeneralAsync(GetPriceOfferReportGeneralsInput input);

    Task<IRemoteStreamContent> GetListDetailAsExcelFileAsync(GetPriceOfferReportDetailsInput input);

    // Price Offers CRUD operations
    Task<PagedResultDto<PriceOfferWithNavigationListDto>> GetListAsync(GetPriceOffersInput input);

    Task<PagedResultDto<PriceOfferDetailDto>> GetListDetailsAsync(Guid priceOfferId, GetPriceOfferDetailsInput input);

    Task<PagedResultDto<PriceOfferCustomerDto>> GetListCustomersAsync(Guid priceOfferId, GetPriceOfferCustomersInput input);

    Task<PriceOfferDto> GetAsync(Guid id);

    Task<PagedResultDto<PriceOfferWithNavigationListDto>> GetMyApprovalsListAsync(GetPriceOffersInput input);

    Task<PagedResultDto<MessageDto>> GetListMessagesAsync(Guid priceOfferId, GetPriceOfferMessagesInput input);

    Task DeleteAsync(Guid id);

    Task<PriceOfferDto> CreateAsync(PriceOfferCreateDto input);

    Task<PriceOfferDto> UpdateAsync(Guid id, PriceOfferUpdateDto input);

    Task<IRemoteStreamContent> GetListAsExcelFileAsync(PriceOfferExcelDownloadDto input);

    Task<IRemoteStreamContent> GetListDetailsAsExcelFileAsync(Guid priceOfferId, string downloadToken);

    Task<QuoteFlow.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();
    Task<ExcelValidationResult<PriceOfferImportDto>> ValidateAndParseAPAsync(IRemoteStreamContent file, PriceOfferAPImportInput input);
    Task<ExcelValidationResult<PriceOfferImportDto>> ValidateAndParsePPAsync(IRemoteStreamContent file, PriceOfferImportInput input);
    Task<ExcelValidationResult<PriceOfferImportDto>> ValidateAndParseDSAsync(IRemoteStreamContent file, PriceOfferDSImportInput input);
    Task<ExcelValidationResult<PriceOfferImportDto>> ValidateAndParseNBAsync(IRemoteStreamContent file, PriceOfferNBImportInput input);
    Task<PriceOfferDto> ImportAPAsync(ExcelValidationResult<PriceOfferImportDto> validationResult, bool forceSubmit = false);
    Task<PriceOfferDto> ImportPPAsync(ExcelValidationResult<PriceOfferImportDto> validationResult, bool forceSubmit = false);
    Task<PriceOfferDto> ImportDSAsync(ExcelValidationResult<PriceOfferImportDto> validationResult, bool forceSubmit = false);
    Task<PriceOfferDto> ImportNBAsync(ExcelValidationResult<PriceOfferImportDto> validationResult, bool forceSubmit = false);

    Task<ExcelValidationResult<PriceOfferDetailImportDto>> ValidateAndParseAddMoreItemDetailAsync(Guid priceOfferId, IRemoteStreamContent file, bool? getPriceAutomatically);
    Task<List<PriceOfferDetailDto>> ImportAddMoreItemDetailAsync(Guid priceOfferId, ImportAddMoreItemsInput input);

    Task<ExcelValidationResult<PriceOfferUpdateLandingCostImportDto>> ValidateAndParseUpdateLandingCostAsync(Guid priceOfferId, IRemoteStreamContent file);
    Task<List<PriceOfferDetailDto>> ImportUpdateLandingCostAsync(Guid priceOfferId, ExcelValidationResult<PriceOfferUpdateLandingCostImportDto> validationResult);

    Task<PriceOfferDto> PerformActionAsync(Guid id, ActionDto input);
    Task<List<ApproverDto>> GetListApproversAsync(Guid priceOfferId);

    Task<PriceOfferDto> SubmitProjectResultAsync(Guid id, SubmitProjectResultDto input);
    Task<PriceOfferDto> ConfirmPreOrderStatusAsync(Guid id, ConfirmPreOrderStatusDto input);
    Task<PriceOfferDto> AssignSpecialInputPrice(Guid id, AssignSpecialInputPriceDto input);
    Task<MessageDto> SendMessageAsync(Guid priceOfferId, MessageCreateDto input);
    Task<List<PriceOfferDetailDto>> CancelPriceOfferDetailsAsync(Guid priceOfferId, PriceOfferDetailCancelDto input);
}