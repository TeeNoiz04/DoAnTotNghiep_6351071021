import { PSIImportDto } from '@proxy/psis';
import { ExcelValidationResult } from '@proxy/shared/excels';

export enum ImportPSIType {
  PSI_FA = 'PSI_FA',
  PSI_LVS = 'PSI_LVS',
}

export const ImportPSITypeMap: Record<ImportPSIType, string> = {
  [ImportPSIType.PSI_FA]: 'FA',
  [ImportPSIType.PSI_LVS]: 'LVS',
};

export type ImportPSITypeOption = {
  label: string;
  value: ImportPSIType;
};

export type ImportPSIInformation = {
  file?: File | null;
  materialType?: string;
  financeYear?: string;
  note?: string;
};

export type ImportResultMap = {
  [ImportPSIType.PSI_FA]: ExcelValidationResult<PSIImportDto>;
  [ImportPSIType.PSI_LVS]: ExcelValidationResult<PSIImportDto>;
};

export const ImportPSIOptions: ImportPSITypeOption[] = [
  {
    label: 'FA',
    value: ImportPSIType.PSI_FA,
  },
  {
    label: 'LVS',
    value: ImportPSIType.PSI_LVS,
  },
];
