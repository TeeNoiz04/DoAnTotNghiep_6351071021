import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type {
  GetMaterialGroupsInput,
  MaterialGroupCreateDto,
  MaterialGroupDto,
  MaterialGroupUpdateDto,
} from '../materials/material-groups/models';

@Injectable({
  providedIn: 'root',
})
export class MaterialGroupService {
  apiName = 'Default';

  create = (input: MaterialGroupCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, MaterialGroupDto>(
      {
        method: 'POST',
        url: '/api/app/material-groups',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/material-groups/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, MaterialGroupDto>(
      {
        method: 'GET',
        url: `/api/app/material-groups/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetMaterialGroupsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<MaterialGroupDto>>(
      {
        method: 'GET',
        url: '/api/app/material-groups',
        params: {
          filterText: input.filterText,
          code: input.code,
          name: input.name,
          parent: input.parent,
          sortOrderMin: input.sortOrderMin,
          sortOrderMax: input.sortOrderMax,
          note: input.note,
          isDeActive: input.isDeActive,
          materialType: input.materialType,
          materialGroupPSI: input.materialGroupPSI,
          allowKeyAccount: input.allowKeyAccount,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: MaterialGroupUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, MaterialGroupDto>(
      {
        method: 'PUT',
        url: `/api/app/material-groups/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
