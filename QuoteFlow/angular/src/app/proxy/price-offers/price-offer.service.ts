import type {
  AssignSpecialInputPriceDto,
  ConfirmPreOrderStatusDto,
  GetPriceOffersInput,
  ImportAddMoreItemsInput,
  PriceOfferAPImportInput,
  PriceOfferCreateDto,
  PriceOfferDSImportInput,
  PriceOfferDto,
  PriceOfferExcelDownloadDto,
  PriceOfferImportDto,
  PriceOfferImportInput,
  PriceOfferNBImportInput,
  PriceOfferUpdateDto,
  PriceOfferWithNavigationListDto,
  SubmitProjectResultDto,
} from './models';
import type {
  GetPriceOfferCustomersInput,
  PriceOfferCustomerDto,
} from './price-offer-customers/models';
import type {
  GetPriceOfferDetailsInput,
  PriceOfferDetailCancelDto,
  PriceOfferDetailDto,
  PriceOfferDetailImportDto,
  PriceOfferUpdateLandingCostImportDto,
} from './price-offer-details/models';
import type { GetPriceOfferMessagesInput } from './price-offer-messages/models';
import type {
  GetPriceOfferReportDetailsInput,
  PriceOfferReportDetailDto,
} from './price-offer-report-details/models';
import type {
  GetPriceOfferReportGeneralsInput,
  PriceOfferReportGeneralDto,
} from './price-offer-report-generals/models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { ApproverDto } from '../approval-routes/models';
import type { MessageCreateDto, MessageDto } from '../messages/models';
import type { ExcelValidationResult } from '../shared/excels/models';
import type { ActionDto, DownloadTokenResultDto } from '../shared/models';

@Injectable({
  providedIn: 'root',
})
export class PriceOfferService {
  apiName = 'Default';

  assignSpecialInputPriceByIdAndInput = (
    id: string,
    input: AssignSpecialInputPriceDto,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, PriceOfferDto>(
      {
        method: 'POST',
        url: `/api/app/price-offers/${id}/assign-special-input-price`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  cancelPriceOfferDetails = (
    priceOfferId: string,
    input: PriceOfferDetailCancelDto,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, PriceOfferDetailDto[]>(
      {
        method: 'POST',
        url: `/api/app/price-offers/${priceOfferId}/cancel-details`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  confirmPreOrderStatus = (
    id: string,
    input: ConfirmPreOrderStatusDto,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, PriceOfferDto>(
      {
        method: 'POST',
        url: `/api/app/price-offers/${id}/confirm-preorder-status`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  create = (input: PriceOfferCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PriceOfferDto>(
      {
        method: 'POST',
        url: '/api/app/price-offers',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/price-offers/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PriceOfferDto>(
      {
        method: 'GET',
        url: `/api/app/price-offers/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getDownloadToken = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DownloadTokenResultDto>(
      {
        method: 'GET',
        url: '/api/app/price-offers/download-token',
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetPriceOffersInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PriceOfferWithNavigationListDto>>(
      {
        method: 'GET',
        url: '/api/app/price-offers',
        params: {
          filterText: input.filterText,
          priceOfferType: input.priceOfferType,
          materialType: input.materialType,
          buyerId: input.buyerId,
          priceOfferCode: input.priceOfferCode,
          customerTaxCode: input.customerTaxCode,
          customerName: input.customerName,
          approvalStatus: input.approvalStatus,
          createdFrom: input.createdFrom,
          createdTo: input.createdTo,
          relatedToMe: input.relatedToMe,
          projectName: input.projectName,
          projectResultStatus: input.projectResultStatus,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListApprovers = (priceOfferId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ApproverDto[]>(
      {
        method: 'GET',
        url: `/api/app/price-offers/${priceOfferId}/approvers`,
      },
      { apiName: this.apiName, ...config },
    );

  getListAsExcelFile = (input: PriceOfferExcelDownloadDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/price-offers/as-excel-file',
        params: {
          downloadToken: input.downloadToken,
          filterText: input.filterText,
          priceOfferCode: input.priceOfferCode,
          buyerId: input.buyerId,
          buyerTypeId: input.buyerTypeId,
          materialType: input.materialType,
          locationId: input.locationId,
          locationOld: input.locationOld,
          projectName: input.projectName,
          projectTypeId: input.projectTypeId,
          euIndustryId: input.euIndustryId,
          application: input.application,
          country: input.country,
          province: input.province,
          detailedAddress: input.detailedAddress,
          competitorBrand: input.competitorBrand,
          priceGapWithCompetitor: input.priceGapWithCompetitor,
          decisionRight: input.decisionRight,
          poPlannedDateMin: input.poPlannedDateMin,
          poPlannedDateMax: input.poPlannedDateMax,
          deliveryDateMin: input.deliveryDateMin,
          deliveryDateMax: input.deliveryDateMax,
          upcomingPotentialProjects: input.upcomingPotentialProjects,
          otherPJInformation: input.otherPJInformation,
          fileName: input.fileName,
          note: input.note,
          closeDateMin: input.closeDateMin,
          closeDateMax: input.closeDateMax,
          totalAmountMin: input.totalAmountMin,
          totalAmountMax: input.totalAmountMax,
          approvalStatus: input.approvalStatus,
          projectResultStatus: input.projectResultStatus,
          accountNo: input.accountNo,
          keyAccountId: input.keyAccountId,
          keyAccountTypeId: input.keyAccountTypeId,
          keyAccountClassId: input.keyAccountClassId,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListCustomers = (
    priceOfferId: string,
    input: GetPriceOfferCustomersInput,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, PagedResultDto<PriceOfferCustomerDto>>(
      {
        method: 'GET',
        url: `/api/app/price-offers/${input.priceOfferId}/customers`,
        params: {
          priceOfferId,
          filterText: input.filterText,
          saleChannel: input.saleChannel,
          customerId: input.customerId,
          customerTaxCode: input.customerTaxCode,
          customerName: input.customerName,
          customerAddress: input.customerAddress,
          customerNationality: input.customerNationality,
          customerType: input.customerType,
          note: input.note,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListDetailAsExcelFile = (
    input: GetPriceOfferReportDetailsInput,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/price-offers/report/detail-export',
        params: {
          from: input.from,
          to: input.to,
          golfaCode: input.golfaCode,
          modelName: input.modelName,
          buyer: input.buyer,
          priceOfferCode: input.priceOfferCode,
          priceOfferName: input.priceOfferName,
          materialGroup: input.materialGroup,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListDetails = (
    priceOfferId: string,
    input: GetPriceOfferDetailsInput,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, PagedResultDto<PriceOfferDetailDto>>(
      {
        method: 'GET',
        url: `/api/app/price-offers/${priceOfferId}/details`,
        params: {
          filterText: input.filterText,
          golfaCode: input.golfaCode,
          modelName: input.modelName,
          specialSpec1: input.specialSpec1,
          specialSpec2: input.specialSpec2,
          dpoUsedMin: input.dpoUsedMin,
          dpoUsedMax: input.dpoUsedMax,
          qtyMin: input.qtyMin,
          qtyMax: input.qtyMax,
          standardPriceMin: input.standardPriceMin,
          standardPriceMax: input.standardPriceMax,
          standardAmountMin: input.standardAmountMin,
          standardAmountMax: input.standardAmountMax,
          buyerPriceMin: input.buyerPriceMin,
          buyerPriceMax: input.buyerPriceMax,
          requestedAmountMin: input.requestedAmountMin,
          requestedAmountMax: input.requestedAmountMax,
          requestedDiscountRatioMin: input.requestedDiscountRatioMin,
          requestedDiscountRatioMax: input.requestedDiscountRatioMax,
          priceToCustomerMin: input.priceToCustomerMin,
          priceToCustomerMax: input.priceToCustomerMax,
          mevnOfferPriceMin: input.mevnOfferPriceMin,
          mevnOfferPriceMax: input.mevnOfferPriceMax,
          competitorBrand: input.competitorBrand,
          competitorModel: input.competitorModel,
          competitorPriceMin: input.competitorPriceMin,
          competitorPriceMax: input.competitorPriceMax,
          landingCostMin: input.landingCostMin,
          landingCostMax: input.landingCostMax,
          inputPriceMin: input.inputPriceMin,
          inputPriceMax: input.inputPriceMax,
          inputCurrency: input.inputCurrency,
          managerMarginMin: input.managerMarginMin,
          managerMarginMax: input.managerMarginMax,
          priceOfferDetailMarginMin: input.priceOfferDetailMarginMin,
          priceOfferDetailMarginMax: input.priceOfferDetailMarginMax,
          accountCode: input.accountCode,
          note: input.note,
          importGuid: input.importGuid,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListDetailsAsExcelFile = (
    priceOfferId: string,
    downloadToken: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: `/api/app/price-offers/${priceOfferId}/details/as-excel-file`,
        params: { downloadToken },
      },
      { apiName: this.apiName, ...config },
    );

  getListGeneralAsExcelFile = (
    input: GetPriceOfferReportGeneralsInput,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/price-offers/report/general-export',
        params: {
          from: input.from,
          to: input.to,
          buyer: input.buyer,
          customerName: input.customerName,
          priceOfferCode: input.priceOfferCode,
          priceOfferName: input.priceOfferName,
          location: input.location,
          status: input.status,
          materialType: input.materialType,
          orderMin: input.orderMin,
          orderMax: input.orderMax,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListMessages = (
    priceOfferId: string,
    input: GetPriceOfferMessagesInput,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, PagedResultDto<MessageDto>>(
      {
        method: 'GET',
        url: `/api/app/price-offers/${priceOfferId}/messages`,
        params: {
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListReportDetail = (input: GetPriceOfferReportDetailsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PriceOfferReportDetailDto>>(
      {
        method: 'GET',
        url: '/api/app/price-offers/report/detail',
        params: {
          from: input.from,
          to: input.to,
          golfaCode: input.golfaCode,
          modelName: input.modelName,
          buyer: input.buyer,
          priceOfferCode: input.priceOfferCode,
          priceOfferName: input.priceOfferName,
          materialGroup: input.materialGroup,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListReportGeneral = (input: GetPriceOfferReportGeneralsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PriceOfferReportGeneralDto>>(
      {
        method: 'GET',
        url: '/api/app/price-offers/report/general',
        params: {
          from: input.from,
          to: input.to,
          buyer: input.buyer,
          customerName: input.customerName,
          priceOfferCode: input.priceOfferCode,
          priceOfferName: input.priceOfferName,
          location: input.location,
          status: input.status,
          materialType: input.materialType,
          orderMin: input.orderMin,
          orderMax: input.orderMax,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getMyApprovalsList = (input: GetPriceOffersInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PriceOfferWithNavigationListDto>>(
      {
        method: 'GET',
        url: '/api/app/price-offers/my-approvals',
        params: {
          filterText: input.filterText,
          priceOfferType: input.priceOfferType,
          materialType: input.materialType,
          buyerId: input.buyerId,
          priceOfferCode: input.priceOfferCode,
          customerTaxCode: input.customerTaxCode,
          customerName: input.customerName,
          approvalStatus: input.approvalStatus,
          createdFrom: input.createdFrom,
          createdTo: input.createdTo,
          relatedToMe: input.relatedToMe,
          projectName: input.projectName,
          projectResultStatus: input.projectResultStatus,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  importAP = (
    validationResult: ExcelValidationResult<PriceOfferImportDto>,
    forceSubmit?: boolean,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, PriceOfferDto>(
      {
        method: 'POST',
        url: '/api/app/price-offers/import/ap',
        params: { forceSubmit },
        body: validationResult,
      },
      { apiName: this.apiName, ...config },
    );

  importAddMoreItemDetail = (
    priceOfferId: string,
    input: ImportAddMoreItemsInput,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, PriceOfferDetailDto[]>(
      {
        method: 'POST',
        url: `/api/app/price-offers/import/${priceOfferId}/add-more-items`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  importDS = (
    validationResult: ExcelValidationResult<PriceOfferImportDto>,
    forceSubmit?: boolean,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, PriceOfferDto>(
      {
        method: 'POST',
        url: '/api/app/price-offers/import/ds',
        params: { forceSubmit },
        body: validationResult,
      },
      { apiName: this.apiName, ...config },
    );

  importNB = (
    validationResult: ExcelValidationResult<PriceOfferImportDto>,
    forceSubmit?: boolean,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, PriceOfferDto>(
      {
        method: 'POST',
        url: '/api/app/price-offers/import/nb',
        params: { forceSubmit },
        body: validationResult,
      },
      { apiName: this.apiName, ...config },
    );

  importPP = (
    validationResult: ExcelValidationResult<PriceOfferImportDto>,
    forceSubmit?: boolean,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, PriceOfferDto>(
      {
        method: 'POST',
        url: '/api/app/price-offers/import/pp',
        params: { forceSubmit },
        body: validationResult,
      },
      { apiName: this.apiName, ...config },
    );

  importUpdateLandingCost = (
    priceOfferId: string,
    input: ExcelValidationResult<PriceOfferUpdateLandingCostImportDto>,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, PriceOfferDetailDto[]>(
      {
        method: 'POST',
        url: `/api/app/price-offers/${priceOfferId}/import-update-landing-cost`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  performAction = (id: string, input: ActionDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PriceOfferDto>(
      {
        method: 'POST',
        url: `/api/app/price-offers/${id}/perform-action`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  sendMessage = (priceOfferId: string, input: MessageCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, MessageDto>(
      {
        method: 'POST',
        url: `/api/app/price-offers/${priceOfferId}/messages`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  submitProjectResult = (
    id: string,
    input: SubmitProjectResultDto,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, PriceOfferDto>(
      {
        method: 'POST',
        url: `/api/app/price-offers/${id}/submit-project-result`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: PriceOfferUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PriceOfferDto>(
      {
        method: 'PUT',
        url: `/api/app/price-offers/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseAP = (
    file: FormData,
    input: PriceOfferAPImportInput,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, ExcelValidationResult<PriceOfferImportDto>>(
      {
        method: 'POST',
        url: '/api/app/price-offers/validate-and-parse/ap',
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseAddMoreItemDetail = (
    priceOfferId: string,
    file: FormData,
    getPriceAutomatically: boolean,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, ExcelValidationResult<PriceOfferDetailImportDto>>(
      {
        method: 'POST',
        url: `/api/app/price-offers/validate-and-parse/${priceOfferId}/add-more-items`,
        params: { getPriceAutomatically },
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseDS = (
    file: FormData,
    input: PriceOfferDSImportInput,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, ExcelValidationResult<PriceOfferImportDto>>(
      {
        method: 'POST',
        url: '/api/app/price-offers/validate-and-parse/ds',
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseNB = (
    file: FormData,
    input: PriceOfferNBImportInput,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, ExcelValidationResult<PriceOfferImportDto>>(
      {
        method: 'POST',
        url: '/api/app/price-offers/validate-and-parse/nb',
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParsePP = (
    file: FormData,
    input: PriceOfferImportInput,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, ExcelValidationResult<PriceOfferImportDto>>(
      {
        method: 'POST',
        url: '/api/app/price-offers/validate-and-parse/pp',
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseUpdateLandingCost = (
    priceOfferId: string,
    file: FormData,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, ExcelValidationResult<PriceOfferUpdateLandingCostImportDto>>(
      {
        method: 'POST',
        url: `/api/app/price-offers/${priceOfferId}/validate-update-landing-cost`,
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
