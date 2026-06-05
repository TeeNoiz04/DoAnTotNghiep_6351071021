import { Directive, inject, OnInit, ViewChild } from '@angular/core';

import { AbpWindowService, ListService, PermissionService, TrackByService } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Router } from '@angular/router';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { TemplateService } from '@proxy/general-templates';
import { TitleService } from '@app/shared/services/title/title.service';
import { DPOReportViewService } from '@app/report/services/dpo-report.service';
import { DPODataReportDto } from '@proxy/dpos';
import { finalize } from 'rxjs';
import { DPOProcessingReportFilterComponent } from './components/dpo-report-filter/dpo-processing-report-filter.component';
import { DPOProcessingReportViewService } from '@app/report/services/dpo-processing-report.service';

export const ChildTabDependencies = [];

export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractDPOProcessingReportComponent implements OnInit {
  public readonly list = inject(ListService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(DPOProcessingReportViewService);

  public readonly permissionService = inject(PermissionService);
  protected readonly router = inject(Router);
  protected readonly toast = inject(ToasterService);
  protected readonly templateService = inject(TemplateService);
  protected readonly loadingService = inject(LoadingService);
  public readonly titleService = inject(TitleService);
  public readonly abpWindowService = inject(AbpWindowService);
  pageNumber = 0; // trang hiện tại (bắt đầu từ 0)
  pageSize = DEFAULT_PAGE_SIZE; // số dòng mỗi trang
  pagedData = [];
  protected title = 'DPO Processing Report';

  @ViewChild('dpoProcessingReportFilter') dpoProcessingReportFilter: DPOProcessingReportFilterComponent | undefined;

  ngOnInit() {
    this.titleService.setTitle(this.title);
  }

  clearFilters() {
    if (!this.dpoProcessingReportFilter) {
      return;
    }
    this.dpoProcessingReportFilter.form.reset();
    this.dpoProcessingReportFilter.form.get('buyerId')?.setValue('none');
    this.service.clearFilters();
    this.pageNumber = 0;
    this.updatePagedData();
  }

  onSearch() {
    if (!this.dpoProcessingReportFilter) {
      return;
    }
    if (!this.dpoProcessingReportFilter.form.valid) {
      this.toast.error('Please fill in all required fields.');
      this.dpoProcessingReportFilter.form.markAllAsTouched();
      return;
    }
    const values = this.dpoProcessingReportFilter?.form.getRawValue();
    this.service.filters = {
      buyerTypeId: values.buyerTypeId,
      buyerId: values.buyerId === 'none' ? null : values.buyerId,
      fromDate: values.fromdate,
      toDate: values.todate,
    };
    this.loadingService.show();
    this.service.proxyService.getDataDPOProcessingReport(this.service.filters).subscribe({
      next: res => {
        this.service.data = {
          items: res,
          totalCount: res.length,
        };
        this.pageNumber = 0;
        this.updatePagedData();
        this.loadingService.hide();
      },
      error: err => {
        console.error('Error loading DPO Processing Report:', err);
        this.loadingService.hide();
      },
    });
  }

  exportToExcel(): void {
    if (!this.dpoProcessingReportFilter || !this.dpoProcessingReportFilter.form.valid) {
      this.toast.error('Please fill in all required fields before exporting.');
      if (this.dpoProcessingReportFilter) {
        this.dpoProcessingReportFilter.form.markAllAsTouched();
      }
      return;
    }
    this.service.isExportToExcelBusy = true;
    const values = this.dpoProcessingReportFilter.form.getRawValue();
    const exportFilter = {
      buyerTypeId: values.buyerTypeId,
      buyerId: values.buyerId === 'none' ? null : values.buyerId,
      fromDate: values.fromdate,
      toDate: values.todate,
    };

    this.service.proxyService
      .getListDPOProcessingReport(exportFilter)
      .pipe(finalize(() => (this.service.isExportToExcelBusy = false)))
      .subscribe({
        next: (blob: Blob) => {
          this.abpWindowService.downloadBlob(blob, `DPO_Processing_Report_${this.formatExportFileName()}.xlsx`);
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
    this.updatePagedData();
  }

  updatePagedData() {
    const items = this.service.data && Array.isArray(this.service.data.items) ? this.service.data.items : [];
    const start = this.pageNumber * this.pageSize;
    const end = start + this.pageSize;
    this.pagedData = items.slice(start, end);
  }
}
