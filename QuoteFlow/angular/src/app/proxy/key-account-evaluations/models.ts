import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { ExtendedAuditedEntityDto } from '../shared/models';

export interface GetKeyAccountEvaluationsInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  keyAccountId?: string;
  evaluationType?: string;
  evaluationId?: string;
  buyerInfo1?: string;
  buyerInfo2?: string;
  mevnInfo1?: string;
  mevnInfo2?: string;
  competitorInfo1?: string;
  competitorInfo2?: string;
  note?: string;
}

export interface KeyAccountEvaluationCreateDto {
  keyAccountId: string;
  evaluationType: string;
  evaluationId: string;
  buyerInfo1?: string;
  buyerInfo2?: string;
  mevnInfo1?: string;
  mevnInfo2?: string;
  competitorInfo1?: string;
  competitorInfo2?: string;
  note?: string;
}

export interface KeyAccountEvaluationDto extends ExtendedAuditedEntityDto<string> {
  keyAccountId?: string;
  evaluationType?: string;
  evaluationId?: string;
  buyerInfo1?: string;
  buyerInfo2?: string;
  mevnInfo1?: string;
  mevnInfo2?: string;
  competitorInfo1?: string;
  competitorInfo2?: string;
  note?: string;
  concurrencyStamp?: string;
}

export interface KeyAccountEvaluationExcelDownloadDto {
  downloadToken?: string;
  filterText?: string;
  keyAccountId?: string;
  evaluationType?: string;
  evaluationId?: string;
  buyerInfo1?: string;
  buyerInfo2?: string;
  mevnInfo1?: string;
  mevnInfo2?: string;
  competitorInfo1?: string;
  competitorInfo2?: string;
  note?: string;
}

export interface KeyAccountEvaluationUpdateDto {
  keyAccountId: string;
  evaluationType: string;
  evaluationId: string;
  buyerInfo1?: string;
  buyerInfo2?: string;
  mevnInfo1?: string;
  mevnInfo2?: string;
  competitorInfo1?: string;
  competitorInfo2?: string;
  note?: string;
  concurrencyStamp?: string;
}
