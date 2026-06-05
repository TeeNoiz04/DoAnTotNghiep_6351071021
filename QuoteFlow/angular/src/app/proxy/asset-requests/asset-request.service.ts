import type {
  AssetRequestActionDto,
  AssetRequestCreateDto,
  AssetRequestDto,
  AssetRequestUpdateDto,
  AssetRequestUpdateExtendDto,
  GetAssetRequestsInput,
  ResultValidateAssetAuditDto,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { ApproverDto } from '../approval-routes/models';
import type { ActionDto, UserLookupDto } from '../shared/models';

@Injectable({
  providedIn: 'root',
})
export class AssetRequestService {
  apiName = 'Default';

  create = (input: AssetRequestCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AssetRequestDto>(
      {
        method: 'POST',
        url: '/api/app/asset-requests',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/asset-requests/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  extendRequest = (id: string, input: AssetRequestUpdateExtendDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AssetRequestDto>(
      {
        method: 'POST',
        url: `/api/app/asset-requests/${id}/extend`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AssetRequestDto>(
      {
        method: 'GET',
        url: `/api/app/asset-requests/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetAssetRequestsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<AssetRequestDto>>(
      {
        method: 'GET',
        url: '/api/app/asset-requests',
        params: {
          filterText: input.filterText,
          title: input.title,
          requestNo: input.requestNo,
          requestType: input.requestType,
          status: input.status,
          warehouseSrcId: input.warehouseSrcId,
          warehouseSrcName: input.warehouseSrcName,
          warehouseDestId: input.warehouseDestId,
          warehouseDestName: input.warehouseDestName,
          pic_Src: input.pic_Src,
          pic_Dest: input.pic_Dest,
          lending_CustomerTaxCode: input.lending_CustomerTaxCode,
          lending_ReturnDateMin: input.lending_ReturnDateMin,
          lending_ReturnDateMax: input.lending_ReturnDateMax,
          requestOwner: input.requestOwner,
          submittedDateMin: input.submittedDateMin,
          submittedDateMax: input.submittedDateMax,
          note: input.note,
          currentApprovalRouteInstanceId: input.currentApprovalRouteInstanceId,
          currentApprovalStepSequenceMin: input.currentApprovalStepSequenceMin,
          currentApprovalStepSequenceMax: input.currentApprovalStepSequenceMax,
          currentApproverRoleName: input.currentApproverRoleName,
          currentApproverRoleCode: input.currentApproverRoleCode,
          requestTypes: input.requestTypes,
          statuses: input.statuses,
          createBy: input.createBy,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListApproval = (input: GetAssetRequestsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<AssetRequestDto>>(
      {
        method: 'GET',
        url: '/api/app/asset-requests/request-approval',
        params: {
          filterText: input.filterText,
          title: input.title,
          requestNo: input.requestNo,
          requestType: input.requestType,
          status: input.status,
          warehouseSrcId: input.warehouseSrcId,
          warehouseSrcName: input.warehouseSrcName,
          warehouseDestId: input.warehouseDestId,
          warehouseDestName: input.warehouseDestName,
          pic_Src: input.pic_Src,
          pic_Dest: input.pic_Dest,
          lending_CustomerTaxCode: input.lending_CustomerTaxCode,
          lending_ReturnDateMin: input.lending_ReturnDateMin,
          lending_ReturnDateMax: input.lending_ReturnDateMax,
          requestOwner: input.requestOwner,
          submittedDateMin: input.submittedDateMin,
          submittedDateMax: input.submittedDateMax,
          note: input.note,
          currentApprovalRouteInstanceId: input.currentApprovalRouteInstanceId,
          currentApprovalStepSequenceMin: input.currentApprovalStepSequenceMin,
          currentApprovalStepSequenceMax: input.currentApprovalStepSequenceMax,
          currentApproverRoleName: input.currentApproverRoleName,
          currentApproverRoleCode: input.currentApproverRoleCode,
          requestTypes: input.requestTypes,
          statuses: input.statuses,
          createBy: input.createBy,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListApprovers = (assetRequestId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ApproverDto[]>(
      {
        method: 'GET',
        url: `/api/app/asset-requests/${assetRequestId}/approvers`,
      },
      { apiName: this.apiName, ...config },
    );

  getListAssetExcel = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: `/api/app/asset-requests/${id}/export`,
      },
      { apiName: this.apiName, ...config },
    );

  getSaleRolePICLookup = (requestType: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, UserLookupDto[]>(
      {
        method: 'GET',
        url: '/api/app/asset-requests/sale-pic',
        params: { requestType },
      },
      { apiName: this.apiName, ...config },
    );

  importAssetAudit = (resultValidate: ResultValidateAssetAuditDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AssetRequestDto>(
      {
        method: 'POST',
        url: '/api/app/asset-requests/import-audit',
        body: resultValidate,
      },
      { apiName: this.apiName, ...config },
    );

  performAction = (id: string, input: ActionDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AssetRequestDto>(
      {
        method: 'POST',
        url: `/api/app/asset-requests/${id}/actions`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  submitRequest = (input: AssetRequestActionDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AssetRequestDto>(
      {
        method: 'POST',
        url: '/api/app/asset-requests/submit',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: AssetRequestUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AssetRequestDto>(
      {
        method: 'PUT',
        url: `/api/app/asset-requests/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseAsset = (file: FormData, requestId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ResultValidateAssetAuditDto>(
      {
        method: 'POST',
        url: '/api/app/asset-requests/validate-and-parse',
        params: { requestId },
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
