import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { ExtendedAuditedEntityDto } from '../shared/models';

export interface GetStockCategoriesInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  stockCode?: string;
  stockName?: string;
  foc?: boolean;
  mainStock?: boolean;
  damagedStock?: boolean;
  sortOrderMin?: number;
  sortOrderMax?: number;
  isDeactive?: boolean;
  note?: string;
}

export interface StockCategoryCreateDto {
  stockCode: string;
  stockName: string;
  sapCode?: string;
  mainStock?: boolean;
  foc?: boolean;
  damagedStock?: boolean;
  sortOrder?: number;
  isDeactive?: boolean;
  note?: string;
}

export interface StockCategoryDto extends ExtendedAuditedEntityDto<string> {
  stockCode?: string;
  stockName?: string;
  sapCode?: string;
  mainStock?: boolean;
  foc?: boolean;
  damagedStock?: boolean;
  sortOrder?: number;
  isDeactive?: boolean;
  note?: string;
  concurrencyStamp?: string;
}

export interface StockCategoryUpdateDto {
  stockCode: string;
  stockName: string;
  sapCode?: string;
  mainStock?: boolean;
  foc?: boolean;
  damagedStock?: boolean;
  sortOrder?: number;
  isDeactive?: boolean;
  note?: string;
  concurrencyStamp?: string;
}
