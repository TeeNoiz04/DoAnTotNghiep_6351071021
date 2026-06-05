import type {
  GKRDetailDto,
  GKRDetailLockOnOrderStockDto,
  GKRDetailLockShipmentDto,
  GKRDetailLockShipmentEtaEtdDto,
  GKRDetailLockShipmentItemUpdateDto,
  GKRDetailLockStockDto,
} from './gkrdetails/models';
import type {
  GKRCancelItemDto,
  GKRConfirmNoteDto,
  GKRDto,
  GKRExcelDownloadDto,
  GKRImportDto,
  GKRImportInput,
  GKRLockOnOrderStockAutoDto,
  GKRLockStockAutoDto,
  GetGKRsInput,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto, PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { ApproverDto } from '../approval-routes/models';
import type { BatchAutoUnlockStockDto, DPOListPOsDto } from '../dpos/models';
import type { MaterialStockLockStockDto } from '../materials/material-stocks/material-stock-lock-stocks/models';
import type { MessageCreateDto, MessageDto } from '../messages/models';
import type { ExcelValidationResult } from '../shared/excels/models';
import type { DownloadTokenResultDto, NoteMetadataDto } from '../shared/models';

@Injectable({
  providedIn: 'root',
})
export class GKRService {
  apiName = 'Default';

  approve = (id: string, input: NoteMetadataDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GKRDto>(
      {
        method: 'POST',
        url: `/api/app/gkr/${id}/approve`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  batchAutoUnlockOnOrderStock = (input: BatchAutoUnlockStockDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/gkr/details/batch-auto-unlock-on-order-stock',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  batchAutoUnlockStock = (input: BatchAutoUnlockStockDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/gkr/details/batch-auto-unlock',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  cancelItem = (id: string, input: GKRCancelItemDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GKRDto>(
      {
        method: 'POST',
        url: `/api/app/gkr/${id}/cancel`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  confirmLockOnOrder = (id: string, input: NoteMetadataDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GKRDto>(
      {
        method: 'POST',
        url: `/api/app/gkr/${id}/confirm-lock-on-order`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  confirmLockStock = (id: string, input: NoteMetadataDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GKRDto>(
      {
        method: 'POST',
        url: `/api/app/gkr/${id}/confirm-lock-stock`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  confirmNote = (input: GKRConfirmNoteDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GKRDetailDto[]>(
      {
        method: 'POST',
        url: '/api/app/gkr/confirm-note',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/gkr/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  deleteLockShipment = (gkrDetailId: string, poDetailId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/gkr/details/${gkrDetailId}/po-details/${poDetailId}/lock-shipment`,
      },
      { apiName: this.apiName, ...config },
    );

  deleteLockStock = (gkrDetailId: string, lockStockId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/gkr/details/${gkrDetailId}/lock-stocks/${lockStockId}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GKRDto>(
      {
        method: 'GET',
        url: `/api/app/gkr/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getDetail = (gkrId: string, detailId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GKRDetailDto>(
      {
        method: 'GET',
        url: `/api/app/gkr/${gkrId}/details/${detailId}`,
      },
      { apiName: this.apiName, ...config },
    );

  getDetails = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<GKRDetailDto>>(
      {
        method: 'GET',
        url: `/api/app/gkr/${id}/details`,
      },
      { apiName: this.apiName, ...config },
    );

  getDownloadToken = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DownloadTokenResultDto>(
      {
        method: 'GET',
        url: '/api/app/gkr/download-token',
      },
      { apiName: this.apiName, ...config },
    );

  getGKRDataAsExcel = (input: GKRExcelDownloadDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/gkr/data-as-excel-file',
        params: {
          downloadToken: input.downloadToken,
          gkrNo: input.gkrNo,
          materialType: input.materialType,
          status: input.status,
          buyerTypeId: input.buyerTypeId,
          buyerId: input.buyerId,
          buyerShortName: input.buyerShortName,
          orderDateMin: input.orderDateMin,
          orderDateMax: input.orderDateMax,
          totalAmountMin: input.totalAmountMin,
          totalAmountMax: input.totalAmountMax,
          linkedDPONo: input.linkedDPONo,
          gicType: input.gicType,
          gicProcess: input.gicProcess,
          costCenter: input.costCenter,
          materialGroup: input.materialGroup,
          materialCode: input.materialCode,
          modelName: input.modelName,
          specialPriceCode: input.specialPriceCode,
          customerName: input.customerName,
          customerTaxCode: input.customerTaxCode,
          poNo: input.poNo,
          supplierCode: input.supplierCode,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetGKRsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<GKRDto>>(
      {
        method: 'GET',
        url: '/api/app/gkr',
        params: {
          gkrNo: input.gkrNo,
          materialType: input.materialType,
          status: input.status,
          buyerTypeId: input.buyerTypeId,
          buyerId: input.buyerId,
          buyerShortName: input.buyerShortName,
          orderDateMin: input.orderDateMin,
          orderDateMax: input.orderDateMax,
          totalAmountMin: input.totalAmountMin,
          totalAmountMax: input.totalAmountMax,
          linkedDPONo: input.linkedDPONo,
          gicType: input.gicType,
          gicProcess: input.gicProcess,
          costCenter: input.costCenter,
          materialGroup: input.materialGroup,
          materialCode: input.materialCode,
          modelName: input.modelName,
          specialPriceCode: input.specialPriceCode,
          customerName: input.customerName,
          customerTaxCode: input.customerTaxCode,
          poNo: input.poNo,
          supplierCode: input.supplierCode,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListApprovers = (gkrId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ApproverDto[]>(
      {
        method: 'GET',
        url: `/api/app/gkr/${gkrId}/approvers`,
      },
      { apiName: this.apiName, ...config },
    );

  getListAvailablePOs = (
    gkrId: string,
    gkrDetailId: string,
    materialCode: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, ListResultDto<DPOListPOsDto>>(
      {
        method: 'GET',
        url: `/api/app/gkr/${gkrId}/details/${gkrDetailId}/available-pos`,
        params: { materialCode },
      },
      { apiName: this.apiName, ...config },
    );

  getListGRKExport = (input: GetGKRsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/gkr/gkr-export',
        params: {
          gkrNo: input.gkrNo,
          materialType: input.materialType,
          status: input.status,
          buyerTypeId: input.buyerTypeId,
          buyerId: input.buyerId,
          buyerShortName: input.buyerShortName,
          orderDateMin: input.orderDateMin,
          orderDateMax: input.orderDateMax,
          totalAmountMin: input.totalAmountMin,
          totalAmountMax: input.totalAmountMax,
          linkedDPONo: input.linkedDPONo,
          gicType: input.gicType,
          gicProcess: input.gicProcess,
          costCenter: input.costCenter,
          materialGroup: input.materialGroup,
          materialCode: input.materialCode,
          modelName: input.modelName,
          specialPriceCode: input.specialPriceCode,
          customerName: input.customerName,
          customerTaxCode: input.customerTaxCode,
          poNo: input.poNo,
          supplierCode: input.supplierCode,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListLockShipment = (gkrId: string, gkrDetailId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<GKRDetailLockShipmentDto>>(
      {
        method: 'GET',
        url: `/api/app/gkr/${gkrId}/details/${gkrDetailId}/lock-shipments`,
      },
      { apiName: this.apiName, ...config },
    );

  getListLockShipmentEtaEtd = (
    gkrDetailId: string,
    poDetailId: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, ListResultDto<GKRDetailLockShipmentEtaEtdDto>>(
      {
        method: 'GET',
        url: `/api/app/gkr/details/${gkrDetailId}/po-details/${poDetailId}/lock-shipment-eta-etd`,
      },
      { apiName: this.apiName, ...config },
    );

  getListLockStocks = (gkrId: string, detailId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<MaterialStockLockStockDto>>(
      {
        method: 'GET',
        url: `/api/app/gkr/${gkrId}/details/${detailId}/lock-stocks`,
      },
      { apiName: this.apiName, ...config },
    );

  getListMessages = (
    id: string,
    input: PagedAndSortedResultRequestDto,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, PagedResultDto<MessageDto>>(
      {
        method: 'GET',
        url: `/api/app/gkr/${id}/messages`,
        params: {
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListMyApproval = (input: GetGKRsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<GKRDto>>(
      {
        method: 'GET',
        url: '/api/app/gkr/my-approvals',
        params: {
          gkrNo: input.gkrNo,
          materialType: input.materialType,
          status: input.status,
          buyerTypeId: input.buyerTypeId,
          buyerId: input.buyerId,
          buyerShortName: input.buyerShortName,
          orderDateMin: input.orderDateMin,
          orderDateMax: input.orderDateMax,
          totalAmountMin: input.totalAmountMin,
          totalAmountMax: input.totalAmountMax,
          linkedDPONo: input.linkedDPONo,
          gicType: input.gicType,
          gicProcess: input.gicProcess,
          costCenter: input.costCenter,
          materialGroup: input.materialGroup,
          materialCode: input.materialCode,
          modelName: input.modelName,
          specialPriceCode: input.specialPriceCode,
          customerName: input.customerName,
          customerTaxCode: input.customerTaxCode,
          poNo: input.poNo,
          supplierCode: input.supplierCode,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  importGKR = (
    validationResult: ExcelValidationResult<GKRImportDto>,
    force?: boolean,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/gkr/import/gkr',
        params: { force },
        body: validationResult,
      },
      { apiName: this.apiName, ...config },
    );

  lockOnOrderStockAuto = (input: GKRLockOnOrderStockAutoDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/gkr/lock-on-order-stock-auto',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  lockOnOrderStockManual = (
    gkrDetailId: string,
    input: GKRDetailLockOnOrderStockDto,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: `/api/app/gkr/details/${gkrDetailId}/lock-on-order-stock`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  lockStockAuto = (input: GKRLockStockAutoDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/gkr/lock-stock-auto',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  lockStockManual = (id: string, input: GKRDetailLockStockDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: `/api/app/gkr/details/${id}/lock-stock`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  reject = (id: string, input: NoteMetadataDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GKRDto>(
      {
        method: 'POST',
        url: `/api/app/gkr/${id}/reject`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  sendMessage = (id: string, input: MessageCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, MessageDto>(
      {
        method: 'POST',
        url: `/api/app/gkr/${id}/messages`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  updateDetail = (
    gkrDetailId: string,
    qty: number,
    confirmNote: string,
    note: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: `/api/app/gkr/details/${gkrDetailId}`,
        params: { qty, confirmNote, note },
      },
      { apiName: this.apiName, ...config },
    );

  updateLockShipment = (
    gkrDetailId: string,
    poDetailId: string,
    input: GKRDetailLockShipmentItemUpdateDto,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: `/api/app/gkr/details/${gkrDetailId}/po-details/${poDetailId}/lock-shipment`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseGKR = (file: FormData, input: GKRImportInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExcelValidationResult<GKRImportDto>>(
      {
        method: 'POST',
        url: '/api/app/gkr/validation-parse/gkr',
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
