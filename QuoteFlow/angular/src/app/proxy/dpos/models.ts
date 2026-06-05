import type {
  ExtendedAuditedEntityDto,
  ExtendedFullAuditedEntityDto,
  NoteMetadataDto,
} from '../shared/models';
import type { DPODetailDto, ImportDPODetailDto } from './dpodetails/models';
import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { ExcelValidationResult } from '../shared/excels/models';

export interface BatchAutoUnlockStockDto {
  dpoDetailIds: string[];
}

export interface DPOAddExtraFeeDto {
  dpoDetailIds: string[];
  extraFee: number;
  extraFeeNote?: string;
}

export interface DPOAllocateGKRDto extends NoteMetadataDto {
  dpoId: string;
  gkrId: string;
}

export interface DPOCancelDto {
  dpoDetailIds: string[];
  concurrencyStamp: string;
  note: string;
}

export interface DPOConfirmNoteDto {
  dpoDetailIds: string[];
  note?: string;
}

export interface DPOCreateDto {
  dpoNo?: string;
  gicNo?: string;
  dpoType?: string;
  gicType?: string;
  materialType?: string;
  costCenter?: string;
  status?: string;
  buyerTypeId?: string;
  buyerId?: string;
  buyerShortName?: string;
  buyerDescription?: string;
  orderDate?: string;
  totalAmount: number;
  remark?: string;
  fileName?: string;
  referenceDoc?: string;
  gicProcess?: string;
}

export interface DPODataReportDto {
  buyerType?: string;
  buyerShortName?: string;
  materialType?: string;
  january?: number;
  february?: number;
  march?: number;
  april?: number;
  may?: number;
  june?: number;
  july?: number;
  august?: number;
  september?: number;
  october?: number;
  november?: number;
  december?: number;
}

export interface DPODto extends ExtendedFullAuditedEntityDto<string> {
  dpoNo?: string;
  gicNo?: string;
  dpoType?: string;
  gicType?: string;
  materialType?: string;
  costCenter?: string;
  status?: string;
  buyerTypeId?: string;
  buyerId?: string;
  buyerShortName?: string;
  buyerDescription?: string;
  orderDate?: string;
  totalAmount: number;
  totalAmountIncludeExtraFee: number;
  remark?: string;
  fileName?: string;
  referenceDoc?: string;
  gicProcess?: string;
  confirmNoted?: string;
  details: DPODetailDto[];
  flags: DPOFlagsDto;
  concurrencyStamp?: string;
  spec1?: string;
  spec2?: string;
}

export interface DPOExcelDownloadDto {
  downloadToken?: string;
  filterText?: string;
  dpoNo?: string;
  dpoType?: string;
  dpoSubType?: string;
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
}

export interface DPOExportDataInputDto {
  dpoNo?: string;
  status?: string;
  golfaCode?: string;
  model?: string;
  poNo?: string;
  customerName?: string;
  fromDate?: string;
  toDate?: string;
  buyerTypeId?: string;
  buyerId?: string;
  materialType?: string;
  supplierCode?: string;
  spoCode?: string;
  taxCode?: string;
  materialGroup?: string;
}

export interface DPOFlagsDto {
  isEditable: boolean;
  isRemovable: boolean;
  isViewable: boolean;
  canLockStock: boolean;
  canLockOnOrder: boolean;
  canConfirmLockStock: boolean;
  canConfirmLockOnOrder: boolean;
  canApprove: boolean;
  canReject: boolean;
  canEditItem: boolean;
  canDelete: boolean;
  canAllocateGKR: boolean;
}

export interface DPOListPOsDto {
  poDetailId?: string;
  poNo?: string;
  golfaCode?: string;
  poDate?: string;
  qtyAvailable: number;
  machineNumber?: string;
  stcReply?: string;
}

export interface DPOLockOnOrderStockDto extends ExtendedAuditedEntityDto<string> {
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

export interface DPOLockShipmentAutoDto {
  dpoDetailIds: string[];
  note?: string;
}

export interface DPOLockShipmentDto {
  items: DPOLockShipmentItemDto[];
}

export interface DPOLockShipmentItemDto {
  poDetailId: string;
  golfaCode: string;
  qty: number;
  note?: string;
}

export interface DPOLockShipmentItemUpdateDto {
  golfaCode: string;
  qty: number;
  note?: string;
}

export interface DPOLockStockAutoDto {
  dpoId: string;
  stockCategoryId: string;
}

export interface DPOLockStockAutoV2Dto {
  stockCategoryId: string;
  dpoDetailIds: string[];
}

export interface DPOLockStockEtaEtdDto {
  invoiceNo?: string;
  poNo?: string;
  materialCode?: string;
  qty_Allocation: number;
  shipmentMethod?: string;
  eta?: string;
  etd?: string;
  status?: string;
}

export interface DPOProcessingReportDto {
  dpoNo?: string;
  golfaCode?: string;
  model?: string;
  spec1?: string;
  material_Group?: string;
  materialType?: string;
  buyerTypeDescription?: string;
  buyerShortName?: string;
  orderDate?: string;
  requestedETA?: string;
  spoCode?: string;
  customerName?: string;
  status?: string;
  qty?: number;
  unitPrice?: number;
  extrafee?: number;
  amountIncludeExtrafee?: number;
  delivered?: number;
  delivery_Price?: number;
  deliveryOrder_Amount?: number;
  remain_Qty?: number;
  remain_Amount?: number;
}

export interface DPOUpdateDto {
  dpoNo: string;
  dpoType?: string;
  gicType?: string;
  materialType?: string;
  costCenter?: string;
  status?: string;
  buyerTypeId?: string;
  buyerId?: string;
  buyerShortName?: string;
  buyerDescription?: string;
  orderDate?: string;
  totalAmount: number;
  remark?: string;
  fileName?: string;
  referenceDoc?: string;
  gicProcess?: string;
  concurrencyStamp?: string;
}

export interface DpoGkrAllocationDetailDto {
  id?: string;
  golfaCode?: string;
  model?: string;
  gkrQty: number;
  keptQty: number;
  orderQty: number;
  takeQty: number;
  releaseQty: number;
  note?: string;
}

export interface DpoGkrAllocationDto {
  id?: string;
  materialType?: string;
  dpoNo?: string;
  buyerId?: string;
  buyerTypeId?: string;
  buyerTypeDescription?: string;
  buyerShortName?: string;
  orderDate?: string;
  expirationDate?: string;
  totalAmount: number;
  remark?: string;
  linkedNote?: string;
  allocationDetails: DpoGkrAllocationDetailDto[];
}

export interface GetDPOReportInputDto {
  buyerTypeId?: string;
  buyerId?: string;
  fromDate: string;
  toDate: string;
}

export interface GetDPOsInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  dpoNo?: string;
  materialCode?: string;
  modelName?: string;
  poNo?: string;
  customerName?: string;
  orderDateMin?: string;
  orderDateMax?: string;
  buyerId?: string;
  materialType?: string;
  supplierId?: string;
  specialPriceCode?: string;
  materialGroup?: string;
  taxCode?: string;
  salesOrg?: string;
  dpoType?: string;
  dpoSubType?: string;
  costCenter?: string;
  status?: string;
  buyerTypeId?: string;
  buyerShortName?: string;
  buyerDescription?: string;
  totalAmountMin?: number;
  totalAmountMax?: number;
  remark?: string;
  fileName?: string;
}

export interface ImportDPODto {
  materialType?: string;
  dpoNo?: string;
  remark?: string;
  fileName?: string;
  buyerTypeId?: string;
  buyerId?: string;
  confirmDate?: string;
  details: ExcelValidationResult<ImportDPODetailDto>;
}

export interface ImportDPOInput {
  materialType: string;
  buyerId: string;
  buyerTypeId: string;
  confirmDate: string;
}

export interface GICAllocateGKRDto extends NoteMetadataDto {
  gicId: string;
  gkrId: string;
}

export interface GicGkrAllocationDetailDto {
  id?: string;
  golfaCode?: string;
  model?: string;
  gkrQty: number;
  keptQty: number;
  orderQty: number;
  takeQty: number;
  releaseQty: number;
  note?: string;
}

export interface GicGkrAllocationDto {
  id?: string;
  materialType?: string;
  dpoNo?: string;
  buyerId?: string;
  buyerTypeId?: string;
  buyerTypeDescription?: string;
  buyerShortName?: string;
  orderDate?: string;
  expirationDate?: string;
  totalAmount: number;
  remark?: string;
  linkedNote?: string;
  allocationDetails: GicGkrAllocationDetailDto[];
}
