import type { ExtendedAuditedEntityDto } from '../../shared/models';
import type { StockCategoryDto } from '../../stock-categories/models';
import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface MaterialStockDto extends ExtendedAuditedEntityDto<string> {
  materialId?: string;
  stockCategoryId?: string;
  golfaCode?: string;
  model?: string;
  qty?: number;
  locked?: number;
  lockStockKeeping?: number;
  lockStockSO?: number;
  available_Qty?: number;
  note?: string;
  stockCategory: StockCategoryDto;
  concurrencyStamp?: string;
}

export interface GetMaterialStocksInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  materialId?: string;
  stockCategoryId?: string;
  golfaCodes: string[];
  models: string[];
  qtyMin?: number;
  qtyMax?: number;
  lockedMin?: number;
  lockedMax?: number;
  lockStockKeepingMin?: number;
  lockStockKeepingMax?: number;
  lockStockSOMin?: number;
  lockStockSOMax?: number;
  available_QtyMin?: number;
  available_QtyMax?: number;
  note?: string;
  materialType?: string;
}
