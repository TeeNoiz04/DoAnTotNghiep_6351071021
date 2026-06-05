import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { ExtendedAuditedEntityDto } from '../shared/models';
import type { PSIDetailDto, PSIDetailImportDto } from './psidetails/models';
import type { ApprovalHistoryDto } from '../approval-histories/models';
import type { ExcelValidationResult } from '../shared/excels/models';

export interface GetPSIImportsInput {
  fy: number;
  note?: string;
  materialType?: string;
}

export interface GetPSIReportsInput extends PagedAndSortedResultRequestDto {
  fy?: number;
  userName?: string;
}

export interface GetPSIsInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  psiCode?: string;
  fy?: number;
  fileName?: string;
  note?: string;
  status?: string;
  materialType?: string;
}

export interface PSIByProductExportInput extends PagedAndSortedResultRequestDto {
  materialType: string;
  fiscalYear: string;
}

export interface PSICreateDto {
  psiCode: string;
  fy?: number;
  fileName?: string;
  materialType?: string;
  importType?: string;
  note?: string;
}

export interface PSIDto extends ExtendedAuditedEntityDto<string> {
  psiCode?: string;
  fy?: number;
  fileName?: string;
  importType?: string;
  note?: string;
  status?: string;
  materialType?: string;
  currentApprovalRouteInstanceId?: string;
  currentApprovalStepSequence?: string;
  currentApproverRoleName?: string;
  details: PSIDetailDto[];
  approvalHistories: PSIHistoryDto[];
  flags: PSIFlagsDto;
  concurrencyStamp?: string;
}

export interface PSIExcelDownloadDto {
  downloadToken?: string;
  filterText?: string;
  psiCode?: string;
  fy?: number;
  fileName?: string;
  note?: string;
  status?: string;
}

export interface PSIExportDataDto {
  materialGroupPSI?: string;
  description?: string;
  planApril?: number;
  resultApril?: number;
  planMay?: number;
  resultMay?: number;
  planJune?: number;
  resultJune?: number;
  planJuly?: number;
  resultJuly?: number;
  planAugust?: number;
  resultAugust?: number;
  planSeptember?: number;
  resultSeptember?: number;
  planOctober?: number;
  resultOctober?: number;
  planNovember?: number;
  resultNovember?: number;
  planDecember?: number;
  resultDecember?: number;
  planJan?: number;
  resultJan?: number;
  planFeb?: number;
  resultFeb?: number;
  planMarch?: number;
  resultMarch?: number;
}

export interface PSIFlagsDto {
  isEditable: boolean;
  isRemovable: boolean;
  isViewable: boolean;
  isApprovable: boolean;
  isRejectable: boolean;
  isCancellable: boolean;
}

export interface PSIHistoryDto extends ApprovalHistoryDto {
  psi_Id?: string;
}

export interface PSIImportDto {
  psiCode?: string;
  fy?: number;
  fileName?: string;
  note?: string;
  importType?: string;
  materialType?: string;
  details: ExcelValidationResult<PSIDetailImportDto>;
}

export interface PSIReportDto {
  materialGroup?: string;
  description?: string;
  planApril?: number;
  planMay?: number;
  planJune?: number;
  planJuly?: number;
  planAugust?: number;
  planSeptember?: number;
  planOctober?: number;
  planNovember?: number;
  planDecember?: number;
  planJan?: number;
  planFeb?: number;
  planMarch?: number;
  actualApril?: number;
  actualMay?: number;
  actualJune?: number;
  actualJuly?: number;
  actualAugust?: number;
  actualSeptember?: number;
  actualOctober?: number;
  actualNovember?: number;
  actualDecember?: number;
  actualJan?: number;
  actualFeb?: number;
  actualMarch?: number;
}

export interface PSIUpdateDto {
  fileName?: string;
  importType?: string;
  materialType?: string;
  note?: string;
  concurrencyStamp?: string;
}
