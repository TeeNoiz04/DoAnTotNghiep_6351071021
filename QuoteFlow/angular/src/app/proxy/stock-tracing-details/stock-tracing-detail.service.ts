import type {
  GetStockTracingDetailsInput,
  StockTracingDetailCreateDto,
  StockTracingDetailDto,
  StockTracingDetailExcelDownloadDto,
  StockTracingDetailUpdateDto,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { DownloadTokenResultDto } from '../shared/models';

@Injectable({
  providedIn: 'root',
})
export class StockTracingDetailService {
  apiName = 'Default';

  create = (input: StockTracingDetailCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StockTracingDetailDto>(
      {
        method: 'POST',
        url: '/api/app/stock-tracing-details',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/stock-tracing-details/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StockTracingDetailDto>(
      {
        method: 'GET',
        url: `/api/app/stock-tracing-details/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getDownloadToken = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DownloadTokenResultDto>(
      {
        method: 'GET',
        url: '/api/app/stock-tracing-details/download-token',
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetStockTracingDetailsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<StockTracingDetailDto>>(
      {
        method: 'GET',
        url: '/api/app/stock-tracing-details',
        params: {
          filterText: input.filterText,
          stockTracingId: input.stockTracingId,
          reportType: input.reportType,
          rowNoMin: input.rowNoMin,
          rowNoMax: input.rowNoMax,
          packingListCode: input.packingListCode,
          checkListCode: input.checkListCode,
          dateEnteredMin: input.dateEnteredMin,
          dateEnteredMax: input.dateEnteredMax,
          stock: input.stock,
          bu: input.bu,
          customer: input.customer,
          category: input.category,
          giv: input.giv,
          invoice: input.invoice,
          skuCode: input.skuCode,
          skuName: input.skuName,
          quality: input.quality,
          warranty: input.warranty,
          unit: input.unit,
          series: input.series,
          originCode: input.originCode,
          productionDateMin: input.productionDateMin,
          productionDateMax: input.productionDateMax,
          location: input.location,
          golfaCode: input.golfaCode,
          note: input.note,
          materialType: input.materialType,
          model: input.model,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListAsExcelFile = (input: StockTracingDetailExcelDownloadDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/stock-tracing-details/as-excel-file',
        params: {
          downloadToken: input.downloadToken,
          filterText: input.filterText,
          stockTracingId: input.stockTracingId,
          reportType: input.reportType,
          rowNoMin: input.rowNoMin,
          rowNoMax: input.rowNoMax,
          packingListCode: input.packingListCode,
          checkListCode: input.checkListCode,
          dateEnteredMin: input.dateEnteredMin,
          dateEnteredMax: input.dateEnteredMax,
          stock: input.stock,
          bu: input.bu,
          customer: input.customer,
          category: input.category,
          giv: input.giv,
          invoice: input.invoice,
          skuCode: input.skuCode,
          skuName: input.skuName,
          quality: input.quality,
          warranty: input.warranty,
          unit: input.unit,
          series: input.series,
          originCode: input.originCode,
          productionDateMin: input.productionDateMin,
          productionDateMax: input.productionDateMax,
          location: input.location,
          golfaCode: input.golfaCode,
          note: input.note,
          materialType: input.materialType,
        },
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: StockTracingDetailUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StockTracingDetailDto>(
      {
        method: 'PUT',
        url: `/api/app/stock-tracing-details/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
