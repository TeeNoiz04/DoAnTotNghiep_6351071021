import { ABP, AbpWindowService, ListService, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import { inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { BuyerDto, BuyerService, GetBuyersInput } from '@proxy/buyers';
import { Subscription } from 'rxjs';
import { filter, finalize, switchMap } from 'rxjs/operators';
import { BuyerCategoryFilterHelper } from './buyer-category-filter-helper';
import { GetPriceFromOptions } from './get-price-from';

export abstract class AbstractBuyerCategoryViewService {
  protected readonly proxyService = inject(BuyerService);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly list = inject(ListService);
  protected readonly abpWindowService = inject(AbpWindowService);
  protected readonly router = inject(Router);
  protected readonly route = inject(ActivatedRoute);
  protected readonly loadingService = inject(LoadingService);

  private currentSubscription?: Subscription;

  isExportToExcelBusy = false;
  public getPriceFromOptions = GetPriceFromOptions;

  data: PagedResultDto<BuyerDto> = {
    items: [],
    totalCount: 0,
  };

  filters = {
    deactive: false,
  } as GetBuyersInput;

  delete(record: BuyerDto) {
    this.confirmationService
      .warn('::DeleteConfirmationMessage', '::AreYouSure', { messageLocalizationParams: [] })
      .pipe(
        filter(status => status === Confirmation.Status.confirm),
        switchMap(() => this.proxyService.delete(record.id)),
      )
      .subscribe(this.list.get);
  }

  hookToQuery() {
    // Sync from query params first
    this.syncFromQueryParams();
    if (this.filters.deactive === undefined) {
      this.filters.deactive = false;
    }

    const getData = (query: ABP.PageQueryParams) =>
      this.proxyService.getList({
        ...query,
        ...this.filters,
        filterText: this.filters.filterText,
      });

    const setData = (list: PagedResultDto<BuyerDto>) => {
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
    //this.filters = BuyerCategoryFilterHelper.fromQueryParams(queryParams);
  }

  updateQueryParams(replaceUrl = false): void {
    // const queryParams = BuyerCategoryFilterHelper.toQueryParams(this.filters);
    // this.router.navigate([], {
    //   relativeTo: this.route,
    //   queryParams,
    //   queryParamsHandling: replaceUrl ? undefined : 'merge',
    // });
  }

  clearFilters() {
    this.filters = {
      isDeactive: false,
      maxResultCount: DEFAULT_PAGE_SIZE,
    } as GetBuyersInput;
    this.updateQueryParams(true); // Pass true to replace all query params
    this.list.get();
  }

  exportToExcel() {
    // this.isExportToExcelBusy = true;
    // this.proxyService
    //   .getDownloadToken()
    //   .pipe(
    //     switchMap(({ token }) =>
    //       this.proxyService.getListAsExcelFile({
    //         downloadToken: token,
    //         filterText: this.list.filter,
    //         ...this.filters,
    //       }),
    //     ),
    //     finalize(() => (this.isExportToExcelBusy = false)),
    //   )
    //   .subscribe(result => {
    //     this.abpWindowService.downloadBlob(result, 'Buyer.xlsx');
    //   });
  }

  getPriceFromLabel(value: number): string {
    const option = GetPriceFromOptions.find(x => x.value === value);
    return option ? option.label : '';
  }
}
