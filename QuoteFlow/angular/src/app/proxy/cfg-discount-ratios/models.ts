import type { ExtendedAuditedEntityDto } from '../shared/models';
import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface CfgDiscountRatioDto extends ExtendedAuditedEntityDto<string> {
  approval_Type?: string;
  product_Type?: string;
  accountClassify?: string;
  kaType?: string;
  value_Min?: number;
  value_Max?: number;
  discountRatio?: number;
  note?: string;
  concurrencyStamp?: string;
}

export interface CfgDiscountRatioUpdateDto {
  approval_Type?: string;
  product_Type?: string;
  accountClassify?: string;
  value_Min?: number;
  value_Max?: number;
  discountRatio?: number;
  note?: string;
  concurrencyStamp?: string;
}

export interface GetCfgDiscountRatiosInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  approval_Type?: string;
  product_Type?: string;
  accountClassify?: string;
  kaType?: string;
  value_MinMin?: number;
  value_MinMax?: number;
  value_MaxMin?: number;
  value_MaxMax?: number;
  discountRatioMin?: number;
  discountRatioMax?: number;
  note?: string;
}
