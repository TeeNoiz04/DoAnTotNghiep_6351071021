import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { KeyAccountEvaluationDto } from '../key-account-evaluations/models';
import type { ExtendedAuditedEntityDto } from '../shared/models';
import type { BuyerDto } from '../buyers/models';
import type { CustomerPICDto } from '../customer-pics/models';
import type { ApprovalHistoryDto } from '../approval-histories/models';
import type { AttachmentDto } from '../attachments/models';

export interface GetKeyAccountDetailReportsInput extends PagedAndSortedResultRequestDto {
  buyerId?: string;
  taxCode?: string;
  keyAccountCode?: string;
  keyAccountName?: string;
  model?: string;
  golfaCode?: string;
  fromDate?: string;
  toDate?: string;
  invoiceFromDate?: string;
  invoiceToDate?: string;
  materialType?: string;
  materialGroup?: string;
}

export interface GetKeyAccountGeneralReportsInput extends PagedAndSortedResultRequestDto {
  buyer?: string;
  taxCode?: string;
  keyAccountCode?: string;
  keyAccountName?: string;
  fromDate?: string;
  toDate?: string;
  invoiceFromDate?: string;
  invoiceToDate?: string;
  materialType?: string;
  materialGroup?: string;
}

export interface GetKeyAccountUpgradesInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  buyerId?: string;
  financeYear?: number;
  taxCode?: string;
  keyAccountCode?: string;
  keyAccountShortName?: string;
  keyAccountName?: string;
  keyAccountType?: string;
  keyAccountClass?: string;
  status?: string;
  materialType?: string;
  customerTaxCode?: string;
}

export interface GetKeyAccountsInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  buyerId?: string;
  taxCode?: string;
  keyAccountCode?: string;
  keyAccountShortName?: string;
  keyAccountName?: string;
  keyAccountType?: string;
  keyAccountClass?: string;
  status?: string;
  customerTaxCode?: string;
}

export interface KeyAccountCreateDto {
  buyerId: string;
  buyerShortName?: string;
  customerTaxCode: string;
  keyAccountCode: string;
  keyAccountShortName?: string;
  keyAccountName?: string;
  keyAccountType?: string;
  keyAccountClassBuyer?: string;
  keyAccountClass?: string;
  materialType?: string;
  customerName?: string;
  customerPhone?: string;
  registrationYear?: number;
  customerLocation: string;
  customerCountry?: string;
  customerProvince?: string;
  customerAddress?: string;
  customerWebsite?: string;
  industry?: string;
  targetEndUsers?: string;
  lastRegisteredProjectCode?: string;
  currentSaleRoute?: string;
  note?: string;
  keyAccountEvaluationFinancial: KeyAccountEvaluationDto[];
  keyAccountEvaluationProduct: KeyAccountEvaluationDto[];
}

export interface KeyAccountDetailReportDto {
  no: number;
  taxCode?: string;
  keyAccountCode?: string;
  keyAccountName?: string;
  golfaCode?: string;
  model?: string;
  dpoNo?: string;
  buyer?: string;
  materialType?: string;
  materialGroup?: string;
  dpoQty: number;
  dpoUnitPrice: number;
  dpoAmount: number;
  invoiceNo?: string;
  invoiceDate?: string;
  invoiceQty: number;
  amountDO: number;
  qtyDO: number;
  dpoDate?: string;
}

export interface KeyAccountDto extends ExtendedAuditedEntityDto<string> {
  buyerId?: string;
  buyerShortName?: string;
  customerTaxCode?: string;
  keyAccountCode?: string;
  keyAccountShortName?: string;
  keyAccountName?: string;
  keyAccountType?: string;
  keyAccountClassBuyer?: string;
  keyAccountClass?: string;
  materialType?: string;
  customerName?: string;
  customerPhone?: string;
  registrationYear?: number;
  customerLocation?: string;
  customerCountry?: string;
  customerProvince?: string;
  customerAddress?: string;
  customerWebsite?: string;
  industry?: string;
  targetEndUsers?: string;
  lastRegisteredProjectCode?: string;
  currentSaleRoute?: string;
  note?: string;
  isDeactive: boolean;
  status?: string;
  currentApprovalRouteInstanceId?: string;
  currentApproverRoleCode?: string;
  currentApproverRoleName?: string;
  currentApprovalStepSequence?: number;
  concurrencyStamp?: string;
  buyer: BuyerDto;
  customerPICs: CustomerPICDto[];
  keyAccountEvaluationFinancial: KeyAccountEvaluationDto[];
  keyAccountEvaluationProduct: KeyAccountEvaluationDto[];
  approvalHistories: ApprovalHistoryDto[];
  attachments: AttachmentDto[];
  flags: KeyAccountFlagDto;
}

export interface KeyAccountExcelDownloadDto {
  downloadToken?: string;
  filterText?: string;
  buyerId?: string;
  keyAccountCode?: string;
  keyAccountShortName?: string;
  keyAccountName?: string;
  keyAccountType?: string;
  keyAccountClass?: string;
  status?: string;
  customerTaxCode?: string;
}

export interface KeyAccountFlagDto {
  isEditable: boolean;
  isRemovable: boolean;
  isViewable: boolean;
  isSubmitable: boolean;
  isSavable: boolean;
  isAbleToDeleteAttachment: boolean;
  isAbleToAddAttachment: boolean;
  isApprovable: boolean;
  isRejectable: boolean;
  isCancellable: boolean;
}

export interface KeyAccountGeneralReportDto {
  no: number;
  taxCode?: string;
  keyAccountCode?: string;
  keyAccountName?: string;
  keyAccountClassName?: string;
  keyAccountTypeName?: string;
  materialType?: string;
  materialGroup?: string;
  dpoAmount: number;
  deliveredAmount: number;
  name?: string;
}

export interface KeyAccountListDto extends ExtendedAuditedEntityDto<string> {
  buyerId?: string;
  customerTaxCode?: string;
  keyAccountCode?: string;
  keyAccountShortName?: string;
  keyAccountName?: string;
  keyAccountType?: string;
  keyAccountClassBuyer?: string;
  keyAccountClass?: string;
  customerLocation?: string;
  status?: string;
  currentApprovalRouteInstanceId?: string;
  currentApproverRoleName?: string;
  currentApprovalStepSequence?: string;
  flags: KeyAccountFlagDto;
}

export interface KeyAccountUpdateDto {
  buyerId: string;
  buyerShortName?: string;
  customerTaxCode: string;
  keyAccountCode: string;
  keyAccountShortName?: string;
  keyAccountName?: string;
  keyAccountType?: string;
  keyAccountClassBuyer?: string;
  keyAccountClass?: string;
  materialType?: string;
  customerName?: string;
  customerPhone?: string;
  registrationYear?: number;
  customerLocation: string;
  customerCountry?: string;
  customerProvince?: string;
  customerAddress?: string;
  customerWebsite?: string;
  industry?: string;
  targetEndUsers?: string;
  lastRegisteredProjectCode?: string;
  currentSaleRoute?: string;
  note?: string;
  keyAccountEvaluationFinancial: KeyAccountEvaluationDto[];
  keyAccountEvaluationProduct: KeyAccountEvaluationDto[];
  concurrencyStamp?: string;
}

export interface KeyAccountUpgradeDto {
  fy?: string;
  ka_Id?: string;
  keyAccountCode?: string;
  keyAccountName?: string;
  customerTaxCode?: string;
  keyAccountType?: string;
  buyer?: string;
  materialType?: string;
  revenue: number;
  concurrencyStamp?: string;
  current_KAClass?: string;
  suggested_KAClass?: string;
  colorIndicator?: string;
  classificationValue?: string;
  keyAccountClass?: string;
}

export interface KeyAccountUpgradesInput {
  keyAccountClassCurrentCode?: string;
  keyAccountClassCode?: string;
  keyAccountClassName?: string;
  note?: string;
  concurrencyStamp?: string;
}

export interface KeyAccountWithNavigationListDto extends KeyAccountListDto {
  buyer: BuyerDto;
  approvalHistories: ApprovalHistoryDto[];
}
