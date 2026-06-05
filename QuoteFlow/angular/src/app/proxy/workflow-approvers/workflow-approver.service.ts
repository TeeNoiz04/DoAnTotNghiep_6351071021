import type {
  GetWorkflowApproversInput,
  WorkflowApproverCreateDto,
  WorkflowApproverDto,
  WorkflowApproverUpdateDto,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class WorkflowApproverService {
  apiName = 'Default';

  create = (input: WorkflowApproverCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, WorkflowApproverDto>(
      {
        method: 'POST',
        url: '/api/app/workflow-approvers',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/workflow-approvers/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, WorkflowApproverDto>(
      {
        method: 'GET',
        url: `/api/app/workflow-approvers/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetWorkflowApproversInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<WorkflowApproverDto>>(
      {
        method: 'GET',
        url: '/api/app/workflow-approvers',
        params: {
          filterText: input.filterText,
          wfId: input.wfId,
          approver: input.approver,
          note: input.note,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: WorkflowApproverUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, WorkflowApproverDto>(
      {
        method: 'PUT',
        url: `/api/app/workflow-approvers/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
