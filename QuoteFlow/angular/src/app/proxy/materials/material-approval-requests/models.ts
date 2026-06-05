import type { ExtendedAuditedEntityDto } from '../../shared/models';
import type { MaterialApprovalRequestDetailDto } from '../material-approval-request-details/models';
import type { MaterialFlagsDto } from '../models';
import type { ApprovalHistoryDto } from '../../approval-histories/models';
import type { ApprovalRouteDto } from '../../approval-routes/models';

export interface MaterialApprovalRequestDto extends ExtendedAuditedEntityDto<string> {
  importType?: string;
  fileName?: string;
  note?: string;
  status?: string;
  requestNo?: string;
  currentApprovalRouteInstanceId?: string;
  currentApprovalStepSequence?: number;
  currentApproverRoleCode?: string;
  currentApproverRoleName?: string;
  materialApprovalDetails: MaterialApprovalRequestDetailDto[];
  materialHistories: MaterialApprovalRequestHistoryDto[];
  materialRoutes: MaterialApprovalRequestRouteDto[];
  flags: MaterialFlagsDto;
  concurrencyStamp?: string;
}

export interface MaterialApprovalRequestHistoryDto extends ApprovalHistoryDto {
  materialApprovalRequestId?: string;
}

export interface MaterialApprovalRequestRouteDto extends ApprovalRouteDto {
  materialApprovalRequestId?: string;
}
