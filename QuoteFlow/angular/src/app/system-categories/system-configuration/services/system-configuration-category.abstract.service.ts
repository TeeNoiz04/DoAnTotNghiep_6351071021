import { inject } from '@angular/core';
import { ConfirmationService } from '@abp/ng.theme.shared';
import { ABP, AbpWindowService, ListService, PagedResultDto } from '@abp/ng.core';
import { ActivatedRoute, Router } from '@angular/router';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { Subscription } from 'rxjs';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import {
  GetSystemConfigurationsInput,
  SystemConfigurationDto,
  SystemConfigurationService,
} from '@proxy/system-configurations';

export abstract class AbstractSystemConfigurationViewService {
  protected readonly proxyService = inject(SystemConfigurationService);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly list = inject(ListService);
  protected readonly abpWindowService = inject(AbpWindowService);
  protected readonly router = inject(Router);
  protected readonly route = inject(ActivatedRoute);
  protected readonly loadingService = inject(LoadingService);

  private currentSubscription?: Subscription;

  filters = {
    isSystemCfg: false,
    maxResultCount: DEFAULT_PAGE_SIZE,
  } as GetSystemConfigurationsInput;

  data: PagedResultDto<SystemConfigurationDto> = {
    items: [],
    totalCount: 0,
  };

  hookToQuery() {
    const getData = (query: ABP.PageQueryParams) =>
      this.proxyService.getList({
        ...query,
        ...this.filters,
        filterText: this.filters.filterText,
        cfgType: 'Parametters',
      });

    const setData = (list: PagedResultDto<SystemConfigurationDto>) => {
      this.data = list;
    };

    if (!this.currentSubscription) {
      this.currentSubscription = this.list.hookToQuery(getData).subscribe({
        next: res => {
          setData(res);
        },
        error: () => this.loadingService.hide(),
      });
    } else {
      this.currentSubscription.unsubscribe();
      this.currentSubscription = this.list.hookToQuery(getData).subscribe({
        next: res => {
          setData(res);
        },
        error: () => this.loadingService.hide(),
      });
    }
  }

  clearFilters() {
    this.filters = {
      isSystemCfg: false,
      maxResultCount: DEFAULT_PAGE_SIZE,
    } as GetSystemConfigurationsInput;
    this.list.get();
  }
}
