import type { ExtendedAuditedEntityDto } from '../shared/models';

export interface ApprovalHistoryDto extends ExtendedAuditedEntityDto<string> {
  entityType?: string;
  approverRoleCode?: string;
  approverRoleName?: string;
  approverUsername?: string;
  approverFullName?: string;
  action?: string;
  actionDate?: string;
  note?: string;
  isLastApprovalInCurrentWorkflow: boolean;
  concurrencyStamp?: string;
}
