import type {
  GetStockCategoriesInput,
  StockCategoryCreateDto,
  StockCategoryDto,
  StockCategoryUpdateDto,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class StockCategoryService {
  apiName = 'Default';

  create = (input: StockCategoryCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StockCategoryDto>(
      {
        method: 'POST',
        url: '/api/app/stock-categories',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/stock-categories/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StockCategoryDto>(
      {
        method: 'GET',
        url: `/api/app/stock-categories/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetStockCategoriesInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<StockCategoryDto>>(
      {
        method: 'GET',
        url: '/api/app/stock-categories',
        params: {
          filterText: input.filterText,
          stockCode: input.stockCode,
          stockName: input.stockName,
          foc: input.foc,
          mainStock: input.mainStock,
          damagedStock: input.damagedStock,
          sortOrderMin: input.sortOrderMin,
          sortOrderMax: input.sortOrderMax,
          isDeactive: input.isDeactive,
          note: input.note,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: StockCategoryUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StockCategoryDto>(
      {
        method: 'PUT',
        url: `/api/app/stock-categories/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
