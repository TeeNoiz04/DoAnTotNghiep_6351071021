import type { ExtendedFullAuditedEntityDto } from '../../shared/models';

export interface GICDetailDto extends ExtendedFullAuditedEntityDto<string> {
  dpoId?: string;
  status?: string;
  rowNo?: number;
  golfaCode?: string;
  model?: string;
  spec1?: string;
  spec2?: string;
  qty?: number;
  unitPrice?: number;
  landedCost?: number;
  amount?: number;
  requestedETA?: string;
  spoId?: string;
  spoCode?: string;
  customerId?: string;
  customerTaxCode?: string;
  customerName?: string;
  customerType?: string;
  lockStock?: number;
  lockStockSO?: number;
  lockShipment?: number;
  delivered?: number;
  needDelivery?: number;
  note?: string;
  orderReason?: string;
  accountNo?: string;
  extrafee?: number;
  extrafeeUsedInSO?: number;
  extrafeeAvailable?: number;
  extrafeeNote?: string;
  confirmNoted?: string;
  amountIncludeExtraFee?: number;
  availableStockQty: number;
  onOrderStockAvailable: number;
  damagedProduct?: string;
  productSerialNo?: string;
  mevnSellingInvoiceNo?: string;
  concurrencyStamp?: string;
}

export interface GICDetailImportDto {
  orderReason?: string;
  golfaCode?: string;
  model?: string;
  spec1?: string;
  spec2?: string;
  qty: number;
  unitPrice: number;
  note?: string;
  rowNo?: number;
  amount: number;
  customerName?: string;
  damagedProduct?: string;
  productSerialNo?: string;
  mevnSellingInvoiceNo?: string;
}

export interface GICDetailLockStockDto {
  golfaCode: string;
  stockCategoryId: string;
  lockQty: number;
  note?: string;
}

export interface GICDetailUpdateLockStockDto {
  stockCategoryId: string;
  oldQty: number;
  newQty: number;
  note: string;
}
