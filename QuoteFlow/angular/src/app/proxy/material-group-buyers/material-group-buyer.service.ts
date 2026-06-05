import type {
  GetMaterialGroupBuyersInput,
  MaterialGroupBuyerCreatesDto,
  MaterialGroupBuyerDto,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class MaterialGroupBuyerService {
  apiName = 'Default';

  create = (input: MaterialGroupBuyerCreatesDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, MaterialGroupBuyerDto[]>(
      {
        method: 'POST',
        url: '/api/app/material-group-buyers',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (buyerId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/material-group-buyers/buyer/${buyerId}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, MaterialGroupBuyerDto>(
      {
        method: 'GET',
        url: `/api/app/material-group-buyers/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetMaterialGroupBuyersInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<MaterialGroupBuyerDto>>(
      {
        method: 'GET',
        url: '/api/app/material-group-buyers',
        params: {
          filterText: input.filterText,
          materialGroupId: input.materialGroupId,
          materialGroupCode: input.materialGroupCode,
          buyerId: input.buyerId,
          buyerShortName: input.buyerShortName,
          note: input.note,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListByBuyer = (buyerId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, MaterialGroupBuyerDto[]>(
      {
        method: 'GET',
        url: `/api/app/material-group-buyers/buyer/${buyerId}`,
      },
      { apiName: this.apiName, ...config },
    );

  update = (input: MaterialGroupBuyerCreatesDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, MaterialGroupBuyerDto[]>(
      {
        method: 'PUT',
        url: '/api/app/material-group-buyers',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
