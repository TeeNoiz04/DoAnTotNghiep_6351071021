import type {
  GetPSIImportsInput,
  GetPSIReportsInput,
  GetPSIsInput,
  PSIByProductExportInput,
  PSICreateDto,
  PSIDto,
  PSIExcelDownloadDto,
  PSIExportDataDto,
  PSIImportDto,
  PSIReportDto,
  PSIUpdateDto,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { ApproverDto } from '../approval-routes/models';
import type { ExcelValidationResult } from '../shared/excels/models';
import type { ActionDto, DownloadTokenResultDto } from '../shared/models';

@Injectable({
  providedIn: 'root',
})
export class PSIService {
  apiName = 'Default';

  create = (input: PSICreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PSIDto>(
      {
        method: 'POST',
        url: '/api/app/psi',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/psi/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PSIDto>(
      {
        method: 'GET',
        url: `/api/app/psi/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getDetail = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PSIDto>(
      {
        method: 'GET',
        url: `/api/app/psi/detail/${id}/psi-data`,
      },
      { apiName: this.apiName, ...config },
    );

  getDownloadToken = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DownloadTokenResultDto>(
      {
        method: 'GET',
        url: '/api/app/psi/download-token',
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetPSIsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PSIDto>>(
      {
        method: 'GET',
        url: '/api/app/psi',
        params: {
          filterText: input.filterText,
          psiCode: input.psiCode,
          fy: input.fy,
          fileName: input.fileName,
          note: input.note,
          status: input.status,
          materialType: input.materialType,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListApprovers = (pSI_Id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ApproverDto[]>(
      {
        method: 'GET',
        url: '/api/app/psi/approvers',
        params: { pSI_Id },
      },
      { apiName: this.apiName, ...config },
    );

  getListAsExcel = (input: GetPSIReportsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/psi/psi-report/export',
        params: {
          fy: input.fy,
          userName: input.userName,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListAsExcelFile = (input: PSIExcelDownloadDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/psi/as-excel-file',
        params: {
          downloadToken: input.downloadToken,
          filterText: input.filterText,
          psiCode: input.psiCode,
          fy: input.fy,
          fileName: input.fileName,
          note: input.note,
          status: input.status,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListPSIReport = (input: GetPSIReportsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PSIReportDto>>(
      {
        method: 'GET',
        url: '/api/app/psi/psi-report',
        params: {
          fy: input.fy,
          userName: input.userName,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListPSIReportData = (input: PSIByProductExportInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PSIExportDataDto[]>(
      {
        method: 'GET',
        url: '/api/app/psi/psi-report-data',
        params: {
          materialType: input.materialType,
          fiscalYear: input.fiscalYear,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListPending = (input: GetPSIsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<PSIDto>>(
      {
        method: 'GET',
        url: '/api/app/psi/pending',
        params: {
          filterText: input.filterText,
          psiCode: input.psiCode,
          fy: input.fy,
          fileName: input.fileName,
          note: input.note,
          status: input.status,
          materialType: input.materialType,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getPSIByProductExport = (input: PSIByProductExportInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'POST',
        responseType: 'blob',
        url: '/api/app/psi/psi-by-product-export',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  importFA = (
    validationResult: ExcelValidationResult<PSIImportDto>,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/psi/import-psi-fa',
        body: validationResult,
      },
      { apiName: this.apiName, ...config },
    );

  importLVS = (
    validationResult: ExcelValidationResult<PSIImportDto>,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/psi/import-psi-lvs',
        body: validationResult,
      },
      { apiName: this.apiName, ...config },
    );

  performAction = (id: string, input: ActionDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PSIDto>(
      {
        method: 'POST',
        url: '/api/app/psi/perform-actions',
        params: { id },
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: PSIUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PSIDto>(
      {
        method: 'PUT',
        url: `/api/app/psi/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseFA = (file: FormData, input: GetPSIImportsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExcelValidationResult<PSIImportDto>>(
      {
        method: 'POST',
        url: '/api/app/psi/validate-psi-fa',
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseLVS = (
    file: FormData,
    input: GetPSIImportsInput,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, ExcelValidationResult<PSIImportDto>>(
      {
        method: 'POST',
        url: '/api/app/psi/validate-psi-lvs',
      },
      { apiName: this.apiName, ...config },
    );

  validationPSI = (
    materialType: string,
    currency: string,
    total: number,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/psi/validation-psi',
        params: { materialType, currency, total },
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
