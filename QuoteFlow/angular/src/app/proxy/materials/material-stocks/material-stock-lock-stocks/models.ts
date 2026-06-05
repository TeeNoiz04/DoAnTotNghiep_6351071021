import type { ExtendedFullAuditedEntityDto } from '../../../shared/models';
import type { StockCategoryDto } from '../../../stock-categories/models';

export interface MaterialStockLockStockDto extends ExtendedFullAuditedEntityDto<string> {
  golfaCode?: string;
  dpoDetailId?: string;
  stockCategoryId?: string;
  qty: number;
  note?: string;
  releasedLock: number;
  stockCategory: StockCategoryDto;
}
