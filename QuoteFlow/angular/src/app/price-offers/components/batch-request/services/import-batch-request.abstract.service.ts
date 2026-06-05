import { ABP, ListService, PagedResultDto } from '@abp/ng.core';
import { inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import {
  GetSpoBatchRequestsInput,
  SpoBatchRequestDto,
  SpoBatchRequestService,
} from '@proxy/spo-batch-requests';
import { Subscription } from 'rxjs';

export abstract class AbstractImportBatchRequestViewService {
  protected readonly proxyService = inject(SpoBatchRequestService);
  protected readonly list = inject(ListService);
  protected readonly router = inject(Router);
  protected readonly route = inject(ActivatedRoute);
  protected readonly loadingService = inject(LoadingService);

  private currentSubscription?: Subscription;

  isExportToExcelBusy = false;

  data: PagedResultDto<SpoBatchRequestDto> = {
    items: [],
    totalCount: 0,
  };

  filters = {
    isDeactive: false,
    maxResultCount: DEFAULT_PAGE_SIZE,
    status: 'IN_PROGRESS',
  } as GetSpoBatchRequestsInput;

  private isFirstLoad = true;

  hookToQuery() {
    // Chỉ đọc từ URL lần đầu tiên
    if (this.isFirstLoad) {
      const statusFromUrl = this.route.snapshot.queryParams['status'];
      if (statusFromUrl) {
        this.filters.status = statusFromUrl;
      }
      this.isFirstLoad = false;
    }

    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        status: this.filters.status ?? null,
      },
      queryParamsHandling: 'merge',
      replaceUrl: true,
    });

    const getData = (query: ABP.PageQueryParams) =>
      this.proxyService.getList({
        ...query,
        ...this.filters,
      });

    const setData = (list: PagedResultDto<SpoBatchRequestDto>) => {
      this.data = list;
    };

    if (!this.currentSubscription) {
      this.currentSubscription = this.list.hookToQuery(getData).subscribe({
        next: res => setData(res),
        error: () => this.loadingService.hide(),
      });
    } else {
      this.currentSubscription.unsubscribe();
      this.currentSubscription = this.list.hookToQuery(getData).subscribe({
        next: res => setData(res),
        error: () => this.loadingService.hide(),
      });
    }
  }

  clearFilters() {
    this.filters = {
      isDeactive: false,
      maxResultCount: DEFAULT_PAGE_SIZE,
      status: 'IN_PROGRESS',
    } as GetSpoBatchRequestsInput;

    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { status: 'IN_PROGRESS' },
      replaceUrl: true,
    });

    this.list.get();
  }

  exportToExcel() {
    // Implement if needed - for future enhancement
  }
}
