import type { ExtendedAuditedEntityDto } from '../shared/models';
import type { CargoDataDto } from './cargo-datas/models';
import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { ExcelValidationResult } from '../shared/excels/models';

export interface CargoCreateDto {
  fileName?: string;
  note?: string;
}

export interface CargoDto extends ExtendedAuditedEntityDto<string> {
  fileName?: string;
  note?: string;
  supplierCode?: string;
  materialType?: string;
  details: CargoDataDto[];
  concurrencyStamp?: string;
}

export interface CargoExcelInput {
  materialType?: string;
  supplierCode?: string;
  note?: string;
}

export interface CargoImportDto {
  invoiceNo?: string;
  srNo?: string;
  poDetailCode?: string;
  golfaCode?: string;
  machineNo?: string;
  classification?: string;
  product?: string;
  model?: string;
  spec1?: string;
  spec2?: string;
  spec3?: string;
  orderQty?: string;
  exWorkQty?: string;
  nonExWorks?: string;
  stockQuantity?: string;
  shipped?: string;
  waitForShip?: string;
  shipDate?: string;
  order?: string;
  shippingDate?: string;
  shipmentMethod?: string;
  poRef?: string;
  etA1?: string;
  etA2?: string;
  mevnRequest?: string;
  stcReply?: string;
  eu?: string;
  mevnAddedRequest?: string;
  soDate?: string;
  cellMarker?: string;
  shippingForm?: string;
  poDetailId?: string;
}

export interface CargoReportDto {
  poNo?: string;
  po_Date?: string;
  poDetailCode?: string;
  golfaCode?: string;
  model?: string;
  machineNumber?: string;
  orderQty: number;
  inSTCH: number;
  shipped?: string;
  waitForShip: number;
  soDate?: string;
  dpoNo?: string;
  qtyNeed: number;
  mevnRequest?: string;
  mevnAddedRequest?: string;
  stcReply?: string;
}

export interface CargoUpdateDto {
  fileName?: string;
  note?: string;
  concurrencyStamp?: string;
}

export interface GetCargoReportsInput extends PagedAndSortedResultRequestDto {
  poNo?: string;
  materialCode?: string;
  modelName?: string;
  from?: string;
  to?: string;
}

export interface GetCargosInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  fileName?: string;
  note?: string;
  supplierCode?: string;
  materialType?: string;
}

export interface ImportCargoRequestDto {
  data: ExcelValidationResult<CargoImportDto>;
  input: CargoExcelInput;
}
