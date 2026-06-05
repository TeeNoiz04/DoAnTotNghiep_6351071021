import type { MaterialApprovalRequestDetailDto } from './material-approval-request-details/models';
import type { MaterialApprovalRequestDto } from './material-approval-requests/models';
import type { MaterialFactoryUpdateExcelDto } from './material-import/material-factory/models';
import type { MaterialSAPUpdateExcelDto } from './material-import/material-sap/models';
import type { MaterialStatusUpdateExcelDto } from './material-import/material-status/models';
import type {
  GetMaterialsApprovalInput,
  GetMaterialsInput,
  MaterialCreateDto,
  MaterialDto,
  MaterialNewRegistrationImportDto,
  MaterialUpdateDto,
  MaterialUpdateInventoryPlanImportDto,
  MaterialUpdatePriceImportDto,
  MaterialUpdateWithoutPriceImportDto,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { ApproverDto } from '../approval-routes/models';
import type { HistoryTrackingDto } from '../history-trackings/models';
import type { ExcelValidationResult } from '../shared/excels/models';
import type { ActionDto } from '../shared/models';

@Injectable({
  providedIn: 'root',
})
export class MaterialService {
  apiName = 'Default';

  create = (input: MaterialCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, MaterialDto>(
      {
        method: 'POST',
        url: '/api/app/materials',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/materials/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, MaterialDto>(
      {
        method: 'GET',
        url: `/api/app/materials/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetMaterialsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<MaterialDto>>(
      {
        method: 'GET',
        url: '/api/app/materials',
        params: {
          golfaCodes: input.golfaCodes,
          models: input.models,
          sapCode: input.sapCode,
          materialType: input.materialType,
          materialGroup: input.materialGroup,
          supplier: input.supplier,
          supplierBUId: input.supplierBUId,
          supplierBU: input.supplierBU,
          materialStatus: input.materialStatus,
          stockQty: input.stockQty,
          onOrderStock: input.onOrderStock,
          isDeactive: input.isDeactive,
          stockId: input.stockId,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListApproval = (input: GetMaterialsApprovalInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<MaterialApprovalRequestDto>>(
      {
        method: 'GET',
        url: '/api/app/materials/approval',
        params: {
          golfaCode: input.golfaCode,
          model: input.model,
          importType: input.importType,
          approvalStatus: input.approvalStatus,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListApprovers = (materialId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ApproverDto[]>(
      {
        method: 'GET',
        url: `/api/app/materials/${materialId}/approvers`,
      },
      { apiName: this.apiName, ...config },
    );

  getListAsExcelFile = (input: GetMaterialsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/materials/export',
        params: {
          golfaCodes: input.golfaCodes,
          models: input.models,
          sapCode: input.sapCode,
          materialType: input.materialType,
          materialGroup: input.materialGroup,
          supplier: input.supplier,
          supplierBUId: input.supplierBUId,
          supplierBU: input.supplierBU,
          materialStatus: input.materialStatus,
          stockQty: input.stockQty,
          onOrderStock: input.onOrderStock,
          isDeactive: input.isDeactive,
          stockId: input.stockId,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListByApprovalId = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<MaterialApprovalRequestDetailDto>>(
      {
        method: 'GET',
        url: `/api/app/materials/${id}/approval-details`,
      },
      { apiName: this.apiName, ...config },
    );

  getListMaterialHistory = (golfaCode: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, HistoryTrackingDto[]>(
      {
        method: 'GET',
        url: '/api/app/materials/history-tracking',
        params: { golfaCode },
      },
      { apiName: this.apiName, ...config },
    );

  getListMyApproval = (input: GetMaterialsApprovalInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<MaterialApprovalRequestDto>>(
      {
        method: 'GET',
        url: '/api/app/materials/my-approval',
        params: {
          golfaCode: input.golfaCode,
          model: input.model,
          importType: input.importType,
          approvalStatus: input.approvalStatus,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getMaterialApprovalDetail = (approvalId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, MaterialApprovalRequestDto>(
      {
        method: 'GET',
        url: `/api/app/materials/my-approval-detail/${approvalId}`,
      },
      { apiName: this.apiName, ...config },
    );

  getMaterialHistoryAsExcel = (golfaCode: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/materials/history-tracking/export',
        params: { golfaCode },
      },
      { apiName: this.apiName, ...config },
    );

  importMaterialNewRegistration = (
    data: ExcelValidationResult<MaterialNewRegistrationImportDto>,
    note?: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, MaterialApprovalRequestDto>(
      {
        method: 'POST',
        url: '/api/app/materials/import/material-new-registration',
        params: { note },
        body: data,
      },
      { apiName: this.apiName, ...config },
    );

  importMaterialUpdatePrice = (
    data: ExcelValidationResult<MaterialUpdatePriceImportDto>,
    note?: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, MaterialApprovalRequestDto>(
      {
        method: 'POST',
        url: '/api/app/materials/import/material-update-price',
        params: { note },
        body: data,
      },
      { apiName: this.apiName, ...config },
    );

  importMaterialUpdateWithoutPrice = (
    data: ExcelValidationResult<MaterialUpdateWithoutPriceImportDto>,
    note?: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, MaterialApprovalRequestDto>(
      {
        method: 'POST',
        url: '/api/app/materials/import/update-without-price',
        params: { note },
        body: data,
      },
      { apiName: this.apiName, ...config },
    );

  materialUpdateInventoryPlan = (
    data: ExcelValidationResult<MaterialUpdateInventoryPlanImportDto>,
    note?: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, MaterialApprovalRequestDto>(
      {
        method: 'POST',
        url: '/api/app/materials/import/update-inventory-plan',
        params: { note },
        body: data,
      },
      { apiName: this.apiName, ...config },
    );

  performAction = (id: string, input: ActionDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, MaterialApprovalRequestDto>(
      {
        method: 'POST',
        url: '/api/app/materials/perform-action',
        params: { id },
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: MaterialUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, MaterialDto>(
      {
        method: 'PUT',
        url: `/api/app/materials/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  updateMaterialFactory = (
    data: ExcelValidationResult<MaterialFactoryUpdateExcelDto>,
    note: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/materials/import/factory',
        params: { note },
        body: data,
      },
      { apiName: this.apiName, ...config },
    );

  updateMaterialSAP = (
    data: ExcelValidationResult<MaterialSAPUpdateExcelDto>,
    note: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/materials/import/sap',
        params: { note },
        body: data,
      },
      { apiName: this.apiName, ...config },
    );

  updateMaterialStatus = (
    data: ExcelValidationResult<MaterialStatusUpdateExcelDto>,
    note: string,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, MaterialApprovalRequestDto>(
      {
        method: 'POST',
        url: '/api/app/materials/import/status',
        params: { note },
        body: data,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseFactory = (file: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExcelValidationResult<MaterialFactoryUpdateExcelDto>>(
      {
        method: 'POST',
        url: '/api/app/materials/validate-and-parse/factory',
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseNewRegistration = (file: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExcelValidationResult<MaterialNewRegistrationImportDto>>(
      {
        method: 'POST',
        url: '/api/app/materials/validate-and-parse/new-registration',
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseSAP = (file: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExcelValidationResult<MaterialSAPUpdateExcelDto>>(
      {
        method: 'POST',
        url: '/api/app/materials/validate-and-parse/sap',
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseStatus = (file: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExcelValidationResult<MaterialStatusUpdateExcelDto>>(
      {
        method: 'POST',
        url: '/api/app/materials/validate-and-parse/status',
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseUpdateInventoryPlan = (file: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExcelValidationResult<MaterialUpdateInventoryPlanImportDto>>(
      {
        method: 'POST',
        url: '/api/app/materials/validate-and-parse/update-inventory-plan',
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseUpdatePrice = (file: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExcelValidationResult<MaterialUpdatePriceImportDto>>(
      {
        method: 'POST',
        url: '/api/app/materials/validate-and-parse/update-price',
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseUpdateWithoutPrice = (file: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExcelValidationResult<MaterialUpdateWithoutPriceImportDto>>(
      {
        method: 'POST',
        url: '/api/app/materials/validate-and-parse/update-without-price',
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
