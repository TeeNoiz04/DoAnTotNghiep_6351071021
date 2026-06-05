import type {
  GetListDetailDPOsInput,
  GetPurchaseOrdersInput,
  PurchaseOrderAddedDetailDPODto,
  PurchaseOrderCreateDto,
  PurchaseOrderDpoNoteDto,
  PurchaseOrderDto,
  PurchaseOrderLinkedDPODto,
  PurchaseOrderListDetailDPODto,
  PurchaseOrderListDetailDto,
  PurchaseOrderListDetailFOCDto,
  PurchaseOrderListDetailWarningStockDto,
  PurchaseOrderListQtyImportedDto,
  PurchaseOrderUpdateDto,
} from './models';
import type {
  PurchaseOrderDetailDto,
  PurchaseOrderDetailUpdateDto,
  PurchaseOrderDetailUpdateRequestInfoDto,
} from './purchase-order-details/models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { PurchaseOrdersSapImportsExcelDto } from '../purchase-orders-sap-imports/excel/models';
import type { ExcelValidationResult } from '../shared/excels/models';

@Injectable({
  providedIn: 'root',
})
export class PurchaseOrderService {
  apiName = 'Default';

  create = (input: PurchaseOrderCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PurchaseOrderDto>(
      {
        method: 'POST',
        url: '/api/app/purchase-orders',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  createDetailDPOASyncByInput = (
    input: PurchaseOrderAddedDetailDPODto[],
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/purchase-orders/added-list-detail-dpo',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/purchase-orders/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  deleteDetail = (id: string[], config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: '/api/app/purchase-orders/detail',
        params: { id },
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PurchaseOrderDto>(
      {
        method: 'GET',
        url: `/api/app/purchase-orders/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getDpoNotes = (poDetailId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PurchaseOrderDpoNoteDto[]>(
      {
        method: 'GET',
        url: `/api/app/purchase-orders/dpo-notes/${poDetailId}`,
      },
      { apiName: this.apiName, ...config },
    );

  getFASCMPOFile = (poId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/purchase-orders/fascm-export',
        params: { poId },
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetPurchaseOrdersInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PurchaseOrderDto>>(
      {
        method: 'GET',
        url: '/api/app/purchase-orders',
        params: {
          poNo: input.poNo,
          poDateMin: input.poDateMin,
          poDateMax: input.poDateMax,
          posapNo: input.posapNo,
          dpoNo: input.dpoNo,
          statusCode: input.statusCode,
          materialType: input.materialType,
          supplierBUId: input.supplierBUId,
          supplierBUCode: input.supplierBUCode,
          supplierId: input.supplierId,
          supplierCode: input.supplierCode,
          materialCode: input.materialCode,
          model: input.model,
          overDueDate: input.overDueDate,
          haveSAPPO: input.haveSAPPO,
          createSource: input.createSource,
          movingOrderNo: input.movingOrderNo,
          machineNumber: input.machineNumber,
          customer: input.customer,
          listPO: input.listPO,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListAsExcelFile = (input: GetPurchaseOrdersInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'POST',
        responseType: 'blob',
        url: '/api/app/purchase-orders/as-excel-file',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  getListDetail = (pOId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PurchaseOrderListDetailDto>>(
      {
        method: 'GET',
        url: `/api/app/purchase-orders/${pOId}/details`,
      },
      { apiName: this.apiName, ...config },
    );

  getListDetailDPO = (input: GetListDetailDPOsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PurchaseOrderListDetailDPODto[]>(
      {
        method: 'GET',
        url: '/api/app/purchase-orders/list-detail-dpo',
        params: {
          filterText: input.filterText,
          poId: input.poId,
          golfaCodes: input.golfaCodes,
          supplierBUId: input.supplierBUId,
          materialType: input.materialType,
          epa: input.epa,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListDetailFOC = (input: GetListDetailDPOsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PurchaseOrderListDetailFOCDto[]>(
      {
        method: 'GET',
        url: '/api/app/purchase-orders/list-detail-foc',
        params: {
          filterText: input.filterText,
          poId: input.poId,
          golfaCodes: input.golfaCodes,
          supplierBUId: input.supplierBUId,
          materialType: input.materialType,
          epa: input.epa,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListDetailWarningStock = (input: GetListDetailDPOsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PurchaseOrderListDetailWarningStockDto[]>(
      {
        method: 'GET',
        url: '/api/app/purchase-orders/list-detail-warning-stock',
        params: {
          filterText: input.filterText,
          poId: input.poId,
          golfaCodes: input.golfaCodes,
          supplierBUId: input.supplierBUId,
          materialType: input.materialType,
          epa: input.epa,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListLinkedDPO = (poDetailId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PurchaseOrderLinkedDPODto>>(
      {
        method: 'GET',
        url: `/api/app/purchase-orders/list-linked-dpo/${poDetailId}`,
      },
      { apiName: this.apiName, ...config },
    );

  getListPODataReport = (input: GetPurchaseOrdersInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'POST',
        responseType: 'blob',
        url: '/api/app/purchase-orders/po-data-report',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  getListPOExport = (input: GetPurchaseOrdersInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'POST',
        responseType: 'blob',
        url: '/api/app/purchase-orders/po-export',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  getListQtyImported = (pODetailId: string, isReceipt: boolean, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PurchaseOrderListQtyImportedDto>>(
      {
        method: 'GET',
        url: `/api/app/purchase-orders/list-qty-imported/${pODetailId}`,
        params: { isReceipt },
      },
      { apiName: this.apiName, ...config },
    );

  getNewPrice = (
    golfaCode: string,
    model: string,
    accountNo: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, number>(
      {
        method: 'GET',
        url: '/api/app/purchase-orders/new-price',
        params: { golfaCode, model, accountNo },
      },
      { apiName: this.apiName, ...config },
    );

  getStandardPricePOFile = (poId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/purchase-orders/standard-export',
        params: { poId },
      },
      { apiName: this.apiName, ...config },
    );

  importPO = (
    data: ExcelValidationResult<PurchaseOrdersSapImportsExcelDto>,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/purchase-orders/import',
        body: data,
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: PurchaseOrderUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PurchaseOrderDto>(
      {
        method: 'PUT',
        url: `/api/app/purchase-orders/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  updateDetail = (id: string, input: PurchaseOrderDetailUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PurchaseOrderDetailDto>(
      {
        method: 'PUT',
        url: '/api/app/purchase-orders/update-details',
        params: { id },
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  updateRequestInfo = (
    id: string,
    input: PurchaseOrderDetailUpdateRequestInfoDto,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, PurchaseOrderDetailDto>(
      {
        method: 'POST',
        url: `/api/app/purchase-orders/${id}/request-info`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  updateSendToSupplier = (
    id: string,
    sendToSupplier: boolean,
    concurrencyStamp?: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'PUT',
        url: '/api/app/purchase-orders/update-send-to-supplier',
        params: { id, sendToSupplier, concurrencyStamp },
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParse = (file: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExcelValidationResult<PurchaseOrdersSapImportsExcelDto>>(
      {
        method: 'POST',
        url: '/api/app/purchase-orders/import-po',
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
