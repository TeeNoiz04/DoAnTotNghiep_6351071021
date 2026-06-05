import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { ExtendedAuditedEntityDto } from '../shared/models';

export interface GetStockImportPrioritiesInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  dpoNo?: string;
  poNo?: string;
  materialCode?: string;
  model?: string;
  statusCode?: string;
  qtyMin?: number;
  qtyMax?: number;
  priorityMin?: number;
  priorityMax?: number;
  qtyUsedMin?: number;
  qtyUsedMax?: number;
  qtyAvailableMin?: number;
  qtyAvailableMax?: number;
  note?: string;
  importGuid?: string;
}

export interface StockImportPriorityCreateDto {
  dpoNo: string;
  poNo: string;
  materialCode: string;
  model?: string;
  statusCode?: string;
  qty: number;
  priority?: number;
  qtyUsed?: number;
  qtyAvailable?: number;
  note?: string;
  importGuid?: string;
}

export interface StockImportPriorityDto extends ExtendedAuditedEntityDto<string> {
  dpoNo?: string;
  poNo?: string;
  materialCode?: string;
  model?: string;
  statusCode?: string;
  qty: number;
  priority?: number;
  qtyUsed?: number;
  qtyAvailable?: number;
  note?: string;
  importGuid?: string;
  isExist: boolean;
  concurrencyStamp?: string;
}

export interface StockImportPriorityUpdateDto {
  dpoNo: string;
  poNo: string;
  materialCode: string;
  model?: string;
  statusCode?: string;
  qty: number;
  priority?: number;
  qtyUsed?: number;
  qtyAvailable?: number;
  note?: string;
  importGuid?: string;
  concurrencyStamp?: string;
}
