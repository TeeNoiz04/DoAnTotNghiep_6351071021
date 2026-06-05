import { ABP } from '@abp/ng.core';

export interface ListStatePreservation {
  queryParams?: ABP.PageQueryParams;
  advancedFilters?: any; // For any additional complex filtering
}

export interface RequestListStatePreservation extends ListStatePreservation {
  filters?: any;
}
