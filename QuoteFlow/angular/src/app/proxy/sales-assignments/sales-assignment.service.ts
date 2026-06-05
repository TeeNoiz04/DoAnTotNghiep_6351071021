import type {
  GetSalesAssignmentInput,
  SaleReportByCustomerDto,
  SaleReportByCustomerR05Dto,
  SaleReportInput,
  SalesAssignmentCreateDto,
  SalesAssignmentDto,
  SalesAssignmentUpdateDto,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { UserLookupDto } from '../shared/models';

@Injectable({
  providedIn: 'root',
})
export class SalesAssignmentService {
  apiName = 'Default';

  create = (input: SalesAssignmentCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SalesAssignmentDto[]>(
      {
        method: 'POST',
        url: '/api/app/sales-assignments',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/sales-assignments/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SalesAssignmentDto>(
      {
        method: 'GET',
        url: `/api/app/sales-assignments/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetSalesAssignmentInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<SalesAssignmentDto>>(
      {
        method: 'GET',
        url: '/api/app/sales-assignments',
        params: {
          filterText: input.filterText,
          saleUserName: input.saleUserName,
          materialType: input.materialType,
          locationId: input.locationId,
          buyerId: input.buyerId,
          buyerTypeId: input.buyerTypeId,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListSaleReportDetail = (input: SaleReportInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<SaleReportByCustomerDto>>(
      {
        method: 'GET',
        url: '/api/app/sales-assignments/report-sale-data',
        params: {
          fromDate: input.fromDate,
          toDate: input.toDate,
          invoiceFromDate: input.invoiceFromDate,
          invoiceToDate: input.invoiceToDate,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListSaleReportDetailAsExcel = (input: SaleReportInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/sales-assignments/report-sale',
        params: {
          fromDate: input.fromDate,
          toDate: input.toDate,
          invoiceFromDate: input.invoiceFromDate,
          invoiceToDate: input.invoiceToDate,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListSaleReportGeneral = (input: SaleReportInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<SaleReportByCustomerR05Dto>>(
      {
        method: 'GET',
        url: '/api/app/sales-assignments/report-sale-general-data',
        params: {
          fromDate: input.fromDate,
          toDate: input.toDate,
          invoiceFromDate: input.invoiceFromDate,
          invoiceToDate: input.invoiceToDate,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListSaleReportGeneralAsExcel = (input: SaleReportInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/sales-assignments/report-sale-general',
        params: {
          fromDate: input.fromDate,
          toDate: input.toDate,
          invoiceFromDate: input.invoiceFromDate,
          invoiceToDate: input.invoiceToDate,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListUserLookupByName = (name: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, UserLookupDto[]>(
      {
        method: 'GET',
        url: '/api/app/sales-assignments/user-lookups',
        params: { name },
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: SalesAssignmentUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SalesAssignmentDto>(
      {
        method: 'PUT',
        url: `/api/app/sales-assignments/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
