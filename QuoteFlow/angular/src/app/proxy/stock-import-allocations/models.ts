import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { ExtendedAuditedEntityDto } from '../shared/models';

export interface GetStockImportAllocationsInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  stockImportId?: string;
  stockImportDetail_Id?: string;
  invoiceNo?: string;
  poDetailId?: string;
  poNo?: string;
  dpoDetailId?: string;
  dpoNo?: string;
  materialCode?: string;
  qty_ImportMin?: number;
  qty_ImportMax?: number;
  priceMin?: number;
  priceMax?: number;
  qty_RequestedMin?: number;
  qty_RequestedMax?: number;
  qty_Import_ForAllocationMin?: number;
  qty_Import_ForAllocationMax?: number;
  qty_AllocationMin?: number;
  qty_AllocationMax?: number;
  allocation_OrderMin?: number;
  allocation_OrderMax?: number;
  allocationStep?: string;
  note?: string;
}

export interface StockImportAllocationCreateDto {
  stockImportId?: string;
  stockImportDetail_Id: string;
  invoiceNo?: string;
  poDetailId?: string;
  poNo?: string;
  dpoDetailId?: string;
  dpoNo?: string;
  materialCode?: string;
  qty_Import?: number;
  price?: number;
  qty_Requested?: number;
  qty_Import_ForAllocation?: number;
  qty_Allocation?: number;
  allocation_Order?: number;
  allocationStep?: string;
  note?: string;
}

export interface StockImportAllocationDto extends ExtendedAuditedEntityDto<string> {
  stockImportId?: string;
  stockImportDetail_Id?: string;
  invoiceNo?: string;
  poDetailId?: string;
  poNo?: string;
  dpoDetailId?: string;
  dpoNo?: string;
  materialCode?: string;
  qty_Import?: number;
  price?: number;
  qty_Requested?: number;
  qty_Import_ForAllocation?: number;
  qty_Allocation?: number;
  allocation_Order?: number;
  allocationStep?: string;
  note?: string;
  concurrencyStamp?: string;
}

export interface StockImportAllocationUpdateDto {
  stockImportId?: string;
  stockImportDetail_Id: string;
  invoiceNo?: string;
  poDetailId?: string;
  poNo?: string;
  dpoDetailId?: string;
  dpoNo?: string;
  materialCode?: string;
  qty_Import?: number;
  price?: number;
  qty_Requested?: number;
  qty_Import_ForAllocation?: number;
  qty_Allocation?: number;
  allocation_Order?: number;
  allocationStep?: string;
  note?: string;
  concurrencyStamp?: string;
}

export interface ExportStockImportAllocationInput {
  invoiceNo?: string;
  buyer?: string;
}

export interface StockImportAllocationExportDto {
  invoiceNo?: string;
  etd?: string;
  eta?: string;
  shipmentMethod?: string;
  buyer?: string;
  dpoNo?: string;
  materialCode?: string;
  model?: string;
  spec1?: string;
  qty_Allocation?: string;
  dpoPrice?: number;
  materialType?: string;
  material_Group?: string;
  dpo_Amount?: number;
  note?: string;
  material_Size?: string;
  qty?: number;
  sap_Code?: string;
  allocationStep?: string;
}
