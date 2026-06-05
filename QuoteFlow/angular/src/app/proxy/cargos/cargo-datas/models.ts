import type { ExtendedAuditedEntityDto } from '../../shared/models';
import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface CargoDataDto extends ExtendedAuditedEntityDto<string> {
  cargoId?: string;
  poDetailId?: string;
  poDetailCode?: string;
  golfaCode?: string;
  model?: string;
  poRef?: string;
  invoiceNo?: string;
  srNo?: string;
  classification?: string;
  product?: string;
  materialType?: string;
  spec1?: string;
  spec2?: string;
  spec3?: string;
  orderQty?: string;
  exWorkQty?: string;
  nonExWorkQty?: string;
  inSTCH?: string;
  shipped?: string;
  waitForShip?: string;
  shipDate?: string;
  orderDate?: string;
  inSTCHDate?: string;
  shipmentMethod?: string;
  etA1?: string;
  etA2?: string;
  mevnRequest?: string;
  stcReply?: string;
  eu?: string;
  mevnAddedRequest?: string;
  npd?: string;
  plannedShipment?: string;
  soDate?: string;
  cellMarker?: string;
  shipmentForm?: string;
  machineNumber?: string;
  fileName?: string;
  supplierCode?: string;
  note?: string;
}

export interface GetCargoDataInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  cargoId?: string;
  materialCode?: string;
  model?: string;
  machineNumber?: string;
  po?: string;
  eu?: string;
  fileName?: string;
  materialType?: string;
  supplier?: string;
}
