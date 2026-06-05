import type {
  GetStockImportDetailsInput,
  StockImportDetailCreateDto,
  StockImportDetailDto,
  StockImportDetailUpdateDto,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class StockImportDetailService {
  apiName = 'Default';

  create = (input: StockImportDetailCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StockImportDetailDto>(
      {
        method: 'POST',
        url: '/api/app/stock-import-details',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/stock-import-details/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StockImportDetailDto>(
      {
        method: 'GET',
        url: `/api/app/stock-import-details/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetStockImportDetailsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<StockImportDetailDto>>(
      {
        method: 'GET',
        url: '/api/app/stock-import-details',
        params: {
          filterText: input.filterText,
          stockImportId: input.stockImportId,
          invoiceNo: input.invoiceNo,
          itemModel: input.itemModel,
          materialCode: input.materialCode,
          unit: input.unit,
          qtyMin: input.qtyMin,
          qtyMax: input.qtyMax,
          priceMin: input.priceMin,
          priceMax: input.priceMax,
          amountMin: input.amountMin,
          amountMax: input.amountMax,
          gensanchiNM: input.gensanchiNM,
          etaMin: input.etaMin,
          etaMax: input.etaMax,
          etdMin: input.etdMin,
          etdMax: input.etdMax,
          shipmentMethod: input.shipmentMethod,
          billNo: input.billNo,
          machineNumber: input.machineNumber,
          poNo: input.poNo,
          cdNo: input.cdNo,
          note: input.note,
          deliveryTerm: input.deliveryTerm,
          origin: input.origin,
          invoiceDateMin: input.invoiceDateMin,
          invoiceDateMax: input.invoiceDateMax,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListAsExcelFile = (input: GetStockImportDetailsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/stock-import-details/export',
        params: {
          filterText: input.filterText,
          stockImportId: input.stockImportId,
          invoiceNo: input.invoiceNo,
          itemModel: input.itemModel,
          materialCode: input.materialCode,
          unit: input.unit,
          qtyMin: input.qtyMin,
          qtyMax: input.qtyMax,
          priceMin: input.priceMin,
          priceMax: input.priceMax,
          amountMin: input.amountMin,
          amountMax: input.amountMax,
          gensanchiNM: input.gensanchiNM,
          etaMin: input.etaMin,
          etaMax: input.etaMax,
          etdMin: input.etdMin,
          etdMax: input.etdMax,
          shipmentMethod: input.shipmentMethod,
          billNo: input.billNo,
          machineNumber: input.machineNumber,
          poNo: input.poNo,
          cdNo: input.cdNo,
          note: input.note,
          deliveryTerm: input.deliveryTerm,
          origin: input.origin,
          invoiceDateMin: input.invoiceDateMin,
          invoiceDateMax: input.invoiceDateMax,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: StockImportDetailUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StockImportDetailDto>(
      {
        method: 'PUT',
        url: `/api/app/stock-import-details/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
