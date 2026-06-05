import type {
  GetKeyAccountEvaluationsInput,
  KeyAccountEvaluationCreateDto,
  KeyAccountEvaluationDto,
  KeyAccountEvaluationExcelDownloadDto,
  KeyAccountEvaluationUpdateDto,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { DownloadTokenResultDto } from '../shared/models';

@Injectable({
  providedIn: 'root',
})
export class KeyAccountEvaluationService {
  apiName = 'Default';

  create = (input: KeyAccountEvaluationCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, KeyAccountEvaluationDto>(
      {
        method: 'POST',
        url: '/api/app/key-account-evaluations',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/key-account-evaluations/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, KeyAccountEvaluationDto>(
      {
        method: 'GET',
        url: `/api/app/key-account-evaluations/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getDownloadToken = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DownloadTokenResultDto>(
      {
        method: 'GET',
        url: '/api/app/key-account-evaluations/download-token',
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetKeyAccountEvaluationsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<KeyAccountEvaluationDto>>(
      {
        method: 'GET',
        url: '/api/app/key-account-evaluations',
        params: {
          filterText: input.filterText,
          keyAccountId: input.keyAccountId,
          evaluationType: input.evaluationType,
          evaluationId: input.evaluationId,
          buyerInfo1: input.buyerInfo1,
          buyerInfo2: input.buyerInfo2,
          mevnInfo1: input.mevnInfo1,
          mevnInfo2: input.mevnInfo2,
          competitorInfo1: input.competitorInfo1,
          competitorInfo2: input.competitorInfo2,
          note: input.note,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListAsExcelFile = (
    input: KeyAccountEvaluationExcelDownloadDto,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/key-account-evaluations/as-excel-file',
        params: {
          downloadToken: input.downloadToken,
          filterText: input.filterText,
          keyAccountId: input.keyAccountId,
          evaluationType: input.evaluationType,
          evaluationId: input.evaluationId,
          buyerInfo1: input.buyerInfo1,
          buyerInfo2: input.buyerInfo2,
          mevnInfo1: input.mevnInfo1,
          mevnInfo2: input.mevnInfo2,
          competitorInfo1: input.competitorInfo1,
          competitorInfo2: input.competitorInfo2,
          note: input.note,
        },
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: KeyAccountEvaluationUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, KeyAccountEvaluationDto>(
      {
        method: 'PUT',
        url: `/api/app/key-account-evaluations/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
