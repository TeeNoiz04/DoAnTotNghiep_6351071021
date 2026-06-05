import type { ExtendedAuditedEntityDto, ExtendedFullAuditedEntityDto } from '../shared/models';
import type { GICDetailDto, GICDetailImportDto } from './gicdetails/models';
import type { ExcelValidationResult } from '../shared/excels/models';
import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GICAddExtraFeeDto {
  gicDetailIds: string[];
  extraFee: number;
  extraFeeNote?: string;
}

export interface GICCancelDto {
  gicDetailIds: string[];
  concurrencyStamp: string;
  note: string;
}

export interface GICConfirmNoteDto {
  gicId?: string;
  gicDetailIds: string[];
  note?: string;
}

export interface GICDto extends ExtendedFullAuditedEntityDto<string> {
  dpoNo?: string;
  dpoType?: string;
  gicType?: string;
  materialType?: string;
  costCenter?: string;
  status?: string;
  buyerTypeId?: string;
  buyerId?: string;
  buyerShortName?: string;
  buyerTypeDescription?: string;
  orderDate?: string;
  totalAmount: number;
  remark?: string;
  fileName?: string;
  referenceDoc?: string;
  referenceDocDate?: string;
  gicProcess?: string;
  details: GICDetailDto[];
  concurrencyStamp?: string;
  flags: GICFlagsDto;
}

export interface GICExcelDownloadDto {
  downloadToken?: string;
  filterText?: string;
  gicNo?: string;
  gicType?: string;
  gicProcess?: string;
  materialType?: string;
  costCenter?: string;
  status?: string;
  buyerTypeId?: string;
  buyerId?: string;
  buyerShortName?: string;
  buyerDescription?: string;
  orderDateMin?: string;
  orderDateMax?: string;
  totalAmountMin?: number;
  totalAmountMax?: number;
  remark?: string;
  fileName?: string;
  sorting?: string;
  maxResultCount: number;
  skipCount: number;
}

export interface GICFlagsDto {
  isEditable: boolean;
  isRemovable: boolean;
  isViewable: boolean;
  canCancel: boolean;
  canAddExtraFee: boolean;
  canConfirmNote: boolean;
  canLockStock: boolean;
  canLockOnOrder: boolean;
  canConfirmLockStock: boolean;
  canConfirmLockOnOrder: boolean;
  canEditItem: boolean;
  canAllocateGKR: boolean;
}

export interface GICImportDto {
  gicType?: string;
  materialType?: string;
  costCenter?: string;
  status?: string;
  buyerTypeId?: string;
  buyerId?: string;
  buyerShortName?: string;
  buyerTypeDescription?: string;
  gicProcess?: string;
  referenceDoc?: string;
  referenceDocDate?: string;
  orderDate?: string;
  totalAmount: number;
  remark?: string;
  fileName?: string;
  details: ExcelValidationResult<GICDetailImportDto>;
}

export interface GICImportInput {
  materialType?: string;
  confirmDate?: string;
  buyerId?: string;
  buyerShortName?: string;
  buyerTypeId?: string;
  buyerTypeDescription?: string;
  gicProcessDescription?: string;
  gicTypeDescription?: string;
  costCenter?: string;
  refDoc?: string;
  referenceDocDate?: string;
  note?: string;
}

export interface GICListPOsDto {
  poDetailId?: string;
  poNo?: string;
  golfaCode?: string;
  poDate?: string;
  qtyAvailable: number;
  machineNumber?: string;
  stcReply?: string;
}

export interface GICLockOnOrderStockDto extends ExtendedAuditedEntityDto<string> {
  poDetailId?: string;
  poNo?: string;
  poQty: number;
  poDate?: string;
  machineNumber?: string;
  stcReply?: string;
  status?: string;
  note?: string;
  qtyLocked?: number;
  qtyImported?: number;
  qtyNeedImport?: number;
}

export interface GICLockShipmentDto {
  items: GICLockShipmentItemDto[];
}

export interface GICLockShipmentItemDto {
  poDetailId: string;
  golfaCode: string;
  qty: number;
  note?: string;
}

export interface GICLockShipmentItemUpdateDto {
  golfaCode: string;
  qty: number;
  note?: string;
}

export interface GICLockStockAutoDto {
  gicId: string;
  stockCategoryId: string;
}

export interface GICLockStockAutoV2Dto {
  stockCategoryId: string;
  gicDetailIds: string[];
}

export interface GICLockStockEtaEtdDto {
  invoiceNo?: string;
  poNo?: string;
  materialCode?: string;
  qty_Allocation: number;
  shipmentMethod?: string;
  eta?: string;
  etd?: string;
  status?: string;
}

export interface GetGICsInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  gicNo?: string;
  gicType?: string;
  gicProcess?: string;
  materialType?: string;
  costCenter?: string;
  materialCode?: string;
  modelName?: string;
  status?: string;
  buyerTypeId?: string;
  buyerId?: string;
  buyerShortName?: string;
  orderDateMin?: string;
  orderDateMax?: string;
  totalAmountMin?: number;
  totalAmountMax?: number;
  remark?: string;
}
