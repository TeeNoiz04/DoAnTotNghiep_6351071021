import type {
  GICDetailDto,
  GICDetailLockStockDto,
  GICDetailUpdateLockStockDto,
} from './gicdetails/models';
import type {
  GICAddExtraFeeDto,
  GICCancelDto,
  GICConfirmNoteDto,
  GICDto,
  GICExcelDownloadDto,
  GICImportDto,
  GICImportInput,
  GICListPOsDto,
  GICLockOnOrderStockDto,
  GICLockShipmentDto,
  GICLockShipmentItemUpdateDto,
  GICLockStockAutoDto,
  GICLockStockAutoV2Dto,
  GICLockStockEtaEtdDto,
  GetGICsInput,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type {
  BatchAutoUnlockStockDto,
  DPOLockShipmentAutoDto,
  GICAllocateGKRDto,
  GicGkrAllocationDto,
} from '../dpos/models';
import type { MaterialStockLockStockDto } from '../materials/material-stocks/material-stock-lock-stocks/models';
import type {
  SaleOrderListModalDPODto,
  SaleOrderListModalDeliveryDto,
} from '../sale-orders/models';
import type { ExcelValidationResult } from '../shared/excels/models';
import type { DownloadTokenResultDto, NoteMetadataDto } from '../shared/models';

@Injectable({
  providedIn: 'root',
})
export class GICService {
  apiName = 'Default';

  addExtraFee = (input: GICAddExtraFeeDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GICDetailDto[]>(
      {
        method: 'POST',
        url: '/api/app/gic/add-extra-fee',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  allocateGkrToGic = (input: GICAllocateGKRDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/gic/allocate-gkr',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  batchAutoUnlockOnOrderStock = (input: BatchAutoUnlockStockDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/gic/details/batch-auto-unlock-on-order-stock',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  batchAutoUnlockStock = (input: BatchAutoUnlockStockDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/gic/details/batch-auto-unlock',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  cancel = (id: string, input: GICCancelDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GICDto>(
      {
        method: 'POST',
        url: `/api/app/gic/${id}/cancel`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  confirmLockOnOrder = (id: string, input: NoteMetadataDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GICDto>(
      {
        method: 'POST',
        url: `/api/app/gic/${id}/confirm-lock-on-order`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  confirmLockStock = (id: string, input: NoteMetadataDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GICDto>(
      {
        method: 'POST',
        url: `/api/app/gic/${id}/confirm-lock-stock`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  confirmNote = (input: GICConfirmNoteDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GICDetailDto[]>(
      {
        method: 'POST',
        url: '/api/app/gic/confirm-note',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/gic/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  deleteLockOnOrderStock = (
    gicDetailId: string,
    poDetailId: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/gic/details/${gicDetailId}/lock-on-order-stock/${poDetailId}`,
      },
      { apiName: this.apiName, ...config },
    );

  deleteLockStock = (gicDetailId: string, lockStockId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/gic/details/${gicDetailId}/lock-stocks/${lockStockId}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GICDto>(
      {
        method: 'GET',
        url: `/api/app/gic/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getAvailableGkrsForAllocation = (gicId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GicGkrAllocationDto[]>(
      {
        method: 'GET',
        url: `/api/app/gic/${gicId}/available-gkrs`,
      },
      { apiName: this.apiName, ...config },
    );

  getDetail = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GICDetailDto>(
      {
        method: 'GET',
        url: `/api/app/gic/details/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getDetails = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<GICDetailDto>>(
      {
        method: 'GET',
        url: `/api/app/gic/${id}/details`,
      },
      { apiName: this.apiName, ...config },
    );

  getDownloadToken = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DownloadTokenResultDto>(
      {
        method: 'GET',
        url: '/api/app/gic/download-token',
      },
      { apiName: this.apiName, ...config },
    );

  getGkrAllocations = (gicId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, GicGkrAllocationDto[]>(
      {
        method: 'GET',
        url: `/api/app/gic/${gicId}/gkr-allocations`,
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetGICsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<GICDto>>(
      {
        method: 'GET',
        url: '/api/app/gic',
        params: {
          filterText: input.filterText,
          gicNo: input.gicNo,
          gicType: input.gicType,
          gicProcess: input.gicProcess,
          materialType: input.materialType,
          costCenter: input.costCenter,
          materialCode: input.materialCode,
          modelName: input.modelName,
          status: input.status,
          buyerTypeId: input.buyerTypeId,
          buyerId: input.buyerId,
          buyerShortName: input.buyerShortName,
          orderDateMin: input.orderDateMin,
          orderDateMax: input.orderDateMax,
          totalAmountMin: input.totalAmountMin,
          totalAmountMax: input.totalAmountMax,
          remark: input.remark,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListAsExcelFile = (input: GICExcelDownloadDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/gic/as-excel-file',
        params: {
          downloadToken: input.downloadToken,
          filterText: input.filterText,
          gicNo: input.gicNo,
          gicType: input.gicType,
          gicProcess: input.gicProcess,
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
          sorting: input.sorting,
          maxResultCount: input.maxResultCount,
          skipCount: input.skipCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListAvailablePOs = (
    gicId: string,
    gicDetailId: string,
    materialCode: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, ListResultDto<GICListPOsDto>>(
      {
        method: 'GET',
        url: `/api/app/gic/${gicId}/details/${gicDetailId}/available-pos`,
        params: { materialCode },
      },
      { apiName: this.apiName, ...config },
    );

  getListGICExport = (input: GetGICsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/gic/export/gic',
        params: {
          filterText: input.filterText,
          gicNo: input.gicNo,
          gicType: input.gicType,
          gicProcess: input.gicProcess,
          materialType: input.materialType,
          costCenter: input.costCenter,
          materialCode: input.materialCode,
          modelName: input.modelName,
          status: input.status,
          buyerTypeId: input.buyerTypeId,
          buyerId: input.buyerId,
          buyerShortName: input.buyerShortName,
          orderDateMin: input.orderDateMin,
          orderDateMax: input.orderDateMax,
          totalAmountMin: input.totalAmountMin,
          totalAmountMax: input.totalAmountMax,
          remark: input.remark,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListLockOnOrderStock = (gicId: string, gicDetailId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<GICLockOnOrderStockDto>>(
      {
        method: 'GET',
        url: `/api/app/gic/${gicId}/details/${gicDetailId}/lock-on-order-stock`,
      },
      { apiName: this.apiName, ...config },
    );

  getListLockStockEtaEtd = (
    gicDetailId: string,
    poDetailId: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, ListResultDto<GICLockStockEtaEtdDto>>(
      {
        method: 'GET',
        url: `/api/app/gic/details/${gicDetailId}/po-details/${poDetailId}/lock-stock-eta-etd`,
      },
      { apiName: this.apiName, ...config },
    );

  getListSOGICFOCReport = (input: GetGICsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/gic/export/gic-foc',
        params: {
          filterText: input.filterText,
          gicNo: input.gicNo,
          gicType: input.gicType,
          gicProcess: input.gicProcess,
          materialType: input.materialType,
          costCenter: input.costCenter,
          materialCode: input.materialCode,
          modelName: input.modelName,
          status: input.status,
          buyerTypeId: input.buyerTypeId,
          buyerId: input.buyerId,
          buyerShortName: input.buyerShortName,
          orderDateMin: input.orderDateMin,
          orderDateMax: input.orderDateMax,
          totalAmountMin: input.totalAmountMin,
          totalAmountMax: input.totalAmountMax,
          remark: input.remark,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListSOGICInternalUserReport = (input: GetGICsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/gic/export/gic-internal-use',
        params: {
          filterText: input.filterText,
          gicNo: input.gicNo,
          gicType: input.gicType,
          gicProcess: input.gicProcess,
          materialType: input.materialType,
          costCenter: input.costCenter,
          materialCode: input.materialCode,
          modelName: input.modelName,
          status: input.status,
          buyerTypeId: input.buyerTypeId,
          buyerId: input.buyerId,
          buyerShortName: input.buyerShortName,
          orderDateMin: input.orderDateMin,
          orderDateMax: input.orderDateMax,
          totalAmountMin: input.totalAmountMin,
          totalAmountMax: input.totalAmountMax,
          remark: input.remark,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListSOGICWarrantyReport = (input: GetGICsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/gic/export/gic-warranty',
        params: {
          filterText: input.filterText,
          gicNo: input.gicNo,
          gicType: input.gicType,
          gicProcess: input.gicProcess,
          materialType: input.materialType,
          costCenter: input.costCenter,
          materialCode: input.materialCode,
          modelName: input.modelName,
          status: input.status,
          buyerTypeId: input.buyerTypeId,
          buyerId: input.buyerId,
          buyerShortName: input.buyerShortName,
          orderDateMin: input.orderDateMin,
          orderDateMax: input.orderDateMax,
          totalAmountMin: input.totalAmountMin,
          totalAmountMax: input.totalAmountMax,
          remark: input.remark,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListSaleOrderModalDPO = (gicDetailId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SaleOrderListModalDPODto[]>(
      {
        method: 'GET',
        url: `/api/app/gic/details/${gicDetailId}/sale-order`,
      },
      { apiName: this.apiName, ...config },
    );

  getListSaleOrderModalDelivery = (gicDetailId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SaleOrderListModalDeliveryDto[]>(
      {
        method: 'GET',
        url: `/api/app/gic/details/${gicDetailId}/delivery`,
      },
      { apiName: this.apiName, ...config },
    );

  getLockStocks = (gicId: string, detailId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<MaterialStockLockStockDto>>(
      {
        method: 'GET',
        url: `/api/app/gic/${gicId}/details/${detailId}/lock-stocks`,
      },
      { apiName: this.apiName, ...config },
    );

  importInternal = (
    validationResult: ExcelValidationResult<GICImportDto>,
    force?: boolean,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/gic/import/internal',
        params: { force },
        body: validationResult,
      },
      { apiName: this.apiName, ...config },
    );

  importSponsor = (
    validationResult: ExcelValidationResult<GICImportDto>,
    force?: boolean,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/gic/import/sponsor',
        params: { force },
        body: validationResult,
      },
      { apiName: this.apiName, ...config },
    );

  importWarranty = (
    validationResult: ExcelValidationResult<GICImportDto>,
    force?: boolean,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/gic/import/warranty',
        params: { force },
        body: validationResult,
      },
      { apiName: this.apiName, ...config },
    );

  importWriteOff = (
    validationResult: ExcelValidationResult<GICImportDto>,
    force?: boolean,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/gic/import/write-off',
        params: { force },
        body: validationResult,
      },
      { apiName: this.apiName, ...config },
    );

  lockShipment = (gicDetailId: string, input: GICLockShipmentDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: `/api/app/gic/details/${gicDetailId}/lock-shipment`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  lockShipmentAuto = (input: DPOLockShipmentAutoDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/gic/details/lock-shipment-auto',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  lockStock = (id: string, input: GICDetailLockStockDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: `/api/app/gic/details/${id}/lock-stock`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  lockStockAuto = (input: GICLockStockAutoDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/gic/lock-stock-auto',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  lockStockAutoV2 = (input: GICLockStockAutoV2Dto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/gic/lock-stock-auto-v2',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  updateLockShipment = (
    gicDetailId: string,
    poDetailId: string,
    input: GICLockShipmentItemUpdateDto,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'PUT',
        url: `/api/app/gic/details/${gicDetailId}/lock-shipment/${poDetailId}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  updateLockStockDetail = (
    gicDetailId: string,
    input: GICDetailUpdateLockStockDto,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'PUT',
        url: `/api/app/gic/details/${gicDetailId}/lock-stock`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseInternal = (
    file: FormData,
    input: GICImportInput,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, ExcelValidationResult<GICImportDto>>(
      {
        method: 'POST',
        url: '/api/app/gic/validation-parse/internal',
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseSponsor = (
    file: FormData,
    input: GICImportInput,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, ExcelValidationResult<GICImportDto>>(
      {
        method: 'POST',
        url: '/api/app/gic/validation-parse/sponsor',
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseWarranty = (
    file: FormData,
    input: GICImportInput,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, ExcelValidationResult<GICImportDto>>(
      {
        method: 'POST',
        url: '/api/app/gic/validation-parse/warranty',
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseWriteOff = (
    file: FormData,
    input: GICImportInput,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, ExcelValidationResult<GICImportDto>>(
      {
        method: 'POST',
        url: '/api/app/gic/validation-parse/write-off',
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
