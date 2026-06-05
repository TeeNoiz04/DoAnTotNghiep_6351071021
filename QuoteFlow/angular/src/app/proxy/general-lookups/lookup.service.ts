import type { GetBuyerTypeLookupInput, GetSalePICLookupInput } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { AddMoreItemHistoryDto } from '../add-more-item-histories/models';
import type { ApprovalHistoryDto } from '../approval-histories/models';
import type { GetCustomersInput } from '../customers/models';
import type {
  AccountCodeLookupDto,
  GicTypeLookupDto,
  KeyAccountLookupDto,
  LookupDto,
  StockCategoryLookupDto,
  SupplierBULookupDto,
  SupplierPOLookupDto,
  UserLookupDto,
} from '../shared/models';
import type { SpecialInputPriceLookupDto } from '../special-input-prices/models';

@Injectable({
  providedIn: 'root',
})
export class LookupService {
  apiName = 'Default';

  addMoreItemHistory = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AddMoreItemHistoryDto[]>(
      {
        method: 'GET',
        url: `/api/app/lookups/history-add-more/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getAccountNo = (materialCode: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<AccountCodeLookupDto>>(
      {
        method: 'GET',
        url: '/api/app/lookups/account-no',
        params: { materialCode },
      },
      { apiName: this.apiName, ...config },
    );

  getAllSalePICLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, UserLookupDto[]>(
      {
        method: 'GET',
        url: '/api/app/lookups/get-list-pic',
      },
      { apiName: this.apiName, ...config },
    );

  getBuyerLookup = (byPassBuyerCheck: boolean, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/buyers',
        params: { byPassBuyerCheck },
      },
      { apiName: this.apiName, ...config },
    );

  getBuyerLookupByBuyerType = (buyerTypeId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: `/api/app/lookups/buyers/${buyerTypeId}`,
      },
      { apiName: this.apiName, ...config },
    );

  getBuyerTypeLookup = (input: GetBuyerTypeLookupInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/buyer-types',
        params: { buyerTypeCode: input.buyerTypeCode },
      },
      { apiName: this.apiName, ...config },
    );

  getBuyersNotAssignedToMaterialGroup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/buyer-not-in-material-group',
      },
      { apiName: this.apiName, ...config },
    );

  getConditionLookupWorkflow = (type: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<string>>(
      {
        method: 'GET',
        url: '/api/app/lookups/workflow-conditions',
        params: { type },
      },
      { apiName: this.apiName, ...config },
    );

  getCurrencyCategoryLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/currency-category',
      },
      { apiName: this.apiName, ...config },
    );

  getCustomerTaxCodeLookup = (input: GetCustomersInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/customer-taxcode',
        params: {
          filterText: input.filterText,
          taxCode: input.taxCode,
          customerName: input.customerName,
          customerShortName: input.customerShortName,
          address: input.address,
          phone: input.phone,
          website: input.website,
          province: input.province,
          customerType: input.customerType,
          customerIndustry: input.customerIndustry,
          fromDate: input.fromDate,
          toDate: input.toDate,
          isDeactive: input.isDeactive,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getCustomerTypeLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/customer-types',
      },
      { apiName: this.apiName, ...config },
    );

  getDPOApprovalHistories = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ApprovalHistoryDto[]>(
      {
        method: 'GET',
        url: `/api/app/lookups/history-dpo/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getDamagedStockCategoryLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/stock-category/damaged',
      },
      { apiName: this.apiName, ...config },
    );

  getEUIndustryLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/eu-industries',
      },
      { apiName: this.apiName, ...config },
    );

  getFOCStockCategoryLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/stock-foc',
      },
      { apiName: this.apiName, ...config },
    );

  getFinancialCategoryLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/financial',
      },
      { apiName: this.apiName, ...config },
    );

  getFiscalYearOfDistributorTargetLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<number>>(
      {
        method: 'GET',
        url: '/api/app/lookups/fiscal-year-distributor-target',
      },
      { apiName: this.apiName, ...config },
    );

  getGicTypesLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<GicTypeLookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/gic-types',
      },
      { apiName: this.apiName, ...config },
    );

  getKAbySPO = (spoType: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, string[]>(
      {
        method: 'GET',
        url: '/api/app/lookups/katype-by-spotype',
        params: { spoType },
      },
      { apiName: this.apiName, ...config },
    );

  getKeyAccountClassChildLookup = (
    keyAccountTypeCode: string,
    hiddenNA: boolean,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: `/api/app/lookups/${keyAccountTypeCode}/key-account-classify-child`,
        params: { hiddenNA },
      },
      { apiName: this.apiName, ...config },
    );

  getKeyAccountClassLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/key-account-classes',
      },
      { apiName: this.apiName, ...config },
    );

  getKeyAccountEvaluationFinancialLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/key-account-evaluations/financial',
      },
      { apiName: this.apiName, ...config },
    );

  getKeyAccountEvaluationProductLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/key-account-evaluations/product',
      },
      { apiName: this.apiName, ...config },
    );

  getKeyAccountLookup = (buyerId: string, materialType: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<KeyAccountLookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/key-account',
        params: { buyerId, materialType },
      },
      { apiName: this.apiName, ...config },
    );

  getKeyAccountTypeLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/key-account-types',
      },
      { apiName: this.apiName, ...config },
    );

  getLevelLookupWorkflow = (type: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<number>>(
      {
        method: 'GET',
        url: '/api/app/lookups/workflow-levels',
        params: { type },
      },
      { apiName: this.apiName, ...config },
    );

  getListAllUserLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, UserLookupDto[]>(
      {
        method: 'GET',
        url: '/api/app/lookups/all-user',
      },
      { apiName: this.apiName, ...config },
    );

  getListUserLookupByName = (name: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, UserLookupDto[]>(
      {
        method: 'GET',
        url: '/api/app/lookups/user-by-username',
        params: { name },
      },
      { apiName: this.apiName, ...config },
    );

  getListUserPICLookupByInput = (input: GetSalePICLookupInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, UserLookupDto[]>(
      {
        method: 'GET',
        url: '/api/app/lookups/sale-pic-by-materialtype',
        params: { materialType: input.materialType, userName: input.userName },
      },
      { apiName: this.apiName, ...config },
    );

  getLocationLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/locations',
      },
      { apiName: this.apiName, ...config },
    );

  getMainStockCategoryLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/main-stock-category',
      },
      { apiName: this.apiName, ...config },
    );

  getMaterialGroupByType = (type: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/material-group-by-type',
        params: { type },
      },
      { apiName: this.apiName, ...config },
    );

  getMaterialGroupLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/material-groups',
      },
      { apiName: this.apiName, ...config },
    );

  getMaterialGroupPSILookup = (materialType: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/material-group-psi',
        params: { materialType },
      },
      { apiName: this.apiName, ...config },
    );

  getMaterialTypeLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/material-types',
      },
      { apiName: this.apiName, ...config },
    );

  getNationalityLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/nationality',
      },
      { apiName: this.apiName, ...config },
    );

  getProductCategoryLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/product',
      },
      { apiName: this.apiName, ...config },
    );

  getProjectTypeLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/project-types',
      },
      { apiName: this.apiName, ...config },
    );

  getSOHistories = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ApprovalHistoryDto[]>(
      {
        method: 'GET',
        url: `/api/app/lookups/history-so/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getSpecialInputPriceLookup = (materialType: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<SpecialInputPriceLookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/special-input-prices',
        params: { materialType },
      },
      { apiName: this.apiName, ...config },
    );

  getSpoTypeLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, string[]>(
      {
        method: 'GET',
        url: '/api/app/lookups/spo-types',
      },
      { apiName: this.apiName, ...config },
    );

  getStockCategoryLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/stock-category',
      },
      { apiName: this.apiName, ...config },
    );

  getStockCategoryLookupWithAvailableAmount = (
    materialCode: string,
    damagedStock: boolean,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, ListResultDto<StockCategoryLookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/stock-category-with-available-amount',
        params: { materialCode, damagedStock },
      },
      { apiName: this.apiName, ...config },
    );

  getSupplierBUBySupplierAndMaterialTypeLookup = (
    supplierId: string,
    materialType: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, ListResultDto<SupplierBULookupDto<string>>>(
      {
        method: 'GET',
        url: `/api/app/lookups/supplierBU/${supplierId}/material-type/${materialType}`,
      },
      { apiName: this.apiName, ...config },
    );

  getSupplierBUBySupplierLookup = (supplierId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<SupplierBULookupDto<string>>>(
      {
        method: 'GET',
        url: `/api/app/lookups/supplierBU/${supplierId}`,
      },
      { apiName: this.apiName, ...config },
    );

  getSupplierBULookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/supplierBU',
      },
      { apiName: this.apiName, ...config },
    );

  getSupplierByMaterialTypeLookup = (materialType: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: `/api/app/lookups/supplier/by-material-type/${materialType}`,
      },
      { apiName: this.apiName, ...config },
    );

  getSupplierLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/supplier',
      },
      { apiName: this.apiName, ...config },
    );

  getSupplierPOLookup = (
    materialType: string,
    currency: string,
    createSource: string,
    epa: boolean,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, SupplierPOLookupDto[]>(
      {
        method: 'GET',
        url: '/api/app/lookups/supplier-po',
        params: { materialType, currency, createSource, epa },
      },
      { apiName: this.apiName, ...config },
    );

  getUserLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, UserLookupDto[]>(
      {
        method: 'GET',
        url: '/api/app/lookups/users',
      },
      { apiName: this.apiName, ...config },
    );

  getVendorLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/vendors',
      },
      { apiName: this.apiName, ...config },
    );

  getWareHouseLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<LookupDto<string>>>(
      {
        method: 'GET',
        url: '/api/app/lookups/ware-house',
      },
      { apiName: this.apiName, ...config },
    );

  getYearDistinctPSI = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<number>>(
      {
        method: 'GET',
        url: '/api/app/lookups/fy-psi',
      },
      { apiName: this.apiName, ...config },
    );

  getYearLookup = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<number>>(
      {
        method: 'GET',
        url: '/api/app/lookups/fiscal-year',
      },
      { apiName: this.apiName, ...config },
    );

  getYearLookupKeyAccount = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<number>>(
      {
        method: 'GET',
        url: '/api/app/lookups/fiscal-year-key-account',
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
