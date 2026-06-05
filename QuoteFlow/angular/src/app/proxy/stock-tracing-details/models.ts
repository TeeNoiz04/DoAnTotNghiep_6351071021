import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { ReportType } from '../stock-tracings/report-type.enum';
import type { ExtendedAuditedEntityDto } from '../shared/models';

export interface GetStockTracingDetailsInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  stockTracingId?: string;
  reportType?: ReportType;
  rowNoMin?: number;
  rowNoMax?: number;
  packingListCode?: string;
  checkListCode?: string;
  dateEnteredMin?: string;
  dateEnteredMax?: string;
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
  originCode?: string;
  productionDateMin?: string;
  productionDateMax?: string;
  location?: string;
  golfaCode?: string;
  note?: string;
  materialType?: string;
  model?: string;
}

export interface StockTracingDetailCreateDto {
  stockTracingId: string;
  reportType?: ReportType;
  rowNo?: number;
  packingListCode?: string;
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
  originCode?: string;
  productionDate?: string;
  location?: string;
  golfaCode?: string;
  note?: string;
}

export interface StockTracingDetailDto extends ExtendedAuditedEntityDto<string> {
  stockTracingId?: string;
  reportType?: ReportType;
  rowNo?: number;
  packingListCode?: string;
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
  originCode?: string;
  productionDate?: string;
  location?: string;
  golfaCode?: string;
  note?: string;
  concurrencyStamp?: string;
}

export interface StockTracingDetailExcelDownloadDto {
  downloadToken?: string;
  filterText?: string;
  stockTracingId?: string;
  reportType?: ReportType;
  rowNoMin?: number;
  rowNoMax?: number;
  packingListCode?: string;
  checkListCode?: string;
  dateEnteredMin?: string;
  dateEnteredMax?: string;
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
  originCode?: string;
  productionDateMin?: string;
  productionDateMax?: string;
  location?: string;
  golfaCode?: string;
  note?: string;
  materialType?: string;
}

export interface StockTracingDetailUpdateDto {
  stockTracingId: string;
  reportType?: ReportType;
  rowNo?: number;
  packingListCode?: string;
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
  originCode?: string;
  productionDate?: string;
  location?: string;
  golfaCode?: string;
  note?: string;
  concurrencyStamp?: string;
}
