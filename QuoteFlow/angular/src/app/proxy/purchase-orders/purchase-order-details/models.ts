import type { ExtendedAuditedEntityDto } from '../../shared/models';

export interface PurchaseOrderDetailDto extends ExtendedAuditedEntityDto<string> {
  purchaseOrderId?: string;
  golfaCode?: string;
  model?: string;
  statusCode?: string;
  qty?: number;
  price?: number;
  amount?: number;
  amountVND?: number;
  note?: string;
  projectCode?: string;
  accountNo?: string;
  qtyImported?: number;
  qtyLocked?: number;
  qtyAvailable?: number;
  qtyNeedImport?: number;
  leadTime?: number;
  maxlot?: number;
  poDetailCode?: string;
  urgent?: boolean;
  requestETA?: string;
  customer?: string;
  concurrencyStamp?: string;
}

export interface PurchaseOrderDetailUpdateDto {
  purchaseOrderId?: string;
  golfaCode: string;
  model?: string;
  statusCode?: string;
  qty?: number;
  price?: number;
  amount?: number;
  amountVND?: number;
  note?: string;
  projectCode?: string;
  poDetailCode?: string;
  accountNo?: string;
  qtyImported?: number;
  qtyLocked?: number;
  qtyAvailable?: number;
  qtyNeedImport?: number;
  leadTime?: number;
  maxlot?: number;
  urgent?: boolean;
  requestETA?: string;
  customer?: string;
  concurrencyStamp?: string;
}

export interface PurchaseOrderDetailUpdateRequestInfoDto {
  urgent?: boolean;
  requestETA?: string;
  customer?: string;
  concurrencyStamp?: string;
}
