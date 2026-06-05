import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { ExtendedAuditedEntityDto } from '../shared/models';

export interface GetSystemConfigurationsInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  cfgKey?: string;
  cfgValue?: string;
  description?: string;
  isSystemCfg?: boolean;
  cfgType?: string;
}

export interface SystemConfigurationCreateDto {
  cfgKey: string;
  cfgValue: string;
  description?: string;
  cfgType?: string;
  isSystemCfg: boolean;
}

export interface SystemConfigurationDto extends ExtendedAuditedEntityDto<string> {
  cfgKey?: string;
  cfgValue?: string;
  description?: string;
  isSystemCfg: boolean;
  cfgType?: string;
}

export interface SystemConfigurationUpdateDto {
  cfgKey: string;
  cfgValue: string;
  description?: string;
  isSystemCfg: boolean;
  cfgType?: string;
}
