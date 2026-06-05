import type {
  CustomerCreateDto,
  CustomerDto,
  CustomerExcelDownloadDto,
  CustomerImportDto,
  CustomerUpdateDto,
  GetCustomersInput,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { ExcelValidationResult } from '../shared/excels/models';
import type { DownloadTokenResultDto } from '../shared/models';

@Injectable({
  providedIn: 'root',
})
export class CustomerService {
  apiName = 'Default';

  create = (input: CustomerCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CustomerDto>(
      {
        method: 'POST',
        url: '/api/app/customers',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/customers/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CustomerDto>(
      {
        method: 'GET',
        url: `/api/app/customers/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getDownloadToken = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DownloadTokenResultDto>(
      {
        method: 'GET',
        url: '/api/app/customers/download-token',
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetCustomersInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<CustomerDto>>(
      {
        method: 'GET',
        url: '/api/app/customers',
        params: {
          filterText: input.filterText,
          taxCode: input.taxCode,
          customerName: input.customerName,
          customerShortName: input.customerShortName,
          address: input.address,
          phone: input.phone,
          website: input.website,
          province: input.province,
          customerType: input.customerType,
          customerIndustry: input.customerIndustry,
          fromDate: input.fromDate,
          toDate: input.toDate,
          isDeactive: input.isDeactive,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListAsExcelFile = (input: CustomerExcelDownloadDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/customers/as-excel-file',
        params: {
          downloadToken: input.downloadToken,
          taxCode: input.taxCode,
          customerName: input.customerName,
          customerShortName: input.customerShortName,
          address: input.address,
          phone: input.phone,
          country: input.country,
          province: input.province,
          website: input.website,
          customerType: input.customerType,
          customerIndustry: input.customerIndustry,
          fromDate: input.fromDate,
          toDate: input.toDate,
          note: input.note,
          isDeactive: input.isDeactive,
        },
      },
      { apiName: this.apiName, ...config },
    );

  importCustomer = (
    dataImport: ExcelValidationResult<CustomerImportDto>,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, CustomerDto[]>(
      {
        method: 'POST',
        url: '/api/app/customers/import',
        body: dataImport,
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: CustomerUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CustomerDto>(
      {
        method: 'PUT',
        url: `/api/app/customers/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseCustomer = (file: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExcelValidationResult<CustomerImportDto>>(
      {
        method: 'POST',
        url: '/api/app/customers/validater-and-parse',
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
