import type { StockImportPriorityExcelDto } from './excel/models';
import type {
  GetStockImportPrioritiesInput,
  StockImportPriorityCreateDto,
  StockImportPriorityDto,
  StockImportPriorityUpdateDto,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { ExcelValidationResult } from '../shared/excels/models';

@Injectable({
  providedIn: 'root',
})
export class StockImportPriorityService {
  apiName = 'Default';

  create = (input: StockImportPriorityCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StockImportPriorityDto>(
      {
        method: 'POST',
        url: '/api/app/stock-import-priorities',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/stock-import-priorities/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  deleteMany = (ids: string[], config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/stock-import-priorities/delete-list',
        body: ids,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StockImportPriorityDto>(
      {
        method: 'GET',
        url: `/api/app/stock-import-priorities/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetStockImportPrioritiesInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<StockImportPriorityDto>>(
      {
        method: 'GET',
        url: '/api/app/stock-import-priorities',
        params: {
          filterText: input.filterText,
          dpoNo: input.dpoNo,
          poNo: input.poNo,
          materialCode: input.materialCode,
          model: input.model,
          statusCode: input.statusCode,
          qtyMin: input.qtyMin,
          qtyMax: input.qtyMax,
          priorityMin: input.priorityMin,
          priorityMax: input.priorityMax,
          qtyUsedMin: input.qtyUsedMin,
          qtyUsedMax: input.qtyUsedMax,
          qtyAvailableMin: input.qtyAvailableMin,
          qtyAvailableMax: input.qtyAvailableMax,
          note: input.note,
          importGuid: input.importGuid,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  importStock = (
    data: ExcelValidationResult<StockImportPriorityExcelDto>,
    config?: Partial<Rest.Config>,
  ) =>
    this.restService.request<any, void>(
      {
        method: 'POST',
        url: '/api/app/stock-import-priorities/import',
        body: data,
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: StockImportPriorityUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StockImportPriorityDto>(
      {
        method: 'PUT',
        url: `/api/app/stock-import-priorities/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  validateAndParseStock = (file: FormData, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ExcelValidationResult<StockImportPriorityExcelDto>>(
      {
        method: 'POST',
        url: '/api/app/stock-import-priorities/validation',
        body: file,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
