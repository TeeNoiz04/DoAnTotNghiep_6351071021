import type { AuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { SpoBatchRequestDetailDto } from './spo-batch-request-details/models';

export interface GetSpoBatchRequestsInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  requestNo?: string;
  importType?: string;
  fileName?: string;
  note?: string;
  status?: string;
  spoCode?: string;
  golfaCode?: string;
}

export interface SpoBatchRequestDto extends AuditedEntityDto<string> {
  requestNo?: string;
  importType?: string;
  fileName?: string;
  note?: string;
  status?: string;
  spoBatchRequestDetails: SpoBatchRequestDetailDto[];
  concurrencyStamp?: string;
}
