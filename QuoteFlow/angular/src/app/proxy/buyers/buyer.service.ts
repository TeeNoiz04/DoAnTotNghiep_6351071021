import type { BuyerCreateDto, BuyerDto, BuyerUpdateDto, GetBuyersInput } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class BuyerService {
  apiName = 'Default';

  create = (input: BuyerCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BuyerDto>(
      {
        method: 'POST',
        url: '/api/app/buyers',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/buyers/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BuyerDto>(
      {
        method: 'GET',
        url: `/api/app/buyers/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetBuyersInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<BuyerDto>>(
      {
        method: 'GET',
        url: '/api/app/buyers',
        params: {
          filterText: input.filterText,
          buyerTypeId: input.buyerTypeId,
          buyerCode: input.buyerCode,
          shortName: input.shortName,
          fullName: input.fullName,
          taxCode: input.taxCode,
          address: input.address,
          contactPerson: input.contactPerson,
          contactEmail: input.contactEmail,
          contactPhoneNumber: input.contactPhoneNumber,
          paymentTermCode: input.paymentTermCode,
          paymentTermDescription: input.paymentTermDescription,
          creditLimitMax: input.creditLimitMax,
          creditLimitMin: input.creditLimitMin,
          creditExposureMax: input.creditExposureMax,
          creditExposureMin: input.creditExposureMin,
          appliedPrice: input.appliedPrice,
          deactive: input.deactive,
          note: input.note,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListAsExcelFile = (input: GetBuyersInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/buyers/as-excel-file',
        params: {
          filterText: input.filterText,
          buyerTypeId: input.buyerTypeId,
          buyerCode: input.buyerCode,
          shortName: input.shortName,
          fullName: input.fullName,
          taxCode: input.taxCode,
          address: input.address,
          contactPerson: input.contactPerson,
          contactEmail: input.contactEmail,
          contactPhoneNumber: input.contactPhoneNumber,
          paymentTermCode: input.paymentTermCode,
          paymentTermDescription: input.paymentTermDescription,
          creditLimitMax: input.creditLimitMax,
          creditLimitMin: input.creditLimitMin,
          creditExposureMax: input.creditExposureMax,
          creditExposureMin: input.creditExposureMin,
          appliedPrice: input.appliedPrice,
          deactive: input.deactive,
          note: input.note,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: BuyerUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BuyerDto>(
      {
        method: 'PUT',
        url: `/api/app/buyers/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
