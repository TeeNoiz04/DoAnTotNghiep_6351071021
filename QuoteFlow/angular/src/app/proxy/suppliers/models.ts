import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { ExtendedAuditedEntityDto } from '../shared/models';

export interface GetSuppliersInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  supplierCode?: string;
  shortName?: string;
  fullName?: string;
  taxCode?: string;
  address?: string;
  isDeactive?: boolean;
}

export interface SupplierCreateDto {
  supplierCode: string;
  sapCode?: string;
  shortName: string;
  fullName: string;
  taxCode?: string;
  address?: string;
  isDeactive: boolean;
}

export interface SupplierDto extends ExtendedAuditedEntityDto<string> {
  supplierCode?: string;
  sapCode?: string;
  shortName?: string;
  fullName?: string;
  taxCode?: string;
  address?: string;
  isDeactive?: boolean;
  concurrencyStamp?: string;
}

export interface SupplierUpdateDto {
  supplierCode: string;
  sapCode?: string;
  shortName: string;
  fullName: string;
  taxCode?: string;
  address?: string;
  isDeactive?: boolean;
  concurrencyStamp?: string;
}
