import type {
  CfgDiscountRatioDto,
  CfgDiscountRatioUpdateDto,
  GetCfgDiscountRatiosInput,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CfgDiscountRatioService {
  apiName = 'Default';

  getList = (input: GetCfgDiscountRatiosInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<CfgDiscountRatioDto>>(
      {
        method: 'GET',
        url: '/api/app/cfg-discount-ratios',
        params: {
          filterText: input.filterText,
          approval_Type: input.approval_Type,
          product_Type: input.product_Type,
          accountClassify: input.accountClassify,
          kaType: input.kaType,
          value_MinMin: input.value_MinMin,
          value_MinMax: input.value_MinMax,
          value_MaxMin: input.value_MaxMin,
          value_MaxMax: input.value_MaxMax,
          discountRatioMin: input.discountRatioMin,
          discountRatioMax: input.discountRatioMax,
          note: input.note,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: CfgDiscountRatioUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CfgDiscountRatioDto>(
      {
        method: 'PUT',
        url: `/api/app/cfg-discount-ratios/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
