import type {
  GetStockImportAllocationsInput,
  StockImportAllocationCreateDto,
  StockImportAllocationDto,
  StockImportAllocationUpdateDto,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class StockImportAllocationService {
  apiName = 'Default';

  create = (input: StockImportAllocationCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StockImportAllocationDto>(
      {
        method: 'POST',
        url: '/api/app/stock-import-allocations',
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/app/stock-import-allocations/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StockImportAllocationDto>(
      {
        method: 'GET',
        url: `/api/app/stock-import-allocations/${id}`,
      },
      { apiName: this.apiName, ...config },
    );

  getList = (input: GetStockImportAllocationsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<StockImportAllocationDto>>(
      {
        method: 'GET',
        url: '/api/app/stock-import-allocations',
        params: {
          filterText: input.filterText,
          stockImportId: input.stockImportId,
          stockImportDetail_Id: input.stockImportDetail_Id,
          invoiceNo: input.invoiceNo,
          poDetailId: input.poDetailId,
          poNo: input.poNo,
          dpoDetailId: input.dpoDetailId,
          dpoNo: input.dpoNo,
          materialCode: input.materialCode,
          qty_ImportMin: input.qty_ImportMin,
          qty_ImportMax: input.qty_ImportMax,
          priceMin: input.priceMin,
          priceMax: input.priceMax,
          qty_RequestedMin: input.qty_RequestedMin,
          qty_RequestedMax: input.qty_RequestedMax,
          qty_Import_ForAllocationMin: input.qty_Import_ForAllocationMin,
          qty_Import_ForAllocationMax: input.qty_Import_ForAllocationMax,
          qty_AllocationMin: input.qty_AllocationMin,
          qty_AllocationMax: input.qty_AllocationMax,
          allocation_OrderMin: input.allocation_OrderMin,
          allocation_OrderMax: input.allocation_OrderMax,
          allocationStep: input.allocationStep,
          note: input.note,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config },
    );

  update = (id: string, input: StockImportAllocationUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, StockImportAllocationDto>(
      {
        method: 'PUT',
        url: `/api/app/stock-import-allocations/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config },
    );

  constructor(private restService: RestService) {}
}
