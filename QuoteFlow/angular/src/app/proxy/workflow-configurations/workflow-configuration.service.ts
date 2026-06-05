import type {
  GetWorkflowConfigurationsInput,
  WorkflowConfigurationDto,
  WorkflowConfigurationUpdateDto,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class WorkflowConfigurationService {
  apiName = 'Default';

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/workflow-configurations/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, WorkflowConfigurationDto>(
      {
        method: 'GET',
        url: `/api/app/workflow-configurations/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetWorkflowConfigurationsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<WorkflowConfigurationDto>>(
      {
        method: 'GET',
        url: '/api/app/workflow-configurations',
        params: {
          filterText: input.filterText,
          workflowType: input.workflowType,
          workflowLevel: input.workflowLevel,
          workflowLevelMin: input.workflowLevelMin,
          workflowLevelMax: input.workflowLevelMax,
          workflowRole: input.workflowRole,
          condition: input.condition,
          note: input.note,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: WorkflowConfigurationUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, WorkflowConfigurationDto>(
      {
        method: 'PUT',
        url: `/api/app/workflow-configurations/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
