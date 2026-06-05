import type {
  AssetCreateDto,
  AssetDto,
  AssetImportDto,
  AssetUpdateDto,
  GetAssetsInput,
  SearchAssetInput,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { HistoryTrackingDto } from '../history-trackings/models';
import type { ExcelValidationResult } from '../shared/excels/models';

@Injectable({
  providedIn: 'root',
})
export class AssetService {
  apiName = 'Default';

  create = (input: AssetCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AssetDto>(
      {
        method: 'POST',
        url: '/api/app/assets',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/assets/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AssetDto>(
      {
        method: 'GET',
        url: `/api/app/assets/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getAllList = (input: GetAssetsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AssetDto[]>(
      {
        method: 'GET',
        url: '/api/app/assets/all',
        params: {
          filterText: input.filterText,
          assetName: input.assetName,
          assetClass: input.assetClass,
          assetType: input.assetType,
          warehouseId: input.warehouseId,
          warehouseName: input.warehouseName,
          salePIC: input.salePIC,
          codeMain: input.codeMain,
          codeSub: input.codeSub,
          codeMain_AF: input.codeMain_AF,
          codeSub_AF: input.codeSub_AF,
          numberOfComponentMin: input.numberOfComponentMin,
          numberOfComponentMax: input.numberOfComponentMax,
          por: input.por,
          pr: input.pr,
          giv: input.giv,
          materialCode: input.materialCode,
          modelName: input.modelName,
          unit: input.unit,
          priceMin: input.priceMin,
          priceMax: input.priceMax,
          amountMin: input.amountMin,
          amountMax: input.amountMax,
          section: input.section,
          status: input.status,
          lendingInformation: input.lendingInformation,
          note: input.note,
          source: input.source,
          invoicePriceMin: input.invoicePriceMin,
          invoicePriceMax: input.invoicePriceMax,
          division: input.division,
          department: input.department,
          sectionSAP: input.sectionSAP,
          reg: input.reg,
          requestNo: input.requestNo,
          isPendingApproval: input.isPendingApproval,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getHistory = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, HistoryTrackingDto[]>(
      {
        method: 'GET',
        url: `/api/app/assets/${id}/history`,
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetAssetsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<AssetDto>>(
      {
        method: 'GET',
        url: '/api/app/assets',
        params: {
          filterText: input.filterText,
          assetName: input.assetName,
          assetClass: input.assetClass,
          assetType: input.assetType,
          warehouseId: input.warehouseId,
          warehouseName: input.warehouseName,
          salePIC: input.salePIC,
          codeMain: input.codeMain,
          codeSub: input.codeSub,
          codeMain_AF: input.codeMain_AF,
          codeSub_AF: input.codeSub_AF,
          numberOfComponentMin: input.numberOfComponentMin,
          numberOfComponentMax: input.numberOfComponentMax,
          por: input.por,
          pr: input.pr,
          giv: input.giv,
          materialCode: input.materialCode,
          modelName: input.modelName,
          unit: input.unit,
          priceMin: input.priceMin,
          priceMax: input.priceMax,
          amountMin: input.amountMin,
          amountMax: input.amountMax,
          section: input.section,
          status: input.status,
          lendingInformation: input.lendingInformation,
          note: input.note,
          source: input.source,
          invoicePriceMin: input.invoicePriceMin,
          invoicePriceMax: input.invoicePriceMax,
          division: input.division,
          department: input.department,
          sectionSAP: input.sectionSAP,
          reg: input.reg,
          requestNo: input.requestNo,
          isPendingApproval: input.isPendingApproval,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListAssetExcel = (input: GetAssetsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/assets/as-file-excel',
        params: {
          filterText: input.filterText,
          assetName: input.assetName,
          assetClass: input.assetClass,
          assetType: input.assetType,
          warehouseId: input.warehouseId,
          warehouseName: input.warehouseName,
          salePIC: input.salePIC,
          codeMain: input.codeMain,
          codeSub: input.codeSub,
          codeMain_AF: input.codeMain_AF,
          codeSub_AF: input.codeSub_AF,
          numberOfComponentMin: input.numberOfComponentMin,
          numberOfComponentMax: input.numberOfComponentMax,
          por: input.por,
          pr: input.pr,
          giv: input.giv,
          materialCode: input.materialCode,
          modelName: input.modelName,
          unit: input.unit,
          priceMin: input.priceMin,
          priceMax: input.priceMax,
          amountMin: input.amountMin,
          amountMax: input.amountMax,
          section: input.section,
          status: input.status,
          lendingInformation: input.lendingInformation,
          note: input.note,
          source: input.source,
          invoicePriceMin: input.invoicePriceMin,
          invoicePriceMax: input.invoicePriceMax,
          division: input.division,
          department: input.department,
          sectionSAP: input.sectionSAP,
          reg: input.reg,
          requestNo: input.requestNo,
          isPendingApproval: input.isPendingApproval,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  getListAssetReportCategoryExcel = (input: GetAssetsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/app/assets/export-report-asset',
        params: {
          filterText: input.filterText,
          assetName: input.assetName,
          assetClass: input.assetClass,
          assetType: input.assetType,
          warehouseId: input.warehouseId,
          warehouseName: input.warehouseName,
          salePIC: input.salePIC,
          codeMain: input.codeMain,
          codeSub: input.codeSub,
          codeMain_AF: input.codeMain_AF,
          codeSub_AF: input.codeSub_AF,
          numberOfComponentMin: input.numberOfComponentMin,
          numberOfComponentMax: input.numberOfComponentMax,
          por: input.por,
          pr: input.pr,
          giv: input.giv,
          materialCode: input.materialCode,
          modelName: input.modelName,
          unit: input.unit,
          priceMin: input.priceMin,
          priceMax: input.priceMax,
          amountMin: input.amountMin,
          amountMax: input.amountMax,
          section: input.section,
          status: input.status,
          lendingInformation: input.lendingInformation,
          note: input.note,
          source: input.source,
          invoicePriceMin: input.invoicePriceMin,
          invoicePriceMax: input.invoicePriceMax,
          division: input.division,
          department: input.department,
          sectionSAP: input.sectionSAP,
          reg: input.reg,
          requestNo: input.requestNo,
          isPendingApproval: input.isPendingApproval,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  importAsset = (data: ExcelValidationResult<AssetImportDto>, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AssetDto[]>(
      {
        method: 'POST',
        url: '/api/app/assets/import',
        body: data,
      },
      { apiName: this.apiName, ...config },
    );

  search = (input: SearchAssetInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<AssetDto>>(
      {
        method: 'GET',
        url: '/api/app/assets/search',
        params: {
          requestId: input.requestId,
          filterText: input.filterText,
          assetName: input.assetName,
          assetClass: input.assetClass,
          assetType: input.assetType,
          warehouseId: input.warehouseId,
          warehouseName: input.warehouseName,
          salePIC: input.salePIC,
          codeMain: input.codeMain,
          codeSub: input.codeSub,
          codeMain_AF: input.codeMain_AF,
          codeSub_AF: input.codeSub_AF,
          numberOfComponentMin: input.numberOfComponentMin,
          numberOfComponentMax: input.numberOfComponentMax,
          por: input.por,
          pr: input.pr,
          giv: input.giv,
          materialCode: input.materialCode,
          modelName: input.modelName,
          unit: input.unit,
          priceMin: input.priceMin,
          priceMax: input.priceMax,
          amountMin: input.amountMin,
          amountMax: input.amountMax,
          section: input.section,
          status: input.status,
          lendingInformation: input.lendingInformation,
          note: input.note,
          source: input.source,
          invoicePriceMin: input.invoicePriceMin,
          invoicePriceMax: input.invoicePriceMax,
          division: input.division,
          department: input.department,
          sectionSAP: input.sectionSAP,
          reg: input.reg,
          requestNo: input.requestNo,
          isPendingApproval: input.isPendingApproval,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: AssetUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AssetDto>(
      {
        method: 'PUT',
        url: `/api/app/assets/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  updateAsset = (data: ExcelValidationResult<AssetImportDto>, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AssetDto[]>(
      {
        method: 'POST',
        url: '/api/app/assets/update',
        body: data,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseAsset = (file: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExcelValidationResult<AssetImportDto>>(
      {
        method: 'POST',
        url: '/api/app/assets/validate-and-parse',
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseAssetUpdate = (file: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExcelValidationResult<AssetImportDto>>(
      {
        method: 'POST',
        url: '/api/app/assets/validate-and-parse-update',
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
