import type {
  GetSuppliersInput,
  SupplierCreateDto,
  SupplierDto,
  SupplierUpdateDto,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class SupplierService {
  apiName = 'Default';

  create = (input: SupplierCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SupplierDto>(
      {
        method: 'POST',
        url: '/api/app/suppliers',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/suppliers/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SupplierDto>(
      {
        method: 'GET',
        url: `/api/app/suppliers/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetSuppliersInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<SupplierDto>>(
      {
        method: 'GET',
        url: '/api/app/suppliers',
        params: {
          filterText: input.filterText,
          supplierCode: input.supplierCode,
          shortName: input.shortName,
          fullName: input.fullName,
          taxCode: input.taxCode,
          address: input.address,
          isDeactive: input.isDeactive,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: SupplierUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SupplierDto>(
      {
        method: 'PUT',
        url: `/api/app/suppliers/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
