import type { ExtendedAuditedEntityDto, ExtendedFullAuditedEntityDto } from '../../shared/models';

export interface GKRDetailDto extends ExtendedFullAuditedEntityDto<string> {
  dpoId?: string;
  rowNo?: number;
  status?: string;
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
  lockShipment?: number;
  needDelivery?: number;
  note?: string;
  orderReason?: string;
  accountNo?: string;
  confirmNoted?: string;
  dpoUsed?: number;
  availableStockQty: number;
  onOrderStockAvailable: number;
  materialStatus?: string;
  concurrencyStamp?: string;
}

export interface GKRDetailImportDto {
  rowNo?: number;
  golfaCode?: string;
  model?: string;
  spec1?: string;
  spec2?: string;
  spoId?: string;
  spoCode?: string;
  qty: number;
  unitPrice: number;
  amount: number;
  requestedETA?: string;
  customerName?: string;
  customerTaxCode?: string;
  customerType?: string;
  customerIndustry?: string;
  note?: string;
}

export interface GKRDetailLockOnOrderStockDto {
  items: GKRLockShipmentItemDto[];
}

export interface GKRDetailLockShipmentDto extends ExtendedAuditedEntityDto<string> {
  poDetailId?: string;
  poNo?: string;
  poQty: number;
  poDate?: string;
  machineNumber?: string;
  stcReply?: string;
  status?: string;
  note?: string;
  qtyLocked?: number;
  qtyImported?: number;
  qtyNeedImport?: number;
}

export interface GKRDetailLockShipmentEtaEtdDto {
  invoiceNo?: string;
  poNo?: string;
  materialCode?: string;
  qty_Allocation: number;
  shipmentMethod?: string;
  eta?: string;
  etd?: string;
  status?: string;
}

export interface GKRDetailLockShipmentItemUpdateDto {
  golfaCode: string;
  qty: number;
  note?: string;
}

export interface GKRDetailLockStockDto {
  golfaCode: string;
  stockCategoryId: string;
  lockQty: number;
  note?: string;
}

export interface GKRLockShipmentItemDto {
  poDetailId: string;
  golfaCode: string;
  qty: number;
  note?: string;
}
