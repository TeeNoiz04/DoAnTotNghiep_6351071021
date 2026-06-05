import { inject, Injectable } from '@angular/core';
import { ABP, AbpWindowService, ListService, PagedResultDto } from '@abp/ng.core';
import { ActivatedRoute, Router } from '@angular/router';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { map, Subscription, finalize } from 'rxjs';
import {
  StockManagementService,
  GetInventoryReportsInput,
  InventoryReportDto,
} from '@proxy/stock-managements';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { ToasterService } from '@abp/ng.theme.shared';

@Injectable()
export class InventoryReportViewService {
  private readonly proxyService = inject(StockManagementService);
  public readonly list = inject(ListService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly loadingService = inject(LoadingService);
  private readonly abpWindowService = inject(AbpWindowService);
  private readonly toast = inject(ToasterService);

  private currentSubscription?: Subscription;
  isExportBusy = false;

  data: PagedResultDto<InventoryReportDto> = {
    items: [],
    totalCount: 0,
  };

  filters: GetInventoryReportsInput = {
    skipCount: 0,
    maxResultCount: DEFAULT_PAGE_SIZE,
  };

  hookToQuery(): void {
    const getData = (query: ABP.PageQueryParams) => {
      this.filters = {
        ...this.filters,
        skipCount: query.skipCount || 0,
        maxResultCount: query.maxResultCount || DEFAULT_PAGE_SIZE,
      };

      return this.proxyService.getListInventoryReport(this.filters);
    };

    const setData = (list: PagedResultDto<InventoryReportDto>) => {
      this.data = list;
    };

    if (this.currentSubscription) {
      this.currentSubscription.unsubscribe();
    }

    this.currentSubscription = this.list.hookToQuery(getData).subscribe({
      next: res => {
        setData(res);
      },
      error: () => {
        this.loadingService.hide();
        this.toast.error('Failed to load inventory report data');
      },
    });
  }

  search(searchFilters: Partial<GetInventoryReportsInput>): void {
    this.filters = {
      ...this.filters,
      ...searchFilters,
      skipCount: 0, // Reset to first page when searching
    };

    this.loadingService.show();

    this.proxyService
      .getListInventoryReport(this.filters)
      .pipe(finalize(() => this.loadingService.hide()))
      .subscribe({
        next: res => {
          this.data = res;
          this.updateUrlParams();
        },
        error: () => {
          this.toast.error('Failed to search inventory report data');
        },
      });
  }

  clearFilters(): void {
    this.filters = {
      skipCount: 0,
      maxResultCount: DEFAULT_PAGE_SIZE,
    };
    this.hookToQuery();
  }

  exportToExcel(exportFilters?: Partial<GetInventoryReportsInput>): void {
    const finalFilters = exportFilters ? { ...this.filters, ...exportFilters } : this.filters;

    this.isExportBusy = true;
    this.loadingService.show();

    this.proxyService
      .getListExcelInventoryReport(finalFilters)
      .pipe(
        finalize(() => {
          this.isExportBusy = false;
          this.loadingService.hide();
        }),
      )
      .subscribe({
        next: (response: Blob) => {
          const fileName = `inventory-report-${new Date().getTime()}.xlsx`;
          const url = window.URL.createObjectURL(response);
          const link = document.createElement('a');
          link.href = url;
          link.download = fileName;
          link.click();
          window.URL.revokeObjectURL(url);
          this.toast.success('Export completed successfully');
        },
        error: () => {
          this.toast.error('Failed to export inventory report');
        },
      });
  }

  onPage(event: { offset: number; limit: number }): void {
    this.filters.skipCount = event.offset * event.limit;
    this.filters.maxResultCount = event.limit;
    this.search({});
  }

  private updateUrlParams(): void {
    const queryParams: Record<string, string> = {};

    if (this.filters.materialCode) {
      queryParams.materialCode = this.filters.materialCode;
    }
    if (this.filters.inventoryCategory) {
      queryParams.inventoryCategory = this.filters.inventoryCategory;
    }
    if (this.filters.materialGroup) {
      queryParams.materialGroup = this.filters.materialGroup;
    }

    this.router.navigate([], {
      relativeTo: this.route,
      queryParams,
      queryParamsHandling: 'merge',
    });
  }
}
