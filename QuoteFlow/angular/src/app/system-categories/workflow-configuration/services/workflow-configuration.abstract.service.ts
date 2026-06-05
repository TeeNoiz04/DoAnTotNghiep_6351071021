import { inject } from '@angular/core';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import { ABP, AbpWindowService, ListService, PagedResultDto } from '@abp/ng.core';
import { filter, switchMap } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { Subscription } from 'rxjs';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import {
  GetWorkflowConfigurationsInput,
  WorkflowConfigurationDto,
  WorkflowConfigurationService,
} from '@proxy/workflow-configurations';

export abstract class AbstractWorkflowConfigurationViewService {
  protected readonly proxyService = inject(WorkflowConfigurationService);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly list = inject(ListService);
  protected readonly abpWindowService = inject(AbpWindowService);
  protected readonly router = inject(Router);
  protected readonly route = inject(ActivatedRoute);
  protected readonly loadingService = inject(LoadingService);

  private currentSubscription?: Subscription;
  isExportToExcelBusy = false;
  filters = {
    maxResultCount: DEFAULT_PAGE_SIZE,
  } as GetWorkflowConfigurationsInput;

  data: PagedResultDto<WorkflowConfigurationDto> = {
    items: [],
    totalCount: 0,
  };

  delete(record: WorkflowConfigurationDto) {
    this.confirmationService
      .warn('::DeleteConfirmationMessage', '::AreYouSure', { messageLocalizationParams: [] })
      .pipe(
        filter(status => status === Confirmation.Status.confirm),
        switchMap(() => this.proxyService.delete(record.id)),
      )
      .subscribe(this.list.get);
  }

  hookToQuery() {
    const getData = (query: ABP.PageQueryParams) =>
      this.proxyService.getList({
        ...query,
        ...this.filters,
      });

    const setData = (list: PagedResultDto<WorkflowConfigurationDto>) => {
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
      maxResultCount: DEFAULT_PAGE_SIZE,
    } as GetWorkflowConfigurationsInput;
    this.list.get();
  }
}
