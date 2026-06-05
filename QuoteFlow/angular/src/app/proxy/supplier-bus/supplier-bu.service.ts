import type {
  GetSupplierBUsInput,
  SupplierBUCreateDto,
  SupplierBUDto,
  SupplierBUExcelDownloadDto,
  SupplierBUImportDto,
  SupplierBUUpdateDto,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { ExcelValidationResult } from '../shared/excels/models';
import type { DownloadTokenResultDto } from '../shared/models';

@Injectable({
  providedIn: 'root',
})
export class SupplierBUService {
  apiName = 'Default';

  changeDeactiveSupplierBU = (ids: string[], config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/supplier-bUs/change-active',
        body: ids,
      },
      { apiName: this.apiName, ...config },
    );

  create = (input: SupplierBUCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SupplierBUDto>(
      {
        method: 'POST',
        url: '/api/app/supplier-bUs',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/supplier-bUs/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SupplierBUDto>(
      {
        method: 'GET',
        url: `/api/app/supplier-bUs/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getDownloadToken = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DownloadTokenResultDto>(
      {
        method: 'GET',
        url: '/api/app/supplier-bUs/download-token',
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetSupplierBUsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<SupplierBUDto>>(
      {
        method: 'GET',
        url: '/api/app/supplier-bUs',
        params: {
          filterText: input.filterText,
          supplierBUCode: input.supplierBUCode,
          supplierBURemarks: input.supplierBURemarks,
          orderMethod: input.orderMethod,
          poTemplate: input.poTemplate,
          contact: input.contact,
          email: input.email,
          incoTerm: input.incoTerm,
          paymentTermCode: input.paymentTermCode,
          paymentDescription: input.paymentDescription,
          currency: input.currency,
          materialType: input.materialType,
          supplierId: input.supplierId,
          supplierCode: input.supplierCode,
          supplierShortName: input.supplierShortName,
          supplierAddress: input.supplierAddress,
          sortOrderMin: input.sortOrderMin,
          sortOrderMax: input.sortOrderMax,
          fascmVendorCode: input.fascmVendorCode,
          fascmBuyerCode: input.fascmBuyerCode,
          fascmConsigneeCode: input.fascmConsigneeCode,
          fascmSectionCode: input.fascmSectionCode,
          fascmPaymentTerm: input.fascmPaymentTerm,
          fascmFreightMethod: input.fascmFreightMethod,
          fascmDeliveryTerms: input.fascmDeliveryTerms,
          fascmPlaceOfDeliveryTerms: input.fascmPlaceOfDeliveryTerms,
          fascmShippingMarkCode: input.fascmShippingMarkCode,
          isDeactive: input.isDeactive,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListAsExcelFile = (input: SupplierBUExcelDownloadDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/supplier-bUs/as-excel-file',
        params: {
          downloadToken: input.downloadToken,
          filterText: input.filterText,
          supplierBUCode: input.supplierBUCode,
          supplierBURemarks: input.supplierBURemarks,
          orderMethod: input.orderMethod,
          poTemplate: input.poTemplate,
          contact: input.contact,
          email: input.email,
          incoTerm: input.incoTerm,
          paymentTermCode: input.paymentTermCode,
          paymentDescription: input.paymentDescription,
          currency: input.currency,
          materialType: input.materialType,
          supplierId: input.supplierId,
          supplierCode: input.supplierCode,
          supplierShortName: input.supplierShortName,
          supplierAddress: input.supplierAddress,
          sortOrderMin: input.sortOrderMin,
          sortOrderMax: input.sortOrderMax,
          fascmVendorCode: input.fascmVendorCode,
          fascmBuyerCode: input.fascmBuyerCode,
          fascmConsigneeCode: input.fascmConsigneeCode,
          fascmSectionCode: input.fascmSectionCode,
          fascmPaymentTerm: input.fascmPaymentTerm,
          fascmFreightMethod: input.fascmFreightMethod,
          fascmDeliveryTerms: input.fascmDeliveryTerms,
          fascmPlaceOfDeliveryTerms: input.fascmPlaceOfDeliveryTerms,
          fascmShippingMarkCode: input.fascmShippingMarkCode,
          isDeactive: input.isDeactive,
        },
      },
      { apiName: this.apiName, ...config },
    );

  importSupplierBU = (
    dataImport: ExcelValidationResult<SupplierBUImportDto>,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, SupplierBUDto[]>(
      {
        method: 'POST',
        url: '/api/app/supplier-bUs/import',
        body: dataImport,
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: SupplierBUUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, SupplierBUDto>(
      {
        method: 'PUT',
        url: `/api/app/supplier-bUs/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseSupplierBU = (file: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExcelValidationResult<SupplierBUImportDto>>(
      {
        method: 'POST',
        url: '/api/app/supplier-bUs/validater-and-parse',
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
