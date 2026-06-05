import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { ExtendedAuditedEntityDto } from '../shared/models';
import type { MaterialStockUploadDetailDto } from '../material-stock-upload-details/models';

export interface DataMaterialOverallStockReportDto {
  materialType?: string;
  material_Group?: string;
  available_Stock?: number;
  keeping_Stock?: number;
  on_Order_Stock?: number;
  stockWarning?: number;
}

export interface GetInventoryReportsInput extends PagedAndSortedResultRequestDto {
  materialCode?: string;
  inventoryCategory?: string;
  materialGroup?: string;
}

export interface GetStockHistoriesInput {
  stockCode: string;
  golfaCode: string;
  actionFrom: string;
  actionTo?: string;
  note?: string;
}

export interface GetStockManagementApprovalsInput extends PagedAndSortedResultRequestDto {
  golfaCode?: string;
  model?: string;
  importType?: string;
  approvalStatus?: string;
}

export interface GetStockManagementsListInput extends PagedAndSortedResultRequestDto {
  supplierCode?: string;
  supplierBUCode?: string;
  materialType?: string;
  golfaCode?: string;
  model?: string;
  materialGroup?: string;
  stockCategoryId?: string;
  greaterStockQty?: number;
  greaterOnOrderStockQty?: number;
  status?: string;
}

export interface InventoryReportDto {
  golfaCode?: string;
  model?: string;
  spec1?: string;
  sap_Code?: string;
  inventoryCategory?: string;
  material_Group?: string;
  standard_Price?: number;
  landedCost?: number;
  availableStock_Qty: number;
  availableStock_Amount: number;
  gkr_Qty: number;
  gkr_Amount: number;
  lockedStock_Qty: number;
  lockedStock_Amount: number;
  inventory_Qty: number;
  inventory_Amount: number;
  mevnBackOrder_OnOrder_Qty: number;
  mevnBackOrder_OnOrder_Amount: number;
  mevnBackOrder_Locked_Qty: number;
  mevnBackOrder_Locked_Amount: number;
  stockWarning_Qty: number;
  stockWarning_Amount: number;
  mevnBackOrder_Qty: number;
  mevnBackOrder_Amount: number;
}

export interface LockShipmentDto {
  golfaCode?: string;
  qty?: number;
  qtyNeed?: number;
  qtyDisposed?: number;
  poNo?: string;
  dpoNo?: string;
  createdBy?: string;
  created?: string;
  modifiedBy?: string;
  modified?: string;
  machineNo?: string;
  supplierReply?: string;
  mevnAddRequest?: string;
  mevnRequest?: string;
}

export interface LockedDto {
  dpoNo?: string;
  buyerShortName?: string;
  stockCategoryId?: string;
  stockName?: string;
  lockedQty?: number;
  createdBy?: string;
  created?: string;
  modifiedBy?: string;
  modified?: string;
}

export interface OnOrderStockDto {
  poNo?: string;
  materialCode?: string;
  qtyAvailable?: number;
  poDate?: string;
  machineNo?: string;
  supplierReply?: string;
  mevnAddRequest?: string;
  mevnRequest?: string;
}

export interface StockManagementListDto {
  golfaCode?: string;
  model?: string;
  spec1?: string;
  materialStatus?: string;
  standard_Price: number;
  stock_Qty: number;
  locked_Qty: number;
  lockStockSO_Qty: number;
  available_Qty: number;
  lockshipment_Qty: number;
  onOderStock: number;
  stockCategoryId?: string;
  sap_Code?: string;
  material_Group?: string;
  referenceLeadTime?: string;
}

export interface StockManagementUploadDto extends ExtendedAuditedEntityDto<string> {
  requestNo?: string;
  importType?: string;
  fileName?: string;
  note?: string;
  status?: string;
  materialStockUploadDetails: MaterialStockUploadDetailDto[];
  concurrencyStamp?: string;
}

export interface StockOfSODto {
  soNo?: string;
  stockName?: string;
  doNo?: string;
  buyer?: string;
  qty?: number;
  createdBy?: string;
  created?: string;
  modifiedBy?: string;
  modified?: string;
}

export interface StockQtyDto {
  stockCode?: string;
  stockName?: string;
  golfaCode?: string;
  qty?: number;
  availableStock?: number;
  createdBy?: string;
  created?: string;
  modifiedBy?: string;
  modified?: string;
}
