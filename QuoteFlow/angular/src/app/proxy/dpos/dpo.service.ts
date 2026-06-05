import type {
  DPODetailCreateDto,
  DPODetailDto,
  DPODetailLockStockDto,
  DPODetailUpdateLockStockDto,
  GetDPODetailsInput,
} from './dpodetails/models';
import type { GetDPOMessagesInput } from './dpomessages/models';
import type {
  BatchAutoUnlockStockDto,
  DPOAddExtraFeeDto,
  DPOAllocateGKRDto,
  DPOCancelDto,
  DPOConfirmNoteDto,
  DPOCreateDto,
  DPODataReportDto,
  DPODto,
  DPOExcelDownloadDto,
  DPOExportDataInputDto,
  DPOListPOsDto,
  DPOLockOnOrderStockDto,
  DPOLockShipmentAutoDto,
  DPOLockShipmentDto,
  DPOLockShipmentItemUpdateDto,
  DPOLockStockAutoDto,
  DPOLockStockAutoV2Dto,
  DPOLockStockEtaEtdDto,
  DPOProcessingReportDto,
  DPOUpdateDto,
  DpoGkrAllocationDto,
  GetDPOReportInputDto,
  GetDPOsInput,
  ImportDPODto,
  ImportDPOInput,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { MaterialStockLockStockDto } from '../materials/material-stocks/material-stock-lock-stocks/models';
import type { MessageCreateDto, MessageDto } from '../messages/models';
import type {
  SaleOrderListModalDPODto,
  SaleOrderListModalDeliveryDto,
} from '../sale-orders/models';
import type { ExcelValidationResult } from '../shared/excels/models';
import type { DownloadTokenResultDto, NoteMetadataDto } from '../shared/models';

@Injectable({
  providedIn: 'root',
})
export class DPOService {
  apiName = 'Default';

  addExtraFee = (input: DPOAddExtraFeeDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DPODetailDto[]>(
      {
        method: 'POST',
        url: '/api/app/dpos/add-extra-fee',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  allocateGkrToDpo = (input: DPOAllocateGKRDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/dpos/allocate-gkr',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  approve = (id: string, input: NoteMetadataDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DPODto>(
      {
        method: 'POST',
        url: `/api/app/dpos/${id}/approve`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  batchAutoUnlockOnOrderStock = (input: BatchAutoUnlockStockDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/dpos/details/batch-auto-unlock-on-order-stock',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  batchAutoUnlockStock = (input: BatchAutoUnlockStockDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/dpos/details/batch-auto-unlock',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  cancel = (id: string, input: DPOCancelDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DPODto>(
      {
        method: 'POST',
        url: `/api/app/dpos/${id}/cancel`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  confirmLockOnOrder = (id: string, input: NoteMetadataDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DPODto>(
      {
        method: 'POST',
        url: `/api/app/dpos/${id}/confirm-lock-on-order`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  confirmLockStock = (id: string, input: NoteMetadataDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DPODto>(
      {
        method: 'POST',
        url: `/api/app/dpos/${id}/confirm-lock-stock`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  confirmNote = (input: DPOConfirmNoteDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DPODetailDto[]>(
      {
        method: 'POST',
        url: '/api/app/dpos/confirm-note',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  create = (input: DPOCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DPODto>(
      {
        method: 'POST',
        url: '/api/app/dpos',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  createDPODetail = (input: DPODetailCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DPODetailDto>(
      {
        method: 'POST',
        url: '/api/app/dpos/details',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/dpos/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  deleteDPODetail = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/dpos/details/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  deleteLockOnOrderStock = (
    dpoDetailId: string,
    poDetailId: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/dpos/${dpoDetailId}/lock-on-order-stocks/${poDetailId}`,
      },
      { apiName: this.apiName, ...config },
    );

  deleteLockStock = (dpoDetailId: string, lockStockId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/dpos/${dpoDetailId}/lock-stocks/${lockStockId}`,
      },
      { apiName: this.apiName, ...config },
    );

  exportDataAsExcel = (input: DPOExportDataInputDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/dpos/export-data-excel',
        params: {
          dpoNo: input.dpoNo,
          status: input.status,
          golfaCode: input.golfaCode,
          model: input.model,
          poNo: input.poNo,
          customerName: input.customerName,
          fromDate: input.fromDate,
          toDate: input.toDate,
          buyerTypeId: input.buyerTypeId,
          buyerId: input.buyerId,
          materialType: input.materialType,
          supplierCode: input.supplierCode,
          spoCode: input.spoCode,
          taxCode: input.taxCode,
          materialGroup: input.materialGroup,
        },
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DPODto>(
      {
        method: 'GET',
        url: `/api/app/dpos/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getAvailableGkrsForAllocation = (dpoId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DpoGkrAllocationDto[]>(
      {
        method: 'GET',
        url: `/api/app/dpos/${dpoId}/available-gkrs`,
      },
      { apiName: this.apiName, ...config },
    );

  getDPODetail = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DPODetailDto>(
      {
        method: 'GET',
        url: `/api/app/dpos/details/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getDataDPOProcessingReport = (input: GetDPOReportInputDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DPOProcessingReportDto[]>(
      {
        method: 'GET',
        url: '/api/app/dpos/list-report-r24',
        params: {
          buyerTypeId: input.buyerTypeId,
          buyerId: input.buyerId,
          fromDate: input.fromDate,
          toDate: input.toDate,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getDataDPOReport = (input: GetDPOReportInputDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DPODataReportDto[]>(
      {
        method: 'GET',
        url: '/api/app/dpos/data-report',
        params: {
          buyerTypeId: input.buyerTypeId,
          buyerId: input.buyerId,
          fromDate: input.fromDate,
          toDate: input.toDate,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getDownloadToken = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DownloadTokenResultDto>(
      {
        method: 'GET',
        url: '/api/app/dpos/download-token',
      },
      { apiName: this.apiName, ...config },
    );

  getDownloadTokenDPODtail = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DownloadTokenResultDto>(
      {
        method: 'GET',
        url: '/api/app/dpos/details/download-token',
      },
      { apiName: this.apiName, ...config },
    );

  getGkrAllocations = (dpoId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DpoGkrAllocationDto[]>(
      {
        method: 'GET',
        url: `/api/app/dpos/${dpoId}/gkr-allocations`,
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetDPOsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<DPODto>>(
      {
        method: 'GET',
        url: '/api/app/dpos',
        params: {
          filterText: input.filterText,
          dpoNo: input.dpoNo,
          materialCode: input.materialCode,
          modelName: input.modelName,
          poNo: input.poNo,
          customerName: input.customerName,
          orderDateMin: input.orderDateMin,
          orderDateMax: input.orderDateMax,
          buyerId: input.buyerId,
          materialType: input.materialType,
          supplierId: input.supplierId,
          specialPriceCode: input.specialPriceCode,
          materialGroup: input.materialGroup,
          taxCode: input.taxCode,
          salesOrg: input.salesOrg,
          dpoType: input.dpoType,
          dpoSubType: input.dpoSubType,
          costCenter: input.costCenter,
          status: input.status,
          buyerTypeId: input.buyerTypeId,
          buyerShortName: input.buyerShortName,
          buyerDescription: input.buyerDescription,
          totalAmountMin: input.totalAmountMin,
          totalAmountMax: input.totalAmountMax,
          remark: input.remark,
          fileName: input.fileName,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListAsExcelFile = (input: DPOExcelDownloadDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/dpos/as-excel-file',
        params: {
          downloadToken: input.downloadToken,
          filterText: input.filterText,
          dpoNo: input.dpoNo,
          dpoType: input.dpoType,
          dpoSubType: input.dpoSubType,
          materialType: input.materialType,
          costCenter: input.costCenter,
          status: input.status,
          buyerTypeId: input.buyerTypeId,
          buyerId: input.buyerId,
          buyerShortName: input.buyerShortName,
          buyerDescription: input.buyerDescription,
          orderDateMin: input.orderDateMin,
          orderDateMax: input.orderDateMax,
          totalAmountMin: input.totalAmountMin,
          totalAmountMax: input.totalAmountMax,
          remark: input.remark,
          fileName: input.fileName,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListAvailablePOs = (
    dpoId: string,
    dpoDetailId: string,
    materialCode: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, ListResultDto<DPOListPOsDto>>(
      {
        method: 'GET',
        url: `/api/app/dpos/${dpoId}/details/${dpoDetailId}/list-available-pos`,
        params: { materialCode },
      },
      { apiName: this.apiName, ...config },
    );

  getListDPODetail = (input: GetDPODetailsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<DPODetailDto>>(
      {
        method: 'GET',
        url: '/api/app/dpos/details',
        params: {
          filterText: input.filterText,
          dpoId: input.dpoId,
          status: input.status,
          golfaCode: input.golfaCode,
          model: input.model,
          spec1: input.spec1,
          spec2: input.spec2,
          qtyMin: input.qtyMin,
          qtyMax: input.qtyMax,
          unitPriceMin: input.unitPriceMin,
          unitPriceMax: input.unitPriceMax,
          amountMin: input.amountMin,
          amountMax: input.amountMax,
          requestedETAMin: input.requestedETAMin,
          requestedETAMax: input.requestedETAMax,
          spoId: input.spoId,
          spoCode: input.spoCode,
          customerTaxCode: input.customerTaxCode,
          customerName: input.customerName,
          lockStockMin: input.lockStockMin,
          lockStockMax: input.lockStockMax,
          lockStockSOMin: input.lockStockSOMin,
          lockStockSOMax: input.lockStockSOMax,
          lockShipmentMin: input.lockShipmentMin,
          lockShipmentMax: input.lockShipmentMax,
          deliveredMin: input.deliveredMin,
          deliveredMax: input.deliveredMax,
          needDeliveryMin: input.needDeliveryMin,
          needDeliveryMax: input.needDeliveryMax,
          note: input.note,
          orderReason: input.orderReason,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListDPOProcessingReport = (input: GetDPOReportInputDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/dpos/report-r24',
        params: {
          buyerTypeId: input.buyerTypeId,
          buyerId: input.buyerId,
          fromDate: input.fromDate,
          toDate: input.toDate,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListDPOReport = (input: GetDPOReportInputDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/dpos/report',
        params: {
          buyerTypeId: input.buyerTypeId,
          buyerId: input.buyerId,
          fromDate: input.fromDate,
          toDate: input.toDate,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListLockOnOrderStock = (dpoId: string, dpoDetailId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<DPOLockOnOrderStockDto>>(
      {
        method: 'GET',
        url: `/api/app/dpos/${dpoId}/details/${dpoDetailId}/list-lock-on-order-stock`,
      },
      { apiName: this.apiName, ...config },
    );

  getListLockStockEtaEtd = (
    dpoDetailId: string,
    poDetailId: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, ListResultDto<DPOLockStockEtaEtdDto>>(
      {
        method: 'GET',
        url: `/api/app/dpos/${dpoDetailId}/lock-stock-eta-etd/${poDetailId}`,
      },
      { apiName: this.apiName, ...config },
    );

  getListMessages = (dpoId: string, input: GetDPOMessagesInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<MessageDto>>(
      {
        method: 'GET',
        url: `/api/app/dpos/${dpoId}/messages`,
        params: {
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListSaleOrderModalDPO = (dpoDetailId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SaleOrderListModalDPODto[]>(
      {
        method: 'GET',
        url: `/api/app/dpos/details/${dpoDetailId}/sale-order`,
      },
      { apiName: this.apiName, ...config },
    );

  getListSaleOrderModalDelivery = (dpoDetailId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SaleOrderListModalDeliveryDto[]>(
      {
        method: 'GET',
        url: `/api/app/dpos/details/${dpoDetailId}/delivery`,
      },
      { apiName: this.apiName, ...config },
    );

  getLockStocks = (dpoId: string, detailId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<MaterialStockLockStockDto>>(
      {
        method: 'GET',
        url: `/api/app/dpos/${dpoId}/details/${detailId}/lock-stock`,
      },
      { apiName: this.apiName, ...config },
    );

  importDPO = (
    data: ExcelValidationResult<ImportDPODto>,
    force?: boolean,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, DPODto>(
      {
        method: 'POST',
        url: '/api/app/dpos/import/dpo',
        params: { force },
        body: data,
      },
      { apiName: this.apiName, ...config },
    );

  lockShipment = (dpoDetailId: string, input: DPOLockShipmentDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: `/api/app/dpos/${dpoDetailId}/lock-shipment`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  lockShipmentAuto = (input: DPOLockShipmentAutoDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/dpos/lock-shipment-auto',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  lockStock = (id: string, input: DPODetailLockStockDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: `/api/app/dpos/details/${id}/lock-stock`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  lockStockAuto = (input: DPOLockStockAutoDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/dpos/lock-stock-auto',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  lockStockAutoV2 = (input: DPOLockStockAutoV2Dto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/dpos/lock-stock-auto-v2',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  reject = (id: string, input: NoteMetadataDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DPODto>(
      {
        method: 'POST',
        url: `/api/app/dpos/${id}/reject`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  sendMessage = (dpoId: string, input: MessageCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, MessageDto>(
      {
        method: 'POST',
        url: `/api/app/dpos/${dpoId}/messages`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: DPOUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DPODto>(
      {
        method: 'PUT',
        url: `/api/app/dpos/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  updateDPODetail = (
    dpoDetailId: string,
    qty: number,
    confirmNote: string,
    note: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'PUT',
        url: '/api/app/dpos/update-detail',
        params: { dpoDetailId, qty, confirmNote, note },
      },
      { apiName: this.apiName, ...config },
    );

  updateLockShipment = (
    dpoDetailId: string,
    poDetailId: string,
    input: DPOLockShipmentItemUpdateDto,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: `/api/app/dpos/${dpoDetailId}/lock-shipment-item/${poDetailId}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  updateLockStockDetail = (
    dpoDetailId: string,
    input: DPODetailUpdateLockStockDto,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: `/api/app/dpos/${dpoDetailId}/lock-stock-detail`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseDPO = (file: FormData, input: ImportDPOInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExcelValidationResult<ImportDPODto>>(
      {
        method: 'POST',
        url: '/api/app/dpos/validate-and-parse/dpo',
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
