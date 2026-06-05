import type { ExtendedAuditedEntityDto } from '../shared/models';
import type { EntityDto } from '@abp/ng.core';

export interface ApprovalRouteDto extends ExtendedAuditedEntityDto<string> {
  entityId?: string;
  entityType?: string;
  instanceId?: string;
  stepSequence: number;
  approver?: string;
  approverRoleCode?: string;
  approverRoleName?: string;
  approvalDate?: string;
  notes?: string;
  isApproved: boolean;
  concurrencyStamp?: string;
}

export interface ApproverDto extends EntityDto<string> {
  fullName?: string;
  username?: string;
  roleCode?: string;
  roleName?: string;
}
