import type {
  GetSystemConfigurationsInput,
  SystemConfigurationCreateDto,
  SystemConfigurationDto,
  SystemConfigurationUpdateDto,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class SystemConfigurationService {
  apiName = 'Default';

  create = (input: SystemConfigurationCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SystemConfigurationDto>(
      {
        method: 'POST',
        url: '/api/app/system-configurations',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/system-configurations/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SystemConfigurationDto>(
      {
        method: 'GET',
        url: `/api/app/system-configurations/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetSystemConfigurationsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<SystemConfigurationDto>>(
      {
        method: 'GET',
        url: '/api/app/system-configurations',
        params: {
          filterText: input.filterText,
          cfgKey: input.cfgKey,
          cfgValue: input.cfgValue,
          description: input.description,
          isSystemCfg: input.isSystemCfg,
          cfgType: input.cfgType,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: SystemConfigurationUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SystemConfigurationDto>(
      {
        method: 'PUT',
        url: `/api/app/system-configurations/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
