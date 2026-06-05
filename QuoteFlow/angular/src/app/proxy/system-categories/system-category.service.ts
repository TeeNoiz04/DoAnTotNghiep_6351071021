import type {
  GetSystemCategoriesInput,
  SystemCategoryCreateDto,
  SystemCategoryDto,
  SystemCategoryExcelDownloadDto,
  SystemCategoryUpdateDto,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { DownloadTokenResultDto } from '../shared/models';

@Injectable({
  providedIn: 'root',
})
export class SystemCategoryService {
  apiName = 'Default';

  create = (input: SystemCategoryCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SystemCategoryDto>(
      {
        method: 'POST',
        url: '/api/app/system-categories',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/system-categories/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SystemCategoryDto>(
      {
        method: 'GET',
        url: `/api/app/system-categories/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getDownloadToken = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DownloadTokenResultDto>(
      {
        method: 'GET',
        url: '/api/app/system-categories/download-token',
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetSystemCategoriesInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<SystemCategoryDto>>(
      {
        method: 'GET',
        url: '/api/app/system-categories',
        params: {
          filterText: input.filterText,
          parentId: input.parentId,
          code: input.code,
          description: input.description,
          valueMin: input.valueMin,
          valueMax: input.valueMax,
          categoryType: input.categoryType,
          note: input.note,
          isDeactive: input.isDeactive,
          sortOrderMin: input.sortOrderMin,
          sortOrderMax: input.sortOrderMax,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListAsExcelFile = (input: SystemCategoryExcelDownloadDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/system-categories/as-excel-file',
        params: {
          downloadToken: input.downloadToken,
          filterText: input.filterText,
          parentId: input.parentId,
          code: input.code,
          description: input.description,
          valueMin: input.valueMin,
          valueMax: input.valueMax,
          categoryType: input.categoryType,
          note: input.note,
          isDeactive: input.isDeactive,
          sortOrderMin: input.sortOrderMin,
          sortOrderMax: input.sortOrderMax,
        },
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: SystemCategoryUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SystemCategoryDto>(
      {
        method: 'PUT',
        url: `/api/app/system-categories/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
