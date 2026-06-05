import { ABP, AbpWindowService, ListService, PagedResultDto } from '@abp/ng.core';
import { ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { DatePipe } from '@angular/common';
import { inject } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { DPODataReportDto, DPOService, GetDPOReportInputDto } from '@proxy/dpos';
import { finalize, map, Subscription } from 'rxjs';

export abstract class AbstractDPOReportViewService {
  public readonly proxyService = inject(DPOService);
  protected readonly list = inject(ListService);
  protected readonly route = inject(ActivatedRoute);
  protected readonly router = inject(Router);
  protected readonly loadingService = inject(LoadingService);
  protected readonly fb = inject(FormBuilder);
  public readonly toast = inject(ToasterService);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly abpWindowService = inject(AbpWindowService);
  protected readonly datePipe = inject(DatePipe);
  private currentSubscription: Subscription;
  isExportToExcelBusy = false;

  data: PagedResultDto<DPODataReportDto> = {
    items: [],
    totalCount: 0,
  };

  filters: GetDPOReportInputDto = {
    buyerTypeId: null,
    buyerId: null,
    fromDate: null,
    toDate: null,
  };

  formatDate(datetime: string, format: string = 'yyyy-MM-dd'): string {
    return datetime ? this.datePipe.transform(datetime, format) : null;
  }

  hookToQuery() {
    const getData = (query: ABP.PageQueryParams) =>
      this.proxyService
        .getDataDPOReport({
          ...query,
          ...this.filters,
        })
        .pipe(
          map(
            items =>
              ({
                items,
                totalCount: items.length,
              }) as PagedResultDto<DPODataReportDto>,
          ),
        );

    const setData = (list: PagedResultDto<DPODataReportDto>) => {
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
      buyerTypeId: null,
      buyerId: null,
      fromDate: null,
      toDate: null,
    };
  }

  exportToExcel(filters: GetDPOReportInputDto): void {
    this.isExportToExcelBusy = true;

    this.proxyService
      .getListDPOReport(filters)
      .pipe(finalize(() => (this.isExportToExcelBusy = false)))
      .subscribe({
        next: (blob: Blob) => {
          const now = new Date();
          const dateStr = now.toISOString().split('T')[0];
          this.abpWindowService.downloadBlob(blob, `DPO_Report_${dateStr}.xlsx`);
          this.toast.success('Report exported successfully');
        },
        error: error => {
          console.error('Export failed:', error);
          this.toast.error('Failed to export report. Please try again.');
        },
      });
  }
}
