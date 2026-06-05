import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { ExtendedAuditedEntityDto } from '../shared/models';

export interface GetWorkflowApproversInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  wfId?: string;
  approver?: string;
  note?: string;
}

export interface WorkflowApproverCreateDto {
  wfId: string;
  approver: string;
  note?: string;
}

export interface WorkflowApproverDto extends ExtendedAuditedEntityDto<string> {
  wfId?: string;
  approver?: string;
  note?: string;
  concurrencyStamp?: string;
}

export interface WorkflowApproverUpdateDto {
  wfId: string;
  approver: string;
}
