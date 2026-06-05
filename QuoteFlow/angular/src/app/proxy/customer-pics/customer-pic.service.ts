import type { CustomerPICCreateDto, CustomerPICDto, CustomerPICUpdateDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CustomerPICService {
  apiName = 'Default';

  create = (input: CustomerPICCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CustomerPICDto>(
      {
        method: 'POST',
        url: '/api/app/customer-pICs',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/customer-pICs/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: CustomerPICUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CustomerPICDto>(
      {
        method: 'PUT',
        url: `/api/app/customer-pICs/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
