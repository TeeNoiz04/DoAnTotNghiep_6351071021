import type { ExtendedAuditedEntityDto } from '../shared/models';

export interface CustomerPICCreateDto {
  keyAccountId: string;
  picName?: string;
  picPhone?: string;
  picEmail?: string;
  picJobTitle?: string;
  remark?: string;
}

export interface CustomerPICDto extends ExtendedAuditedEntityDto<string> {
  keyAccountId?: string;
  picName?: string;
  picPhone?: string;
  picEmail?: string;
  picJobTitle?: string;
  remark?: string;
  concurrencyStamp?: string;
}

export interface CustomerPICUpdateDto {
  keyAccountId: string;
  picName?: string;
  picPhone?: string;
  picEmail?: string;
  picJobTitle?: string;
  remark?: string;
  concurrencyStamp?: string;
}
