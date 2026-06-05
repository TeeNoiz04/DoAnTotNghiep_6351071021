import type {
  GetKeyAccountDetailReportsInput,
  GetKeyAccountGeneralReportsInput,
  GetKeyAccountUpgradesInput,
  GetKeyAccountsInput,
  KeyAccountCreateDto,
  KeyAccountDetailReportDto,
  KeyAccountDto,
  KeyAccountExcelDownloadDto,
  KeyAccountGeneralReportDto,
  KeyAccountUpdateDto,
  KeyAccountUpgradeDto,
  KeyAccountUpgradesInput,
  KeyAccountWithNavigationListDto,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { ApproverDto } from '../approval-routes/models';
import type { ActionDto, DownloadTokenResultDto } from '../shared/models';

@Injectable({
  providedIn: 'root',
})
export class KeyAccountService {
  apiName = 'Default';

  create = (input: KeyAccountCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, KeyAccountDto>(
      {
        method: 'POST',
        url: '/api/app/key-accounts',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/key-accounts/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, KeyAccountDto>(
      {
        method: 'GET',
        url: `/api/app/key-accounts/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getDownloadToken = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DownloadTokenResultDto>(
      {
        method: 'GET',
        url: '/api/app/key-accounts/download-token',
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetKeyAccountsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<KeyAccountWithNavigationListDto>>(
      {
        method: 'GET',
        url: '/api/app/key-accounts',
        params: {
          filterText: input.filterText,
          buyerId: input.buyerId,
          taxCode: input.taxCode,
          keyAccountCode: input.keyAccountCode,
          keyAccountShortName: input.keyAccountShortName,
          keyAccountName: input.keyAccountName,
          keyAccountType: input.keyAccountType,
          keyAccountClass: input.keyAccountClass,
          status: input.status,
          customerTaxCode: input.customerTaxCode,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListApprovers = (keyAccountId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ApproverDto[]>(
      {
        method: 'GET',
        url: `/api/app/key-accounts/${keyAccountId}/approvers`,
      },
      { apiName: this.apiName, ...config },
    );

  getListAsExcelFile = (input: KeyAccountExcelDownloadDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/key-accounts/as-excel-file',
        params: {
          downloadToken: input.downloadToken,
          filterText: input.filterText,
          buyerId: input.buyerId,
          keyAccountCode: input.keyAccountCode,
          keyAccountShortName: input.keyAccountShortName,
          keyAccountName: input.keyAccountName,
          keyAccountType: input.keyAccountType,
          keyAccountClass: input.keyAccountClass,
          status: input.status,
          customerTaxCode: input.customerTaxCode,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListDetailReport = (input: GetKeyAccountDetailReportsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<KeyAccountDetailReportDto>>(
      {
        method: 'GET',
        url: '/api/app/key-accounts/key-account-detail-report/get-list',
        params: {
          buyerId: input.buyerId,
          taxCode: input.taxCode,
          keyAccountCode: input.keyAccountCode,
          keyAccountName: input.keyAccountName,
          model: input.model,
          golfaCode: input.golfaCode,
          fromDate: input.fromDate,
          toDate: input.toDate,
          invoiceFromDate: input.invoiceFromDate,
          invoiceToDate: input.invoiceToDate,
          materialType: input.materialType,
          materialGroup: input.materialGroup,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListDetailReportAsExcelFile = (
    input: GetKeyAccountDetailReportsInput,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/key-accounts/key-account-detail-report/as-excel-file',
        params: {
          buyerId: input.buyerId,
          taxCode: input.taxCode,
          keyAccountCode: input.keyAccountCode,
          keyAccountName: input.keyAccountName,
          model: input.model,
          golfaCode: input.golfaCode,
          fromDate: input.fromDate,
          toDate: input.toDate,
          invoiceFromDate: input.invoiceFromDate,
          invoiceToDate: input.invoiceToDate,
          materialType: input.materialType,
          materialGroup: input.materialGroup,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListGeneralReport = (input: GetKeyAccountGeneralReportsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<KeyAccountGeneralReportDto>>(
      {
        method: 'GET',
        url: '/api/app/key-accounts/key-account-general-report/get-list',
        params: {
          buyer: input.buyer,
          taxCode: input.taxCode,
          keyAccountCode: input.keyAccountCode,
          keyAccountName: input.keyAccountName,
          fromDate: input.fromDate,
          toDate: input.toDate,
          invoiceFromDate: input.invoiceFromDate,
          invoiceToDate: input.invoiceToDate,
          materialType: input.materialType,
          materialGroup: input.materialGroup,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListGeneralReportAsExcelFile = (
    input: GetKeyAccountGeneralReportsInput,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/key-accounts/key-account-general-report/as-excel-file',
        params: {
          buyer: input.buyer,
          taxCode: input.taxCode,
          keyAccountCode: input.keyAccountCode,
          keyAccountName: input.keyAccountName,
          fromDate: input.fromDate,
          toDate: input.toDate,
          invoiceFromDate: input.invoiceFromDate,
          invoiceToDate: input.invoiceToDate,
          materialType: input.materialType,
          materialGroup: input.materialGroup,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListKeyAccountClassificationAsExcelFile = (
    input: GetKeyAccountUpgradesInput,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/key-accounts/report-keyAccount-classification',
        params: {
          filterText: input.filterText,
          buyerId: input.buyerId,
          financeYear: input.financeYear,
          taxCode: input.taxCode,
          keyAccountCode: input.keyAccountCode,
          keyAccountShortName: input.keyAccountShortName,
          keyAccountName: input.keyAccountName,
          keyAccountType: input.keyAccountType,
          keyAccountClass: input.keyAccountClass,
          status: input.status,
          materialType: input.materialType,
          customerTaxCode: input.customerTaxCode,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListPending = (input: GetKeyAccountsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<KeyAccountWithNavigationListDto>>(
      {
        method: 'GET',
        url: '/api/app/key-accounts/my-approval',
        params: {
          filterText: input.filterText,
          buyerId: input.buyerId,
          taxCode: input.taxCode,
          keyAccountCode: input.keyAccountCode,
          keyAccountShortName: input.keyAccountShortName,
          keyAccountName: input.keyAccountName,
          keyAccountType: input.keyAccountType,
          keyAccountClass: input.keyAccountClass,
          status: input.status,
          customerTaxCode: input.customerTaxCode,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListUpgrade = (input: GetKeyAccountUpgradesInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<KeyAccountUpgradeDto>>(
      {
        method: 'GET',
        url: '/api/app/key-accounts/key-account-upgrade/get-list',
        params: {
          filterText: input.filterText,
          buyerId: input.buyerId,
          financeYear: input.financeYear,
          taxCode: input.taxCode,
          keyAccountCode: input.keyAccountCode,
          keyAccountShortName: input.keyAccountShortName,
          keyAccountName: input.keyAccountName,
          keyAccountType: input.keyAccountType,
          keyAccountClass: input.keyAccountClass,
          status: input.status,
          materialType: input.materialType,
          customerTaxCode: input.customerTaxCode,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListUpgradeAsExcelFile = (input: GetKeyAccountUpgradesInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/key-accounts/key-account-upgrade/as-excel-file',
        params: {
          filterText: input.filterText,
          buyerId: input.buyerId,
          financeYear: input.financeYear,
          taxCode: input.taxCode,
          keyAccountCode: input.keyAccountCode,
          keyAccountShortName: input.keyAccountShortName,
          keyAccountName: input.keyAccountName,
          keyAccountType: input.keyAccountType,
          keyAccountClass: input.keyAccountClass,
          status: input.status,
          materialType: input.materialType,
          customerTaxCode: input.customerTaxCode,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  performAction = (id: string, input: ActionDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, KeyAccountDto>(
      {
        method: 'POST',
        url: `/api/app/key-accounts/${id}/perform-action`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: KeyAccountUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, KeyAccountDto>(
      {
        method: 'PUT',
        url: `/api/app/key-accounts/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  updateKeyAccountClass = (
    id: string,
    input: KeyAccountUpgradesInput,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, KeyAccountDto>(
      {
        method: 'PUT',
        url: `/api/app/key-accounts/${id}/upgrades`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
