import { TemplateRef } from '@angular/core';
import { ExcelValidationResult } from '@proxy/shared/excels';
import { SpoBatchRequestDetailImportDto } from '@proxy/spo-batch-requests/spo-batch-request-details';

export enum ImportBatchRequestType {
  BatchRequest = 'BATCH_REQUEST',
}

export const ImportBatchRequestTypeMap: Record<ImportBatchRequestType, string> = {
  [ImportBatchRequestType.BatchRequest]: 'BATCH REQUEST',
};

export const BatchRequest_OPTIONS = [
  {
    value: ImportBatchRequestType.BatchRequest,
    label: ImportBatchRequestTypeMap[ImportBatchRequestType.BatchRequest],
  },
];

export type ImportBatchRequestTypeOption = {
  label: string;
  value: ImportBatchRequestType;
};
export type ImportResultMap = {
  [ImportBatchRequestType.BatchRequest]: ExcelValidationResult<SpoBatchRequestDetailImportDto>;
};
export type ImportBatchRequestInformation = {
  file?: File | null;
  note?: string;
};

export const ImportDownloadTemplateType = {
  [ImportBatchRequestType.BatchRequest]: 'BATCH_REQUEST',
};
export const ImportBatchRequestColumns: { [key in ImportBatchRequestType]: TableColumn[] } = {
  [ImportBatchRequestType.BatchRequest]: [
    { prop: 'status', name: 'Status', format: 'status' },
    { prop: 'spoCode', name: 'SPO Code' },
    { prop: 'golfaCode', name: 'Material Code' },
    { prop: 'action', name: 'Action' },
    { prop: 'actionDate', name: 'Action Date', format: 'date' },
    { prop: 'note', name: 'Note' },
  ],
};
export interface TableColumn {
  prop: string;
  name: string;
  minWidth?: number;
  sortable?: boolean;
  format?: string;
  frozenLeft?: boolean;
  cellTemplate?: TemplateRef<any>;
}
