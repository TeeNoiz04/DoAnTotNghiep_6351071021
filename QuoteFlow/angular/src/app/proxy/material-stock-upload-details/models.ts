import type { AuditedEntityDto } from '@abp/ng.core';

export interface MaterialStockUploadDetailDto extends AuditedEntityDto<string> {
  requestId?: string;
  materialCode?: string;
  model?: string;
  storage?: string;
  storageDestination?: string;
  qty?: number;
  refDoc?: string;
  remark?: string;
  storageDesc_Id?: string;
  storageSrc_Id?: string;
  concurrencyStamp?: string;
}

export interface MaterialStockUploadDetailImportInventoryDto {
  requestId?: string;
  materialCode?: string;
  model?: string;
  storage?: string;
  qty?: number;
  refDoc?: string;
  remark?: string;
  storageSrc_Id?: string;
}

export interface MaterialStockUploadDetailImportTransferDto {
  requestId?: string;
  materialCode?: string;
  model?: string;
  storage?: string;
  storageDestination?: string;
  qty?: number;
  remark?: string;
  storageDesc_Id?: string;
  storageSrc_Id?: string;
}
