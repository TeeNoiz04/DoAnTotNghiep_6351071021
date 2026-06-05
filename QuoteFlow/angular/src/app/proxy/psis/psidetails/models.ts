import type { ExtendedAuditedEntityDto } from '../../shared/models';

export interface PSIDetailDto extends ExtendedAuditedEntityDto<string> {
  materialGroup?: string;
  description?: string;
  fy?: number;
  month?: number;
  plan?: number;
  note?: string;
  concurrencyStamp?: string;
}

export interface PSIDetailImportDto {
  materialGroup?: string;
  description?: string;
  fy?: number;
  month?: number;
  plan?: number;
  note?: string;
}
