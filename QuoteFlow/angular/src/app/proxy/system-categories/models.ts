import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { ExtendedAuditedEntityDto } from '../shared/models';

export interface GetSystemCategoriesInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  parentId?: string;
  code?: string;
  description?: string;
  valueMin?: number;
  valueMax?: number;
  categoryType?: string;
  note?: string;
  isDeactive?: boolean;
  sortOrderMin?: number;
  sortOrderMax?: number;
}

export interface SystemCategoryCreateDto {
  parentId?: string;
  code: string;
  description: string;
  value?: number;
  categoryType: string;
  note?: string;
}

export interface SystemCategoryDto extends ExtendedAuditedEntityDto<string> {
  parentId?: string;
  code?: string;
  description?: string;
  value?: number;
  categoryType?: string;
  note?: string;
  isDeactive: boolean;
  sortOrder: number;
  concurrencyStamp?: string;
}

export interface SystemCategoryExcelDownloadDto {
  downloadToken?: string;
  filterText?: string;
  parentId?: string;
  code?: string;
  description?: string;
  valueMin?: number;
  valueMax?: number;
  categoryType?: string;
  note?: string;
  isDeactive?: boolean;
  sortOrderMin?: number;
  sortOrderMax?: number;
}

export interface SystemCategoryListDto extends ExtendedAuditedEntityDto<string> {
  code?: string;
  description?: string;
  categoryType?: string;
  value?: number;
  isDeactive: boolean;
  note?: string;
}

export interface SystemCategoryUpdateDto {
  parentId?: string;
  description: string;
  categoryType: string;
  value?: number;
  note?: string;
  isDeactive: boolean;
  concurrencyStamp?: string;
}
