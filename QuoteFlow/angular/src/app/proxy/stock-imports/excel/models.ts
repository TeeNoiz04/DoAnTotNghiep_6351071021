export interface StockImportExcelDto {
  no?: number;
  invoiceNo?: string;
  machineNo?: string;
  materialCode?: string;
  model?: string;
  poNo?: string;
  importQty?: number;
  unitPrice?: number;
  amount?: number;
  unit?: string;
  origin?: string;
  etd?: string;
  eta?: string;
  shipmentMethod?: string;
  invoiceDate?: string;
}
