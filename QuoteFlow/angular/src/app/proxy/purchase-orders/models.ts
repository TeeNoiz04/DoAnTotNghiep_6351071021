import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { ExtendedAuditedEntityDto } from '../shared/models';
import type { SupplierDto } from '../suppliers/models';
import type { SupplierBUDto } from '../supplier-bus/models';
import type { PurchaseOrderDetailDto } from './purchase-order-details/models';

export interface GetListDetailDPOsInput {
  filterText?: string;
  poId?: string;
  golfaCodes?: string;
  supplierBUId?: string;
  materialType?: string;
  epa: boolean;
}

export interface GetPurchaseOrdersInput extends PagedAndSortedResultRequestDto {
  poNo?: string;
  poDateMin?: string;
  poDateMax?: string;
  posapNo?: string;
  dpoNo?: string;
  statusCode?: string;
  materialType?: string;
  supplierBUId?: string;
  supplierBUCode?: string;
  supplierId?: string;
  supplierCode?: string;
  materialCode?: string;
  model?: string;
  overDueDate?: boolean;
  haveSAPPO?: boolean;
  createSource?: string;
  movingOrderNo?: string;
  machineNumber?: string;
  customer?: string;
  listPO?: string;
}

export interface PurchaseOrderAddedDetailDPODto {
  poId?: string;
  dpoDetailId?: string;
  golfaCode?: string;
  model?: string;
  qty: number;
  accountNo?: string;
  price: number;
  currency?: string;
  maxlot: number;
  customerName?: string;
  requestETA?: string;
  urgent?: boolean;
  userName?: string;
}

export interface PurchaseOrderCreateDto {
  poDate?: string;
  posapNo?: string;
  posapDate?: string;
  createSource?: string;
  materialType?: string;
  supplierId?: string;
  supplierCode?: string;
  supplierBUId?: string;
  supplierBUCode?: string;
  currency: string;
  epa: boolean;
  ourRef?: string;
}

export interface PurchaseOrderDpoNoteDto {
  dpoNo?: string;
  remark?: string;
}

export interface PurchaseOrderDto extends ExtendedAuditedEntityDto<string> {
  poNo?: string;
  poDate?: string;
  posapNo?: string;
  posapDate?: string;
  statusCode?: string;
  createSource?: string;
  materialType?: string;
  supplierBUId?: string;
  supplierBUCode?: string;
  supplierId?: string;
  supplierCode?: string;
  currency?: string;
  epa: boolean;
  ourRef?: string;
  totalAmount?: number;
  sendToSupplier: boolean;
  supplier: SupplierDto;
  supplierBU: SupplierBUDto;
  purchaseOrderDetails: PurchaseOrderDetailDto[];
  concurrencyStamp?: string;
}

export interface PurchaseOrderLinkedDPODto {
  materialCode?: string;
  qty: number;
  qtyDisposed: number;
  qtyNeed: number;
  note?: string;
  modified?: string;
  modifiedBy?: string;
  dpoNo?: string;
  orderDate?: string;
  customerName?: string;
  customerTaxCode?: string;
}

export interface PurchaseOrderListDetailDPODto {
  rowNum?: number;
  dpoNo?: string;
  dpoDetailId?: string;
  golfaCode?: string;
  model?: string;
  spoCode?: string;
  accountNo?: string;
  note?: string;
  qty?: number;
  needOrder?: number;
  inputPrice?: number;
  materialInputPrice?: number;
  price?: number;
  maxlot?: number;
  customerName?: string;
  requestETA?: string;
  urgent?: boolean;
  hasAccountToSelect?: boolean;
}

export interface PurchaseOrderListDetailDto {
  id?: string;
  statusCode?: string;
  poDetailCode?: string;
  golfaCode?: string;
  model?: string;
  qty?: number;
  price?: number;
  amount?: number;
  amountVND?: number;
  qtyLocked?: number;
  freeQty?: number;
  invoiceQty?: number;
  importedQty?: number;
  remaining?: number;
  accountNo?: string;
  customer?: string;
  requestETA?: string;
  urgent?: boolean;
  machineNumber?: string;
  supplierReply?: string;
  mevn_Add_Request?: string;
  note?: string;
  replyFromVendor?: string;
  materialInputPrice?: number;
  deliveryDueDate?: string;
  lastModificationTime?: string;
  lastModifierName?: string;
  concurrencyStamp?: string;
  dpoNote?: string;
}

export interface PurchaseOrderListDetailFOCDto {
  rowNum?: number;
  golfaCode?: string;
  model?: string;
  stockWarning: number;
  availableStock: number;
  stockOnShipment: number;
  needOrder: number;
  input_Price: number;
  maxlot: number;
}

export interface PurchaseOrderListDetailWarningStockDto {
  rowNum?: number;
  golfaCode?: string;
  model?: string;
  inputPrice?: number;
  materialInputPrice?: number;
  maxlot?: number;
  stockWarning?: number;
  stockQty?: number;
  onOrderStock?: number;
  needOrder?: number;
  materialGroup?: string;
  inventoryCategory?: string;
  hasAccountToSelect?: boolean;
  accountNo?: string;
}

export interface PurchaseOrderListQtyImportedDto {
  invoiceNo?: string;
  materialCode?: string;
  dpoNo?: string;
  qty_Allocation?: number;
  price?: number;
  creationTime?: string;
}

export interface PurchaseOrderUpdateDto {
  poDate?: string;
  posapNo?: string;
  posapDate?: string;
  createSource?: string;
  materialType?: string;
  supplierId?: string;
  supplierCode?: string;
  supplierBUId?: string;
  supplierBUCode?: string;
  currency: string;
  epa: boolean;
  ourRef?: string;
  concurrencyStamp?: string;
}
