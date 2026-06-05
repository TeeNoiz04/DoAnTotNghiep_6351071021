import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { ExcelValidationResult } from '../shared/excels/models';
import type { StockImportExcelDto } from './excel/models';
import type { ExtendedAuditedEntityDto } from '../shared/models';
import type { StockImportDetailDto } from '../stock-import-details/models';

export interface GetAllocationInvoicesInput {
  invoiceNo?: string;
  fileName?: string;
  status?: string;
  poNo?: string;
  golfaCode?: string;
  stockDateMin?: string;
  stockDateMax?: string;
  note?: string;
  materialType?: string;
  supplierCode?: string;
  buyerId?: string;
  buyerCode?: string;
  buyerType?: string;
  listInvoice?: string;
}

export interface GetPurchaseInvoicesInput {
  invoiceNo?: string;
  fileName?: string;
  status?: string;
  poNo?: string;
  golfaCode?: string;
  stockDateMin?: string;
  stockDateMax?: string;
  note?: string;
  materialType?: string;
  supplierCode?: string;
  buyerId?: string;
  buyerCode?: string;
  buyerType?: string;
  listInvoice?: string;
}

export interface GetStockImportConfirmsInput {
  invoiceNo?: string;
  stockCategoryId?: string;
  supplierId?: string;
  supplierCode?: string;
  invoiceType?: string;
  invoiceDate?: string;
  stockDate?: string;
  shipmentMethod?: string;
  etd?: string;
  eta?: string;
  cdNo?: string;
  atd?: string;
  ata?: string;
  receivingReportDate?: string;
  whArrivalDate?: string;
  billNo?: string;
  deliveryTerm?: string;
  cdDate?: string;
  cdNote?: string;
  note?: string;
  stockNameConfirmed?: string;
  stockCodeConfirmed?: string;
}

export interface GetStockImportListsInput extends PagedAndSortedResultRequestDto {
  invoiceNo?: string;
  fromDate?: string;
  toDate?: string;
  status?: string;
  materialType?: string;
  prPONo?: string;
  golfaCode?: string;
}

export interface GetStockImportsInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  invoiceNo?: string;
  fileName?: string;
  status?: string;
  poNo?: string;
  golfaCode?: string;
  stockDateMin?: string;
  stockDateMax?: string;
  note?: string;
  materialType?: string;
  supplierCode?: string;
  buyerId?: string;
  buyerType?: string;
}

export interface InvoiceImportInput {
  supplierId?: string;
  supplierCode?: string;
  invoiceNo?: string;
  invoiceType?: string;
  invoiceDate?: string;
  shipmentMethod?: string;
  etd?: string;
  eta?: string;
  note?: string;
  stockDate?: string;
  deliveryTerm?: string;
}

export interface ResultValidateInvoiceImport {
  input: InvoiceImportInput;
  result: ExcelValidationResult<StockImportExcelDto>;
}

export interface StockImportCreateDto {
  invoiceNo: string;
  invoiceType?: string;
  deliveryTerm?: string;
  supplierId?: string;
  supplierCode?: string;
  fileName: string;
  status?: string;
  invoiceDate?: string;
  stockDate?: string;
  shipmentMethod?: string;
  etd?: string;
  eta?: string;
  billNo?: string;
  cdNo?: string;
  cdDate?: string;
  atd?: string;
  ata?: string;
  receivingReportDate?: string;
  whArrivalDate?: string;
  stockNameConfirmed?: string;
  stockCodeConfirmed?: string;
  note?: string;
}

export interface StockImportDto extends ExtendedAuditedEntityDto<string> {
  invoiceNo?: string;
  invoiceType?: string;
  deliveryTerm?: string;
  supplierId?: string;
  supplierCode?: string;
  fileName?: string;
  status?: string;
  invoiceDate?: string;
  stockDate?: string;
  shipmentMethod?: string;
  etd?: string;
  eta?: string;
  billNo?: string;
  cdNo?: string;
  atd?: string;
  ata?: string;
  cdDate?: string;
  receivingReportDate?: string;
  whArrivalDate?: string;
  stockNameConfirmed?: string;
  stockCodeConfirmed?: string;
  note?: string;
  confirmNote?: string;
  totalQty?: number;
  totalAmount?: number;
  details: StockImportDetailDto[];
  concurrencyStamp?: string;
}

export interface StockImportListDto {
  invoiceNo?: string;
  materialCode?: string;
  model?: string;
  spec1?: string;
  sap_Code?: string;
  poNo?: string;
  posapNo?: string;
  posapDate?: string;
  qty_Import: number;
  qty_Allocation: number;
  price: number;
  amount: number;
  etd?: string;
  eta?: string;
  shipmentMethod?: string;
  note?: string;
  dpoNo?: string;
  buyer?: string;
  dpoPrice: number;
  qty: number;
  dpo_Amount: number;
  allocationStep?: string;
  allocation_Order?: number;
  machineNumber?: string;
}

export interface StockImportUpdateDto {
  invoiceNo: string;
  invoiceType?: string;
  deliveryTerm?: string;
  supplierId?: string;
  supplierCode?: string;
  fileName: string;
  status?: string;
  invoiceDate?: string;
  stockDate?: string;
  shipmentMethod?: string;
  etd?: string;
  eta?: string;
  billNo?: string;
  cdNo?: string;
  cdDate?: string;
  atd?: string;
  ata?: string;
  receivingReportDate?: string;
  whArrivalDate?: string;
  note?: string;
  stockNameConfirmed?: string;
  stockCodeConfirmed?: string;
  concurrencyStamp?: string;
}
