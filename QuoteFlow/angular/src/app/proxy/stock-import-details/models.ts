import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { ExtendedAuditedEntityDto } from '../shared/models';

export interface GetStockImportDetailsInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  stockImportId?: string;
  invoiceNo?: string;
  itemModel?: string;
  materialCode?: string;
  unit?: string;
  qtyMin?: number;
  qtyMax?: number;
  priceMin?: number;
  priceMax?: number;
  amountMin?: number;
  amountMax?: number;
  gensanchiNM?: string;
  etaMin?: string;
  etaMax?: string;
  etdMin?: string;
  etdMax?: string;
  shipmentMethod?: string;
  billNo?: string;
  machineNumber?: string;
  poNo?: string;
  cdNo?: string;
  note?: string;
  deliveryTerm?: string;
  origin?: string;
  invoiceDateMin?: string;
  invoiceDateMax?: string;
}

export interface StockImportDetailCreateDto {
  stockImportId: string;
  invoiceNo: string;
  itemModel?: string;
  materialCode?: string;
  unit?: string;
  qty?: number;
  price?: number;
  amount?: number;
  gensanchiNM?: string;
  eta?: string;
  etd?: string;
  shipmentMethod?: string;
  billNo?: string;
  machineNumber?: string;
  poNo: string;
  cdNo?: string;
  note?: string;
  deliveryTerm?: string;
  origin?: string;
  stockDate?: string;
}

export interface StockImportDetailDto extends ExtendedAuditedEntityDto<string> {
  stockImportId?: string;
  invoiceNo?: string;
  itemModel?: string;
  materialCode?: string;
  unit?: string;
  qty?: number;
  price?: number;
  amount?: number;
  gensanchiNM?: string;
  eta?: string;
  etd?: string;
  shipmentMethod?: string;
  billNo?: string;
  machineNumber?: string;
  poNo?: string;
  cdNo?: string;
  note?: string;
  deliveryTerm?: string;
  origin?: string;
  invoiceDate?: string;
  concurrencyStamp?: string;
}

export interface StockImportDetailUpdateDto {
  stockImportId: string;
  invoiceNo: string;
  itemModel?: string;
  materialCode?: string;
  unit?: string;
  qty?: number;
  price?: number;
  amount?: number;
  gensanchiNM?: string;
  eta?: string;
  etd?: string;
  shipmentMethod?: string;
  billNo?: string;
  machineNumber?: string;
  poNo: string;
  cdNo?: string;
  note?: string;
  deliveryTerm?: string;
  origin?: string;
  stockDate?: string;
  concurrencyStamp?: string;
}
