import { TemplateRef } from '@angular/core';
import { AppPermissions } from '@app/app.permissions';
import {
  MaterialStockUploadDetailImportInventoryDto,
  MaterialStockUploadDetailImportTransferDto,
} from '@proxy/material-stock-upload-details/models';
import { ExcelValidationResult } from '@proxy/shared/excels';

export enum ImportStockManagementType {
  StockInventory = 'Stock.Inventory',
  StockTransfer = 'Stock.Transfer',
}

export type ImportMaterialInformation = {
  file?: File | null;
  fromDate?: Date | null;
  toDate?: Date | null;
  note?: string;
};

export type ImportStockManagementTypeOption = {
  label: string;
  value: ImportStockManagementType;
  requiredPolicy?: string;
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

export const ImportDownloadTemplateType = {
  [ImportStockManagementType.StockInventory]: 'STOCK_INVENTORY',
  [ImportStockManagementType.StockTransfer]: 'STOCK_TRANSFER',
};

export type ImportResultMap = {
  [ImportStockManagementType.StockInventory]: ExcelValidationResult<MaterialStockUploadDetailImportInventoryDto>;
  [ImportStockManagementType.StockTransfer]: ExcelValidationResult<MaterialStockUploadDetailImportTransferDto>;
};

export const ImportMaterialOptions: ImportStockManagementTypeOption[] = [
  {
    label: 'Stock Inventory',
    value: ImportStockManagementType.StockInventory,
    requiredPolicy: `${AppPermissions.MaterialStocks.Uploads.StockInventory}`,
  },
  {
    label: 'Stock Transfer',
    value: ImportStockManagementType.StockTransfer,
    requiredPolicy: `${AppPermissions.MaterialStocks.Uploads.StockTransfer}`,
  },
];

export const ImportStockColumns: { [key in ImportStockManagementType]: TableColumn[] } = {
  [ImportStockManagementType.StockInventory]: [
    { prop: 'materialCode', name: 'Material code', frozenLeft: true },
    { prop: 'model', name: 'Model Name', frozenLeft: true },
    { prop: 'storage', name: 'Storage' },
    { prop: 'qty', name: 'Actual Qty' },
    { prop: 'refDoc', name: 'Ref Doc' },
    { prop: 'remark', name: 'Remark' },
  ],
  [ImportStockManagementType.StockTransfer]: [
    { prop: 'materialCode', name: 'Material code', frozenLeft: true },
    { prop: 'model', name: 'Model Name', frozenLeft: true },
    { prop: 'storage', name: 'Source storage' },
    { prop: 'storageDestination', name: 'Destination storage' },
    { prop: 'qty', name: 'Qty' },
    { prop: 'remark', name: 'Remark' },
  ],
};
