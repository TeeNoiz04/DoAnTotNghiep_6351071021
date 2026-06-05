import type { ExtendedFullAuditedEntityDto } from '../shared/models';
import type { GKRDetailDto, GKRDetailImportDto } from './gkrdetails/models';
import type { ExcelValidationResult } from '../shared/excels/models';
import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface GKRCancelItemDto {
  gkrDetailIds: string[];
  concurrencyStamp: string;
  note: string;
}

export interface GKRConfirmNoteDto {
  gkrDetailIds: string[];
  note?: string;
}

export interface GKRDto extends ExtendedFullAuditedEntityDto<string> {
  dpoNo?: string;
  dpoType?: string;
  gicType?: string;
  referenceDoc?: string;
  referenceDocDate?: string;
  gicProcess?: string;
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
  concurrencyStamp?: string;
  details: GKRDetailDto[];
  flags: GKRFlagsDto;
  linkedDpoNo?: string;
  linkedDpoId?: string;
  linkedNote?: string;
  expirationDate?: string;
  reason?: string;
  salePicUsername?: string;
  salePicFullName?: string;
  salePicTeamId?: string;
  currentApprovalRouteInstanceId?: string;
  currentApprovalStepSequence?: number;
  currentApproverRoleCode?: string;
  currentApproverRoleName?: string;
}

export interface GKRExcelDownloadDto {
  downloadToken?: string;
  gkrNo?: string;
  materialType?: string;
  status?: string;
  buyerTypeId?: string;
  buyerId?: string;
  buyerShortName?: string;
  orderDateMin?: string;
  orderDateMax?: string;
  totalAmountMin?: number;
  totalAmountMax?: number;
  linkedDPONo?: string;
  gicType?: string;
  gicProcess?: string;
  costCenter?: string;
  materialGroup?: string;
  materialCode?: string;
  modelName?: string;
  specialPriceCode?: string;
  customerName?: string;
  customerTaxCode?: string;
  poNo?: string;
  supplierCode?: string;
}

export interface GKRFlagsDto {
  isEditable: boolean;
  isRemovable: boolean;
  isViewable: boolean;
  canCancel: boolean;
  canConfirmNote: boolean;
  canLockStock: boolean;
  canLockOnOrder: boolean;
  canConfirmLockStock: boolean;
  canConfirmLockOnOrder: boolean;
  canApprove: boolean;
  canReject: boolean;
  canEditItem: boolean;
}

export interface GKRImportDto {
  materialType?: string;
  dpoNo?: string;
  remark?: string;
  fileName?: string;
  buyerTypeId?: string;
  buyerId?: string;
  buyerShortName?: string;
  buyerTypeDescription?: string;
  confirmDate?: string;
  expirationDate?: string;
  reason?: string;
  salePicUsername?: string;
  salePicFullName?: string;
  salePicTeamId?: string;
  details: ExcelValidationResult<GKRDetailImportDto>;
}

export interface GKRImportInput {
  materialType: string;
  buyerId?: string;
  buyerTypeId?: string;
  confirmDate: string;
  expirationDate: string;
  reason?: string;
  salePicUsername?: string;
}

export interface GKRLockOnOrderStockAutoDto {
  gkrDetailIds: string[];
  note?: string;
}

export interface GKRLockStockAutoDto {
  stockCategoryId: string;
  gkrDetailIds: string[];
}

export interface GetGKRsInput extends PagedAndSortedResultRequestDto {
  gkrNo?: string;
  materialType?: string;
  status?: string;
  buyerTypeId?: string;
  buyerId?: string;
  buyerShortName?: string;
  orderDateMin?: string;
  orderDateMax?: string;
  totalAmountMin?: number;
  totalAmountMax?: number;
  linkedDPONo?: string;
  gicType?: string;
  gicProcess?: string;
  costCenter?: string;
  materialGroup?: string;
  materialCode?: string;
  modelName?: string;
  specialPriceCode?: string;
  customerName?: string;
  customerTaxCode?: string;
  poNo?: string;
  supplierCode?: string;
}
