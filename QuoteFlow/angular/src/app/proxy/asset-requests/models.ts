import type { ApprovalHistoryDto } from '../approval-histories/models';
import type { ApprovalRouteDto } from '../approval-routes/models';
import type { ExtendedAuditedEntityDto } from '../shared/models';
import type { AssetRequestDetailDto } from '../asset-request-details/models';
import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { ExcelValidationResult } from '../shared/excels/models';

export interface AssetAuditImportDto {
  assetName?: string;
  description?: string;
  source?: string;
  materialCode?: string;
  modelName?: string;
  qty?: number;
  unit?: string;
  price?: number;
  invoicePrice?: number;
  locationWH?: string;
  whId?: string;
  assetType?: string;
  assetClass?: string;
  codeMain?: string;
  codeSub?: string;
  codeMain_AF?: string;
  codeSub_AF?: string;
  salePIC?: string;
  sectionSAP?: string;
  division?: string;
  department?: string;
  section?: string;
  reg?: string;
  por?: string;
  pr?: string;
  giv?: string;
  note?: string;
  countedQuantity?: number;
  variance?: number;
  auditResult?: string;
  auditNote?: string;
  fai_PIC?: string;
  fap_PIC?: string;
  ia_PIC?: string;
  af_PIC?: string;
  id?: string;
  assetId?: string;
  requestId?: string;
  concurrencyStamp?: string;
}

export interface AssetRequestActionDto {
  id?: string;
  action?: string;
  note?: string;
  concurrencyStamp?: string;
}

export interface AssetRequestApprovalHistoryDto extends ApprovalHistoryDto {
  assetRequestId?: string;
}

export interface AssetRequestApprovalRouteDto extends ApprovalRouteDto {
  assetRequestId?: string;
}

export interface AssetRequestCreateDto {
  requestType?: string;
  lending_Target?: string;
  title?: string;
  warehouseSrcId?: string;
  warehouseSrcName?: string;
  warehouseDestId?: string;
  warehouseDestName?: string;
  pic_Src?: string;
  pic_Dest?: string;
  lending_CustomerTaxCode?: string;
  lending_ReturnDate?: string;
  requestOwner?: string;
  submittedDate?: string;
  note?: string;
  lending_CustomerName?: string;
  agreementNo?: string;
  audit_FromDate?: string;
  audit_ToDate?: string;
}

export interface AssetRequestDto extends ExtendedAuditedEntityDto<string> {
  requestNo?: string;
  lending_Target?: string;
  title?: string;
  requestType?: string;
  status?: string;
  warehouseSrcId?: string;
  warehouseSrcName?: string;
  warehouseDestId?: string;
  warehouseDestName?: string;
  pic_Src?: string;
  pic_Dest?: string;
  lending_CustomerTaxCode?: string;
  lending_ReturnDate?: string;
  requestOwner?: string;
  submittedDate?: string;
  note?: string;
  agreementNo?: string;
  lendingInvoiceNo?: string;
  returnInvoiceNo?: string;
  extensionDoc?: string;
  currentApprovalRouteInstanceId?: string;
  currentApprovalStepSequence?: number;
  currentApproverRoleName?: string;
  currentApproverRoleCode?: string;
  audit_FromDate?: string;
  audit_ToDate?: string;
  details: AssetRequestDetailDto[];
  approvalRoutes: AssetRequestApprovalRouteDto[];
  approvalHistories: AssetRequestApprovalHistoryDto[];
  flags: AssetRequestFlagsDto;
  concurrencyStamp?: string;
  lending_CustomerName?: string;
  deliveryNote?: string;
}

export interface AssetRequestFlagsDto {
  isEditable: boolean;
  isRemovable: boolean;
  isViewable: boolean;
  isApprovable: boolean;
  isRejectable: boolean;
  isCancellable: boolean;
  isExtendable: boolean;
  canCancelItem: boolean;
  canSubmitable: boolean;
  canReturn: boolean;
  isEditLendingable: boolean;
  canCreate: boolean;
}

export interface AssetRequestUpdateDto {
  title?: string;
  lending_Target?: string;
  warehouseSrcId?: string;
  warehouseSrcName?: string;
  warehouseDestId?: string;
  warehouseDestName?: string;
  pic_Src?: string;
  pic_Dest?: string;
  lending_CustomerTaxCode?: string;
  lending_ReturnDate?: string;
  requestOwner?: string;
  submittedDate?: string;
  note?: string;
  lending_CustomerName?: string;
  agreementNo?: string;
  lendingInvoiceNo?: string;
  returnInvoiceNo?: string;
  deliveryNote?: string;
  audit_FromDate?: string;
  audit_ToDate?: string;
  concurrencyStamp?: string;
}

export interface AssetRequestUpdateExtendDto extends AssetRequestUpdateDto {
  extensionDoc?: string;
}

export interface GetAssetRequestsInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  title?: string;
  requestNo?: string;
  requestType?: string;
  status?: string;
  warehouseSrcId?: string;
  warehouseSrcName?: string;
  warehouseDestId?: string;
  warehouseDestName?: string;
  pic_Src?: string;
  pic_Dest?: string;
  lending_CustomerTaxCode?: string;
  lending_ReturnDateMin?: string;
  lending_ReturnDateMax?: string;
  requestOwner?: string;
  submittedDateMin?: string;
  submittedDateMax?: string;
  note?: string;
  currentApprovalRouteInstanceId?: string;
  currentApprovalStepSequenceMin?: number;
  currentApprovalStepSequenceMax?: number;
  currentApproverRoleName?: string;
  currentApproverRoleCode?: string;
  requestTypes?: string;
  statuses?: string;
  createBy?: string;
}

export interface ResultValidateAssetAuditDto {
  resultImport: ExcelValidationResult<AssetAuditImportDto>;
  requestId?: string;
}
