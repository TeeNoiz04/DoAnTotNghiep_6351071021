import type { AuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface MaterialGroupDto extends AuditedEntityDto<string> {
  code?: string;
  name?: string;
  parent?: string;
  sortOrder: number;
  note?: string;
  isDeActive: boolean;
  materialType?: string;
  materialGroupPSI?: string;
  allowKeyAccount: boolean;
  concurrencyStamp?: string;
}

export interface GetMaterialGroupsInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  code?: string;
  name?: string;
  parent?: string;
  sortOrderMin?: number;
  sortOrderMax?: number;
  note?: string;
  isDeActive?: boolean;
  materialType?: string;
  materialGroupPSI?: string;
  allowKeyAccount?: boolean;
}

export interface MaterialGroupCreateDto {
  code: string;
  name: string;
  parent?: string;
  sortOrder: number;
  note?: string;
  isDeActive: boolean;
  materialType?: string;
  materialGroupPSI?: string;
  allowKeyAccount: boolean;
}

export interface MaterialGroupUpdateDto {
  code: string;
  name: string;
  parent?: string;
  sortOrder: number;
  note?: string;
  isDeActive: boolean;
  materialType?: string;
  materialGroupPSI?: string;
  allowKeyAccount: boolean;
  concurrencyStamp?: string;
}
