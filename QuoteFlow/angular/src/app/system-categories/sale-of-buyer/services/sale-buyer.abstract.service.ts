import { ABP, AbpWindowService, ListService, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import { inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import {
  GetSalesAssignmentInput,
  SalesAssignmentDto,
  SalesAssignmentService,
} from '@proxy/sales-assignments';
import { Subscription } from 'rxjs';
import { filter, switchMap } from 'rxjs/operators';
import { BuyerCategoryFilterHelper } from './buyer-category-filter-helper';

export abstract class AbstractSaleBuyerViewService {
  protected readonly proxyService = inject(SalesAssignmentService);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly list = inject(ListService);
  protected readonly abpWindowService = inject(AbpWindowService);
  protected readonly router = inject(Router);
  protected readonly route = inject(ActivatedRoute);
  protected readonly loadingService = inject(LoadingService);

  private currentSubscription?: Subscription;

  isExportToExcelBusy = false;

  data: PagedResultDto<SalesAssignmentDto> = {
    items: [],
    totalCount: 0,
  };

  filters = {} as GetSalesAssignmentInput;

  delete(record: SalesAssignmentDto) {
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
        filterText: this.filters.filterText,
      });

    const setData = (list: PagedResultDto<SalesAssignmentDto>) => {
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

  syncFromQueryParams(): void {
    const queryParams = this.route.snapshot.queryParams;
    this.filters = BuyerCategoryFilterHelper.fromQueryParams(queryParams);
  }

  updateQueryParams(replaceUrl = false): void {
    const queryParams = BuyerCategoryFilterHelper.toQueryParams(this.filters);
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams,
      queryParamsHandling: replaceUrl ? undefined : 'merge',
    });
  }

  clearFilters() {
    this.filters = {
      maxResultCount: DEFAULT_PAGE_SIZE,
    } as GetSalesAssignmentInput;
    this.updateQueryParams(true); // Pass true to replace all query params
    this.list.get();
  }
}
