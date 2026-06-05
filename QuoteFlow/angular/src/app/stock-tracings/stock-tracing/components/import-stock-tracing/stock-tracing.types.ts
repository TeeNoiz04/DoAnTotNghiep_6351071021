export enum ImportStockTracingType {
  Delivery = 'Delivery',
  Receipt = 'Receipt',
  Inventory = 'Inventory',
}

export type ImportStockTracingInformation = {
  file?: File | null;
  fromDate?: Date | null;
  toDate?: Date | null;
  entered?: Date | null;
  note?: string;
};
