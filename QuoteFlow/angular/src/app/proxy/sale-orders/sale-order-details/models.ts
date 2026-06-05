import type { ExtendedAuditedEntityDto } from '../../shared/models';
import type { DPODetailDto } from '../../dpos/dpodetails/models';
import type { StockCategoryDto } from '../../stock-categories/models';

export interface SaleOrderDetailDto extends ExtendedAuditedEntityDto<string> {
  no?: number;
  saleOrderId?: string;
  dpoDetailId?: string;
  statusCode?: string;
  golfaCode?: string;
  qty?: number;
  price?: number;
  amount?: number;
  vat?: number;
  stockCategoryId?: string;
  note?: string;
  extrafee_Note?: string;
  lockStockId?: string;
  extrafee?: number;
  amountIncludeExtrafee?: number;
  sapLandingCost?: number;
  sapAmountLandingCost?: number;
  gicPorNo?: string;
  gicPrNo?: string;
  gicSalePIC?: string;
  gicLocation?: string;
  gicReservationNo?: string;
  gicGivNo?: string;
  gicGivDate?: string;
  changeNote?: string;
  disposed?: string;
  dpoDetail: DPODetailDto;
  stockCategory: StockCategoryDto;
  concurrencyStamp?: string;
}

export interface SaleOrderDetailUpdateDto {
  saleOrderId: string;
  dpoDetailId?: string;
  statusCode?: string;
  golfaCode?: string;
  qty?: number;
  price?: number;
  amount?: number;
  vat?: number;
  stockCategoryId?: string;
  note?: string;
  extrafee_Note?: string;
  lockStockId?: string;
  extrafee?: number;
  amountIncludeExtrafee?: number;
  sapLandingCost?: number;
  sapAmountLandingCost?: number;
  gicPorNo?: string;
  gicPrNo?: string;
  gicSalePIC?: string;
  gicLocation?: string;
  gicReservationNo?: string;
  gicGivNo?: string;
  gicGivDate?: string;
  changeNote?: string;
  disposed?: string;
  concurrencyStamp?: string;
}
