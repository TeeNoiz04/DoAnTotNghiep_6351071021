import type {
  GetStockTracingsInput,
  StockTracingCreateDto,
  StockTracingDeliveryImportDto,
  StockTracingDto,
  StockTracingExcelDownloadDto,
  StockTracingInventoryImportDto,
  StockTracingReceiptImportDto,
  StockTracingUpdateDto,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { ExcelValidationResult } from '../shared/excels/models';
import type { DownloadTokenResultDto } from '../shared/models';

@Injectable({
  providedIn: 'root',
})
export class StockTracingService {
  apiName = 'Default';

  create = (input: StockTracingCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StockTracingDto>(
      {
        method: 'POST',
        url: '/api/app/stock-tracings',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/stock-tracings/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StockTracingDto>(
      {
        method: 'GET',
        url: `/api/app/stock-tracings/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getDownloadToken = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DownloadTokenResultDto>(
      {
        method: 'GET',
        url: '/api/app/stock-tracings/download-token',
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetStockTracingsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<StockTracingDto>>(
      {
        method: 'GET',
        url: '/api/app/stock-tracings',
        params: {
          filterText: input.filterText,
          fileName: input.fileName,
          reportType: input.reportType,
          fromDateMin: input.fromDateMin,
          fromDateMax: input.fromDateMax,
          toDateMin: input.toDateMin,
          toDateMax: input.toDateMax,
          note: input.note,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListAsExcelFile = (input: StockTracingExcelDownloadDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/stock-tracings/as-excel-file',
        params: {
          downloadToken: input.downloadToken,
          filterText: input.filterText,
          fileName: input.fileName,
          reportType: input.reportType,
          fromDateMin: input.fromDateMin,
          fromDateMax: input.fromDateMax,
          toDateMin: input.toDateMin,
          toDateMax: input.toDateMax,
          note: input.note,
        },
      },
      { apiName: this.apiName, ...config },
    );

  importStockTracingDelivery = (
    data: ExcelValidationResult<StockTracingDeliveryImportDto>,
    fromDate: string,
    toDate: string,
    note: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, StockTracingDto>(
      {
        method: 'POST',
        url: '/api/app/stock-tracings/import/delivery',
        params: { fromDate, toDate, note },
        body: data,
      },
      { apiName: this.apiName, ...config },
    );

  importStockTracingInvantory = (
    data: ExcelValidationResult<StockTracingInventoryImportDto>,
    entered: string,
    note: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, StockTracingDto>(
      {
        method: 'POST',
        url: '/api/app/stock-tracings/import/invetory',
        params: { entered, note },
        body: data,
      },
      { apiName: this.apiName, ...config },
    );

  importStockTracingReceipt = (
    data: ExcelValidationResult<StockTracingReceiptImportDto>,
    fromDate: string,
    toDate: string,
    note: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, StockTracingDto>(
      {
        method: 'POST',
        url: '/api/app/stock-tracings/import/receipt',
        params: { fromDate, toDate, note },
        body: data,
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: StockTracingUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StockTracingDto>(
      {
        method: 'PUT',
        url: `/api/app/stock-tracings/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndStockTracingDelivery = (
    file: FormData,
    fromDate: string,
    toDate: string,
    note: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, ExcelValidationResult<StockTracingDeliveryImportDto>>(
      {
        method: 'POST',
        url: '/api/app/stock-tracings/validation/delivery',
        params: { fromDate, toDate, note },
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndStockTracingInventory = (
    file: FormData,
    dateEntered: string,
    note: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, ExcelValidationResult<StockTracingInventoryImportDto>>(
      {
        method: 'POST',
        url: '/api/app/stock-tracings/validation/inventory',
        params: { dateEntered, note },
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndStockTracingReceipt = (
    file: FormData,
    fromDate: string,
    toDate: string,
    note: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, ExcelValidationResult<StockTracingReceiptImportDto>>(
      {
        method: 'POST',
        url: '/api/app/stock-tracings/validation/receipt',
        params: { fromDate, toDate, note },
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
