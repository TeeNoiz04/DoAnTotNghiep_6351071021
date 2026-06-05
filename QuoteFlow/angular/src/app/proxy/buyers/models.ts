import type { ExtendedAuditedEntityDto } from '../shared/models';
import type { SystemCategoryListDto } from '../system-categories/models';
import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface BuyerCreateDto {
  buyerTypeId: string;
  buyerTypeCode: string;
  buyerCode: string;
  shortName?: string;
  fullName?: string;
  taxCode?: string;
  address?: string;
  contactPerson?: string;
  contactEmail?: string;
  contactPhoneNumber?: string;
  paymentTermCode?: string;
  paymentTermDescription?: string;
  creditLimit?: number;
  creditExposure?: number;
  appliedPrice?: number;
  deactive?: boolean;
  note?: string;
}

export interface BuyerDto extends ExtendedAuditedEntityDto<string> {
  buyerTypeId?: string;
  buyerTypeCode?: string;
  buyerCode?: string;
  shortName?: string;
  fullName?: string;
  taxCode?: string;
  address?: string;
  contactPerson?: string;
  contactEmail?: string;
  contactPhoneNumber?: string;
  paymentTermCode?: string;
  paymentTermDescription?: string;
  creditLimit?: number;
  creditExposure?: number;
  availableCredit?: number;
  appliedPrice?: number;
  deactive?: boolean;
  note?: string;
  concurrencyStamp?: string;
  buyerType: SystemCategoryListDto;
}

export interface BuyerListDto extends ExtendedAuditedEntityDto<string> {
  buyerTypeId?: string;
  buyerTypeCode?: string;
  discriptionCategory?: string;
  buyerCode?: string;
  shortName?: string;
  fullName?: string;
  taxCode?: string;
  address?: string;
  contactPerson?: string;
  contactEmail?: string;
  contactPhoneNumber?: string;
  paymentTermCode?: string;
  paymentTermDescription?: string;
  creditLimit?: number;
  creditExposure?: number;
  availableCredit?: number;
  appliedPrice?: number;
  deactive?: boolean;
  note?: string;
}

export interface BuyerUpdateDto {
  buyerTypeId: string;
  buyerTypeCode: string;
  buyerCode: string;
  shortName?: string;
  fullName?: string;
  taxCode?: string;
  address?: string;
  contactPerson?: string;
  contactEmail?: string;
  contactPhoneNumber?: string;
  paymentTermCode?: string;
  paymentTermDescription?: string;
  creditLimit?: number;
  creditExposure?: number;
  appliedPrice?: number;
  deactive?: boolean;
  note?: string;
  concurrencyStamp: string;
}

export interface GetBuyersInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  buyerTypeId?: string;
  buyerCode?: string;
  shortName?: string;
  fullName?: string;
  taxCode?: string;
  address?: string;
  contactPerson?: string;
  contactEmail?: string;
  contactPhoneNumber?: string;
  paymentTermCode?: string;
  paymentTermDescription?: string;
  creditLimitMax?: number;
  creditLimitMin?: number;
  creditExposureMax?: number;
  creditExposureMin?: number;
  appliedPrice?: number;
  deactive?: boolean;
  note?: string;
}
