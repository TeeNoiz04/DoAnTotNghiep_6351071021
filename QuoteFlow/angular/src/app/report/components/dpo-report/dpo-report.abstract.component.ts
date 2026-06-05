import { Directive, inject, OnInit, ViewChild } from '@angular/core';

import { AbpWindowService, ListService, PermissionService, TrackByService } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Router } from '@angular/router';
import { DEFAULT_PAGE_SIZE } from '@app/shared/constants';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { TemplateService } from '@proxy/general-templates';
import { TitleService } from '@app/shared/services/title/title.service';
import { DPOReportViewService } from '@app/report/services/dpo-report.service';
import { DPOReportFilterComponent } from './components/dpo-report-filter/dpo-report-filter.component';
import { DPODataReportDto } from '@proxy/dpos';
import { finalize } from 'rxjs';

export const ChildTabDependencies = [];

export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractDPOReportComponent implements OnInit {
  public readonly list = inject(ListService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(DPOReportViewService);

  public readonly permissionService = inject(PermissionService);
  protected readonly router = inject(Router);
  protected readonly toast = inject(ToasterService);
  protected readonly templateService = inject(TemplateService);
  protected readonly loadingService = inject(LoadingService);
  public readonly titleService = inject(TitleService);
  public readonly abpWindowService = inject(AbpWindowService);

  protected title = 'DPO Received By Material Type';

  @ViewChild('dpoReportFilter') dpoReportFilter: DPOReportFilterComponent | undefined;

  ngOnInit() {
    this.titleService.setTitle(this.title);
    this.list.maxResultCount = DEFAULT_PAGE_SIZE;
  }

  clearFilters() {
    if (!this.dpoReportFilter) {
      return;
    }
    this.dpoReportFilter.form.reset();
    this.dpoReportFilter.form.get('buyerId')?.setValue('none');
    this.service.clearFilters();
  }

  onSearch() {
    if (!this.dpoReportFilter) {
      return;
    }
    if (!this.dpoReportFilter.form.valid) {
      this.toast.error('Please fill in all required fields.');
      this.dpoReportFilter.form.markAllAsTouched();
      return;
    }
    const values = this.dpoReportFilter?.form.getRawValue();
    this.service.filters = {
      buyerTypeId: values.buyerTypeId,
      buyerId: values.buyerId === 'none' ? null : values.buyerId,
      fromDate: values.fromdate,
      toDate: values.todate,
    };
    this.service.hookToQuery();
  }

  exportToExcel(): void {
    if (!this.dpoReportFilter || !this.dpoReportFilter.form.valid) {
      this.toast.error('Please fill in all required fields before exporting.');
      if (this.dpoReportFilter) {
        this.dpoReportFilter.form.markAllAsTouched();
      }
      return;
    }
    this.service.isExportToExcelBusy = true;
    const values = this.dpoReportFilter.form.getRawValue();
    const exportFilter = {
      buyerTypeId: values.buyerTypeId,
      buyerId: values.buyerId === 'none' ? null : values.buyerId,
      fromDate: values.fromdate,
      toDate: values.todate,
    };

    this.service.proxyService
      .getListDPOReport(exportFilter)
      .pipe(finalize(() => (this.service.isExportToExcelBusy = false)))
      .subscribe({
        next: (blob: Blob) => {
          this.abpWindowService.downloadBlob(blob, `DPO_Report_${this.formatExportFileName()}.xlsx`);
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

  abstract getTotalAmount(): number;
  abstract getTotalByType(type: string): number;
  abstract getRowTotal(item: DPODataReportDto): number;
  abstract getMonthlyTotal(month: string): number;
  abstract getMonthlyTotalByType(month: string, type: string): number;
}
