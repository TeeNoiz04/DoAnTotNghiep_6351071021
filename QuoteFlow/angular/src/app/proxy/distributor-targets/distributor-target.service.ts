import type {
  DistributorTargetCreateDto,
  DistributorTargetDto,
  DistributorTargetExcelDownloadDto,
  DistributorTargetUpdateDto,
  GetDistributorTargetsInput,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { DownloadTokenResultDto } from '../shared/models';

@Injectable({
  providedIn: 'root',
})
export class DistributorTargetService {
  apiName = 'Default';

  create = (input: DistributorTargetCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DistributorTargetDto>(
      {
        method: 'POST',
        url: '/api/app/distributor-targets',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/distributor-targets/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DistributorTargetDto>(
      {
        method: 'GET',
        url: `/api/app/distributor-targets/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getDownloadToken = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DownloadTokenResultDto>(
      {
        method: 'GET',
        url: '/api/app/distributor-targets/download-token',
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetDistributorTargetsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<DistributorTargetDto>>(
      {
        method: 'GET',
        url: '/api/app/distributor-targets',
        params: {
          filterText: input.filterText,
          buyerTypeId: input.buyerTypeId,
          buyerId: input.buyerId,
          buyerCode: input.buyerCode,
          buyerName: input.buyerName,
          materialType: input.materialType,
          financeYearMin: input.financeYearMin,
          financeYearMax: input.financeYearMax,
          firstFYTargetMin: input.firstFYTargetMin,
          firstFYTargetMax: input.firstFYTargetMax,
          secondFYTargetMin: input.secondFYTargetMin,
          secondFYTargetMax: input.secondFYTargetMax,
          note: input.note,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListAsExcelFile = (input: DistributorTargetExcelDownloadDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/distributor-targets/as-excel-file',
        params: {
          downloadToken: input.downloadToken,
          filterText: input.filterText,
          buyerTypeId: input.buyerTypeId,
          buyerId: input.buyerId,
          buyerCode: input.buyerCode,
          materialType: input.materialType,
          financeYearMin: input.financeYearMin,
          financeYearMax: input.financeYearMax,
          firstFYTargetMin: input.firstFYTargetMin,
          firstFYTargetMax: input.firstFYTargetMax,
          secondFYTargetMin: input.secondFYTargetMin,
          secondFYTargetMax: input.secondFYTargetMax,
          note: input.note,
        },
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: DistributorTargetUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DistributorTargetDto>(
      {
        method: 'PUT',
        url: `/api/app/distributor-targets/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
