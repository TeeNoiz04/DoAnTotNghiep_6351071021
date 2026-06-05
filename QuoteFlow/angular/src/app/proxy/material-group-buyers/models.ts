import type { PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { ExtendedAuditedEntityDto } from '../shared/models';
import type { MaterialGroupDto } from '../materials/material-groups/models';
import type { BuyerDto } from '../buyers/models';

export interface GetMaterialGroupBuyersInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  materialGroupId?: string;
  materialGroupCode?: string;
  buyerId?: string;
  buyerShortName?: string;
  note?: string;
}

export interface MaterialGroupBuyerCreatesDto {
  buyerId: string;
  buyerShortName?: string;
  materialGroups: MaterialGroupOfBuyerCreateDto[];
  note?: string;
}

export interface MaterialGroupBuyerDto extends ExtendedAuditedEntityDto<string> {
  materialGroupId?: string;
  materialGroupCode?: string;
  buyerId?: string;
  buyerShortName?: string;
  note?: string;
  materialGroup: MaterialGroupDto;
  buyer: BuyerDto;
  concurrencyStamp?: string;
}

export interface MaterialGroupOfBuyerCreateDto {
  materialGroupId?: string;
  materialGroupCode?: string;
}
