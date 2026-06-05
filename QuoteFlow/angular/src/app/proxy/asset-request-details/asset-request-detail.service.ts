import type {
  AssetRequestDetailCreateDto,
  AssetRequestDetailDto,
  AssetRequestDetailUpdateDto,
  GetAssetRequestDetailsInput,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AssetRequestDetailService {
  apiName = 'Default';

  create = (input: AssetRequestDetailCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AssetRequestDetailDto>(
      {
        method: 'POST',
        url: '/api/app/asset-request-details',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  createMany = (input: AssetRequestDetailCreateDto[], config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/asset-request-details/add-items',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/asset-request-details/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  deleteMany = (ids: string[], config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: '/api/app/asset-request-details/delete-many',
        body: ids,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AssetRequestDetailDto>(
      {
        method: 'GET',
        url: `/api/app/asset-request-details/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetAssetRequestDetailsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<AssetRequestDetailDto>>(
      {
        method: 'GET',
        url: '/api/app/asset-request-details',
        params: {
          filterText: input.filterText,
          requestId: input.requestId,
          assetId: input.assetId,
          assetName: input.assetName,
          isDeleted: input.isDeleted,
          status: input.status,
          note: input.note,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: AssetRequestDetailUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AssetRequestDetailDto>(
      {
        method: 'PUT',
        url: `/api/app/asset-request-details/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
