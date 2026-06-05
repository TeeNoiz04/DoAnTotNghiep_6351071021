import type {
  SaleOrderExcelDto,
  SaleOrderGICFOCExcelDto,
  SaleOrderGICInternalUseChangeExcelDto,
  SaleOrderGICInternalUseExcelDto,
  SaleOrderGICWarrantyExcelDto,
  SaleOrderGICWriteOffExcelDto,
} from './excel/models';
import type {
  GetSaleOrderListDetailDPOsInput,
  GetSaleOrderListDetailGICsInput,
  GetSaleOrdersInput,
  SODetailExtrafeeUpdateInput,
  SaleOrderAddedDetailDPODto,
  SaleOrderCreateDto,
  SaleOrderDto,
  SaleOrderListDetailDPODto,
  SaleOrderListDetailGICDto,
  SaleOrderUpdateDto,
} from './models';
import type { SaleOrderDetailDto, SaleOrderDetailUpdateDto } from './sale-order-details/models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { ExcelValidationResult } from '../shared/excels/models';

@Injectable({
  providedIn: 'root',
})
export class SaleOrderService {
  apiName = 'Default';

  confirmDeliveryById = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: `/api/app/sale-orders/confirm-delivery/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  confirmDeliveryGICById = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: `/api/app/sale-orders/confirm-delivery-gic/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  create = (input: SaleOrderCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SaleOrderDto>(
      {
        method: 'POST',
        url: '/api/app/sale-orders',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  createDetailDPO = (input: SaleOrderAddedDetailDPODto[], config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/sale-orders/added-list-detail-dpo',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/sale-orders/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  deleteDetail = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/sale-orders/detail/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SaleOrderDto>(
      {
        method: 'GET',
        url: `/api/app/sale-orders/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetSaleOrdersInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<SaleOrderDto>>(
      {
        method: 'GET',
        url: '/api/app/sale-orders',
        params: {
          soNo: input.soNo,
          sosapNo: input.sosapNo,
          materialType: input.materialType,
          dpoNo: input.dpoNo,
          materialCode: input.materialCode,
          invoiceNo: input.invoiceNo,
          lstSO: input.lstSO,
          buyerType: input.buyerType,
          buyerId: input.buyerId,
          buyerCode: input.buyerCode,
          buyerName: input.buyerName,
          orderDateMin: input.orderDateMin,
          orderDateMax: input.orderDateMax,
          statusCode: input.statusCode,
          model: input.model,
          taxCode: input.taxCode,
          buyerTypeId: input.buyerTypeId,
          soDateFrom: input.soDateFrom,
          soDateTo: input.soDateTo,
          vatDateFrom: input.vatDateFrom,
          vatDateTo: input.vatDateTo,
          materialGroup: input.materialGroup,
          soType: input.soType,
          completelyClosed: input.completelyClosed,
          gicType: input.gicType,
          gicProcess: input.gicProcess,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListAsExcelFile = (input: GetSaleOrdersInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'POST',
        responseType: 'blob',
        url: '/api/app/sale-orders/as-list-excel/export-sap-so',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  getListDetailDPO = (input: GetSaleOrderListDetailDPOsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<SaleOrderListDetailDPODto>>(
      {
        method: 'GET',
        url: '/api/app/sale-orders/list-detail-dpo',
        params: {
          buyerId: input.buyerId,
          materialType: input.materialType,
          vat: input.vat,
          stockCategoryId: input.stockCategoryId,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListDetailGIC = (input: GetSaleOrderListDetailGICsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<SaleOrderListDetailGICDto>>(
      {
        method: 'GET',
        url: '/api/app/sale-orders/list-detail-gic',
        params: {
          gicType: input.gicType,
          gicProcess: input.gicProcess,
          buyerId: input.buyerId,
          materialType: input.materialType,
          vat: input.vat,
          stockCategoryId: input.stockCategoryId,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListGICAsExcelFile = (input: GetSaleOrdersInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'POST',
        responseType: 'blob',
        url: '/api/app/sale-orders/export-so-gic-data',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  getListGICInternalUseChangeAsExcelFile = (
    input: GetSaleOrdersInput,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, Blob>(
      {
        method: 'POST',
        responseType: 'blob',
        url: '/api/app/sale-orders/as-list-excel/export-internal-use-change',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  getListSODataAsExcelFile = (input: GetSaleOrdersInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'POST',
        responseType: 'blob',
        url: '/api/app/sale-orders/export-so-data',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  importSO = (data: ExcelValidationResult<SaleOrderExcelDto>, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/sale-orders/import',
        body: data,
      },
      { apiName: this.apiName, ...config },
    );

  importSOGICFOC = (
    data: ExcelValidationResult<SaleOrderGICFOCExcelDto>,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/sale-orders/import-gic-foc',
        body: data,
      },
      { apiName: this.apiName, ...config },
    );

  importSOGICInternalUse = (
    data: ExcelValidationResult<SaleOrderGICInternalUseExcelDto>,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/sale-orders/import-gic-internal-use',
        body: data,
      },
      { apiName: this.apiName, ...config },
    );

  importSOGICInternalUseChange = (
    data: ExcelValidationResult<SaleOrderGICInternalUseChangeExcelDto>,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/sale-orders/import-gic-internal-use-change',
        body: data,
      },
      { apiName: this.apiName, ...config },
    );

  importSOGICWarranty = (
    data: ExcelValidationResult<SaleOrderGICWarrantyExcelDto>,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/sale-orders/import-gic-warranty',
        body: data,
      },
      { apiName: this.apiName, ...config },
    );

  importSOGICWriteOff = (
    data: ExcelValidationResult<SaleOrderGICWriteOffExcelDto>,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/sale-orders/import-gic-write-off',
        body: data,
      },
      { apiName: this.apiName, ...config },
    );

  reOpenSO = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/sale-orders/re-open-so',
        params: { id },
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: SaleOrderUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SaleOrderDto>(
      {
        method: 'PUT',
        url: `/api/app/sale-orders/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  updateDetail = (id: string, input: SaleOrderDetailUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SaleOrderDetailDto>(
      {
        method: 'PUT',
        url: '/api/app/sale-orders/update-detail',
        params: { id },
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  updateNote = (id: string, input: SaleOrderDetailUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SaleOrderDetailDto>(
      {
        method: 'PUT',
        url: '/api/app/sale-orders/update-note',
        params: { id },
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  updateSODetailExtrafee = (input: SODetailExtrafeeUpdateInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'PUT',
        url: '/api/app/sale-orders/so-detail-extrafee',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParse = (file: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExcelValidationResult<SaleOrderExcelDto>>(
      {
        method: 'POST',
        url: '/api/app/sale-orders/validate-and-parse',
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseGICFOC = (file: FormData, gicType: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExcelValidationResult<SaleOrderGICFOCExcelDto>>(
      {
        method: 'POST',
        url: '/api/app/sale-orders/validate-and-parse-gic-foc',
        params: { gicType },
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseGICInternalUse = (
    file: FormData,
    gicType: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, ExcelValidationResult<SaleOrderGICInternalUseExcelDto>>(
      {
        method: 'POST',
        url: '/api/app/sale-orders/validate-and-parse-gic-internal-use',
        params: { gicType },
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseGICInternalUseChange = (
    file: FormData,
    gicType: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, ExcelValidationResult<SaleOrderGICInternalUseChangeExcelDto>>(
      {
        method: 'POST',
        url: '/api/app/sale-orders/validate-and-parse-gic-internal-use-change',
        params: { gicType },
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseGICWarranty = (file: FormData, gicType: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExcelValidationResult<SaleOrderGICWarrantyExcelDto>>(
      {
        method: 'POST',
        url: '/api/app/sale-orders/validate-and-parse-gic-warranty',
        params: { gicType },
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseGICWriteOff = (file: FormData, gicType: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExcelValidationResult<SaleOrderGICWriteOffExcelDto>>(
      {
        method: 'POST',
        url: '/api/app/sale-orders/validate-and-parse-gic-write-off',
        params: { gicType },
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
