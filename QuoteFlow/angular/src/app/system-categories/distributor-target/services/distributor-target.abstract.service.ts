import { ABP, AbpWindowService, ListService, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import { inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { Subscription } from 'rxjs';
import { filter, switchMap, tap } from 'rxjs/operators';
import { BuyerCategoryFilterHelper } from './distributor-target-filter-helper';
import {
  DistributorTargetDto,
  DistributorTargetService,
  GetDistributorTargetsInput,
} from '@proxy/distributor-targets';
import { LookupService } from '@proxy/general-lookups';

export abstract class AbstractDistributorTargetViewService {
  protected readonly proxyService = inject(DistributorTargetService);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly list = inject(ListService);
  protected readonly abpWindowService = inject(AbpWindowService);
  protected readonly router = inject(Router);
  protected readonly route = inject(ActivatedRoute);
  protected readonly loadingService = inject(LoadingService);
  public readonly lookupService = inject(LookupService);
  private currentSubscription?: Subscription;

  isExportToExcelBusy = false;
  fiscalYears: number[] = [];
  data: PagedResultDto<DistributorTargetDto> = {
    items: [],
    totalCount: 0,
  };

  filters = {} as GetDistributorTargetsInput;

  delete(record: DistributorTargetDto) {
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

    const setData = (list: PagedResultDto<DistributorTargetDto>) => {
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
    const targetYear =
      new Date().getMonth() < 3 ? new Date().getFullYear() - 1 : new Date().getFullYear();

    const defaultYear =
      this.fiscalYears && this.fiscalYears.includes(targetYear)
        ? targetYear
        : this.fiscalYears && this.fiscalYears.length > 0
          ? this.fiscalYears[0]
          : targetYear;

    this.filters = {
      maxResultCount: DEFAULT_PAGE_SIZE,
      financeYearMax: defaultYear,
      materialType: 'FA',
    } as GetDistributorTargetsInput;

    this.list.get();
  }
  generateFiscalYears() {
    return this.lookupService.getFiscalYearOfDistributorTargetLookup().pipe(
      tap(result => {
        this.fiscalYears = result?.items || [];
      }),
    );
  }
}
