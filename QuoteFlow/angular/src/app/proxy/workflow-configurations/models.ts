import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { ExtendedAuditedEntityDto } from '../shared/models';
import type { WorkflowApproverDto, WorkflowApproverUpdateDto } from '../workflow-approvers/models';

export interface GetWorkflowConfigurationsInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  workflowType?: string;
  workflowLevel?: number;
  workflowLevelMin?: number;
  workflowLevelMax?: number;
  workflowRole?: string;
  condition?: string;
  note?: string;
}

export interface WorkflowConfigurationDto extends ExtendedAuditedEntityDto<string> {
  workflowType?: string;
  workflowLevel: number;
  workflowRole?: string;
  condition?: string;
  note?: string;
  approvers?: string;
  workflowApprovers: WorkflowApproverDto[];
  concurrencyStamp?: string;
}

export interface WorkflowConfigurationUpdateDto {
  workflowRole: string;
  note?: string;
  approvers: WorkflowApproverUpdateDto[];
  concurrencyStamp?: string;
}
