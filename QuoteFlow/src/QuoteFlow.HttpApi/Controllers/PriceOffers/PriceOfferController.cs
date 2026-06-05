using Asp.Versioning;
using QuoteFlow.ApprovalRoutes;
using QuoteFlow.Buyers;
using QuoteFlow.Messages;
using QuoteFlow.PriceOffers;
using QuoteFlow.PriceOffers.PriceOfferCustomers;
using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.PriceOffers.PriceOfferMessages;
using QuoteFlow.PriceOffers.PriceOfferReportDetails;
using QuoteFlow.PriceOffers.PriceOfferReportGenerals;
using QuoteFlow.Shared;
using QuoteFlow.Shared.Excels;
using QuoteFlow.SystemCategories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Content;

namespace QuoteFlow.Controllers.PriceOffers;

[RemoteService]
[Area("app")]
[ControllerName("PriceOffer")]
[Route("api/app/price-offers")]

public class PriceOfferController : AbpController, IPriceOffersAppService
{
    protected IPriceOffersAppService _priceOffersAppService;
    protected ISystemCategoriesAppService _systemCategoriesAppService;
    protected IBuyersAppService _buyersAppService;
    public PriceOfferController(IPriceOffersAppService priceOffersAppService, ISystemCategoriesAppService systemCategoriesAppService, IBuyersAppService buyersAppService)
    {
        _priceOffersAppService = priceOffersAppService;
        _systemCategoriesAppService = systemCategoriesAppService;
        _buyersAppService = buyersAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<PriceOfferWithNavigationListDto>> GetListAsync(GetPriceOffersInput input)
    {
        return _priceOffersAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{priceOfferId}/details")]
    public virtual Task<PagedResultDto<PriceOfferDetailDto>> GetListDetailsAsync(Guid priceOfferId, GetPriceOfferDetailsInput input)
    {
        return _priceOffersAppService.GetListDetailsAsync(priceOfferId, input);
    }

    [HttpGet]
    [Route("{priceOfferId}/customers")]
    public virtual Task<PagedResultDto<PriceOfferCustomerDto>> GetListCustomersAsync(Guid priceOfferId, GetPriceOfferCustomersInput input)
    {
        return _priceOffersAppService.GetListCustomersAsync(priceOfferId, input);
    }


    [HttpGet]
    [Route("{id}")]
    public virtual Task<PriceOfferDto> GetAsync(Guid id)
    {
        return _priceOffersAppService.GetAsync(id);
    }

    [HttpGet]
    [Route("my-approvals")]
    public virtual Task<PagedResultDto<PriceOfferWithNavigationListDto>> GetMyApprovalsListAsync(GetPriceOffersInput input)
    {
        return _priceOffersAppService.GetMyApprovalsListAsync(input);
    }

    [HttpGet]
    [Route("{priceOfferId}/messages")]
    public virtual Task<PagedResultDto<MessageDto>> GetListMessagesAsync(Guid priceOfferId, GetPriceOfferMessagesInput input)
    {
        return _priceOffersAppService.GetListMessagesAsync(priceOfferId, input);
    }

    [HttpPost]
    public virtual Task<PriceOfferDto> CreateAsync(PriceOfferCreateDto input)
    {
        return _priceOffersAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<PriceOfferDto> UpdateAsync(Guid id, PriceOfferUpdateDto input)
    {
        return _priceOffersAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _priceOffersAppService.DeleteAsync(id);
    }

    [HttpGet]
    [Route("as-excel-file")]
    public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(PriceOfferExcelDownloadDto input)
    {
        return _priceOffersAppService.GetListAsExcelFileAsync(input);
    }

    [HttpGet]
    [Route("{priceOfferId}/details/as-excel-file")]
    public virtual Task<IRemoteStreamContent> GetListDetailsAsExcelFileAsync(Guid priceOfferId, [FromQuery] string downloadToken)
    {
        return _priceOffersAppService.GetListDetailsAsExcelFileAsync(priceOfferId, downloadToken);
    }

    [HttpGet]
    [Route("download-token")]
    public virtual Task<QuoteFlow.Shared.DownloadTokenResultDto> GetDownloadTokenAsync()
    {
        return _priceOffersAppService.GetDownloadTokenAsync();
    }

    [HttpGet]
    [Route("report/detail")]
    public virtual Task<PagedResultDto<PriceOfferReportDetailDto>> GetListReportDetailAsync(GetPriceOfferReportDetailsInput input)
    {
        //var buyers = _buyersAppService.GetListAsync(new() { }).Result.Items.ToList();

        //var materialTypes = _systemCategoriesAppService.GetListAsync(new() { CategoryType = CategoryTypes.MaterialType }).Result.Items.ToList();

        //var detailList = PriceOfferSeeder.GenerateReportDetail(1000, 1234, buyers, materialTypes);

        //var list = PriceOfferSeeder.GenerateReportDetail(detailList);

        //var pagedResult = new PagedResultDto<PriceOfferReportDetailDto>
        //{
        //    TotalCount = list.Count,
        //    Items = list.Skip(input.SkipCount).Take(input.MaxResultCount).ToList()
        //};
        //return Task.FromResult(pagedResult);
        return _priceOffersAppService.GetListReportDetailAsync(input);
    }

    [HttpGet]
    [Route("report/general")]
    public Task<PagedResultDto<PriceOfferReportGeneralDto>> GetListReportGeneralAsync(GetPriceOfferReportGeneralsInput input)
    {
        //var buyers = _buyersAppService.GetListAsync(new() { }).Result.Items.ToList();

        //var materialTypes = _systemCategoriesAppService.GetListAsync(new() { CategoryType = CategoryTypes.MaterialType }).Result.Items.ToList();

        //var generalList = PriceOfferSeeder.GenerateGeneralReportList(1000, 1234, buyers, materialTypes);

        //var list = PriceOfferSeeder.GenerateGeneralReportList(generalList);

        //var pagedResult = new PagedResultDto<PriceOfferReportGeneralDto>
        //{
        //    TotalCount = list.Count,
        //    Items = list.Skip(input.SkipCount).Take(input.MaxResultCount).ToList()
        //};
        //return Task.FromResult(pagedResult);

        return _priceOffersAppService.GetListReportGeneralAsync(input);
    }
    [HttpGet]
    [Route("report/detail-export")]
    public Task<IRemoteStreamContent> GetListDetailAsExcelFileAsync(GetPriceOfferReportDetailsInput input)
    {
        return _priceOffersAppService.GetListDetailAsExcelFileAsync(input);
    }
    [HttpGet]
    [Route("report/general-export")]
    public Task<IRemoteStreamContent> GetListGeneralAsExcelFileAsync(GetPriceOfferReportGeneralsInput input)
    {
        return _priceOffersAppService.GetListGeneralAsExcelFileAsync(input);
    }

    [HttpPost]
    [Route("validate-and-parse/pp")]
    public Task<ExcelValidationResult<PriceOfferImportDto>> ValidateAndParsePPAsync(
        [FromForm] IRemoteStreamContent file,
        [FromForm] PriceOfferImportInput input)
    {
        return _priceOffersAppService.ValidateAndParsePPAsync(file, input);
    }

    [HttpPost]
    [Route("import/pp")]
    public Task<PriceOfferDto> ImportPPAsync(ExcelValidationResult<PriceOfferImportDto> validationResult, bool forceSubmit = false)
    {
        return _priceOffersAppService.ImportPPAsync(validationResult, forceSubmit);
    }

    [HttpPost]
    [Route("validate-and-parse/{priceOfferId}/add-more-items")]
    public Task<ExcelValidationResult<PriceOfferDetailImportDto>> ValidateAndParseAddMoreItemDetailAsync(Guid priceOfferId, IRemoteStreamContent file, bool? getPriceAutomatically)
    {
        return _priceOffersAppService.ValidateAndParseAddMoreItemDetailAsync(priceOfferId, file, getPriceAutomatically);
    }

    [HttpPost]
    [Route("import/{priceOfferId}/add-more-items")]
    public Task<List<PriceOfferDetailDto>> ImportAddMoreItemDetailAsync(Guid priceOfferId, [FromBody] ImportAddMoreItemsInput input)
    {
        return _priceOffersAppService.ImportAddMoreItemDetailAsync(priceOfferId, input);
    }
    [HttpPost]
    [Route("validate-and-parse/ap")]
    public Task<ExcelValidationResult<PriceOfferImportDto>> ValidateAndParseAPAsync([FromForm] IRemoteStreamContent file, [FromForm] PriceOfferAPImportInput input)
    {
        return _priceOffersAppService.ValidateAndParseAPAsync(file, input);
    }

    [HttpPost]
    [Route("import/ap")]
    public Task<PriceOfferDto> ImportAPAsync(ExcelValidationResult<PriceOfferImportDto> validationResult, bool forceSubmit = false)
    {
        return _priceOffersAppService.ImportAPAsync(validationResult, forceSubmit);
    }
    [HttpPost]
    [Route("validate-and-parse/ds")]
    public Task<ExcelValidationResult<PriceOfferImportDto>> ValidateAndParseDSAsync([FromForm] IRemoteStreamContent file, [FromForm] PriceOfferDSImportInput input)
    {
        return _priceOffersAppService.ValidateAndParseDSAsync(file, input);
    }
    [HttpPost]
    [Route("import/ds")]
    public Task<PriceOfferDto> ImportDSAsync(ExcelValidationResult<PriceOfferImportDto> validationResult, bool forceSubmit = false)
    {
        return _priceOffersAppService.ImportDSAsync(validationResult, forceSubmit);
    }

    [HttpPost]
    [Route("validate-and-parse/nb")]
    public Task<ExcelValidationResult<PriceOfferImportDto>> ValidateAndParseNBAsync([FromForm] IRemoteStreamContent file, [FromForm] PriceOfferNBImportInput input)
    {
        return _priceOffersAppService.ValidateAndParseNBAsync(file, input);
    }

    [HttpPost]
    [Route("import/nb")]
    public Task<PriceOfferDto> ImportNBAsync(ExcelValidationResult<PriceOfferImportDto> validationResult, bool forceSubmit = false)
    {
        return _priceOffersAppService.ImportNBAsync(validationResult, forceSubmit);
    }

    [HttpPost]
    [Route("{id}/perform-action")]
    public Task<PriceOfferDto> PerformActionAsync(Guid id, ActionDto input)
    {
        return _priceOffersAppService.PerformActionAsync(id, input);
    }

    [HttpGet]
    [Route("{priceOfferId}/approvers")]
    public Task<List<ApproverDto>> GetListApproversAsync(Guid priceOfferId)
    {
        return _priceOffersAppService.GetListApproversAsync(priceOfferId);
    }

    [HttpPost]
    [Route("{id}/submit-project-result")]
    public Task<PriceOfferDto> SubmitProjectResultAsync(Guid id, SubmitProjectResultDto input)
    {
        return _priceOffersAppService.SubmitProjectResultAsync(id, input);
    }

    [HttpPost]
    [Route("{id}/confirm-preorder-status")]
    public Task<PriceOfferDto> ConfirmPreOrderStatusAsync(Guid id, ConfirmPreOrderStatusDto input)
    {
        return _priceOffersAppService.ConfirmPreOrderStatusAsync(id, input);
    }

    [HttpPost]
    [Route("{priceOfferId}/validate-update-landing-cost")]
    public Task<ExcelValidationResult<PriceOfferUpdateLandingCostImportDto>> ValidateAndParseUpdateLandingCostAsync(Guid priceOfferId, IRemoteStreamContent file)
    {
        return _priceOffersAppService.ValidateAndParseUpdateLandingCostAsync(priceOfferId, file);
    }

    [HttpPost]
    [Route("{priceOfferId}/import-update-landing-cost")]
    public Task<List<PriceOfferDetailDto>> ImportUpdateLandingCostAsync(Guid priceOfferId, ExcelValidationResult<PriceOfferUpdateLandingCostImportDto> input)
    {
        return _priceOffersAppService.ImportUpdateLandingCostAsync(priceOfferId, input);
    }

    [HttpPost]
    [Route("{id}/assign-special-input-price")]
    public Task<PriceOfferDto> AssignSpecialInputPrice(Guid id, AssignSpecialInputPriceDto input)
    {
        return _priceOffersAppService.AssignSpecialInputPrice(id, input);
    }

    [HttpPost]
    [Route("{priceOfferId}/messages")]
    public Task<MessageDto> SendMessageAsync(Guid priceOfferId, MessageCreateDto input)
    {
        return _priceOffersAppService.SendMessageAsync(priceOfferId, input);
    }

    [HttpPost]
    [Route("{priceOfferId}/cancel-details")]
    public Task<List<PriceOfferDetailDto>> CancelPriceOfferDetailsAsync(Guid priceOfferId, PriceOfferDetailCancelDto input)
    {
        return _priceOffersAppService.CancelPriceOfferDetailsAsync(priceOfferId, input);
    }
}