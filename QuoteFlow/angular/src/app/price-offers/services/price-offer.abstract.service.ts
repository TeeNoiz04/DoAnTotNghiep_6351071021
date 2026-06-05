import {
  ABP,
  AbpWindowService,
  ListService,
  PagedResultDto,
  PermissionService,
} from '@abp/ng.core';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import { inject } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { filter, finalize, Observable, Subscription, switchMap, tap } from 'rxjs';
import type {
  GetPriceOffersInput,
  PriceOfferDto,
  PriceOfferListDto,
} from '../../proxy/price-offers/models';
import { PriceOfferService } from '../../proxy/price-offers/price-offer.service';
import { PriceOfferFilterHelper } from './price-offer-filter-helper';
import { AddMoreItemHistoryDto } from '@proxy/add-more-item-histories';
import { LookupService } from '@proxy/general-lookups';

export abstract class AbstractPriceOfferViewService {
  protected readonly proxyService = inject(PriceOfferService);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly list = inject(ListService);
  protected readonly abpWindowService = inject(AbpWindowService);
  protected readonly fb = inject(FormBuilder);
  protected readonly loadingService = inject(LoadingService);
  protected readonly router = inject(Router);
  protected readonly route = inject(ActivatedRoute);
  protected readonly permissionSerivce = inject(PermissionService);
  protected readonly lookupService = inject(LookupService);
  private currentSubscription?: Subscription;
  isExportToExcelBusy = false;

  data: PagedResultDto<PriceOfferListDto> = {
    items: [],
    totalCount: 0,
  };

  filters: GetPriceOffersInput = {
    maxResultCount: DEFAULT_PAGE_SIZE,
    relatedToMe: false,
  };

  delete(record: PriceOfferDto) {
    this.confirmationService
      .warn('::DeleteConfirmationMessage', '::AreYouSure', { messageLocalizationParams: [] })
      .pipe(
        filter(status => status === Confirmation.Status.confirm),
        switchMap(() => this.proxyService.delete(record.id)),
      )
      .subscribe(this.list.get);
  }

  hookToQuery(isApprovalView: boolean = false) {
    // Sync from query params first
    this.syncFromQueryParams();

    if (!isApprovalView) {
      const getData = (query: ABP.PageQueryParams) =>
        this.proxyService.getList({
          ...query,
          ...this.filters,
          filterText: this.filters.filterText,
          maxResultCount: DEFAULT_PAGE_SIZE,
        });

      const setData = (list: PagedResultDto<PriceOfferListDto>) => {
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
    } else {
      const getData = (query: ABP.PageQueryParams) =>
        this.proxyService.getMyApprovalsList({
          ...query,
          ...this.filters,
          filterText: this.filters.filterText,
          maxResultCount: DEFAULT_PAGE_SIZE,
        });

      const setData = (list: PagedResultDto<PriceOfferListDto>) => {
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
  }

  syncFromQueryParams(): void {
    const queryParams = this.route.snapshot.queryParams;
    // Use the filter helper to map query parameters to filters
    this.filters = PriceOfferFilterHelper.fromQueryParams(queryParams);
  }

  updateQueryParams(replaceUrl = false): void {
    // Use the filter helper to convert filters to query parameters
    const queryParams = PriceOfferFilterHelper.toQueryParams(this.filters);

    this.router.navigate([], {
      relativeTo: this.route,
      queryParams,
      queryParamsHandling: replaceUrl ? undefined : 'merge',
    });
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
        this.abpWindowService.downloadBlob(result, 'PriceOffer.xlsx');
      });
  }

  getListApprovers(priceOfferId: string) {
    return this.proxyService.getListApprovers(priceOfferId);
  }

  addMoreItemHistories: AddMoreItemHistoryDto[] = [];
  loadAddMoreItemHistories(id: string): Observable<any> {
    return this.lookupService.addMoreItemHistory(id).pipe(
      finalize(() => {}),
      tap(result => {
        this.addMoreItemHistories = result;
      }),
    );
  }
}
