import type { ExtendedAuditedEntityDto } from '../shared/models';
import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface DistributorTargetCreateDto {
  buyerTypeId?: string;
  buyerId: string;
  buyerCode?: string;
  buyerName?: string;
  materialType: string;
  financeYear?: number;
  firstFYTarget?: number;
  secondFYTarget?: number;
  note?: string;
}

export interface DistributorTargetDto extends ExtendedAuditedEntityDto<string> {
  buyerTypeId?: string;
  buyerId?: string;
  buyerCode?: string;
  buyerName?: string;
  materialType?: string;
  financeYear?: number;
  firstFYTarget?: number;
  secondFYTarget?: number;
  note?: string;
  concurrencyStamp?: string;
}

export interface DistributorTargetExcelDownloadDto {
  downloadToken?: string;
  filterText?: string;
  buyerTypeId?: string;
  buyerId?: string;
  buyerCode?: string;
  materialType?: string;
  financeYearMin?: number;
  financeYearMax?: number;
  firstFYTargetMin?: number;
  firstFYTargetMax?: number;
  secondFYTargetMin?: number;
  secondFYTargetMax?: number;
  note?: string;
}

export interface DistributorTargetUpdateDto {
  buyerTypeId?: string;
  buyerId: string;
  buyerCode?: string;
  buyerName?: string;
  materialType: string;
  financeYear?: number;
  firstFYTarget?: number;
  secondFYTarget?: number;
  note?: string;
  concurrencyStamp?: string;
}

export interface GetDistributorTargetsInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  buyerTypeId?: string;
  buyerId?: string;
  buyerCode?: string;
  buyerName?: string;
  materialType?: string;
  financeYearMin?: number;
  financeYearMax?: number;
  firstFYTargetMin?: number;
  firstFYTargetMax?: number;
  secondFYTargetMin?: number;
  secondFYTargetMax?: number;
  note?: string;
}
