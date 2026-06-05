import { ABP, ListService, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import { inject, Injectable } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DPOService } from '@app/proxy/dpos';
import { DPODto, GetDPOsInput } from '@app/proxy/dpos/models';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { filter, finalize, Subscription, switchMap, tap } from 'rxjs';
import { DpoFilterHelper } from './dpo-filter-helper';
import { TemplateService } from '@proxy/general-templates';
import { LookupService } from '@proxy/general-lookups';
import { ApprovalHistoryDto } from '@proxy/approval-histories';

@Injectable()
export class DPOViewService {
  protected readonly proxyService = inject(DPOService);
  protected readonly list = inject(ListService);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly loadingService = inject(LoadingService);
  protected readonly router = inject(Router);
  protected readonly route = inject(ActivatedRoute);
  protected readonly proxyTemplateService = inject(TemplateService);
  protected readonly proxyLookupService = inject(LookupService);

  private currentSubscription?: Subscription;
  isExportToExcelBusy = false;

  filters: GetDPOsInput = {
    maxResultCount: DEFAULT_PAGE_SIZE,
  };

  data: PagedResultDto<DPODto> = {
    items: [],
    totalCount: 0,
  };
  delete(record: DPODto) {
    this.confirmationService
      .warn('::DeleteConfirmationMessage', '::AreYouSure', {
        messageLocalizationParams: [record.dpoNo],
      })
      .pipe(
        filter(status => status === Confirmation.Status.confirm),
        switchMap(() => this.proxyService.delete(record.id)),
      )
      .subscribe(this.list.get);
  }
  historyDPO: ApprovalHistoryDto[] = [];
  viewHistory(record: DPODto) {
    this.proxyLookupService.getDPOApprovalHistories(record.id).subscribe(history => {
      // Handle the retrieved history data as needed
      this.historyDPO = history;
    });
  }

  hookToQuery() {
    // Sync from query params first
    this.syncFromQueryParams();
    const getData = (query: ABP.PageQueryParams) =>
      this.proxyService.getList({
        ...query,
        ...this.filters,
        filterText: this.filters.filterText,
        maxResultCount: DEFAULT_PAGE_SIZE,
      });

    const setData = (list: PagedResultDto<DPODto>) => {
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
    // Use the filter helper to map query parameters to filters
    this.filters = DpoFilterHelper.fromQueryParams(queryParams);
  }

  updateQueryParams(replaceUrl = false): void {
    // Use the filter helper to convert filters to query parameters
    const queryParams = DpoFilterHelper.toQueryParams(this.filters);

    this.router.navigate([], {
      relativeTo: this.route,
      queryParams,
      queryParamsHandling: replaceUrl ? undefined : 'merge',
    });
  }

  clearFilters() {
    this.filters = { maxResultCount: DEFAULT_PAGE_SIZE };
  }

  exportToExcel() {
    this.isExportToExcelBusy = true;
    this.loadingService.show();

    return this.proxyService.getDownloadToken().pipe(
      tap(({ token }) => {
        const params = {
          downloadToken: token,
          ...this.filters,
        };
        this.proxyService.getListAsExcelFile(params).subscribe({
          next: (response: Blob) => {
            const link = document.createElement('a');
            link.href = window.URL.createObjectURL(response);
            link.download = `DPO_${new Date().toISOString().slice(0, 10)}.xlsx`;
            link.click();
            window.URL.revokeObjectURL(link.href);
          },
          error: error => {
            console.error('Download failed:', error);
          },
          complete: () => {
            this.isExportToExcelBusy = false;
            this.loadingService.hide();
          },
        });
      }),
      finalize(() => {
        this.isExportToExcelBusy = false;
        this.loadingService.hide();
      }),
    );
  }
  downloadTemplate() {
    return this.proxyTemplateService.getDPOTemplate().subscribe({
      next: (blob: Blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `Template_DPO.xlsx`;
        a.click();
        window.URL.revokeObjectURL(url);
      },
      error: err => {
        console.error('Error downloading template:', err);
      },
    });
  }
}
