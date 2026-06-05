import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { ReportType } from './report-type.enum';
import type { ExtendedAuditedEntityDto } from '../shared/models';
import type { StockTracingDetailDto } from '../stock-tracing-details/models';

export interface GetStockTracingsInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  fileName?: string;
  reportType?: ReportType;
  fromDateMin?: string;
  fromDateMax?: string;
  toDateMin?: string;
  toDateMax?: string;
  note?: string;
}

export interface StockTracingCreateDto {
  fileName?: string;
  reportType?: ReportType;
  fromDate?: string;
  toDate?: string;
  note?: string;
}

export interface StockTracingDeliveryImportDto {
  rowNo?: number;
  checkListCode?: string;
  dateEntered?: string;
  stock?: string;
  bu?: string;
  customer?: string;
  category?: string;
  giv?: string;
  invoice?: string;
  skuCode?: string;
  skuName?: string;
  quality?: string;
  warranty?: string;
  unit?: string;
  series?: string;
  golfaCode?: string;
  originCode?: string;
  productionDate?: string;
  location?: string;
  note?: string;
}

export interface StockTracingDto extends ExtendedAuditedEntityDto<string> {
  fileName?: string;
  reportType?: ReportType;
  fromDate?: string;
  toDate?: string;
  note?: string;
  concurrencyStamp?: string;
  details: StockTracingDetailDto[];
}

export interface StockTracingExcelDownloadDto {
  downloadToken?: string;
  filterText?: string;
  fileName?: string;
  reportType?: ReportType;
  fromDateMin?: string;
  fromDateMax?: string;
  toDateMin?: string;
  toDateMax?: string;
  note?: string;
}

export interface StockTracingInventoryImportDto {
  rowNo?: number;
  wareHouse?: string;
  bUs?: string;
  customer?: string;
  category?: string;
  skuCode?: string;
  skuName?: string;
  quality?: string;
  warranty?: string;
  unit?: string;
  series?: string;
  orginCode?: string;
  productionDate?: string;
  golfaCode?: string;
  note?: string;
}

export interface StockTracingReceiptImportDto {
  rowNo?: number;
  packingListCode?: string;
  dateEntered?: string;
  stock?: string;
  bUs?: string;
  customer?: string;
  category?: string;
  skuCode?: string;
  skuName?: string;
  quaLity?: string;
  warranty?: string;
  unit?: string;
  series?: string;
  orginCode?: string;
  productionDate?: string;
  location?: string;
  golfaCode?: string;
  note?: string;
}

export interface StockTracingUpdateDto {
  fileName?: string;
  reportType?: ReportType;
  fromDate?: string;
  toDate?: string;
  note?: string;
  concurrencyStamp?: string;
}
