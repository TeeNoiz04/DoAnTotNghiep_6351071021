import { mapEnumToOptions } from '@abp/ng.core';

export enum ReportType {
  None = 0,
  Delivery = 1,
  Receipt = 2,
  Inventory = 3,
}

export const reportTypeOptions = mapEnumToOptions(ReportType);
