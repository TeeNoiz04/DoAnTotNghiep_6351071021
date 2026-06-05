import { AbpWindowService, ListService, PermissionService, TrackByService } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Directive, inject, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { TitleService } from '@app/shared/services/title/title.service';
import { SaleReportR05ViewService } from '@app/stock-management/services/sale-report-r05.service';
import { TemplateService } from '@proxy/general-templates';
import { finalize } from 'rxjs';
import { SaleReportR05FilterComponent } from './sale-report-general-filter/sale-report-r05-filter.component';

export const ChildTabDependencies = [];
export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractSaleReportR05Component implements OnInit {
  public readonly list = inject(ListService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(SaleReportR05ViewService);
  public readonly permissionService = inject(PermissionService);
  protected readonly router = inject(Router);
  protected readonly toast = inject(ToasterService);
  protected readonly templateService = inject(TemplateService);
  protected readonly loadingService = inject(LoadingService);
  public readonly titleService = inject(TitleService);
  public readonly abpWindowService = inject(AbpWindowService);

  pageNumber = 0;
  pageSize = DEFAULT_PAGE_SIZE;
  pagedData = [];
  protected title = 'Sale Report R05';

  @ViewChild('saleReportR05Filter') saleReportR05Filter: SaleReportR05FilterComponent | undefined;

  ngOnInit() {
    this.titleService.setTitle(this.title);
  }

  clearFilters() {
    if (!this.saleReportR05Filter) {
      return;
    }
    this.saleReportR05Filter.form.reset();
    this.service.clearFilters();
    this.pageNumber = 0;
    this.updatePagedData();
  }

  onSearch() {
    if (!this.saleReportR05Filter) {
      return;
    }

    const values = this.saleReportR05Filter.form.getRawValue();

    this.service.filters = {
      fromDate: values.fromDate,
      toDate: values.toDate,
      invoiceFromDate: values.invoiceFromDate,
      invoiceToDate: values.invoiceToDate,
      skipCount: 0,
      maxResultCount: this.pageSize,
    };

    this.loadingService.show();

    this.service.proxyService.getListSaleReportGeneral(this.service.filters).subscribe({
      next: res => {
        this.service.data = res;
        this.pageNumber = 0;
        this.updatePagedData();
        this.loadingService.hide();
      },
      error: err => {
        console.error('Error loading Sale Report R05:', err);
        this.loadingService.hide();
      },
    });
  }

  exportToExcel(): void {
    this.service.isExportToExcelBusy = true;
    const values = this.saleReportR05Filter.form.getRawValue();

    const exportFilter = {
      fromDate: values.fromDate,
      toDate: values.toDate,
      invoiceFromDate: values.invoiceFromDate,
      invoiceToDate: values.invoiceToDate,
      skipCount: 0,
      maxResultCount: DEFAULT_PAGE_SIZE,
    };

    this.service.proxyService
      .getListSaleReportGeneralAsExcel(exportFilter)
      .pipe(finalize(() => (this.service.isExportToExcelBusy = false)))
      .subscribe({
        next: (blob: Blob) => {
          this.abpWindowService.downloadBlob(blob, `Sale_Report_R05_${this.formatExportFileName()}.xlsx`);
          this.toast.success('Report exported successfully');
        },
        error: error => {
          console.error('Export failed:', error);
          this.toast.error('Failed to export report. Please try again.');
        },
      });
  }

  private formatExportFileName(): string {
    const now = new Date();
    const dateStr = now.toISOString().split('T')[0]; // YYYY-MM-DD
    return `${dateStr}`;
  }

  onPage(event: any) {
    this.pageNumber = event.offset;

    if (!this.saleReportR05Filter) {
      return;
    }

    const values = this.saleReportR05Filter.form.getRawValue();
    const filters = {
      fromDate: values.fromDate,
      toDate: values.toDate,
      invoiceFromDate: values.invoiceFromDate,
      invoiceToDate: values.invoiceToDate,
      skipCount: this.pageNumber * this.pageSize,
      maxResultCount: this.pageSize,
    };

    this.loadingService.show();

    this.service.proxyService.getListSaleReportGeneral(filters).subscribe({
      next: res => {
        this.service.data = res;
        this.updatePagedData();
        this.loadingService.hide();
      },
      error: err => {
        console.error('Error loading Sale Report R05:', err);
        this.loadingService.hide();
      },
    });
  }

  updatePagedData() {
    const items = this.service.data && Array.isArray(this.service.data.items) ? this.service.data.items : [];
    this.pagedData = items;
  }

  get totalCount(): number {
    return this.service.data?.totalCount || 0;
  }

  get hasData(): boolean {
    return this.pagedData && this.pagedData.length > 0;
  }

  get Math() {
    return Math;
  }
}
