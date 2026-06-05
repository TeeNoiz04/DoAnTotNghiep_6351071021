import type { GetSpoBatchRequestsInput, SpoBatchRequestDto } from './models';
import type { SpoBatchRequestDetailImportDto } from './spo-batch-request-details/models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { ExcelValidationResult } from '../shared/excels/models';

@Injectable({
  providedIn: 'root',
})
export class SpoBatchRequestService {
  apiName = 'Default';

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/spo-batch-requests/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  deleteBatchRequest = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/spo-batch-requests/${id}/delete-batch-request`,
      },
      { apiName: this.apiName, ...config },
    );

  deleteBatchRequestItems = (id: string, itemIds: string[], config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/spo-batch-requests/${id}/delete-items`,
        body: itemIds,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SpoBatchRequestDto>(
      {
        method: 'GET',
        url: `/api/app/spo-batch-requests/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetSpoBatchRequestsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<SpoBatchRequestDto>>(
      {
        method: 'GET',
        url: '/api/app/spo-batch-requests',
        params: {
          filterText: input.filterText,
          requestNo: input.requestNo,
          importType: input.importType,
          fileName: input.fileName,
          note: input.note,
          status: input.status,
          spoCode: input.spoCode,
          golfaCode: input.golfaCode,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  importSPOBatchRequest = (
    data: ExcelValidationResult<SpoBatchRequestDetailImportDto>,
    note: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/spo-batch-requests/import-batch-request',
        params: { note },
        body: data,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseBatchRequest = (file: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExcelValidationResult<SpoBatchRequestDetailImportDto>>(
      {
        method: 'POST',
        url: '/api/app/spo-batch-requests/validate-and-parse-batch-request',
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
