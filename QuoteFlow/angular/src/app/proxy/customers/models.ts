import type { ExtendedAuditedEntityDto } from '../shared/models';
import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface CustomerCreateDto {
  taxCode: string;
  customerName: string;
  customerShortName?: string;
  address?: string;
  website?: string;
  phone?: string;
  country?: string;
  province?: string;
  customerIndustry?: string;
  customerType?: string;
  note?: string;
}

export interface CustomerDto extends ExtendedAuditedEntityDto<string> {
  taxCode?: string;
  customerName?: string;
  customerShortName?: string;
  address?: string;
  phone?: string;
  country?: string;
  province?: string;
  website?: string;
  customerType?: string;
  customerIndustry?: string;
  note?: string;
  isDeactive: boolean;
  concurrencyStamp?: string;
}

export interface CustomerExcelDownloadDto {
  downloadToken?: string;
  taxCode?: string;
  customerName?: string;
  customerShortName?: string;
  address?: string;
  phone?: string;
  country?: string;
  province?: string;
  website?: string;
  customerType?: string;
  customerIndustry?: string;
  fromDate?: string;
  toDate?: string;
  note?: string;
  isDeactive: boolean;
}

export interface CustomerImportDto {
  taxCode?: string;
  customerName?: string;
  shortName?: string;
  address?: string;
  phone?: string;
  country?: string;
  province?: string;
  website?: string;
  customerType?: string;
  customerIndustry?: string;
  note?: string;
  isUpdate?: boolean;
  idUpdate?: string;
  concurrencyStamp?: string;
}

export interface CustomerUpdateDto {
  customerName: string;
  customerShortName?: string;
  address?: string;
  website?: string;
  phone?: string;
  country?: string;
  province?: string;
  customerIndustry?: string;
  customerType?: string;
  note?: string;
  isDeactive: boolean;
  concurrencyStamp?: string;
}

export interface GetCustomersInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  taxCode?: string;
  customerName?: string;
  customerShortName?: string;
  address?: string;
  phone?: string;
  website?: string;
  province?: string;
  customerType?: string;
  customerIndustry?: string;
  fromDate?: string;
  toDate?: string;
  isDeactive: boolean;
}
