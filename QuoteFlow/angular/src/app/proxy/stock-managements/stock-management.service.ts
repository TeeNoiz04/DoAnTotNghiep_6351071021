import type {
  DataMaterialOverallStockReportDto,
  GetInventoryReportsInput,
  GetStockHistoriesInput,
  GetStockManagementApprovalsInput,
  GetStockManagementsListInput,
  InventoryReportDto,
  LockShipmentDto,
  LockedDto,
  OnOrderStockDto,
  StockManagementListDto,
  StockManagementUploadDto,
  StockOfSODto,
  StockQtyDto,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { HistoryTrackingDto } from '../history-trackings/models';
import type {
  MaterialStockUploadDetailImportInventoryDto,
  MaterialStockUploadDetailImportTransferDto,
} from '../material-stock-upload-details/models';
import type { GetMaterialStocksInput, MaterialStockDto } from '../materials/material-stocks/models';
import type { MaterialDto } from '../materials/models';
import type { ExcelValidationResult } from '../shared/excels/models';

@Injectable({
  providedIn: 'root',
})
export class StockManagementService {
  apiName = 'Default';

  geDataOverallStockReport = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DataMaterialOverallStockReportDto[]>(
      {
        method: 'GET',
        url: '/api/app/stock-managements/data-report',
      },
      { apiName: this.apiName, ...config },
    );

  get = (golfaCode: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, MaterialDto>(
      {
        method: 'GET',
        url: `/api/app/stock-managements/${golfaCode}`,
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetMaterialStocksInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<MaterialStockDto>>(
      {
        method: 'GET',
        url: '/api/app/stock-managements',
        params: {
          filterText: input.filterText,
          materialId: input.materialId,
          stockCategoryId: input.stockCategoryId,
          golfaCodes: input.golfaCodes,
          models: input.models,
          qtyMin: input.qtyMin,
          qtyMax: input.qtyMax,
          lockedMin: input.lockedMin,
          lockedMax: input.lockedMax,
          lockStockKeepingMin: input.lockStockKeepingMin,
          lockStockKeepingMax: input.lockStockKeepingMax,
          lockStockSOMin: input.lockStockSOMin,
          lockStockSOMax: input.lockStockSOMax,
          available_QtyMin: input.available_QtyMin,
          available_QtyMax: input.available_QtyMax,
          note: input.note,
          materialType: input.materialType,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListExcelInventoryReport = (input: GetInventoryReportsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/stock-managements/inventory-report-excel',
        params: {
          materialCode: input.materialCode,
          inventoryCategory: input.inventoryCategory,
          materialGroup: input.materialGroup,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListInventoryReport = (input: GetInventoryReportsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<InventoryReportDto>>(
      {
        method: 'GET',
        url: '/api/app/stock-managements/inventory-report',
        params: {
          materialCode: input.materialCode,
          inventoryCategory: input.inventoryCategory,
          materialGroup: input.materialGroup,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListOverallStockReport = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/stock-managements/report',
      },
      { apiName: this.apiName, ...config },
    );

  getListStockManagement = (input: GetStockManagementsListInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<StockManagementListDto>>(
      {
        method: 'GET',
        url: '/api/app/stock-managements/stock-management-list',
        params: {
          supplierCode: input.supplierCode,
          supplierBUCode: input.supplierBUCode,
          materialType: input.materialType,
          golfaCode: input.golfaCode,
          model: input.model,
          materialGroup: input.materialGroup,
          stockCategoryId: input.stockCategoryId,
          greaterStockQty: input.greaterStockQty,
          greaterOnOrderStockQty: input.greaterOnOrderStockQty,
          status: input.status,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListStockManagementExcel = (
    input: GetStockManagementsListInput,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/stock-managements/as-file-excel',
        params: {
          supplierCode: input.supplierCode,
          supplierBUCode: input.supplierBUCode,
          materialType: input.materialType,
          golfaCode: input.golfaCode,
          model: input.model,
          materialGroup: input.materialGroup,
          stockCategoryId: input.stockCategoryId,
          greaterStockQty: input.greaterStockQty,
          greaterOnOrderStockQty: input.greaterOnOrderStockQty,
          status: input.status,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListUpload = (input: GetStockManagementApprovalsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<StockManagementUploadDto>>(
      {
        method: 'GET',
        url: '/api/app/stock-managements/upload',
        params: {
          golfaCode: input.golfaCode,
          model: input.model,
          importType: input.importType,
          approvalStatus: input.approvalStatus,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getLockShipment = (materialCode: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, LockShipmentDto[]>(
      {
        method: 'GET',
        url: '/api/app/stock-managements/lock-shipment',
        params: { materialCode },
      },
      { apiName: this.apiName, ...config },
    );

  getLocked = (materialCode: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, LockedDto[]>(
      {
        method: 'GET',
        url: '/api/app/stock-managements/locked',
        params: { materialCode },
      },
      { apiName: this.apiName, ...config },
    );

  getOnOrderStock = (materialCode: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, OnOrderStockDto[]>(
      {
        method: 'GET',
        url: '/api/app/stock-managements/on-order-stock',
        params: { materialCode },
      },
      { apiName: this.apiName, ...config },
    );

  getStockHistory = (input: GetStockHistoriesInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<HistoryTrackingDto>>(
      {
        method: 'GET',
        url: '/api/app/stock-managements/stock-histories',
        params: {
          stockCode: input.stockCode,
          golfaCode: input.golfaCode,
          actionFrom: input.actionFrom,
          actionTo: input.actionTo,
          note: input.note,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getStockHistoryAsExcel = (input: GetStockHistoriesInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/stock-managements/stock-histories/export',
        params: {
          stockCode: input.stockCode,
          golfaCode: input.golfaCode,
          actionFrom: input.actionFrom,
          actionTo: input.actionTo,
          note: input.note,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getStockOfSO = (materialCode: string, stockId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StockOfSODto[]>(
      {
        method: 'GET',
        url: '/api/app/stock-managements/stock-of-so',
        params: { materialCode, stockId },
      },
      { apiName: this.apiName, ...config },
    );

  getStockQty = (materialCode: string, stockId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StockQtyDto[]>(
      {
        method: 'GET',
        url: '/api/app/stock-managements/stock-qty',
        params: { materialCode, stockId },
      },
      { apiName: this.apiName, ...config },
    );

  getUploadDetail = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StockManagementUploadDto>(
      {
        method: 'GET',
        url: `/api/app/stock-managements/${id}/upload-details`,
      },
      { apiName: this.apiName, ...config },
    );

  importMaterialStockInventory = (
    data: ExcelValidationResult<MaterialStockUploadDetailImportInventoryDto>,
    note: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/stock-managements/import-stock-inventory',
        params: { note },
        body: data,
      },
      { apiName: this.apiName, ...config },
    );

  importMaterialStockTransfer = (
    data: ExcelValidationResult<MaterialStockUploadDetailImportTransferDto>,
    note: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/stock-managements/import-stock-transfer',
        params: { note },
        body: data,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseStockInventory = (file: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<
      any,
      ExcelValidationResult<MaterialStockUploadDetailImportInventoryDto>
    >(
      {
        method: 'POST',
        url: '/api/app/stock-managements/validate-parse-stock-inventory',
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseStockTransfer = (file: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<
      any,
      ExcelValidationResult<MaterialStockUploadDetailImportTransferDto>
    >(
      {
        method: 'POST',
        url: '/api/app/stock-managements/validate-parse-stock-transfer',
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
