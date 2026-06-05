import { Directive, inject, OnInit } from '@angular/core';

import { AbpWindowService, ListService, PermissionService, TrackByService } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import { Router } from '@angular/router';
import { LoadingService } from '@app/shared/services/loading/loading.service';
import { TemplateService } from '@proxy/general-templates';
import { TitleService } from '@app/shared/services/title/title.service';
import { StockReportViewService } from '@app/stock-management/services/stock-report.service';
import { finalize } from 'rxjs';

export const ChildTabDependencies = [];

export const ChildComponentDependencies = [];

@Directive()
export abstract class AbstractStockReportComponent implements OnInit {
  public readonly list = inject(ListService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(StockReportViewService);

  public readonly permissionService = inject(PermissionService);
  protected readonly router = inject(Router);
  protected readonly toast = inject(ToasterService);
  protected readonly templateService = inject(TemplateService);
  protected readonly loadingService = inject(LoadingService);
  public readonly titleService = inject(TitleService);
  public readonly abpWindowService = inject(AbpWindowService);

  protected title = 'Overall Stock Report (R21)';

  ngOnInit() {
    this.titleService.setTitle(this.title);
    this.service.hookToQuery();
  }

  exportToExcel(): void {
    this.service.isExportToExcelBusy = true;

    this.service.proxyService
      .getListOverallStockReport()
      .pipe(finalize(() => (this.service.isExportToExcelBusy = false)))
      .subscribe({
        next: (blob: Blob) => {
          this.abpWindowService.downloadBlob(blob, `R21_Overall Stock Report_${this.formatExportFileName()}.xlsx`);
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
}
