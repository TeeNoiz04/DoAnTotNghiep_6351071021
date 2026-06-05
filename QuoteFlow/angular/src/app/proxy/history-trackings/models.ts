import type { AuditedEntityDto } from '@abp/ng.core';

export interface HistoryTrackingDto extends AuditedEntityDto<string> {
  trackingType?: string;
  action?: string;
  objectId?: string;
  golfaCode?: string;
  model?: string;
  qty?: number;
  previousValue?: number;
  nextValue?: number;
  stockId?: string;
  stockName?: string;
  note?: string;
  creatorUsername?: string;
  beforeChange?: string;
  afterChange?: string;
  concurrencyStamp?: string;
}
