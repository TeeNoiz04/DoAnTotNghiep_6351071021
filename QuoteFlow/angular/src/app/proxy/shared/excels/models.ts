export interface ExcelRowResult<T> {
  rowData: T;
  errors: string[];
  warnings: string[];
  rowIndex: number;
  hasErrors: boolean;
  hasWarnings: boolean;
}

export interface ExcelValidationResult<T> {
  hasErrors: boolean;
  hasWarnings: boolean;
  isValid: boolean;
  hasNotFoundWarnings: boolean;
  singleRow: boolean;
  fileName?: string;
  errors: string[];
  warnings: string[];
  listData: ExcelRowResult<T>[];
}
