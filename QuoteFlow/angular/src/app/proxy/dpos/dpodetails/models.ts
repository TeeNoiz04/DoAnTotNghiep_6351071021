import type { ExtendedFullAuditedEntityDto } from '../../shared/models';
import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface DPODetailDto extends ExtendedFullAuditedEntityDto<string> {
  dpoId?: string;
  dpoNo?: string;
  dpoRemark?: string;
  dpoOrderDate?: string;
  status?: string;
  materialStatus?: string;
  rowNo?: number;
  golfaCode?: string;
  model?: string;
  spec1?: string;
  spec2?: string;
  qty?: number;
  unitPrice?: number;
  amount?: number;
  amountIncludeExtraFee?: number;
  requestedETA?: string;
  spoId?: string;
  spoCode?: string;
  customerTaxCode?: string;
  customerName?: string;
  lockStock?: number;
  lockStockSO?: number;
  lockShipment?: number;
  delivered?: number;
  needDelivery?: number;
  note?: string;
  confirmNoted?: string;
  orderReason?: string;
  accountNo?: string;
  extrafee?: number;
  extrafeeUsedInSO?: number;
  extrafeeAvailable?: number;
  extrafeeNote?: string;
  availableStockQty: number;
  onOrderStockAvailable: number;
  concurrencyStamp?: string;
}

export interface ImportDPODetailDto {
  rowNo?: number;
  golfaCode?: string;
  model?: string;
  spec1?: string;
  spec2?: string;
  spoId?: string;
  spoCode?: string;
  customerId?: string;
  qty?: number;
  unitPrice?: number;
  amount?: number;
  requestedETA?: string;
  customerName?: string;
  customerTaxCode?: string;
  customerType?: string;
  customerIndustry?: string;
  note?: string;
}

export interface DPODetailCreateDto {
  dpoId: string;
  status?: string;
  golfaCode: string;
  model?: string;
  spec1?: string;
  spec2?: string;
  qty?: number;
  unitPrice?: number;
  amount?: number;
  requestedETA?: string;
  spoId?: string;
  spoCode?: string;
  customerTaxCode?: string;
  customerName?: string;
  lockStock?: number;
  lockStockSO?: number;
  lockShipment?: number;
  delivered?: number;
  needDelivery?: number;
  note?: string;
  confirmNoted?: string;
  orderReason?: string;
}

export interface DPODetailLockStockDto {
  golfaCode: string;
  stockCategoryId: string;
  lockQty: number;
  note?: string;
}

export interface DPODetailUpdateLockStockDto {
  stockCategoryId: string;
  oldQty: number;
  newQty: number;
  note: string;
}

export interface GetDPODetailsInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  dpoId?: string;
  status?: string;
  golfaCode?: string;
  model?: string;
  spec1?: string;
  spec2?: string;
  qtyMin?: number;
  qtyMax?: number;
  unitPriceMin?: number;
  unitPriceMax?: number;
  amountMin?: number;
  amountMax?: number;
  requestedETAMin?: string;
  requestedETAMax?: string;
  spoId?: string;
  spoCode?: string;
  customerTaxCode?: string;
  customerName?: string;
  lockStockMin?: number;
  lockStockMax?: number;
  lockStockSOMin?: number;
  lockStockSOMax?: number;
  lockShipmentMin?: number;
  lockShipmentMax?: number;
  deliveredMin?: number;
  deliveredMax?: number;
  needDeliveryMin?: number;
  needDeliveryMax?: number;
  note?: string;
  orderReason?: string;
}
