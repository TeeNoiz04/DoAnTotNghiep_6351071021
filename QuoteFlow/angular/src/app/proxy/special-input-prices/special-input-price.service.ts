import type {
  GetSpecialInputPricesInput,
  SpecialInputPriceCreateDto,
  SpecialInputPriceDetailImportDto,
  SpecialInputPriceDto,
  SpecialInputPriceUpdateDto,
} from './models';
import type { SpecialInputPriceDetailDto } from './special-input-price-details/models';
import { RestService, Rest } from '@abp/ng.core';
import type { ListResultDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { ExcelValidationResult } from '../shared/excels/models';

@Injectable({
  providedIn: 'root',
})
export class SpecialInputPriceService {
  apiName = 'Default';

  create = (input: SpecialInputPriceCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SpecialInputPriceDto>(
      {
        method: 'POST',
        url: '/api/app/special-input-prices',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/special-input-prices/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SpecialInputPriceDto>(
      {
        method: 'GET',
        url: `/api/app/special-input-prices/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getDetails = (specialInputPriceId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ListResultDto<SpecialInputPriceDetailDto>>(
      {
        method: 'GET',
        url: `/api/app/special-input-prices/${specialInputPriceId}/details`,
      },
      { apiName: this.apiName, ...config },
    );

  getInputPriceAsExcel = (input: GetSpecialInputPricesInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/special-input-prices/export-input-price',
        params: {
          filterText: input.filterText,
          accountNo: input.accountNo,
          accountName: input.accountName,
          materials: input.materials,
          models: input.models,
          projectName: input.projectName,
          validFromMin: input.validFromMin,
          validFromMax: input.validFromMax,
          validToMin: input.validToMin,
          validToMax: input.validToMax,
          status: input.status,
          note: input.note,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetSpecialInputPricesInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<SpecialInputPriceDto>>(
      {
        method: 'GET',
        url: '/api/app/special-input-prices',
        params: {
          filterText: input.filterText,
          accountNo: input.accountNo,
          accountName: input.accountName,
          materials: input.materials,
          models: input.models,
          projectName: input.projectName,
          validFromMin: input.validFromMin,
          validFromMax: input.validFromMax,
          validToMin: input.validToMin,
          validToMax: input.validToMax,
          status: input.status,
          note: input.note,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  importSpecialInputPriceDetails = (
    data: ExcelValidationResult<SpecialInputPriceDetailImportDto>,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/special-input-prices/import',
        body: data,
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: SpecialInputPriceUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SpecialInputPriceDto>(
      {
        method: 'PUT',
        url: `/api/app/special-input-prices/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseSpecialInputPriceDetails = (file: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExcelValidationResult<SpecialInputPriceDetailImportDto>>(
      {
        method: 'POST',
        url: '/api/app/special-input-prices/validate-and-parse',
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
