import type {
  GetAllocationInvoicesInput,
  GetPurchaseInvoicesInput,
  GetStockImportConfirmsInput,
  GetStockImportListsInput,
  GetStockImportsInput,
  InvoiceImportInput,
  ResultValidateInvoiceImport,
  StockImportCreateDto,
  StockImportDto,
  StockImportListDto,
  StockImportUpdateDto,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type {
  ExportStockImportAllocationInput,
  StockImportAllocationExportDto,
} from '../stock-import-allocations/models';

@Injectable({
  providedIn: 'root',
})
export class StockImportService {
  apiName = 'Default';

  allocateStockImport = (invoiceNo: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/stock-imports/allocation-invoice',
        params: { invoiceNo },
      },
      { apiName: this.apiName, ...config },
    );

  confirmAction = (id: string, confirmDate: string, note: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StockImportDto>(
      {
        method: 'POST',
        url: `/api/app/stock-imports/confirm/${id}`,
        params: { confirmDate, note },
      },
      { apiName: this.apiName, ...config },
    );

  confirmAllocation = (input: GetStockImportConfirmsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/stock-imports/confirm-allocation',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  create = (input: StockImportCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StockImportDto>(
      {
        method: 'POST',
        url: '/api/app/stock-imports',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/stock-imports/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StockImportDto>(
      {
        method: 'GET',
        url: `/api/app/stock-imports/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getInvoiceExcelFile = (input: ExportStockImportAllocationInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/stock-imports/as-list-excel/export-stock-import-allocation',
        params: { invoiceNo: input.invoiceNo, buyer: input.buyer },
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetStockImportsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<StockImportDto>>(
      {
        method: 'GET',
        url: '/api/app/stock-imports',
        params: {
          filterText: input.filterText,
          invoiceNo: input.invoiceNo,
          fileName: input.fileName,
          status: input.status,
          poNo: input.poNo,
          golfaCode: input.golfaCode,
          stockDateMin: input.stockDateMin,
          stockDateMax: input.stockDateMax,
          note: input.note,
          materialType: input.materialType,
          supplierCode: input.supplierCode,
          buyerId: input.buyerId,
          buyerType: input.buyerType,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListAllocation = (input: GetStockImportListsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<StockImportListDto>>(
      {
        method: 'GET',
        url: '/api/app/stock-imports/stock-import-allocation-list',
        params: {
          invoiceNo: input.invoiceNo,
          fromDate: input.fromDate,
          toDate: input.toDate,
          status: input.status,
          materialType: input.materialType,
          prPONo: input.prPONo,
          golfaCode: input.golfaCode,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListAllocationDetails = (
    input: ExportStockImportAllocationInput,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, StockImportAllocationExportDto[]>(
      {
        method: 'GET',
        url: '/api/app/stock-imports/stock-import-allocation-details',
        params: { invoiceNo: input.invoiceNo, buyer: input.buyer },
      },
      { apiName: this.apiName, ...config },
    );

  getListAllocationExport = (input: GetAllocationInvoicesInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/stock-imports/list-allocation-export',
        params: {
          invoiceNo: input.invoiceNo,
          fileName: input.fileName,
          status: input.status,
          poNo: input.poNo,
          golfaCode: input.golfaCode,
          stockDateMin: input.stockDateMin,
          stockDateMax: input.stockDateMax,
          note: input.note,
          materialType: input.materialType,
          supplierCode: input.supplierCode,
          buyerId: input.buyerId,
          buyerCode: input.buyerCode,
          buyerType: input.buyerType,
          listInvoice: input.listInvoice,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListAsExcelFile = (input: GetStockImportListsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/stock-imports/export',
        params: {
          invoiceNo: input.invoiceNo,
          fromDate: input.fromDate,
          toDate: input.toDate,
          status: input.status,
          materialType: input.materialType,
          prPONo: input.prPONo,
          golfaCode: input.golfaCode,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListInvoiceSAPDataFile = (input: GetPurchaseInvoicesInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/stock-imports/invoice-sap',
        params: {
          invoiceNo: input.invoiceNo,
          fileName: input.fileName,
          status: input.status,
          poNo: input.poNo,
          golfaCode: input.golfaCode,
          stockDateMin: input.stockDateMin,
          stockDateMax: input.stockDateMax,
          note: input.note,
          materialType: input.materialType,
          supplierCode: input.supplierCode,
          buyerId: input.buyerId,
          buyerCode: input.buyerCode,
          buyerType: input.buyerType,
          listInvoice: input.listInvoice,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListPurchaseInvoiceFile = (input: GetPurchaseInvoicesInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/stock-imports/purchase-invoice',
        params: {
          invoiceNo: input.invoiceNo,
          fileName: input.fileName,
          status: input.status,
          poNo: input.poNo,
          golfaCode: input.golfaCode,
          stockDateMin: input.stockDateMin,
          stockDateMax: input.stockDateMax,
          note: input.note,
          materialType: input.materialType,
          supplierCode: input.supplierCode,
          buyerId: input.buyerId,
          buyerCode: input.buyerCode,
          buyerType: input.buyerType,
          listInvoice: input.listInvoice,
        },
      },
      { apiName: this.apiName, ...config },
    );

  importStock = (input: ResultValidateInvoiceImport, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StockImportDto>(
      {
        method: 'POST',
        url: '/api/app/stock-imports/import',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  performAction = (id: string, action: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StockImportDto>(
      {
        method: 'POST',
        url: `/api/app/stock-imports/action/${id}`,
        params: { action },
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: StockImportUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StockImportDto>(
      {
        method: 'PUT',
        url: `/api/app/stock-imports/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseStock = (
    file: FormData,
    input: InvoiceImportInput,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, ResultValidateInvoiceImport>(
      {
        method: 'POST',
        url: '/api/app/stock-imports/validate-and-parse',
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
