import { inject } from '@angular/core';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import { ABP, AbpWindowService, ListService, PagedResultDto } from '@abp/ng.core';
import { filter, switchMap, finalize } from 'rxjs/operators';
import type {
  GetSystemCategoriesInput,
  SystemCategoryDto,
} from '../../../proxy/system-categories/models';
import { SystemCategoryService } from '../../../proxy/system-categories/system-category.service';
import { ActivatedRoute, Router } from '@angular/router';
import { SystemCategoryFilterHelper } from './system-category-filer-helper';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { Subscription } from 'rxjs';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { CategoryTypes } from '@app/system-categories/system-category.model';

export abstract class AbstractSystemCategoryViewService {
  protected readonly proxyService = inject(SystemCategoryService);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly list = inject(ListService);
  protected readonly abpWindowService = inject(AbpWindowService);
  protected readonly router = inject(Router);
  protected readonly route = inject(ActivatedRoute);
  protected readonly loadingService = inject(LoadingService);

  private currentSubscription?: Subscription;
  isExportToExcelBusy = false;
  filters = {
    isDeactive: false,
  } as GetSystemCategoriesInput;

  data: PagedResultDto<SystemCategoryDto> = {
    items: [],
    totalCount: 0,
  };

  delete(record: SystemCategoryDto) {
    this.confirmationService
      .warn('::DeleteConfirmationMessage', '::AreYouSure', { messageLocalizationParams: [] })
      .pipe(
        filter(status => status === Confirmation.Status.confirm),
        switchMap(() => this.proxyService.delete(record.id)),
      )
      .subscribe(this.list.get);
  }

  hookToQuery(type?: CategoryTypes) {
    // Sync from query params first
    this.syncFromQueryParams();
    if (this.filters.isDeactive === undefined) {
      this.filters.isDeactive = false;
    }

    this.filters.categoryType = type;

    const getData = (query: ABP.PageQueryParams) =>
      this.proxyService.getList({
        ...query,
        ...this.filters,
        filterText: this.filters.filterText,
      });

    const setData = (list: PagedResultDto<SystemCategoryDto>) => {
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
    this.filters = SystemCategoryFilterHelper.fromQueryParams(queryParams);
  }

  updateQueryParams(replaceUrl = false): void {
    const queryParams = SystemCategoryFilterHelper.toQueryParams(this.filters);
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams,
      queryParamsHandling: replaceUrl ? undefined : 'merge',
    });
  }

  clearFilters(type?: CategoryTypes) {
    this.filters = {
      categoryType: type,
      isDeactive: false,
      maxResultCount: DEFAULT_PAGE_SIZE,
    } as GetSystemCategoriesInput;
    this.updateQueryParams(true); // Pass true to replace all query params
    this.list.get();
  }

  exportToExcel() {
    this.isExportToExcelBusy = true;
    this.proxyService
      .getDownloadToken()
      .pipe(
        switchMap(({ token }) =>
          this.proxyService.getListAsExcelFile({
            downloadToken: token,
            filterText: this.list.filter,
            ...this.filters,
          }),
        ),
        finalize(() => (this.isExportToExcelBusy = false)),
      )
      .subscribe(result => {
        this.abpWindowService.downloadBlob(result, 'SystemCategory.xlsx');
      });
  }
}
