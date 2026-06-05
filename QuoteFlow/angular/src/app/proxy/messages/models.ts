import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { ExtendedFullAuditedEntityDto } from '../shared/models';

export interface GetMessagesInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  userName?: string;
  fullName?: string;
  sendTo?: string;
  comment?: string;
}

export interface MessageCreateDto {
  userName: string;
  fullName: string;
  sendToEmails: string[];
  comment: string;
}

export interface MessageDto extends ExtendedFullAuditedEntityDto<string> {
  userName?: string;
  fullName?: string;
  sendTo?: string;
  comment?: string;
  concurrencyStamp?: string;
}
