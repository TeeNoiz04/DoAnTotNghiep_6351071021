import type { AuditedEntityDto } from '@abp/ng.core';

export interface SpoBatchRequestDetailDto extends AuditedEntityDto<string> {
  requestId?: string;
  spoCode?: string;
  golfaCode?: string;
  action?: string;
  actionDate?: string;
  note?: string;
  status?: string;
  concurrencyStamp?: string;
}

export interface SpoBatchRequestDetailImportDto {
  requestId?: string;
  spoCode?: string;
  golfaCode?: string;
  action?: string;
  actionDate?: string;
  note?: string;
}
