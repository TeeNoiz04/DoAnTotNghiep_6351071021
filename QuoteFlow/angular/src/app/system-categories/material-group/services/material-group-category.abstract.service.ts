import { ABP, AbpWindowService, ListService, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import { inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { MaterialGroupService } from '@proxy/material-groups';
import { GetMaterialGroupsInput, MaterialGroupDto } from '@proxy/materials/material-groups';
import { Subscription } from 'rxjs';
import { filter, switchMap } from 'rxjs/operators';

export abstract class AbstractMaterialGroupViewService {
  protected readonly proxyService = inject(MaterialGroupService);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly list = inject(ListService);
  protected readonly abpWindowService = inject(AbpWindowService);
  protected readonly router = inject(Router);
  protected readonly route = inject(ActivatedRoute);
  protected readonly loadingService = inject(LoadingService);

  private currentSubscription?: Subscription;
  isExportToExcelBusy = false;
  filters = {
    isDeActive: false,
    maxResultCount: DEFAULT_PAGE_SIZE,
  } as GetMaterialGroupsInput;

  data: PagedResultDto<MaterialGroupDto> = {
    items: [],
    totalCount: 0,
  };

  delete(record: MaterialGroupDto) {
    this.confirmationService
      .warn('::DeleteConfirmationMessage', '::AreYouSure', { messageLocalizationParams: [] })
      .pipe(
        filter(status => status === Confirmation.Status.confirm),
        switchMap(() => this.proxyService.delete(record.id)),
      )
      .subscribe(this.list.get);
  }

  hookToQuery() {
    if (this.filters.isDeActive === undefined) {
      this.filters.isDeActive = false;
    }

    const getData = (query: ABP.PageQueryParams) =>
      this.proxyService.getList({
        ...query,
        ...this.filters,
        filterText: this.filters.filterText,
      });

    const setData = (list: PagedResultDto<MaterialGroupDto>) => {
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
      isDeActive: false,
      maxResultCount: DEFAULT_PAGE_SIZE,
    } as GetMaterialGroupsInput;
    this.list.get();
  }
}
